
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
import struct

special_bit = 0b10000000
bool_bit = 0b10000010
none_bit = 0b10000000
from dataclasses import dataclass

obj_box_tags: dict[type, int| None] = {
    int: 0,
    float: 1,
    str: 2,
    dict: 3,
    set: 4,
    list: 5,
    tuple: 6,
    bool: None
}

@dataclass(frozen=True)
class Ptr:
    kind: int
    ind: int

@dataclass(frozen=True)
class DObj:
    o : object
    def serialize_(self, barr: bytearray):
        v = obj_box_tags[self.o.__class__]
        if v:
            barr.append(v)
        serialize_(self.o, barr)

def serialize_(o, barr: bytearray):
    match o:
        case int():
            barr.extend(struct.pack('<i', o))
        case float():
            barr.extend(struct.pack('<f', o))
        case str():
            barr.extend(bytes(o, 'utf-16'))
        case None:
            barr.append(none_bit | special_bit)
        case dict():
            for k, v in o.items():
                serialize_(k, barr)
                serialize_(v, barr)
        case list() | set() | tuple():
            for v in o:
                serialize_(v, barr)
        case _:
            o.serialize_(barr)


class InternString(str):
    pass

    

if __name__ == '__main__':
    b = bytearray()
    serialize_([1, 2, 3], b)
    print(len(b))
