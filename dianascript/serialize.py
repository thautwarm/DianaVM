
"""
from "src/Parser.cs": for Object Encoding

    public static class ConstPoolTag
    {
        public const byte BoolTag = 0b10;
        public const byte SpecialTag = 0b01 << 7;

        public const byte Int = 0;
        public const byte Float = 1;
        public const byte Str = 2;


        public const byte Dict = 3;
        public const byte Set = 4;
        public const byte List = 5;
        public const byte Tuple = 6;

        public const byte Code = 7;
    }
"""
from __future__ import annotations
from dataclasses import dataclass
from pyrsistent import PVector
import struct
from typing import Generic, TypeVar

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

@dataclass(frozen=True)
class Ptr:
    kind: int
    ind: int | None
    def as_ptr(self) -> Ptr:
        return self
    
    def serialize_(self, barr: bytearray):
        assert self.kind < 256
        barr.append(self.kind)
        if self.ind is not None:
            serialize_(self.ind, barr)


def encode_to_7bit(value: int, barr: bytearray):
    data = []
    number = abs(value)
    while number >= 0x80:
        data.append((number | 0x80) & 0xff)
        number >>= 7
    barr.append(number & 0xff)

@dataclass(frozen=True)
class DObj:
    o : object
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

    def as_int(self) -> int:
        return DFlatGraphCode.dobjs.cache(self)

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
            for v in o:
                serialize_(v, barr)
        case tuple():
            for v in o:
                serialize_(v, barr)
        case None:
            barr.append(none_bit | special_bit)
        case _:
            o.serialize_(barr)


global DFlatGraphCode

class InternString(str):

    def as_int(self) -> int:
        global DFlatGraphCode
        try:
            DFlatGraphCode
        except NameError:
            from dianascript.code_cons import DFlatGraphCode
        
        return DFlatGraphCode.internstrings.cache(self)
        

def as_ptr(x):
    if isinstance(x, (int, str, Ptr)):
        raise TypeError
    return x.as_ptr()
        

_T = TypeVar("_T")

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

del _T, TypeVar

if __name__ == '__main__':
    b = bytearray()
    serialize_([1, 2, 3], b)
    print(len(b))
