from __future__ import annotations
from dataclasses import dataclass
from pyrsistent import PVector
from dianascript.serialize import serialize_
from typing import TypeVar, Generic
import struct

_T = TypeVar("_T")

class InternString(str):
    pass
Bytecode = list

special_bit = 0b10000000
true_bit = 0b10000011
false_bit = 0b10000010
none_bit = 0b10000000

obj_box_tags: dict[type, int| None] = {
    int: 0,
    float: 1,
    str: 2,
    dict: 3,
    set: 4,
    list: 5,
    tuple: 6
}

special_objs = {
    (bool, True): true_bit,
    (bool, False): false_bit,
    (type(None), None): none_bit
}

booleans = (false_bit, true_bit)


class DObj:
    o: bool | int | float | str | None
    t: type | None = None
    def __init__(self, o):
        self.o = o
        self.type = type(o)

    def __hash__(self):
        return hash(self.o) ^ id(type)

    def __eq__(self, other):
        if not isinstance(other, DObj):
            return False
        return (self.o == other.o) and (self.type == other.type)
    
    def serialize_(self, barr: bytearray):
        o = self.o
        v = special_objs.get((type(o), o)) # type: ignore
        if v is not None:
            barr.append(v)
            return
        v = obj_box_tags[type(o)]
        if v is not None:
            barr.append(v)
        serialize_(self.o, barr)

def encode_to_7bit(value: int, barr: bytearray):
    data = []
    number = abs(value)
    while number >= 0x80:
        data.append((number | 0x80) & 0xff)
        number >>= 7
    barr.append(number & 0xff)

def serialize_(o, barr: bytearray):
    match o:
        case bool():
            barr.append(booleans[o])
        case int(): 
            barr.extend(struct.pack('<i', o))
        case float():
            barr.extend(struct.pack('<f', o))
        case str(): # or InternString
            encoded = bytes(o, 'utf8')
            encode_to_7bit(len(encoded), barr)
            barr.extend(encoded)
        case dict():
            serialize_(len(o), barr)
            for k, v in o.items():
                serialize_(k, barr)
                serialize_(v, barr)
        case list() | set() | PVector():    
            serialize_(len(o), barr)
            for v in o:  # type: ignore
                serialize_(v, barr)
        case tuple():
            for v in o:  # type: ignore
                serialize_(v, barr)
        case None:
            barr.append(none_bit | special_bit)
        case _:
            o.serialize_(barr)

class Builder(Generic[_T]):
    def __init__(self):
        self._map: dict[_T, int] = {}
        self._revmap: dict[int, _T] = {}
    
    def __getitem__(self, i: int) -> _T:
        return self._revmap[i]

    def __iter__(self):
        for i in range(len(self)):
            yield self._revmap[i]
    
    def __len__(self):
        return len(self._map)

    def cache(self, x: _T) -> int:
        o = object()
        i = self._map.get(x, o)
        if i is o:
            i = self._map[x] = len(self._map)
            self._revmap[i] = x
            return i
        else:
            assert isinstance(i, int)
            return i

    def serialize_(self, arr: bytearray):
        serialize_(len(self), arr)
        for i in range(len(self._revmap)):
            serialize_(self._revmap[i], arr)            

@dataclass(frozen=True)
class Diana_FunctionDef:
    metadataInd: int
    code: int

    TAG = 0

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.metadataInd), barr)
        serialize_(as_int(self.code), barr)
@dataclass(frozen=True)
class Diana_LoadGlobalRef:
    istr: InternString

    TAG = 1

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.istr), barr)
@dataclass(frozen=True)
class Diana_DelVar:
    target: int

    TAG = 2

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.target), barr)
@dataclass(frozen=True)
class Diana_LoadVar:
    i: int

    TAG = 3

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.i), barr)
@dataclass(frozen=True)
class Diana_StoreVar:
    i: int

    TAG = 4

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.i), barr)
@dataclass(frozen=True)
class Diana_Action:
    kind: int

    TAG = 5

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.kind), barr)
@dataclass(frozen=True)
class Diana_ControlIf:
    arg: int

    TAG = 6

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.arg), barr)
@dataclass(frozen=True)
class Diana_JumpIfNot:
    off: int

    TAG = 7

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.off), barr)
@dataclass(frozen=True)
class Diana_JumpIf:
    off: int

    TAG = 8

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.off), barr)
@dataclass(frozen=True)
class Diana_Jump:
    off: int

    TAG = 9

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.off), barr)
@dataclass(frozen=True)
class Diana_Control:
    arg: int

    TAG = 10

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.arg), barr)
@dataclass(frozen=True)
class Diana_Try:
    unwind_start: int
    unwind_stop: int
    errorlabel: int
    finallabel: int

    TAG = 11

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.unwind_start), barr)
        serialize_(as_int(self.unwind_stop), barr)
        serialize_(as_int(self.errorlabel), barr)
        serialize_(as_int(self.finallabel), barr)
@dataclass(frozen=True)
class Diana_Loop:
    body: int

    TAG = 12

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.body), barr)
@dataclass(frozen=True)
class Diana_For:
    body: int

    TAG = 13

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.body), barr)
@dataclass(frozen=True)
class Diana_With:
    body: int

    TAG = 14

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.body), barr)
@dataclass(frozen=True)
class Diana_GetAttr:
    attr: InternString

    TAG = 15

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.attr), barr)
@dataclass(frozen=True)
class Diana_SetAttr:
    attr: InternString

    TAG = 16

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.attr), barr)
@dataclass(frozen=True)
class Diana_SetAttr_Iadd:
    attr: InternString

    TAG = 17

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.attr), barr)
@dataclass(frozen=True)
class Diana_SetAttr_Isub:
    attr: InternString

    TAG = 18

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.attr), barr)
@dataclass(frozen=True)
class Diana_SetAttr_Imul:
    attr: InternString

    TAG = 19

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.attr), barr)
@dataclass(frozen=True)
class Diana_SetAttr_Itruediv:
    attr: InternString

    TAG = 20

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.attr), barr)
@dataclass(frozen=True)
class Diana_SetAttr_Ifloordiv:
    attr: InternString

    TAG = 21

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.attr), barr)
@dataclass(frozen=True)
class Diana_SetAttr_Imod:
    attr: InternString

    TAG = 22

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.attr), barr)
@dataclass(frozen=True)
class Diana_SetAttr_Ipow:
    attr: InternString

    TAG = 23

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.attr), barr)
@dataclass(frozen=True)
class Diana_SetAttr_Ilshift:
    attr: InternString

    TAG = 24

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.attr), barr)
@dataclass(frozen=True)
class Diana_SetAttr_Irshift:
    attr: InternString

    TAG = 25

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.attr), barr)
@dataclass(frozen=True)
class Diana_SetAttr_Ibitor:
    attr: InternString

    TAG = 26

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.attr), barr)
@dataclass(frozen=True)
class Diana_SetAttr_Ibitand:
    attr: InternString

    TAG = 27

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.attr), barr)
@dataclass(frozen=True)
class Diana_SetAttr_Ibitxor:
    attr: InternString

    TAG = 28

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.attr), barr)
@dataclass(frozen=True)
class Diana_DelItem:

    TAG = 29

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_GetItem:

    TAG = 30

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem:

    TAG = 31

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Iadd:

    TAG = 32

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Isub:

    TAG = 33

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Imul:

    TAG = 34

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Itruediv:

    TAG = 35

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Ifloordiv:

    TAG = 36

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Imod:

    TAG = 37

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Ipow:

    TAG = 38

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Ilshift:

    TAG = 39

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Irshift:

    TAG = 40

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Ibitor:

    TAG = 41

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Ibitand:

    TAG = 42

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_SetItem_Ibitxor:

    TAG = 43

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_add:

    TAG = 44

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_sub:

    TAG = 45

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_mul:

    TAG = 46

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_truediv:

    TAG = 47

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_floordiv:

    TAG = 48

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_mod:

    TAG = 49

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_pow:

    TAG = 50

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_lshift:

    TAG = 51

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_rshift:

    TAG = 52

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_bitor:

    TAG = 53

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_bitand:

    TAG = 54

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_bitxor:

    TAG = 55

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_gt:

    TAG = 56

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_lt:

    TAG = 57

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_ge:

    TAG = 58

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_le:

    TAG = 59

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_eq:

    TAG = 60

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_ne:

    TAG = 61

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_in:

    TAG = 62

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_notin:

    TAG = 63

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_UnaryOp_invert:

    TAG = 64

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_UnaryOp_not:

    TAG = 65

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_UnaryOp_neg:

    TAG = 66

    def serialize_(self, barr):
        barr.append(self.TAG)
@dataclass(frozen=True)
class Diana_MKDict:
    n: int

    TAG = 67

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.n), barr)
@dataclass(frozen=True)
class Diana_MKSet:
    n: int

    TAG = 68

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.n), barr)
@dataclass(frozen=True)
class Diana_MKList:
    n: int

    TAG = 69

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.n), barr)
@dataclass(frozen=True)
class Diana_Call:
    n: int

    TAG = 70

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.n), barr)
@dataclass(frozen=True)
class Diana_Format:
    format: int
    argn: int

    TAG = 71

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.format), barr)
        serialize_(as_int(self.argn), barr)
@dataclass(frozen=True)
class Diana_Const:
    p_const: int

    TAG = 72

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.p_const), barr)
@dataclass(frozen=True)
class Diana_MKTuple:
    n: int

    TAG = 73

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.n), barr)
@dataclass(frozen=True)
class Diana_Pack:
    n: int

    TAG = 74

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.n), barr)
@dataclass(frozen=True)
class Diana_Replicate:
    n: int

    TAG = 75

    def serialize_(self, barr):
        barr.append(self.TAG)
        serialize_(as_int(self.n), barr)
@dataclass(frozen=True)
class Diana_Pop:

    TAG = 76

    def serialize_(self, barr):
        barr.append(self.TAG)

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
    freenames: PVector[str]
    localnames: PVector[str]

    def as_int(self) -> int:
        is_vararg = as_int(self.is_vararg)
        freeslots = as_int(self.freeslots)
        nonargcells = as_int(self.nonargcells)
        narg = as_int(self.narg)
        nlocal = as_int(self.nlocal)
        name = as_int(self.name)
        filename = as_int(self.filename)
        lineno = as_int(self.lineno)
        freenames = as_int(self.freenames)
        localnames = as_int(self.localnames)
        return Storage.funcmetas.cache((is_vararg, freeslots, nonargcells, narg, nlocal, name, filename, lineno, freenames, localnames))    
@dataclass(frozen=True)
class Block:
    codes: PVector[Ptr]
    location_data: PVector[tuple[int, int]]
    filename: str

    def as_int(self) -> int:
        codes = as_int(self.codes)
        location_data = as_int(self.location_data)
        filename = as_int(self.filename)
        return Storage.blocks.cache((codes, location_data, filename))    

def as_int(self):
    if isinstance(self, int):
        return self
    if isinstance(self, string):
        return self
    if isinstance(self, DObj):
        return self
    if isinstance(self, InternString):
        return self
    return self.as_int()

class Storage:
    strings : Builder[string] = Builder()
    dobjs : Builder[DObj] = Builder()
    internstrings : Builder[InternString] = Builder()
    funcmetas : Builder[FuncMeta] = Builder()
    blocks : Builder[Block] = Builder()
    def serialize_(self, barr: bytearray):
        self.strings.serialize_(barr)
        self.dobjs.serialize_(barr)
        self.internstrings.serialize_(barr)
        self.funcmetas.serialize_(barr)
        self.blocks.serialize_(barr)
f.internstrings.serialize_(barr)
    
        self.dataclasss.serialize_(barr)
    
        self.dataclasss.serialize_(barr)
    