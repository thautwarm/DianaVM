from _typeshed import SupportsDivMod
import enum
from dianascript.ScopeAnalyzer import SymTable, get_symtable
from ScopeAnalyzer import AnalyzedSymTable
from __future__ import annotations
from ast import *
from dataclasses import dataclass, field
from enum import Enum
from dianascript.code_cons import *
# from dianascript.ScopeAnalyzer import get_tag
from typing import Any, Callable, Generator, overload
import struct
import contextlib




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


BIT_NONLOCAL_BIT = 0b01
BIT_CLASSIFY_BIT = 0b01 << 1 # local -> cell; free -> global

RETURN = 0
GO_AHEAD = 1
LOOP_BREAK = 2
LOOP_CONTINUE = 3


class VarKind(Enum):
    Local = 0
    Cell = BIT_CLASSIFY_BIT
    Free = BIT_NONLOCAL_BIT
    Global = BIT_NONLOCAL_BIT | BIT_CLASSIFY_BIT

    def __ror__(self, t: int) -> int:
        return (t << 2) | self.value            

class AIRGenerator(NodeVisitor):
    def __init__(self, filename: str, symtable: SymTable):
        self.filename = filename
        self.lineno = 1
        self.linenos: list[tuple[int, int]] = [(1, 1)]
        self.symtable = symtable
        self.codeblock: list[Ptr] = []
        self.vstack: list[tuple[str, bool]]  = []
        self.freevar_to_slot: dict[str, int] = {}
        self.localvar_to_slot: dict[str, int] = {}
        self.var_kind_cache: dict[str, VarKind] = {}

    def _get_kind_of_name(self, name):
        if kind := self.var_kind_cache.get(name):
            return kind
        i = self.localvar_to_slot.get(name, None)
        if i is not None:
            if name in self.symtable.analyzed.cellvars:
                kind = VarKind.Cell
            else:
                kind = VarKind.Local
        else:
            if name in self.freevar_to_slot:
                kind = VarKind.Free
            else:
                kind = VarKind.Global
        self.var_kind_cache[name] = kind
        return kind
    
    @overload
    def visit(self, node: expr) -> Callable[[str | None], None]: ...
    @overload
    def visit(self, node: stmt) -> None: ...
    def visit(self, node):
        return NodeVisitor.visit(self, node)
    
    
    def decl_localvar(self, name: str) -> int:
        assert name in self.symtable.analyzed.bounds
        if name in self.localvar_to_slot:
            return self.localvar_to_slot[name]
        i = len(self.vstack)
        self.vstack.append((name, True))
        self.localvar_to_slot[name] = i
        return i
        
    
    def alloc_(self, lhs=None) -> str:
        match lhs: 
            case Name(id=id):
                pass
            case _:
                id = None
        if id is not None:
            return id
        for i in range(len(self.vstack)):
            id, is_used = self.vstack[i]
            if is_used:
                continue
            self.vstack[i] = id, True
        else:
            i  = len(self.vstack)
            id = f".{i}"
            self.vstack.append((id, True))
            self.localvar_to_slot[id] = i
        return id
    
    def loadlocal(self, n):
        for i, (n_, is_used) in enumerate(self.vstack):
            if n == n_:
                return i
        raise NameError(n)
    
    def loadstore_slot(self, n: str):
        kind = self._get_kind_of_name(n)
        match kind:
            case VarKind.Local | VarKind.Cell:
                i = self.localvar_to_slot[n]
            case VarKind.Free:
                i = self.freevar_to_slot[n]
            case VarKind.Global:
                i = DFlatGraphCode.internstrings.cache(InternString(n))
            case _:
                raise
        return i | kind.value
    
    def dealloc_(self, n: str):
        if n.startswith('.'):
            i = self.localvar_to_slot[n]
            n_, used = self.vstack[i]
            assert n_ == n, f"allocation for {n_} and {n} error."
            assert not used, f"{n_} is not used, cannot dealloc."
            self.vstack[i] = n, False

    @contextlib.contextmanager
    def with_alloc(self, lhs=None):
        match lhs: 
            case Name(id=id):
                pass
            case _:
                id = None
        if id is not None:
            try:
                yield id
            finally:
                return
        
        for i in range(len(self.vstack)):
            id, is_used = self.vstack[i]
            if is_used:
                continue
            self.vstack[i] = id, True
        else:
            i  = len(self.vstack)
            id = f".{i}"
            self.vstack.append((id, True))
            self.localvar_to_slot[id] = i
        try:
            yield id
        finally:
            self.vstack[i] = id, False
    
    def __lshift__(self, ir: DianaIR) -> Ptr:
        i = ir.as_ptr()
        ptr = Ptr(ir.TAG, i)
        self.codeblock.append(ptr)
        return ptr
    
    @contextlib.contextmanager
    def enter_block(self):
        old_code = self.codeblock
        old_linenos = self.linenos 
        old_lineno = self.lineno
        try:
            yield
        finally:
            self.codeblock = old_code
            self.linenos = old_linenos
            if self.linenos[-1][0] > old_lineno:
                self.linenos.append((self.linenos[-1][0], self.lineno))
            
    
    def visit_Module(self, node: Module):   
        for e in node.body:
            self.visit(e)
    
    def visit_FunctionDef(self, node: FunctionDef):
        o_lineno = self.lineno

        decorator_regs = []
        for each in node.decorator_list:
            f = self.visit(each)
            reg = self.alloc_()
            f(reg)
            decorator_regs.append(reg)

        subgen = AIRGenerator(self.filename, get_symtable(node))
        
        is_vararg = False
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
        narg = len(node.args.args)
            
        if node.args.vararg:
            is_vararg = True
            args = [*node.args.args, node.args.vararg]
        else:
            args = node.args.args
        
        subgen.symtable.analyzed.freevars
        
        argnames = tuple(arg.arg for arg in args)
        for argname in argnames:
            i = subgen.decl_localvar(argname)
            if argname in subgen.symtable.analyzed.cellvars:
                subgen << Diana_LoadAsCell(i | VarKind.Local)
        for bound in sorted(subgen.symtable.analyzed.bounds):
            subgen.decl_localvar(bound)
        
        free_slots = []
        for index_of_free, free in enumerate(sorted(subgen.symtable.analyzed.freevars)):
            if free in self.symtable.analyzed.cellvars:
                i =  self.localvar_to_slot[free]
                free_slots.append(i | VarKind.Local)
            else:
                assert free in self.symtable.analyzed.freevars
                i = self.freevar_to_slot[free]
                free_slots.append(i | VarKind.Free)
            subgen.freevar_to_slot[free] = index_of_free
        for stmt in node.body:
            subgen.visit(stmt)
        
        block = Block(codes=tuple(subgen.codeblock), location_data=tuple(subgen.linenos), filename=self.filename)
        
        pairs = list(self.localvar_to_slot.items())
        pairs.sort(key=lambda x: x[1])
        localnames = tuple(x[0] for x in pairs)
        pairs = list(self.freevar_to_slot.items())
        pairs.sort(key=lambda x: x[1])
        freenames = tuple(x[0] for x in pairs)
        
        funcmeta = FuncMeta(
            is_vararg=is_vararg, freeslots=tuple(free_slots), narg=narg, name=InternString(node.name), localnames=localnames, freenames=freenames,
            lineno=node.lineno or o_lineno, nlocal = len(subgen.vstack), filename=subgen.filename
        )
        self << Diana_FunctionDef(self.loadstore_slot(node.name), funcmeta.as_ptr(), block.as_ptr())
        

        p_func = self.loadstore_slot(node.name)
        for each in reversed(decorator_regs):
            self << Diana_Call(p_func, each, (p_func, ))
            self.dealloc_(each)
    
    def visit_ClassDef(self, node: ClassDef) -> Any:
        raise NotImplementedError
    
    def visit_AsyncFunctionDef(self, node: AsyncFunctionDef) -> Any:
        raise NotImplementedError

    def visit_Return(self, node: Return) -> Any:
        if not node.value:
            self << Diana_Control(RETURN)
            return
        with self.with_alloc() as s:
            f_call = self.visit(node.value)
            f_call(s)
            self << Diana_Return(self.loadstore_slot(s))
    
    def visit_Delete(self, node: Delete) -> Any:
        for each in node.targets:
            self.visit(each)(None)
        
    def visit_Assign(self, node: Assign) -> Any:
        ref = next((t.id for t in node.targets if isinstance(t, Name) and t.id in self.localvar_to_slot) , None)
        if not ref:
            ref = self.alloc_()
        for each in node.targets:
            self.visit(each)(ref)
        self.dealloc_(ref)
    
    def visit_AugAssign(self, node: AugAssign) -> Any:
        cons = BinOp_map[node.op.__class__]
        if isinstance(node.target, Name):
            ref = node.target.id
        else:
            ref = self.alloc_()
        self.visit(node.target)(ref)
    



BinOp_map = {
    Add: Diana_BinaryOp_add,
    Sub: Diana_BinaryOp_sub,
    Mult: Diana_BinaryOp_mul,
    Div: Diana_BinaryOp_truediv,
    FloorDiv: Diana_BinaryOp_floordiv,
    Mod: Diana_BinaryOp_mod,
    Pow: Diana_BinaryOp_pow,
    LShift: Diana_BinaryOp_lshift,
    RShift: Diana_BinaryOp_rshift,
    BitOr: Diana_BinaryOp_bitor,
    BitAnd: Diana_BinaryOp_bitand,
    BitXor: Diana_BinaryOp_bitxor
}
    
# AIRGenerator = auto_set_loc(AIRGenerator)