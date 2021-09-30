from contextlib import contextmanager
import enum
from typing import Callable
from dianascript.chexpr import *
from dianascript.chlhs import *
from dianascript.chstmt import *
from dianascript.code_cons import *

RETURN = 0
GO_AHEAD = 1
LOOP_BREAK = 2
LOOP_CONTINUE = 3

GLOBAL_FLAG = 0b11

class PlaceHolder:
    pass

class CG:
    def __init__(self, filename: str, is_global=False) -> None:
        self.filename = filename
        self.block: list[Ptr] = []
        self.lineno = 1
        self.local_scope: dict[str, tuple[str, int]] = {} # user name -> mangled name
        self.local_names = []
        self.linenos = []
        self.is_global = is_global
    
    @contextmanager
    def enter_block(self):
        old_code = self.block
        linenos = self.linenos
        try:
            self.block = []
            self.linenos = []
            yield
        finally:
            self.block = old_code
            self.linenos = linenos
        
    @property
    def cur_offset(self):
        return len(self.block)

    def mk_block(self):
        return Block(tuple(self.block), tuple(self.linenos), self.filename)

    def declarevar(self, name: str):
        if not self.is_global:
            i = len(self.local_names)
            var = name + " " + str(i)
            self.local_scope[name] = (var, i)
            self.local_names.append(var)

    def varid(self, name):
        if pair := self.local_scope.get(name):
            return pair[1] << 2
        else:
            return (InternString(name).as_int() << 2) | GLOBAL_FLAG
    
    def __lshift__(self, other: Ptr | PlaceHolder):
        self.block.append(other)
        return len(self.block) - 1
    
    def cg_for_stmts(self, stmts):
        for stmt in stmts:
            self.cg_for_stmt(stmt)
        

    def cg_for_stmt(self, stmt: Chstmt):
        if stmt.loc:
            
            new_lineno = stmt.loc[0]
            if self.lineno < new_lineno:
                self.linenos.append((self.cur_offset, new_lineno))
            self.lineno = new_lineno
    
        match stmt:
            case SFunc(name, args, body):
                narg = len(args)
                start_lineno = self.lineno
                sub_cg = CG(self.filename)
                for arg in args:
                    sub_cg.declarevar(arg)
                sub_cg.cg_for_stmts(body)
                block = sub_cg.mk_block()
                meta = FuncMeta(
                    nonargcells = (),
                    is_vararg = False,
                    freeslots = (),
                    narg = narg,
                    nlocal = len(sub_cg.local_names),
                    name = InternString(name), 
                    filename = self.filename,
                    lineno = start_lineno,
                    freenames=tuple([]),
                    localnames=tuple(sub_cg.local_names),
                )
                d = Diana_FunctionDef(meta.as_int(), block.as_int())
                self << d.as_ptr()
            case SDecl(vars):
                for each in vars:
                    self.declarevar(each)
            
            case SAssign(targets, value):
                self.cg_for_expr(value)
                self << Diana_Replicate(len(targets)).as_ptr()
                for each in reversed(targets):
                    self.cg_for_lhs(each)
            
            case SExpr(expr):
                self.cg_for_expr(expr)
                self << Diana_Pop().as_ptr()
            case SFor(target, iter, body):
                self.cg_for_expr(iter)
                with self.enter_block():
                    if target:
                        self.cg_for_lhs(target)
                    else:
                        self << Diana_Pop().as_ptr()
                    self.cg_for_stmts(body)
                    new_block = self.mk_block()
                    
                self << Diana_For(new_block.as_int()).as_ptr()
        
            case SLoop(body):
                with self.enter_block():
                    self.cg_for_stmts(body)
                    new_block = self.mk_block()
                self << Diana_Loop(new_block.as_int()).as_ptr()
        
            case SIf(cond, [], None):
                self.cg_for_expr(cond)
            
            case SIf(cond, then, None):
                self.cg_for_expr(cond)
                with self.enter_block():
                    self.cg_for_stmts(then)
                    new_block = self.block
                    linenos = self.linenos
                
                target_offset = len(self.block) + len(new_block)
                self.block.extend(new_block)
                self.linenos.extend(linenos)
                self << Diana_JumpIfNot(target_offset).as_ptr()
            
            case SIf(cond, [], orelse):
                self.cg_for_expr(cond)
                with self.enter_block():
                    self.cg_for_stmts(orelse)
                    new_block = self.block
                    linenos = self.linenos
                
                target_offset = len(self.block) + len(new_block)
                self.block.extend(new_block)
                self.linenos.extend(linenos)
                self << Diana_JumpIf(target_offset).as_ptr()
        
            case SIf(cond, then, orelse):
                self.cg_for_expr(cond)
                i_jump_to_orelse_if_not = self << PlaceHolder()
                self.cg_for_stmts(then)
                i_jump_to_success = self << PlaceHolder()
                self.block[i_jump_to_orelse_if_not] = Diana_JumpIfNot(self.cur_offset).as_ptr()
                self.cg_for_stmts(orelse)
                self.block[i_jump_to_success] = Diana_Jump(self.cur_offset).as_ptr()
            
            case SBreak():
                self << Diana_Control(LOOP_BREAK).as_ptr()
            
            case SContinue():
                self << Diana_Control(LOOP_CONTINUE).as_ptr()
            
            case SReturn(val):
                if val is None:
                    self << Diana_Const(DObj(None).as_int()).as_ptr()
            case _:
                raise


    def cg_for_expr(self, expr: Chexpr):
        if expr.loc:
            new_lineno = expr.loc[0]
            if self.lineno < new_lineno:
                self.linenos.append((self.cur_offset, new_lineno))
            self.lineno = new_lineno
        match expr:
            case EVal(val):
                self << Diana_Const(DObj(val).as_int()).as_ptr()
            case EVar(var):
                i = self.varid(var)
                self << Diana_LoadVar(i).as_ptr()
            case EApp(f, args):
                self.cg_for_expr(f)
                for arg in args:
                    self.cg_for_expr(arg)
                self << Diana_Call(len(args)).as_ptr()
            case EIt(value, item):
                self.cg_for_expr(value)
                self.cg_for_expr(item)
                self << Diana_GetItem().as_ptr()
            case EAttr(value, attr):
                self.cg_for_expr(value)
                self << Diana_GetAttr(InternString(attr)).as_ptr()
            case EPar([elt],  False):
                self.cg_for_expr(elt)
            case EPar(elts,  _):
                for each in elts:
                    self.cg_for_expr(each)
                self << Diana_MKTuple(len(elts)).as_ptr()
            case EDict(kvs):
                for k, v in kvs:
                    self.cg_for_expr(k)
                    self.cg_for_expr(v)
                self << Diana_MKDict(len(kvs)).as_ptr()
            case EList(elts):
                for e in elts:
                    self.cg_for_expr(e)
                    
                self << Diana_MKDict(len(elts)).as_ptr()
            
            case ENot(expr):
                self.cg_for_expr(expr)
                self << Diana_UnaryOp_not().as_ptr()
            
            case ENeg(expr):
                self.cg_for_expr(expr)
                self << Diana_UnaryOp_neg().as_ptr()
            
            case EInv(expr):
                self.cg_for_expr(expr)
                self << Diana_UnaryOp_invert().as_ptr()
            
            case EAnd(a, b):
                self.cg_for_expr(a)
                i = self << PlaceHolder()
                self.cg_for_expr(b)
                self.block[i] = Diana_JumpIf(self.cur_offset).as_ptr()
            
            case EOr(a, b):
                self.cg_for_expr(a)
                i = self << PlaceHolder()
                self.cg_for_expr(b)
                self.block[i] = Diana_JumpIfNot(self.cur_offset).as_ptr()
            
            case EOp(left=left, op=op, right=right):
                self.cg_for_expr(left)
                self.cg_for_expr(right)
                self << op_map[op]
            case _:
                raise
    def cg_for_lhs(self, expr: Chlhs):
        match expr:
            case LVar(var):
                i = self.varid(var)
                self << Diana_StoreVar(i).as_ptr()
            case LIt(val, item):
                self.cg_for_expr(val)
                self.cg_for_expr(item)
                self << Diana_SetItem().as_ptr()
            case LAttr(value, attr):
                self.cg_for_expr(value)
                self << Diana_SetAttr(InternString(attr)).as_ptr()
            case _:
                pass

_op_map = {

    "+": "add",
    "-": "sub",
    "*": "mul",
    r"/": "truediv",
    r"//": "floordiv",
    "%": "mod",
    "**": "pow",
    "<<": "lshift",
    ">>": "rshift",
    "|": "bitor",
    "&": "bitand",
    "^": "bitxor",
    ">": "gt",
    "<": "lt",
    ">=": "ge",
    "<=": "le",
    "=": "eq",
    "!=": "neq",
    "in": "in",
    "notin": "notin"
}

_globals = globals()
op_map: dict[str, Ptr] = {each: _globals["Diana_" + v]().as_ptr() for each, v in _op_map.items() }
del _globals, _op_map

def codegen(filename: str, suite: list[Chstmt], barr: bytearray):
    cg = CG(filename, is_global=True)
    cg.cg_for_stmt(SFunc("__main__", [], suite))
    entry = cg.mk_block().as_int()
    serialize_(entry, barr)
    DFlatGraphCode.serialize_(barr)

    