from __future__ import annotations
from dataclasses import dataclass
from dianascript.serialize import *


@dataclass(frozen=True)
class string:
    TAG : int = 0

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> int:
        return DFlatGraphCode.strings.cache(self)


@dataclass(frozen=True)
class DObj:
    TAG : int = 1

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> int:
        return DFlatGraphCode.dobjs.cache(self)


@dataclass(frozen=True)
class InternString:
    TAG : int = 2

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> int:
        return DFlatGraphCode.internstrings.cache(self)


@dataclass(frozen=True)
class Catch:
    exc_target: int
    exc_type: int
    body: Block
    TAG : int = 3

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.exc_target, arr)
        serialize_(self.exc_type, arr)
        serialize_(self.body, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.catchs.cache(self)


@dataclass(frozen=True)
class FuncMeta:
    is_vararg: bool
    freeslots: tuple[int, ...]
    narg: int
    nlocal: int
    name: InternString
    modname: InternString
    filename: str
    TAG : int = 4

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.is_vararg, arr)
        serialize_(self.freeslots, arr)
        serialize_(self.narg, arr)
        serialize_(self.nlocal, arr)
        serialize_(self.name, arr)
        serialize_(self.modname, arr)
        serialize_(self.filename, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.funcmetas.cache(self)


@dataclass(frozen=True)
class Loc:
    location_data: tuple[tuple[int, int], ...]
    TAG : int = 5

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.location_data, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.locs.cache(self)


@dataclass(frozen=True)
class Block:
    codes: tuple[Ptr, ...]
    location_data: int
    TAG : int = 6

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.codes, arr)
        serialize_(self.location_data, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.blocks.cache(self)


@dataclass(frozen=True)
class Diana_FunctionDef:
    target: int
    metadataInd: int
    code: Block
    TAG : int = 7

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.metadataInd, arr)
        serialize_(self.code, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_functiondefs.cache(self)


@dataclass(frozen=True)
class Diana_Return:
    reg: int
    TAG : int = 8

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.reg, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_returns.cache(self)


@dataclass(frozen=True)
class Diana_DelVar:
    target: int
    TAG : int = 9

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_delvars.cache(self)


@dataclass(frozen=True)
class Diana_SetVar:
    target: int
    s_val: int
    TAG : int = 10

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.s_val, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_setvars.cache(self)


@dataclass(frozen=True)
class Diana_JumpIf:
    s_val: int
    offset: int
    TAG : int = 11

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.s_val, arr)
        serialize_(self.offset, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_jumpifs.cache(self)


@dataclass(frozen=True)
class Diana_Jump:
    offset: int
    TAG : int = 12

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.offset, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_jumps.cache(self)


@dataclass(frozen=True)
class Diana_Raise:
    s_exc: int
    TAG : int = 13

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.s_exc, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_raises.cache(self)


@dataclass(frozen=True)
class Diana_Assert:
    value: int
    s_msg: int
    TAG : int = 14

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.value, arr)
        serialize_(self.s_msg, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_asserts.cache(self)


@dataclass(frozen=True)
class Diana_Control:
    token: int
    TAG : int = 15

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.token, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_controls.cache(self)


@dataclass(frozen=True)
class Diana_Try:
    body: Block
    except_handlers: tuple[Catch, ...]
    final_body: Block
    TAG : int = 16

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.body, arr)
        serialize_(self.except_handlers, arr)
        serialize_(self.final_body, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_trys.cache(self)


@dataclass(frozen=True)
class Diana_For:
    target: int
    s_iter: int
    body: Block
    TAG : int = 17

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.s_iter, arr)
        serialize_(self.body, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_fors.cache(self)


@dataclass(frozen=True)
class Diana_With:
    s_resource: int
    s_as: int
    body: Block
    TAG : int = 18

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.s_resource, arr)
        serialize_(self.s_as, arr)
        serialize_(self.body, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_withs.cache(self)


@dataclass(frozen=True)
class Diana_DelItem:
    s_value: int
    s_item: int
    TAG : int = 19

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.s_value, arr)
        serialize_(self.s_item, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_delitems.cache(self)


@dataclass(frozen=True)
class Diana_GetItem:
    target_and_value: int
    s_item: int
    TAG : int = 20

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_value, arr)
        serialize_(self.s_item, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_getitems.cache(self)


@dataclass(frozen=True)
class Diana_BinaryOp_add:
    target_and_left: int
    right: int
    TAG : int = 21

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_binaryop_adds.cache(self)


@dataclass(frozen=True)
class Diana_BinaryOp_sub:
    target_and_left: int
    right: int
    TAG : int = 22

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_binaryop_subs.cache(self)


@dataclass(frozen=True)
class Diana_BinaryOp_mul:
    target_and_left: int
    right: int
    TAG : int = 23

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_binaryop_muls.cache(self)


@dataclass(frozen=True)
class Diana_BinaryOp_truediv:
    target_and_left: int
    right: int
    TAG : int = 24

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_binaryop_truedivs.cache(self)


@dataclass(frozen=True)
class Diana_BinaryOp_floordiv:
    target_and_left: int
    right: int
    TAG : int = 25

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_binaryop_floordivs.cache(self)


@dataclass(frozen=True)
class Diana_BinaryOp_mod:
    target_and_left: int
    right: int
    TAG : int = 26

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_binaryop_mods.cache(self)


@dataclass(frozen=True)
class Diana_BinaryOp_pow:
    target_and_left: int
    right: int
    TAG : int = 27

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_binaryop_pows.cache(self)


@dataclass(frozen=True)
class Diana_BinaryOp_lshift:
    target_and_left: int
    right: int
    TAG : int = 28

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_binaryop_lshifts.cache(self)


@dataclass(frozen=True)
class Diana_BinaryOp_rshift:
    target_and_left: int
    right: int
    TAG : int = 29

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_binaryop_rshifts.cache(self)


@dataclass(frozen=True)
class Diana_BinaryOp_bitor:
    target_and_left: int
    right: int
    TAG : int = 30

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_binaryop_bitors.cache(self)


@dataclass(frozen=True)
class Diana_BinaryOp_bitand:
    target_and_left: int
    right: int
    TAG : int = 31

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_binaryop_bitands.cache(self)


@dataclass(frozen=True)
class Diana_BinaryOp_bitxor:
    target_and_left: int
    right: int
    TAG : int = 32

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_left, arr)
        serialize_(self.right, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_binaryop_bitxors.cache(self)


@dataclass(frozen=True)
class Diana_UnaryOp_invert:
    target_and_value: int
    TAG : int = 33

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_value, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_unaryop_inverts.cache(self)


@dataclass(frozen=True)
class Diana_UnaryOp_not:
    target_and_value: int
    TAG : int = 34

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_value, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_unaryop_nots.cache(self)


@dataclass(frozen=True)
class Diana_Dict:
    target: int
    s_kvs: tuple[tuple[int, int], ...]
    TAG : int = 35

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.s_kvs, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_dicts.cache(self)


@dataclass(frozen=True)
class Diana_Set:
    target: int
    s_elts: tuple[int, ...]
    TAG : int = 36

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.s_elts, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_sets.cache(self)


@dataclass(frozen=True)
class Diana_List:
    target: int
    s_elts: tuple[int, ...]
    TAG : int = 37

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.s_elts, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_lists.cache(self)


@dataclass(frozen=True)
class Diana_Generator:
    target_and_func: int
    TAG : int = 38

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_func, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_generators.cache(self)


@dataclass(frozen=True)
class Diana_Call:
    target: int
    s_f: int
    s_args: tuple[int, ...]
    TAG : int = 39

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.s_f, arr)
        serialize_(self.s_args, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_calls.cache(self)


@dataclass(frozen=True)
class Diana_Format:
    target: int
    format: int
    args: tuple[int, ...]
    TAG : int = 40

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.format, arr)
        serialize_(self.args, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_formats.cache(self)


@dataclass(frozen=True)
class Diana_Const:
    target: int
    p_const: int
    TAG : int = 41

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.p_const, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_consts.cache(self)


@dataclass(frozen=True)
class Diana_GetAttr:
    target_and_value: int
    p_attr: int
    TAG : int = 42

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target_and_value, arr)
        serialize_(self.p_attr, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_getattrs.cache(self)


@dataclass(frozen=True)
class Diana_MoveVar:
    target: int
    slot: int
    TAG : int = 43

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.slot, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_movevars.cache(self)


@dataclass(frozen=True)
class Diana_Tuple:
    target: int
    s_elts: tuple[int, ...]
    TAG : int = 44

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.s_elts, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_tuples.cache(self)


@dataclass(frozen=True)
class Diana_PackTuple:
    targets: tuple[int, ...]
    s_value: int
    TAG : int = 45

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.targets, arr)
        serialize_(self.s_value, arr)

    def as_ptr(self) -> int:
        return DFlatGraphCode.diana_packtuples.cache(self)


class DFlatGraphCode:
    strings : Builder[str] = Builder()
    dobjs : Builder[DObj] = Builder()
    internstrings : Builder[InternString] = Builder()
    catchs : Builder[Catch] = Builder()
    funcmetas : Builder[FuncMeta] = Builder()
    locs : Builder[Loc] = Builder()
    blocks : Builder[Block] = Builder()
    diana_functiondefs : Builder[Diana_FunctionDef] = Builder()
    diana_returns : Builder[Diana_Return] = Builder()
    diana_delvars : Builder[Diana_DelVar] = Builder()
    diana_setvars : Builder[Diana_SetVar] = Builder()
    diana_jumpifs : Builder[Diana_JumpIf] = Builder()
    diana_jumps : Builder[Diana_Jump] = Builder()
    diana_raises : Builder[Diana_Raise] = Builder()
    diana_asserts : Builder[Diana_Assert] = Builder()
    diana_controls : Builder[Diana_Control] = Builder()
    diana_trys : Builder[Diana_Try] = Builder()
    diana_fors : Builder[Diana_For] = Builder()
    diana_withs : Builder[Diana_With] = Builder()
    diana_delitems : Builder[Diana_DelItem] = Builder()
    diana_getitems : Builder[Diana_GetItem] = Builder()
    diana_binaryop_adds : Builder[Diana_BinaryOp_add] = Builder()
    diana_binaryop_subs : Builder[Diana_BinaryOp_sub] = Builder()
    diana_binaryop_muls : Builder[Diana_BinaryOp_mul] = Builder()
    diana_binaryop_truedivs : Builder[Diana_BinaryOp_truediv] = Builder()
    diana_binaryop_floordivs : Builder[Diana_BinaryOp_floordiv] = Builder()
    diana_binaryop_mods : Builder[Diana_BinaryOp_mod] = Builder()
    diana_binaryop_pows : Builder[Diana_BinaryOp_pow] = Builder()
    diana_binaryop_lshifts : Builder[Diana_BinaryOp_lshift] = Builder()
    diana_binaryop_rshifts : Builder[Diana_BinaryOp_rshift] = Builder()
    diana_binaryop_bitors : Builder[Diana_BinaryOp_bitor] = Builder()
    diana_binaryop_bitands : Builder[Diana_BinaryOp_bitand] = Builder()
    diana_binaryop_bitxors : Builder[Diana_BinaryOp_bitxor] = Builder()
    diana_unaryop_inverts : Builder[Diana_UnaryOp_invert] = Builder()
    diana_unaryop_nots : Builder[Diana_UnaryOp_not] = Builder()
    diana_dicts : Builder[Diana_Dict] = Builder()
    diana_sets : Builder[Diana_Set] = Builder()
    diana_lists : Builder[Diana_List] = Builder()
    diana_generators : Builder[Diana_Generator] = Builder()
    diana_calls : Builder[Diana_Call] = Builder()
    diana_formats : Builder[Diana_Format] = Builder()
    diana_consts : Builder[Diana_Const] = Builder()
    diana_getattrs : Builder[Diana_GetAttr] = Builder()
    diana_movevars : Builder[Diana_MoveVar] = Builder()
    diana_tuples : Builder[Diana_Tuple] = Builder()
    diana_packtuples : Builder[Diana_PackTuple] = Builder()

    @classmethod
    def serialize_(cls, arr: bytearray):
        serialize_(cls.strings, arr)
        serialize_(cls.dobjs, arr)
        serialize_(cls.internstrings, arr)
        serialize_(cls.catchs, arr)
        serialize_(cls.funcmetas, arr)
        serialize_(cls.locs, arr)
        serialize_(cls.blocks, arr)
        serialize_(cls.diana_functiondefs, arr)
        serialize_(cls.diana_returns, arr)
        serialize_(cls.diana_delvars, arr)
        serialize_(cls.diana_setvars, arr)
        serialize_(cls.diana_jumpifs, arr)
        serialize_(cls.diana_jumps, arr)
        serialize_(cls.diana_raises, arr)
        serialize_(cls.diana_asserts, arr)
        serialize_(cls.diana_controls, arr)
        serialize_(cls.diana_trys, arr)
        serialize_(cls.diana_fors, arr)
        serialize_(cls.diana_withs, arr)
        serialize_(cls.diana_delitems, arr)
        serialize_(cls.diana_getitems, arr)
        serialize_(cls.diana_binaryop_adds, arr)
        serialize_(cls.diana_binaryop_subs, arr)
        serialize_(cls.diana_binaryop_muls, arr)
        serialize_(cls.diana_binaryop_truedivs, arr)
        serialize_(cls.diana_binaryop_floordivs, arr)
        serialize_(cls.diana_binaryop_mods, arr)
        serialize_(cls.diana_binaryop_pows, arr)
        serialize_(cls.diana_binaryop_lshifts, arr)
        serialize_(cls.diana_binaryop_rshifts, arr)
        serialize_(cls.diana_binaryop_bitors, arr)
        serialize_(cls.diana_binaryop_bitands, arr)
        serialize_(cls.diana_binaryop_bitxors, arr)
        serialize_(cls.diana_unaryop_inverts, arr)
        serialize_(cls.diana_unaryop_nots, arr)
        serialize_(cls.diana_dicts, arr)
        serialize_(cls.diana_sets, arr)
        serialize_(cls.diana_lists, arr)
        serialize_(cls.diana_generators, arr)
        serialize_(cls.diana_calls, arr)
        serialize_(cls.diana_formats, arr)
        serialize_(cls.diana_consts, arr)
        serialize_(cls.diana_getattrs, arr)
        serialize_(cls.diana_movevars, arr)
        serialize_(cls.diana_tuples, arr)
        serialize_(cls.diana_packtuples, arr)
