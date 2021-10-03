from contextlib import contextmanager
from typing import Callable
from dianascript.chexpr import *
from dianascript.chlhs import *
from dianascript.chstmt import *
from dianascript.code_cons import *
from dianascript.logger import logger
from pyrsistent import pvector
from dianascript.serialize import initialize

import logging


initialize()

RETURN = 0
GO_AHEAD = 1
LOOP_BREAK = 2
LOOP_CONTINUE = 3

GLOBAL_FLAG = 0b11

class CG:
    def __init__(self, filename: str, is_global=False) -> None:
        self.filename = filename
        self.block: list[Instr] = []
        self.offset = 0
        self.lineno = 1
        self.local_scope: dict[str, tuple[str, int]] = {} # user name -> mangled name
        self.local_names: list[str] = []
        self.linenos: list[tuple[int, int]] = []
        self.is_global = is_global
        

    @property
    def next_offset(self):
        return self.offset

    def mk_block(self):
        res = []
        for each in self.block:
            assert not isinstance(each, PlaceHolder)
            each.dumps(res)
        return pvector(res)

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
            return (InternString(name).as_flatten() << 2) | GLOBAL_FLAG

    def __lshift__(self, other: Instr):
        self.block.append(other) # type: ignore
        self.offset += other.OFFSET
        return len(self.block) - 1

    def cg_for_stmts(self, stmts):
        for stmt in stmts:
            self.cg_for_stmt(stmt)


    def cg_for_stmt(self, stmt: Chstmt):
        if stmt.loc:

            new_lineno = stmt.loc[0]
            if self.lineno < new_lineno:
                self.linenos.append((self.next_offset, new_lineno))
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
                    bytecode=block,
                    nonargcells = pvector(),
                    is_vararg = False,
                    freeslots = pvector(),
                    narg = narg,
                    nlocal = len(sub_cg.local_names),
                    name = InternString(name),
                    filename = sub_cg.filename,
                    lineno = start_lineno,
                    freenames=pvector([]),
                    localnames=pvector(sub_cg.local_names),
                    linenos = pvector(sub_cg.linenos),
                )
                d = Diana_FunctionDef(meta.as_flatten())
                self << d
                i = self.varid(name)
                self << Diana_StoreVar(i)
            case SDecl(vars):
                for each in vars:
                    self.declarevar(each)

            case SAssign(targets, value):
                self.cg_for_expr(value)
                self << Diana_Replicate(len(targets))
                for each in reversed(targets):
                    self.cg_for_lhs(each)

            case SExpr(expr):
                self.cg_for_expr(expr)
                self << Diana_Pop()
            case SFor(target, iter, body):
                self.cg_for_expr(iter)
                backref = self << PlaceHolder(Diana_For.OFFSET)
                if target:
                    self.cg_for_lhs(target)
                else:
                    self << Diana_Pop()
                self.cg_for_stmts(body)
                self.block[backref] = Diana_For(self.next_offset)

            case SLoop(body):
                backref = self << PlaceHolder(Diana_For.OFFSET)
                self.cg_for_stmts(body)            
                self.block[backref] = Diana_Loop(self.next_offset)

            case SIf(cond, [], None):
                self.cg_for_expr(cond)
                self << Diana_Pop()

            case SIf(cond, then, None):

                self.cg_for_expr(cond)
                i_jump_to_orelse_if_not = self << PlaceHolder(Diana_JumpIfNot.OFFSET)
                self.cg_for_stmts(then)
                self.block[i_jump_to_orelse_if_not] = Diana_JumpIfNot(self.next_offset)
                

            case SIf(cond, [], orelse):
                self.cg_for_expr(cond)
                i_jump_to_orelse_if_not = self << PlaceHolder(Diana_JumpIf.OFFSET)
                self.cg_for_stmts(orelse)
                self.block[i_jump_to_orelse_if_not] = Diana_JumpIf(self.next_offset)

            case SIf(cond, then, orelse):
                self.cg_for_expr(cond)
                i_jump_to_orelse_if_not = self << PlaceHolder(Diana_JumpIfNot.OFFSET)
                self.cg_for_stmts(then)
                i_jump_to_success = self << PlaceHolder(Diana_Jump.OFFSET)
                self.block[i_jump_to_orelse_if_not] = Diana_JumpIfNot(self.next_offset)
                self.cg_for_stmts(orelse)
                self.block[i_jump_to_success] = Diana_Jump(self.next_offset)

            case SBreak():
                self << Diana_Break()

            case SContinue():
                self << Diana_Continue()

            case SReturn(val):
                if val is None:
                    self << Diana_Const(DObj(None).as_flatten())
                else:
                    self.cg_for_expr(val)
                self << Diana_Return()

            case _:
                raise


    def cg_for_expr(self, expr: Chexpr):
        if expr.loc:
            new_lineno = expr.loc[0]
            if self.lineno < new_lineno:
                self.linenos.append((self.next_offset, new_lineno))
            self.lineno = new_lineno
        match expr:
            case EVal(val):
                self << Diana_Const(DObj(val).as_flatten())
            case EVar(var):
                i = self.varid(var)
                self << Diana_LoadVar(i)
            case EApp(f, args):
                self.cg_for_expr(f)
                for arg in args:
                    self.cg_for_expr(arg)
                self << Diana_Call(len(args))
            case EIt(value, item):
                self.cg_for_expr(value)
                self.cg_for_expr(item)
                self << Diana_GetItem()
            case EAttr(value, attr):
                self.cg_for_expr(value)
                self << Diana_GetAttr(InternString(attr))
            case EPar([elt],  False):
                self.cg_for_expr(elt)
            case EPar(elts,  _):
                for each in elts:
                    self.cg_for_expr(each)
                self << Diana_MKTuple(len(elts))
            
            case EDict(kvs):
                for k, v in kvs:
                    self.cg_for_expr(k)
                    self.cg_for_expr(v)
                self << Diana_MKDict(len(kvs))
            
            case EList(elts):
                for e in elts:
                    self.cg_for_expr(e)

                self << Diana_MKList(len(elts))

            case ENot(expr):
                self.cg_for_expr(expr)
                self << Diana_UnaryOp_not()

            case ENeg(expr):
                self.cg_for_expr(expr)
                self << Diana_UnaryOp_neg()

            case EInv(expr):
                self.cg_for_expr(expr)
                self << Diana_UnaryOp_invert()

            case EAnd(a, b):
                self.cg_for_expr(a)
                i = self << PlaceHolder(Diana_JumpIf.OFFSET)
                self.cg_for_expr(b)
                self.block[i] = Diana_JumpIf(self.next_offset)

            case EOr(a, b):
                self.cg_for_expr(a)
                i = self << PlaceHolder(Diana_JumpIfNot.OFFSET)
                self.cg_for_expr(b)
                self.block[i] = Diana_JumpIfNot(self.next_offset)

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
                self << Diana_StoreVar(i)
            case LIt(val, item):
                self.cg_for_expr(val)
                self.cg_for_expr(item)
                self << Diana_SetItem()
            case LAttr(value, attr):
                self.cg_for_expr(value)
                self << Diana_SetAttr(InternString(attr))
            case _:
                raise TypeError(expr)

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
    "!=": "ne",
    "in": "in",
    "notin": "notin"
}

_globals = globals()
op_map: dict[str, Instr] = {each: _globals["Diana_" + v]() for each, v in _op_map.items() }
del _globals, _op_map

def codegen(filename: str, suite: list[Chstmt], barr: bytearray):
    cg = CG(filename, is_global=True)
    cg.cg_for_stmts(suite)
    cg << Diana_Const(DObj(None).as_flatten())
    cg << Diana_Return()
    block = cg.mk_block()
    meta = FuncMeta(
                    bytecode=block,
                    nonargcells = pvector(),
                    is_vararg = False,
                    freeslots = pvector(),
                    narg = 0,
                    nlocal = 0,
                    name = InternString("__main__"),
                    filename = filename,
                    lineno = 1,
                    freenames=pvector([]),
                    localnames=pvector(cg.local_names),
                    linenos = pvector(cg.linenos),
                )
    metaind = meta.as_flatten()
    if logging.DEBUG >= logger.level :
        logger.debug("  entry point:")
        logger.debug(f"  - metaind : {metaind}")

    serialize_(metaind, barr)
    Storage.serialize_(barr)
