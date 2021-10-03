from __future__ import annotations
from dataclasses import dataclass
from pyrsistent import PVector
from dianascript.serialize import DObj, Builder, InternString, serialize_
from typing import TypeVar, Generic
import struct

_T = TypeVar("_T")


Bytecode = PVector[int | InternString]
BytecodeBuilder = list[int | InternString]

@dataclass(frozen=True)
class Diana_FunctionDef:
    metadataInd: int
    TAG = 0
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.metadataInd))
@dataclass(frozen=True)
class Diana_LoadGlobalRef:
    istr: InternString
    TAG = 1
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.istr))
@dataclass(frozen=True)
class Diana_DelVar:
    target: int
    TAG = 2
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.target))
@dataclass(frozen=True)
class Diana_LoadVar:
    i: int
    TAG = 3
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.i))
@dataclass(frozen=True)
class Diana_StoreVar:
    i: int
    TAG = 4
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.i))
@dataclass(frozen=True)
class Diana_Action:
    kind: int
    TAG = 5
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.kind))
@dataclass(frozen=True)
class Diana_Return:
    TAG = 6
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_Break:
    TAG = 7
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_Continue:
    TAG = 8
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_JumpIfNot:
    off: int
    TAG = 9
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.off))
@dataclass(frozen=True)
class Diana_JumpIf:
    off: int
    TAG = 10
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.off))
@dataclass(frozen=True)
class Diana_Jump:
    off: int
    TAG = 11
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.off))
@dataclass(frozen=True)
class Diana_TryCatch:
    unwind_bound: int
    catch_start: int
    catch_bound: int
    TAG = 12
    
    OFFSET = 4
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.unwind_bound))
        flat_code.append(as_flatten(self.catch_start))
        flat_code.append(as_flatten(self.catch_bound))
@dataclass(frozen=True)
class Diana_TryFinally:
    unwind_bound: int
    final_start: int
    final_bound: int
    TAG = 13
    
    OFFSET = 4
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.unwind_bound))
        flat_code.append(as_flatten(self.final_start))
        flat_code.append(as_flatten(self.final_bound))
@dataclass(frozen=True)
class Diana_TryCatchFinally:
    unwind_bound: int
    catch_start: int
    catch_bound: int
    final_start: int
    final_bound: int
    TAG = 14
    
    OFFSET = 6
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.unwind_bound))
        flat_code.append(as_flatten(self.catch_start))
        flat_code.append(as_flatten(self.catch_bound))
        flat_code.append(as_flatten(self.final_start))
        flat_code.append(as_flatten(self.final_bound))
@dataclass(frozen=True)
class Diana_Loop:
    loop_bound: int
    TAG = 15
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.loop_bound))
@dataclass(frozen=True)
class Diana_For:
    for_bound: int
    TAG = 16
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.for_bound))
@dataclass(frozen=True)
class Diana_With:
    with_bound: int
    TAG = 17
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.with_bound))
@dataclass(frozen=True)
class Diana_GetAttr:
    attr: InternString
    TAG = 18
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.attr))
@dataclass(frozen=True)
class Diana_SetAttr:
    attr: InternString
    TAG = 19
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.attr))
@dataclass(frozen=True)
class Diana_SetAttr_Iadd:
    attr: InternString
    TAG = 20
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.attr))
@dataclass(frozen=True)
class Diana_SetAttr_Isub:
    attr: InternString
    TAG = 21
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.attr))
@dataclass(frozen=True)
class Diana_SetAttr_Imul:
    attr: InternString
    TAG = 22
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.attr))
@dataclass(frozen=True)
class Diana_SetAttr_Itruediv:
    attr: InternString
    TAG = 23
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.attr))
@dataclass(frozen=True)
class Diana_SetAttr_Ifloordiv:
    attr: InternString
    TAG = 24
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.attr))
@dataclass(frozen=True)
class Diana_SetAttr_Imod:
    attr: InternString
    TAG = 25
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.attr))
@dataclass(frozen=True)
class Diana_SetAttr_Ipow:
    attr: InternString
    TAG = 26
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.attr))
@dataclass(frozen=True)
class Diana_SetAttr_Ilshift:
    attr: InternString
    TAG = 27
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.attr))
@dataclass(frozen=True)
class Diana_SetAttr_Irshift:
    attr: InternString
    TAG = 28
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.attr))
@dataclass(frozen=True)
class Diana_SetAttr_Ibitor:
    attr: InternString
    TAG = 29
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.attr))
@dataclass(frozen=True)
class Diana_SetAttr_Ibitand:
    attr: InternString
    TAG = 30
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.attr))
@dataclass(frozen=True)
class Diana_SetAttr_Ibitxor:
    attr: InternString
    TAG = 31
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.attr))
@dataclass(frozen=True)
class Diana_DelItem:
    TAG = 32
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_GetItem:
    TAG = 33
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem:
    TAG = 34
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Iadd:
    TAG = 35
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Isub:
    TAG = 36
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Imul:
    TAG = 37
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Itruediv:
    TAG = 38
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Ifloordiv:
    TAG = 39
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Imod:
    TAG = 40
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Ipow:
    TAG = 41
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Ilshift:
    TAG = 42
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Irshift:
    TAG = 43
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Ibitor:
    TAG = 44
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Ibitand:
    TAG = 45
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Ibitxor:
    TAG = 46
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_add:
    TAG = 47
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_sub:
    TAG = 48
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_mul:
    TAG = 49
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_truediv:
    TAG = 50
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_floordiv:
    TAG = 51
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_mod:
    TAG = 52
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_pow:
    TAG = 53
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_lshift:
    TAG = 54
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_rshift:
    TAG = 55
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_bitor:
    TAG = 56
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_bitand:
    TAG = 57
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_bitxor:
    TAG = 58
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_gt:
    TAG = 59
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_lt:
    TAG = 60
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_ge:
    TAG = 61
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_le:
    TAG = 62
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_eq:
    TAG = 63
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_ne:
    TAG = 64
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_in:
    TAG = 65
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_notin:
    TAG = 66
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_UnaryOp_invert:
    TAG = 67
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_UnaryOp_not:
    TAG = 68
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_UnaryOp_neg:
    TAG = 69
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
@dataclass(frozen=True)
class Diana_MKDict:
    n: int
    TAG = 70
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.n))
@dataclass(frozen=True)
class Diana_MKSet:
    n: int
    TAG = 71
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.n))
@dataclass(frozen=True)
class Diana_MKList:
    n: int
    TAG = 72
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.n))
@dataclass(frozen=True)
class Diana_Call:
    n: int
    TAG = 73
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.n))
@dataclass(frozen=True)
class Diana_Format:
    format: int
    argn: int
    TAG = 74
    
    OFFSET = 3
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.format))
        flat_code.append(as_flatten(self.argn))
@dataclass(frozen=True)
class Diana_Const:
    p_const: int
    TAG = 75
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.p_const))
@dataclass(frozen=True)
class Diana_MKTuple:
    n: int
    TAG = 76
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.n))
@dataclass(frozen=True)
class Diana_Pack:
    n: int
    TAG = 77
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.n))
@dataclass(frozen=True)
class Diana_Replicate:
    n: int
    TAG = 78
    
    OFFSET = 2
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)
        flat_code.append(as_flatten(self.n))
@dataclass(frozen=True)
class Diana_Pop:
    TAG = 79
    
    OFFSET = 1
    
    def dumps(self, flat_code: BytecodeBuilder):
        flat_code.append(self.TAG)

@dataclass(frozen=True)
class FuncMeta:
    is_vararg: bool
    freeslots: PVector[int]
    nonargcells: PVector[int]
    narg: int
    nlocal: int
    name: InternString
    filename: str
    lineno: int
    linenos: PVector[tuple[int, int]]
    freenames: PVector[str]
    localnames: PVector[str]
    bytecode: Bytecode
    
    def serialize_(self, barr: bytearray):
        serialize_(self.is_vararg, barr)
        serialize_(self.freeslots, barr)
        serialize_(self.nonargcells, barr)
        serialize_(self.narg, barr)
        serialize_(self.nlocal, barr)
        serialize_(self.name, barr)
        serialize_(self.filename, barr)
        serialize_(self.lineno, barr)
        serialize_(self.linenos, barr)
        serialize_(self.freenames, barr)
        serialize_(self.localnames, barr)
        serialize_(self.bytecode, barr)

    def as_flatten(self) -> int:
        return Storage.funcmetas.cache(self)

def as_flatten(self):
    if isinstance(self, int):
        return self
    return self.as_flatten()

class Storage:
    strings : Builder[str] = Builder()
    internstrings : Builder[InternString] = Builder()
    dobjs : Builder[DObj] = Builder()
    funcmetas : Builder[FuncMeta] = Builder()

    @classmethod
    def serialize_(cls, barr: bytearray):
        cls.strings.serialize_(barr)
        cls.internstrings.serialize_(barr)
        cls.dobjs.serialize_(barr)
        cls.funcmetas.serialize_(barr)

class PlaceHolder:
    def __init__(self, OFFSET: int):
        self.OFFSET = OFFSET

Instr = (
    PlaceHolder
    | Diana_FunctionDef
    | Diana_LoadGlobalRef
    | Diana_DelVar
    | Diana_LoadVar
    | Diana_StoreVar
    | Diana_Action
    | Diana_Return
    | Diana_Break
    | Diana_Continue
    | Diana_JumpIfNot
    | Diana_JumpIf
    | Diana_Jump
    | Diana_TryCatch
    | Diana_TryFinally
    | Diana_TryCatchFinally
    | Diana_Loop
    | Diana_For
    | Diana_With
    | Diana_GetAttr
    | Diana_SetAttr
    | Diana_SetAttr_Iadd
    | Diana_SetAttr_Isub
    | Diana_SetAttr_Imul
    | Diana_SetAttr_Itruediv
    | Diana_SetAttr_Ifloordiv
    | Diana_SetAttr_Imod
    | Diana_SetAttr_Ipow
    | Diana_SetAttr_Ilshift
    | Diana_SetAttr_Irshift
    | Diana_SetAttr_Ibitor
    | Diana_SetAttr_Ibitand
    | Diana_SetAttr_Ibitxor
    | Diana_DelItem
    | Diana_GetItem
    | Diana_SetItem
    | Diana_SetItem_Iadd
    | Diana_SetItem_Isub
    | Diana_SetItem_Imul
    | Diana_SetItem_Itruediv
    | Diana_SetItem_Ifloordiv
    | Diana_SetItem_Imod
    | Diana_SetItem_Ipow
    | Diana_SetItem_Ilshift
    | Diana_SetItem_Irshift
    | Diana_SetItem_Ibitor
    | Diana_SetItem_Ibitand
    | Diana_SetItem_Ibitxor
    | Diana_add
    | Diana_sub
    | Diana_mul
    | Diana_truediv
    | Diana_floordiv
    | Diana_mod
    | Diana_pow
    | Diana_lshift
    | Diana_rshift
    | Diana_bitor
    | Diana_bitand
    | Diana_bitxor
    | Diana_gt
    | Diana_lt
    | Diana_ge
    | Diana_le
    | Diana_eq
    | Diana_ne
    | Diana_in
    | Diana_notin
    | Diana_UnaryOp_invert
    | Diana_UnaryOp_not
    | Diana_UnaryOp_neg
    | Diana_MKDict
    | Diana_MKSet
    | Diana_MKList
    | Diana_Call
    | Diana_Format
    | Diana_Const
    | Diana_MKTuple
    | Diana_Pack
    | Diana_Replicate
    | Diana_Pop
)