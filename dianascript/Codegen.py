from ScopeAnalyzer import AnalyzedSymTable
from __future__ import annotations
from ast import *
from dataclasses import dataclass, field
from enum import Enum
# from dianascript.ScopeAnalyzer import get_tag
from typing import Any
import struct
import contextlib




class BIN_OP:
    ADD: int
    SUB: int
    MUL: int
    FLOORDIV: int
    TRUEDIV: int
    MOD: int
    LSHIFT: int
    RSHIFT: int
    AND: int
    OR: int
    XOR: int
    EQ: int
    NE: int
    IN: int
    POW: int


class Label:
    pass
class CodeBuilder:
    def make_label(self) -> Label:
        raise NotImplementedError
    def add_label(self, label: Label): ...

    def DUP(self, ): ... # TODO
    def POP(self, ): ...
    def RETURN(self, ): ...
    def ERR_CLEAR(self, ): ...
    def LOAD_ITEM(self, ): ...
    def STORE_ITEM(self, ): ...
    def DEL_ITEM(self, ): ...
    def INVERT(self,) : ...
    def LOAD_NEXT(self,) : ... # TODO
    def CALL_NEXT(self, label: Label) : ... # TODO
    
    def CHECK_EXC(self, label: Label) : ... # TODO
    # if falsed, pop_block

    

    def POP_BLOCK(self, ): ...
    def JUMP(self, label: Label): ...
    def JUMP_IF(self, label: Label): ...

    def RAISE(self, op: int): ...
    # op = 0 : reraise
    # op = 1 : raise e
    # op = 2 : raise e from e
    # op = 3 : raise e if e is an exception
    
    def LOAD_ATTR(self, s: str): ...
    def STORE_ATTR(self, s: str): ...
    def LOAD_GLOBAL(self, s: str): ...
    def STORE_GLOBAL(self, s: str): ...
    def REF_GLOBAL(self, s: str): ...
    def LOAD_LOCAL(self, s: str): ...
    def STORE_DEREF(self, s: str): ...
    def LOAD_DEREF(self, s: str): ...
    def STORE_LOCAL(self, s: str): ...
    def REF_LOCAL(self, s: str): ...
    def LOAD_CONST(self, v: object): ...
    def PEEK(self, n: int): ...
    def CALL(self, n: int): ...
    def PUSH_BLOCK(self, target: Label): ...
    def MAKE_FUNCTION(self, flag: int): ...
    
    
    def BIN(self, bin_op: int): ...
    def MK_TUPLE(self, n: int): ...
    def MK_LIST(self, n: int): ...
    def MK_DICT(self, n: int): ...


@dataclass
class FLAGS:
    MAKE_FUNCTION_FREE = 0b01
    MAKE_FUNCTION_DEFAULTS = 0b10

@dataclass
class DCode:
    filename: str
    name: str
    varg: bool
    narg: int
    nlocal: int
    nfree: int
    strings: list[str]
    consts: list[object]
    locs: list[tuple[int, int]]    
    bc: bytearray

def _perform_set_loc(f):
    def ap(self, node):
        try:
            self.lineno = node.lineno, 
        except:
            pass
        return f(self, node)

def auto_set_loc(cls):
    for k in cls.__dict__:
        if k.startswith("visit_") and str.isupper(k[len("visit_")]):
            setattr(cls, k, _perform_set_loc(getattr(cls, k)))



class VarKind(Enum):
    Local = 0
    Cell = 1 # local and is cell
    Global = 2

class VarCtx(Enum):
    LOAD = 0
    STORE = 1
    DEL = 2
    REF = 3

@dataclass
class Var:
    name: str
    ctx : VarCtx
    loc : int
    kind: VarKind = VarKind.Local

    
@auto_set_loc
class AIRGenerator(CodeBuilder, NodeVisitor):
    symtable: AnalyzedSymTable
    def __init__(self, filename: str):
        self.filename = filename
        self.code = self.new_code()
        self.lineno = 1
        
    
    def new_code(self):
        return DCode(self.filename, "", False, 0, 0, 0, [], [], [], bytearray())


    @contextlib.contextmanager
    def enter(self, code: DCode):
        old_code = self.code
        try:
            self.code = code
        finally:
            self.code = old_code
    def visit_Module(self, node: Module):   
        for e in node.body:
            self.visit(e)
    
    def visit_FunctionDef(self, node: FunctionDef):
        for each in node.decorator_list:
            self.visit(each)

        new_code = self.new_code()
        self.symtable =  get_tag(node).analyzed
        flag = 0
        if node.args.defaults:
            raise NotImplementedError
        if node.args.kw_defaults:
            raise NotImplementedError
        if node.args.kwonlyargs:
            raise NotImplementedError
        if node.args.posonlyargs:
            raise NotImplementedError
        if node.args.kwarg:
            raise NotImplementedError
        if node.args.vararg:
            new_code.varg = True
            args = [*node.args.args, node.args.vararg]
        else:
            args = node.args.args
        new_code.strings.extend(arg.arg for arg in args)
        with self.enter(new_code):
            for e in node.body:
                self.visit(e)
        

        if new_code.nfree:
            flag |= FLAGS.MAKE_FUNCTION_FREE
            for each in self.symtable.freevars:
                self.LOAD_LOCAL(each)
            self.MK_TUPLE(new_code.nfree)
        self.MAKE_FUNCTION(flag)
    
    def visit_AsyncFunctionDef(self, node: AsyncFunctionDef):
        raise NotImplementedError
    
    def visit_ClassDef(self, node: ClassDef):
        raise NotImplementedError
    
    def visit_Return(self, node: Return):
        if node.value:
            self.visit(node.value)
        else:
            self.LOAD_CONST(None)
        self.RETURN()
    
    def visit_Delete(self, node: Delete):
        for target in node.targets:
            self.visit(target)
        

    def visit_Assign(self, node: Assign) -> Any:
        self.visit(node.value)
        n = len(node.targets)
        for _ in range(n - 1):
            self.DUP()
        for target in node.targets:
            self.visit(target)
    
    def visit_AugAssign(self, node: AugAssign) -> Any:
        self.visit(node.value)
        handle = getattr(self, 'visit_' + node.target.__class__.__name__)(node.target, load_first=True)
        self.visit(node.target)
        self.bin_op(node.op)
        next(handle)
    
    def visit_AnnAssign(self, node: AnnAssign) -> Any:
        if node.value:
            self.visit(node.value)
            self.visit(node.target)
    
    def visit_For(self, node: For) -> Any:
        match node:
            case For(target=target, iter=iter, body=body, orelse=orelse):
                if orelse:
                    raise NotImplementedError("for else not supported.")
                self.visit(iter)
                self.LOAD_NEXT()
                loop_head = self.make_label()
                loop_exit = self.make_label()
                self.add_label(loop_head)
                self.CALL_NEXT(loop_exit)
                self.visit(target)
                for each in body:
                    self.visit(each)
                self.JUMP(loop_head)
    
    def visit_AsyncFunctionDef(self, node: AsyncFunctionDef) -> Any:
        raise NotImplementedError("async for not supported.")
    
    def visit_Raise(self, node: Raise) -> Any:
        if node.exc:
            self.visit(node.exc)
            if node.cause:
                self.visit(node.exc)
                self.RAISE(2)
            else:
                self.RAISE(1)
        else:
            self.RAISE(0)
    

    def visit_many(self, nodes:list[stmt] | list[expr]):
        for node in nodes:
            self.visit(node)
    def visit_Try(self, node: Try) -> Any:
        
        match node:
            case Try(body=body, handlers=handlers, finalbody=final_body, orelse=orelse):
                if orelse:
                    raise NotImplementedError("orelse in try statements not supported.")
                if_err = self.make_label()
                success = self.make_label()

                self.PUSH_BLOCK(if_err)
                self.visit_many(body)
                for each in body:
                    self.visit(each)
                self.POP_BLOCK()
                self.JUMP(success)
                self.LOAD_CONST(False)
                if not handlers:
                    assert final_body
                    self.add_label(if_err)
                    self.LOAD_CONST(None)
                    self.add_label(success)
                    for each in final_body:
                        self.visit(each)
                    self.RAISE(3)
                elif final_body:
                    final_label = success
                    re_err_jump_here = self.make_label()
                    success = self.make_label()
                    self.JUMP(success)
                    next_l = if_err

                    for handler in handlers:
                        match handler:
                            case ExceptHandler(type=e_type, name=name, body=body):
                                pass
                            case _:
                                raise
                        self.add_label(next_l)
                        
                        if not e_type:
                            self.PUSH_BLOCK(re_err_jump_here)
                            self.visit_many(body)
                            self.POP_BLOCK()
                            self.POP() # pop exc
                            self.LOAD_CONST(False)
                            self.JUMP(final_label)
                            break
                        else:
                            next_l = self.make_label()
                            self.PUSH_BLOCK(re_err_jump_here)
                            self.visit(e_type)
                            self.CHECK_EXC(next_l)
                            if name:
                                self.DUP()
                                self.STORE_LOCAL(name)
                            self.visit_many(body)
                            self.POP() # pop exc
                            self.POP_BLOCK()
                            if name:
                                self.DEL_LOCAL(name)
                            self.JUMP(final_label)
                    # the last "next_l": no exception matches
                    self.add_label(next_l)
                    self.RAISE(1)
                    self.add_label(re_err_jump_here)
                    # at here, there are 2 errors on the stack

                    self.add_label(final_label)
                    if final_body:
                        self.visit_many(final_body)
                        
                    
                    
                    


                


        
        
    
    
    _bin_ops = {
        'Add': BIN_OP.ADD, 'Sub': BIN_OP.SUB, 'Mult': BIN_OP.MUL,
        'Div': BIN_OP.TRUEDIV, 'Mod': BIN_OP.MOD, 'Pow': BIN_OP.POW,
        'LShift': BIN_OP.LSHIFT, 'RShift': BIN_OP.RSHIFT, 'BitOr': BIN_OP.OR,
        'BitXor': BIN_OP.XOR, 'BitAnd': BIN_OP.AND, 'FloorDiv': BIN_OP.FLOORDIV
    }
    def bin_op(self, op: operator):
        v = self._bin_ops.get(op.__class__.__name__)
        if v is None:
            raise NotImplementedError(op.__class__.__name__)
        self.BIN(v)
    
        
