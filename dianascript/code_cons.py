from __future__ import annotations
from dataclasses import dataclass
from dianascript.serialize import *


@dataclass(frozen=True)
class Catch:
    exc_target: int
    exc_type: int
    body: Block
    TAG : int = 0

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.exc_target, arr)
        serialize_(self.exc_type, arr)
        serialize_(self.body, arr)


@dataclass(frozen=True)
class FuncMeta:
    is_vararg: bool
    narg: int
    freeslots: list[int]
    name: InternString
    modname: InternString
    filename: str
    TAG : int = 1

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.is_vararg, arr)
        serialize_(self.narg, arr)
        serialize_(self.freeslots, arr)
        serialize_(self.name, arr)
        serialize_(self.modname, arr)
        serialize_(self.filename, arr)


@dataclass(frozen=True)
class Loc:
    location_data: list[tuple[int, int]]
    TAG : int = 2

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.location_data, arr)


@dataclass(frozen=True)
class Block:
    codes: list[Ptr]
    location_data: int
    TAG : int = 3

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.codes, arr)
        serialize_(self.location_data, arr)


@dataclass(frozen=True)
class Diana_FunctionDef:
    target: int
    metadataInd: int
    code: Block
    TAG : int = 4

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.metadataInd, arr)
        serialize_(self.code, arr)


@dataclass(frozen=True)
class Diana_Return:
    reg: int
    TAG : int = 5

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.reg, arr)


@dataclass(frozen=True)
class Diana_DelVar:
    target: int
    TAG : int = 6

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)


@dataclass(frozen=True)
class Diana_SetVar:
    target: int
    s_val: int
    TAG : int = 7

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.s_val, arr)


@dataclass(frozen=True)
class Diana_JumpIf:
    s_val: int
    offset: int
    TAG : int = 8

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.s_val, arr)
        serialize_(self.offset, arr)


@dataclass(frozen=True)
class Diana_Jump:
    offset: int
    TAG : int = 9

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.offset, arr)


@dataclass(frozen=True)
class Diana_Raise:
    s_exc: int
    TAG : int = 10

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.s_exc, arr)


@dataclass(frozen=True)
class Diana_Assert:
    value: int
    msg: int
    TAG : int = 11

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.value, arr)
        serialize_(self.msg, arr)


@dataclass(frozen=True)
class Diana_Control:
    token: int
    TAG : int = 12

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.token, arr)


@dataclass(frozen=True)
class Diana_Try:
    body: Block
    except_handlers: list[Catch]
    final_body: Block
    TAG : int = 13

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.body, arr)
        serialize_(self.except_handlers, arr)
        serialize_(self.final_body, arr)


@dataclass(frozen=True)
class Diana_For:
    target: int
    s_iter: int
    body: Block
    TAG : int = 14

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.s_iter, arr)
        serialize_(self.body, arr)


@dataclass(frozen=True)
class Diana_With:
    s_resource: int
    s_as: int
    body: Block
    TAG : int = 15

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.s_resource, arr)
        serialize_(self.s_as, arr)
        serialize_(self.body, arr)


@dataclass(frozen=True)
class Diana_DelItem:
    s_value: int
    s_item: int
    TAG : int = 16

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.s_value, arr)
        serialize_(self.s_item, arr)


@dataclass(frozen=True)
class Diana_GetItem:
    target_and_value: int
    s_item: int
    TAG : int = 17

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_value, arr)
        serialize_(self.s_item, arr)


@dataclass(frozen=True)
class Diana_BinaryOp_add:
    target_and_left: int
    right: int
    TAG : int = 18

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Diana_BinaryOp_sub:
    target_and_left: int
    right: int
    TAG : int = 19

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Diana_BinaryOp_mul:
    target_and_left: int
    right: int
    TAG : int = 20

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Diana_BinaryOp_truediv:
    target_and_left: int
    right: int
    TAG : int = 21

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Diana_BinaryOp_floordiv:
    target_and_left: int
    right: int
    TAG : int = 22

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Diana_BinaryOp_mod:
    target_and_left: int
    right: int
    TAG : int = 23

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Diana_BinaryOp_pow:
    target_and_left: int
    right: int
    TAG : int = 24

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Diana_BinaryOp_lshift:
    target_and_left: int
    right: int
    TAG : int = 25

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Diana_BinaryOp_rshift:
    target_and_left: int
    right: int
    TAG : int = 26

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Diana_BinaryOp_bitor:
    target_and_left: int
    right: int
    TAG : int = 27

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Diana_BinaryOp_bitand:
    target_and_left: int
    right: int
    TAG : int = 28

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Diana_BinaryOp_bitxor:
    target_and_left: int
    right: int
    TAG : int = 29

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Diana_UnaryOp_invert:
    target_and_value: int
    TAG : int = 30

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_value, arr)


@dataclass(frozen=True)
class Diana_UnaryOp_not:
    target_and_value: int
    TAG : int = 31

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_value, arr)


@dataclass(frozen=True)
class Diana_Dict:
    target: int
    s_kvs: list[tuple[int, int]]
    TAG : int = 32

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.s_kvs, arr)


@dataclass(frozen=True)
class Diana_Set:
    target: int
    s_elts: list[int]
    TAG : int = 33

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.s_elts, arr)


@dataclass(frozen=True)
class Diana_List:
    target: int
    s_elts: list[int]
    TAG : int = 34

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.s_elts, arr)


@dataclass(frozen=True)
class Diana_Generator:
    target_and_func: int
    TAG : int = 35

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_func, arr)


@dataclass(frozen=True)
class Diana_Call:
    target: int
    s_f: int
    s_args: list[int]
    TAG : int = 36

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.s_f, arr)
        serialize_(self.s_args, arr)


@dataclass(frozen=True)
class Diana_Format:
    target: int
    format: int
    args: list[int]
    TAG : int = 37

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.format, arr)
        serialize_(self.args, arr)


@dataclass(frozen=True)
class Diana_Const:
    target: int
    p_const: int
    TAG : int = 38

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.p_const, arr)


@dataclass(frozen=True)
class Diana_GetAttr:
    target_and_value: int
    p_attr: int
    TAG : int = 39

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_value, arr)
        serialize_(self.p_attr, arr)


@dataclass(frozen=True)
class Diana_MoveVar:
    target: int
    slot: int
    TAG : int = 40

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.slot, arr)


@dataclass(frozen=True)
class Diana_Tuple:
    target: int
    s_elts: list[int]
    TAG : int = 41

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.s_elts, arr)


@dataclass(frozen=True)
class Diana_PackTuple:
    targets: list[int]
    s_value: int
    TAG : int = 42

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.targets, arr)
        serialize_(self.s_value, arr)


