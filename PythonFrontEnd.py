from __future__ import annotations
from ast import *
from dataclasses import dataclass, field
from enum import Enum
import struct
import contextlib
from typing import Mapping


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


class CODE:
    def POP(self, ): ...
    def RETURN(self, ): ...
    def ERR_CLEAR(self, ): ...
    def LOAD_ITEM(self, ): ...
    def STORE_ITEM(self, ): ...
    def DEL_ITEM(self, ): ...
    def INVERT(self,) : ...

    def JUMP(self, ): ...
    def JUMP_IF(self, ): ...
    def POP_BLOCK(self, ): ...

    def LOAD_ATTR(self, s: str): ...
    def STORE_ATTR(self, s: str): ...
    def LOAD_GLOBAL(self, s: str): ...
    def STORE_GLOBAL(self, s: str): ...
    def REF_GLOBAL(self, s: str): ...
    def LOAD_CELL(self, s: str): ...
    def LOAD_LOCAL(self, s: str): ...
    def STORE_CELL(self, s: str): ...
    def STORE_LOCAL(self, s: str): ...
    def REF_LOCAL(self, s: str): ...
    def LOAD_CONST(self, s: str): ...
    def PEEK(self, n: int): ...
    def CALL(self, n: int): ...
    def PUSH_BLOCK(self, target: int): ...
    def MAKE_FUNCTION(self, flag: int): ...

    def BIN(self, bin_op: BIN_OP): ...
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
class AIRGenerator(NodeVisitor):
    def __init__(self, filename: str):
        self.filename = filename
        self.code = self.new_code()
        self.lineno = 1
    
    def new_code(self):
        return DCode(self.filename, "", False, 0, 0, 0, [], [], [], bytearray())

    def MK_TUPLE(self, n: int):
        raise
    
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
            self.MK_TUPLE(new_code.nfree)


v = parse("""
a = 1
""")

