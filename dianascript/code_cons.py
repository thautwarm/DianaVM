from __future__ import annotations
from dataclasses import dataclass
from dianascript.serialize import *


@dataclass(frozen=True)
class Diana_FunctionDef:
    metadataInd: int
    code: int
    TAG = 0

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.metadataInd, arr)
        serialize_(self.code, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_functiondefs.cache(self))


@dataclass(frozen=True)
class Diana_LoadGlobalRef:
    istr: InternString
    TAG = 1

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.istr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_loadglobalrefs.cache(self))


@dataclass(frozen=True)
class Diana_DelVar:
    target: int
    TAG = 2

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_delvars.cache(self))


@dataclass(frozen=True)
class Diana_LoadVar:
    i: int
    TAG = 3

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.i, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_loadvars.cache(self))


@dataclass(frozen=True)
class Diana_StoreVar:
    i: int
    TAG = 4

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.i, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_storevars.cache(self))


@dataclass(frozen=True)
class Diana_Action:
    kind: int
    TAG = 5

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.kind, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_actions.cache(self))


@dataclass(frozen=True)
class Diana_ControlIf:
    arg: int
    TAG = 6

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.arg, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_controlifs.cache(self))


@dataclass(frozen=True)
class Diana_Control:
    arg: int
    TAG = 7

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.arg, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_controls.cache(self))


@dataclass(frozen=True)
class Diana_Try:
    body: int
    except_handlers: tuple[Catch, ...]
    final_body: int
    TAG = 8

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.body, arr)
        serialize_(self.except_handlers, arr)
        serialize_(self.final_body, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_trys.cache(self))


@dataclass(frozen=True)
class Diana_Loop:
    body: int
    TAG = 9

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.body, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_loops.cache(self))


@dataclass(frozen=True)
class Diana_For:
    body: int
    TAG = 10

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.body, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_fors.cache(self))


@dataclass(frozen=True)
class Diana_With:
    body: int
    TAG = 11

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.body, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_withs.cache(self))


@dataclass(frozen=True)
class Diana_GetAttr:
    attr: InternString
    TAG = 12

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_getattrs.cache(self))


@dataclass(frozen=True)
class Diana_SetAttr:
    attr: InternString
    TAG = 13

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattrs.cache(self))


@dataclass(frozen=True)
class Diana_SetAttrOp_add:
    attr: InternString
    TAG = 14

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattrop_adds.cache(self))


@dataclass(frozen=True)
class Diana_SetAttrOp_sub:
    attr: InternString
    TAG = 15

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattrop_subs.cache(self))


@dataclass(frozen=True)
class Diana_SetAttrOp_mul:
    attr: InternString
    TAG = 16

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattrop_muls.cache(self))


@dataclass(frozen=True)
class Diana_SetAttrOp_truediv:
    attr: InternString
    TAG = 17

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattrop_truedivs.cache(self))


@dataclass(frozen=True)
class Diana_SetAttrOp_floordiv:
    attr: InternString
    TAG = 18

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattrop_floordivs.cache(self))


@dataclass(frozen=True)
class Diana_SetAttrOp_mod:
    attr: InternString
    TAG = 19

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattrop_mods.cache(self))


@dataclass(frozen=True)
class Diana_SetAttrOp_pow:
    attr: InternString
    TAG = 20

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattrop_pows.cache(self))


@dataclass(frozen=True)
class Diana_SetAttrOp_lshift:
    attr: InternString
    TAG = 21

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattrop_lshifts.cache(self))


@dataclass(frozen=True)
class Diana_SetAttrOp_rshift:
    attr: InternString
    TAG = 22

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattrop_rshifts.cache(self))


@dataclass(frozen=True)
class Diana_SetAttrOp_bitor:
    attr: InternString
    TAG = 23

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattrop_bitors.cache(self))


@dataclass(frozen=True)
class Diana_SetAttrOp_bitand:
    attr: InternString
    TAG = 24

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattrop_bitands.cache(self))


@dataclass(frozen=True)
class Diana_SetAttrOp_bitxor:
    attr: InternString
    TAG = 25

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattrop_bitxors.cache(self))


@dataclass(frozen=True)
class Diana_DelItem:
    TAG = 26

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_GetItem:
    TAG = 27

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_SetItem:
    TAG = 28

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_SetItemOp_add:
    TAG = 29

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_SetItemOp_sub:
    TAG = 30

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_SetItemOp_mul:
    TAG = 31

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_SetItemOp_truediv:
    TAG = 32

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_SetItemOp_floordiv:
    TAG = 33

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_SetItemOp_mod:
    TAG = 34

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_SetItemOp_pow:
    TAG = 35

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_SetItemOp_lshift:
    TAG = 36

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_SetItemOp_rshift:
    TAG = 37

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_SetItemOp_bitor:
    TAG = 38

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_SetItemOp_bitand:
    TAG = 39

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_SetItemOp_bitxor:
    TAG = 40

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_BinaryOp_add:
    TAG = 41

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_BinaryOp_sub:
    TAG = 42

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_BinaryOp_mul:
    TAG = 43

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_BinaryOp_truediv:
    TAG = 44

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_BinaryOp_floordiv:
    TAG = 45

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_BinaryOp_mod:
    TAG = 46

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_BinaryOp_pow:
    TAG = 47

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_BinaryOp_lshift:
    TAG = 48

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_BinaryOp_rshift:
    TAG = 49

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_BinaryOp_bitor:
    TAG = 50

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_BinaryOp_bitand:
    TAG = 51

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_BinaryOp_bitxor:
    TAG = 52

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_UnaryOp_invert:
    TAG = 53

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_UnaryOp_not:
    TAG = 54

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, 0)


@dataclass(frozen=True)
class Diana_MKDict:
    n: int
    TAG = 55

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.n, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_mkdicts.cache(self))


@dataclass(frozen=True)
class Diana_MKSet:
    n: int
    TAG = 56

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.n, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_mksets.cache(self))


@dataclass(frozen=True)
class Diana_MKList:
    n: int
    TAG = 57

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.n, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_mklists.cache(self))


@dataclass(frozen=True)
class Diana_Call:
    n: int
    TAG = 58

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.n, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_calls.cache(self))


@dataclass(frozen=True)
class Diana_Format:
    format: int
    argn: int
    TAG = 59

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.format, arr)
        serialize_(self.argn, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_formats.cache(self))


@dataclass(frozen=True)
class Diana_Const:
    p_const: int
    TAG = 60

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.p_const, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_consts.cache(self))


@dataclass(frozen=True)
class Diana_MKTuple:
    n: int
    TAG = 61

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.n, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_mktuples.cache(self))


@dataclass(frozen=True)
class Diana_Pack:
    n: int
    TAG = 62

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.n, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_packs.cache(self))


@dataclass(frozen=True)
class Catch:
    exc_type: int
    body: int
    TAG = 0

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.exc_type, arr)
        serialize_(self.body, arr)

    def as_int(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.catchs.cache(self))


@dataclass(frozen=True)
class FuncMeta:
    is_vararg: bool
    freeslots: tuple[int, ...]
    nonargcells: tuple[int, ...]
    narg: int
    nlocal: int
    name: InternString
    filename: str
    lineno: int
    freenames: tuple[str, ...]
    localnames: tuple[str, ...]
    TAG = 1

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.is_vararg, arr)
        serialize_(self.freeslots, arr)
        serialize_(self.nonargcells, arr)
        serialize_(self.narg, arr)
        serialize_(self.nlocal, arr)
        serialize_(self.name, arr)
        serialize_(self.filename, arr)
        serialize_(self.lineno, arr)
        serialize_(self.freenames, arr)
        serialize_(self.localnames, arr)

    def as_int(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.funcmetas.cache(self))


@dataclass(frozen=True)
class Block:
    codes: tuple[Ptr, ...]
    location_data: tuple[tuple[int, int], ...]
    filename: str
    TAG = 2

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.codes, arr)
        serialize_(self.location_data, arr)
        serialize_(self.filename, arr)

    def as_int(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.blocks.cache(self))


class DFlatGraphCode:
    strings : Builder[str] = Builder()
    dobjs : Builder[DObj] = Builder()
    internstrings : Builder[InternString] = Builder()
    catchs : Builder[Catch] = Builder()
    funcmetas : Builder[FuncMeta] = Builder()
    blocks : Builder[Block] = Builder()
    diana_functiondefs : Builder[Diana_FunctionDef] = Builder()
    diana_loadglobalrefs : Builder[Diana_LoadGlobalRef] = Builder()
    diana_delvars : Builder[Diana_DelVar] = Builder()
    diana_loadvars : Builder[Diana_LoadVar] = Builder()
    diana_storevars : Builder[Diana_StoreVar] = Builder()
    diana_actions : Builder[Diana_Action] = Builder()
    diana_controlifs : Builder[Diana_ControlIf] = Builder()
    diana_controls : Builder[Diana_Control] = Builder()
    diana_trys : Builder[Diana_Try] = Builder()
    diana_loops : Builder[Diana_Loop] = Builder()
    diana_fors : Builder[Diana_For] = Builder()
    diana_withs : Builder[Diana_With] = Builder()
    diana_getattrs : Builder[Diana_GetAttr] = Builder()
    diana_setattrs : Builder[Diana_SetAttr] = Builder()
    diana_setattrop_adds : Builder[Diana_SetAttrOp_add] = Builder()
    diana_setattrop_subs : Builder[Diana_SetAttrOp_sub] = Builder()
    diana_setattrop_muls : Builder[Diana_SetAttrOp_mul] = Builder()
    diana_setattrop_truedivs : Builder[Diana_SetAttrOp_truediv] = Builder()
    diana_setattrop_floordivs : Builder[Diana_SetAttrOp_floordiv] = Builder()
    diana_setattrop_mods : Builder[Diana_SetAttrOp_mod] = Builder()
    diana_setattrop_pows : Builder[Diana_SetAttrOp_pow] = Builder()
    diana_setattrop_lshifts : Builder[Diana_SetAttrOp_lshift] = Builder()
    diana_setattrop_rshifts : Builder[Diana_SetAttrOp_rshift] = Builder()
    diana_setattrop_bitors : Builder[Diana_SetAttrOp_bitor] = Builder()
    diana_setattrop_bitands : Builder[Diana_SetAttrOp_bitand] = Builder()
    diana_setattrop_bitxors : Builder[Diana_SetAttrOp_bitxor] = Builder()
    diana_mkdicts : Builder[Diana_MKDict] = Builder()
    diana_mksets : Builder[Diana_MKSet] = Builder()
    diana_mklists : Builder[Diana_MKList] = Builder()
    diana_calls : Builder[Diana_Call] = Builder()
    diana_formats : Builder[Diana_Format] = Builder()
    diana_consts : Builder[Diana_Const] = Builder()
    diana_mktuples : Builder[Diana_MKTuple] = Builder()
    diana_packs : Builder[Diana_Pack] = Builder()

    @classmethod
    def serialize_(cls, arr: bytearray):
        serialize_(cls.strings, arr)
        serialize_(cls.dobjs, arr)
        serialize_(cls.internstrings, arr)
        serialize_(cls.catchs, arr)
        serialize_(cls.funcmetas, arr)
        serialize_(cls.blocks, arr)
        serialize_(cls.diana_functiondefs, arr)
        serialize_(cls.diana_loadglobalrefs, arr)
        serialize_(cls.diana_delvars, arr)
        serialize_(cls.diana_loadvars, arr)
        serialize_(cls.diana_storevars, arr)
        serialize_(cls.diana_actions, arr)
        serialize_(cls.diana_controlifs, arr)
        serialize_(cls.diana_controls, arr)
        serialize_(cls.diana_trys, arr)
        serialize_(cls.diana_loops, arr)
        serialize_(cls.diana_fors, arr)
        serialize_(cls.diana_withs, arr)
        serialize_(cls.diana_getattrs, arr)
        serialize_(cls.diana_setattrs, arr)
        serialize_(cls.diana_setattrop_adds, arr)
        serialize_(cls.diana_setattrop_subs, arr)
        serialize_(cls.diana_setattrop_muls, arr)
        serialize_(cls.diana_setattrop_truedivs, arr)
        serialize_(cls.diana_setattrop_floordivs, arr)
        serialize_(cls.diana_setattrop_mods, arr)
        serialize_(cls.diana_setattrop_pows, arr)
        serialize_(cls.diana_setattrop_lshifts, arr)
        serialize_(cls.diana_setattrop_rshifts, arr)
        serialize_(cls.diana_setattrop_bitors, arr)
        serialize_(cls.diana_setattrop_bitands, arr)
        serialize_(cls.diana_setattrop_bitxors, arr)
        serialize_(cls.diana_mkdicts, arr)
        serialize_(cls.diana_mksets, arr)
        serialize_(cls.diana_mklists, arr)
        serialize_(cls.diana_calls, arr)
        serialize_(cls.diana_formats, arr)
        serialize_(cls.diana_consts, arr)
        serialize_(cls.diana_mktuples, arr)
        serialize_(cls.diana_packs, arr)

DianaIR = Catch | FuncMeta | Block | Diana_FunctionDef | Diana_LoadGlobalRef | Diana_DelVar | Diana_LoadVar | Diana_StoreVar | Diana_Action | Diana_ControlIf | Diana_Control | Diana_Try | Diana_Loop | Diana_For | Diana_With | Diana_GetAttr | Diana_SetAttr | Diana_SetAttrOp_add | Diana_SetAttrOp_sub | Diana_SetAttrOp_mul | Diana_SetAttrOp_truediv | Diana_SetAttrOp_floordiv | Diana_SetAttrOp_mod | Diana_SetAttrOp_pow | Diana_SetAttrOp_lshift | Diana_SetAttrOp_rshift | Diana_SetAttrOp_bitor | Diana_SetAttrOp_bitand | Diana_SetAttrOp_bitxor | Diana_MKDict | Diana_MKSet | Diana_MKList | Diana_Call | Diana_Format | Diana_Const | Diana_MKTuple | Diana_Pack
