from __future__ import annotations
from dataclasses import dataclass
from pyrsistent  import PVector
from dianascript.serialize import *


@dataclass(frozen=True)
class Diana_FunctionDef:
    metadataInd: int
    code: int
    TAG = 0

    def serialize_(self, arr: bytearray):
        serialize_(self.metadataInd, arr)
        serialize_(self.code, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_functiondefs.cache(self))


@dataclass(frozen=True)
class Diana_LoadGlobalRef:
    istr: InternString
    TAG = 1

    def serialize_(self, arr: bytearray):
        serialize_(self.istr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_loadglobalrefs.cache(self))


@dataclass(frozen=True)
class Diana_DelVar:
    targets: PVector[int]
    TAG = 2

    def serialize_(self, arr: bytearray):
        serialize_(self.targets, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_delvars.cache(self))


@dataclass(frozen=True)
class Diana_LoadVar:
    i: int
    TAG = 3

    def serialize_(self, arr: bytearray):
        serialize_(self.i, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_loadvars.cache(self))


@dataclass(frozen=True)
class Diana_StoreVar:
    i: int
    TAG = 4

    def serialize_(self, arr: bytearray):
        serialize_(self.i, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_storevars.cache(self))


@dataclass(frozen=True)
class Diana_Action:
    kind: int
    TAG = 5

    def serialize_(self, arr: bytearray):
        serialize_(self.kind, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_actions.cache(self))


@dataclass(frozen=True)
class Diana_ControlIf:
    arg: int
    TAG = 6

    def serialize_(self, arr: bytearray):
        serialize_(self.arg, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_controlifs.cache(self))


@dataclass(frozen=True)
class Diana_JumpIfNot:
    off: int
    TAG = 7

    def serialize_(self, arr: bytearray):
        serialize_(self.off, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_jumpifnots.cache(self))


@dataclass(frozen=True)
class Diana_JumpIf:
    off: int
    TAG = 8

    def serialize_(self, arr: bytearray):
        serialize_(self.off, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_jumpifs.cache(self))


@dataclass(frozen=True)
class Diana_Jump:
    off: int
    TAG = 9

    def serialize_(self, arr: bytearray):
        serialize_(self.off, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_jumps.cache(self))


@dataclass(frozen=True)
class Diana_Control:
    arg: int
    TAG = 10

    def serialize_(self, arr: bytearray):
        serialize_(self.arg, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_controls.cache(self))


@dataclass(frozen=True)
class Diana_Try:
    body: int
    except_handlers: PVector[Catch]
    final_body: int
    TAG = 11

    def serialize_(self, arr: bytearray):
        serialize_(self.body, arr)
        serialize_(self.except_handlers, arr)
        serialize_(self.final_body, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_trys.cache(self))


@dataclass(frozen=True)
class Diana_Loop:
    body: int
    TAG = 12

    def serialize_(self, arr: bytearray):
        serialize_(self.body, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_loops.cache(self))


@dataclass(frozen=True)
class Diana_For:
    body: int
    TAG = 13

    def serialize_(self, arr: bytearray):
        serialize_(self.body, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_fors.cache(self))


@dataclass(frozen=True)
class Diana_With:
    body: int
    TAG = 14

    def serialize_(self, arr: bytearray):
        serialize_(self.body, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_withs.cache(self))


@dataclass(frozen=True)
class Diana_GetAttr:
    attr: InternString
    TAG = 15

    def serialize_(self, arr: bytearray):
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_getattrs.cache(self))


@dataclass(frozen=True)
class Diana_SetAttr:
    attr: InternString
    TAG = 16

    def serialize_(self, arr: bytearray):
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattrs.cache(self))


@dataclass(frozen=True)
class Diana_SetAttr_Iadd:
    attr: InternString
    TAG = 17

    def serialize_(self, arr: bytearray):
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattr_iadds.cache(self))


@dataclass(frozen=True)
class Diana_SetAttr_Isub:
    attr: InternString
    TAG = 18

    def serialize_(self, arr: bytearray):
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattr_isubs.cache(self))


@dataclass(frozen=True)
class Diana_SetAttr_Imul:
    attr: InternString
    TAG = 19

    def serialize_(self, arr: bytearray):
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattr_imuls.cache(self))


@dataclass(frozen=True)
class Diana_SetAttr_Itruediv:
    attr: InternString
    TAG = 20

    def serialize_(self, arr: bytearray):
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattr_itruedivs.cache(self))


@dataclass(frozen=True)
class Diana_SetAttr_Ifloordiv:
    attr: InternString
    TAG = 21

    def serialize_(self, arr: bytearray):
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattr_ifloordivs.cache(self))


@dataclass(frozen=True)
class Diana_SetAttr_Imod:
    attr: InternString
    TAG = 22

    def serialize_(self, arr: bytearray):
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattr_imods.cache(self))


@dataclass(frozen=True)
class Diana_SetAttr_Ipow:
    attr: InternString
    TAG = 23

    def serialize_(self, arr: bytearray):
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattr_ipows.cache(self))


@dataclass(frozen=True)
class Diana_SetAttr_Ilshift:
    attr: InternString
    TAG = 24

    def serialize_(self, arr: bytearray):
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattr_ilshifts.cache(self))


@dataclass(frozen=True)
class Diana_SetAttr_Irshift:
    attr: InternString
    TAG = 25

    def serialize_(self, arr: bytearray):
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattr_irshifts.cache(self))


@dataclass(frozen=True)
class Diana_SetAttr_Ibitor:
    attr: InternString
    TAG = 26

    def serialize_(self, arr: bytearray):
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattr_ibitors.cache(self))


@dataclass(frozen=True)
class Diana_SetAttr_Ibitand:
    attr: InternString
    TAG = 27

    def serialize_(self, arr: bytearray):
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattr_ibitands.cache(self))


@dataclass(frozen=True)
class Diana_SetAttr_Ibitxor:
    attr: InternString
    TAG = 28

    def serialize_(self, arr: bytearray):
        serialize_(self.attr, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_setattr_ibitxors.cache(self))


@dataclass(frozen=True)
class Diana_DelItem:
    TAG = 29

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_GetItem:
    TAG = 30

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_SetItem:
    TAG = 31

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_SetItem_Iadd:
    TAG = 32

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_SetItem_Isub:
    TAG = 33

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_SetItem_Imul:
    TAG = 34

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_SetItem_Itruediv:
    TAG = 35

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_SetItem_Ifloordiv:
    TAG = 36

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_SetItem_Imod:
    TAG = 37

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_SetItem_Ipow:
    TAG = 38

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_SetItem_Ilshift:
    TAG = 39

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_SetItem_Irshift:
    TAG = 40

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_SetItem_Ibitor:
    TAG = 41

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_SetItem_Ibitand:
    TAG = 42

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_SetItem_Ibitxor:
    TAG = 43

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_add:
    TAG = 44

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_sub:
    TAG = 45

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_mul:
    TAG = 46

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_truediv:
    TAG = 47

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_floordiv:
    TAG = 48

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_mod:
    TAG = 49

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_pow:
    TAG = 50

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_lshift:
    TAG = 51

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_rshift:
    TAG = 52

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_bitor:
    TAG = 53

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_bitand:
    TAG = 54

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_bitxor:
    TAG = 55

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_gt:
    TAG = 56

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_lt:
    TAG = 57

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_ge:
    TAG = 58

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_le:
    TAG = 59

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_eq:
    TAG = 60

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_ne:
    TAG = 61

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_in:
    TAG = 62

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_notin:
    TAG = 63

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_UnaryOp_invert:
    TAG = 64

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_UnaryOp_not:
    TAG = 65

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_UnaryOp_neg:
    TAG = 66

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Diana_MKDict:
    n: int
    TAG = 67

    def serialize_(self, arr: bytearray):
        serialize_(self.n, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_mkdicts.cache(self))


@dataclass(frozen=True)
class Diana_MKSet:
    n: int
    TAG = 68

    def serialize_(self, arr: bytearray):
        serialize_(self.n, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_mksets.cache(self))


@dataclass(frozen=True)
class Diana_MKList:
    n: int
    TAG = 69

    def serialize_(self, arr: bytearray):
        serialize_(self.n, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_mklists.cache(self))


@dataclass(frozen=True)
class Diana_Call:
    n: int
    TAG = 70

    def serialize_(self, arr: bytearray):
        serialize_(self.n, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_calls.cache(self))


@dataclass(frozen=True)
class Diana_Format:
    format: int
    argn: int
    TAG = 71

    def serialize_(self, arr: bytearray):
        serialize_(self.format, arr)
        serialize_(self.argn, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_formats.cache(self))


@dataclass(frozen=True)
class Diana_Const:
    p_const: int
    TAG = 72

    def serialize_(self, arr: bytearray):
        serialize_(self.p_const, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_consts.cache(self))


@dataclass(frozen=True)
class Diana_MKTuple:
    n: int
    TAG = 73

    def serialize_(self, arr: bytearray):
        serialize_(self.n, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_mktuples.cache(self))


@dataclass(frozen=True)
class Diana_Pack:
    n: int
    TAG = 74

    def serialize_(self, arr: bytearray):
        serialize_(self.n, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_packs.cache(self))


@dataclass(frozen=True)
class Diana_Replicate:
    n: int
    TAG = 75

    def serialize_(self, arr: bytearray):
        serialize_(self.n, arr)

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, DFlatGraphCode.diana_replicates.cache(self))


@dataclass(frozen=True)
class Diana_Pop:
    TAG = 76

    def as_ptr(self) -> Ptr:
        return Ptr(self.TAG, None)


@dataclass(frozen=True)
class Catch:
    exc_type: int
    body: int

    def serialize_(self, arr: bytearray):
        serialize_(self.exc_type, arr)
        serialize_(self.body, arr)

    def as_int(self) -> int:
        return DFlatGraphCode.catchs.cache(self)


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

    def serialize_(self, arr: bytearray):
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

    def as_int(self) -> int:
        return DFlatGraphCode.funcmetas.cache(self)


@dataclass(frozen=True)
class Block:
    codes: PVector[Ptr]
    location_data: PVector[tuple[int, int]]
    filename: str

    def serialize_(self, arr: bytearray):
        serialize_(self.codes, arr)
        serialize_(self.location_data, arr)
        serialize_(self.filename, arr)

    def as_int(self) -> int:
        return DFlatGraphCode.blocks.cache(self)


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
    diana_jumpifnots : Builder[Diana_JumpIfNot] = Builder()
    diana_jumpifs : Builder[Diana_JumpIf] = Builder()
    diana_jumps : Builder[Diana_Jump] = Builder()
    diana_controls : Builder[Diana_Control] = Builder()
    diana_trys : Builder[Diana_Try] = Builder()
    diana_loops : Builder[Diana_Loop] = Builder()
    diana_fors : Builder[Diana_For] = Builder()
    diana_withs : Builder[Diana_With] = Builder()
    diana_getattrs : Builder[Diana_GetAttr] = Builder()
    diana_setattrs : Builder[Diana_SetAttr] = Builder()
    diana_setattr_iadds : Builder[Diana_SetAttr_Iadd] = Builder()
    diana_setattr_isubs : Builder[Diana_SetAttr_Isub] = Builder()
    diana_setattr_imuls : Builder[Diana_SetAttr_Imul] = Builder()
    diana_setattr_itruedivs : Builder[Diana_SetAttr_Itruediv] = Builder()
    diana_setattr_ifloordivs : Builder[Diana_SetAttr_Ifloordiv] = Builder()
    diana_setattr_imods : Builder[Diana_SetAttr_Imod] = Builder()
    diana_setattr_ipows : Builder[Diana_SetAttr_Ipow] = Builder()
    diana_setattr_ilshifts : Builder[Diana_SetAttr_Ilshift] = Builder()
    diana_setattr_irshifts : Builder[Diana_SetAttr_Irshift] = Builder()
    diana_setattr_ibitors : Builder[Diana_SetAttr_Ibitor] = Builder()
    diana_setattr_ibitands : Builder[Diana_SetAttr_Ibitand] = Builder()
    diana_setattr_ibitxors : Builder[Diana_SetAttr_Ibitxor] = Builder()
    diana_mkdicts : Builder[Diana_MKDict] = Builder()
    diana_mksets : Builder[Diana_MKSet] = Builder()
    diana_mklists : Builder[Diana_MKList] = Builder()
    diana_calls : Builder[Diana_Call] = Builder()
    diana_formats : Builder[Diana_Format] = Builder()
    diana_consts : Builder[Diana_Const] = Builder()
    diana_mktuples : Builder[Diana_MKTuple] = Builder()
    diana_packs : Builder[Diana_Pack] = Builder()
    diana_replicates : Builder[Diana_Replicate] = Builder()

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
        serialize_(cls.diana_jumpifnots, arr)
        serialize_(cls.diana_jumpifs, arr)
        serialize_(cls.diana_jumps, arr)
        serialize_(cls.diana_controls, arr)
        serialize_(cls.diana_trys, arr)
        serialize_(cls.diana_loops, arr)
        serialize_(cls.diana_fors, arr)
        serialize_(cls.diana_withs, arr)
        serialize_(cls.diana_getattrs, arr)
        serialize_(cls.diana_setattrs, arr)
        serialize_(cls.diana_setattr_iadds, arr)
        serialize_(cls.diana_setattr_isubs, arr)
        serialize_(cls.diana_setattr_imuls, arr)
        serialize_(cls.diana_setattr_itruedivs, arr)
        serialize_(cls.diana_setattr_ifloordivs, arr)
        serialize_(cls.diana_setattr_imods, arr)
        serialize_(cls.diana_setattr_ipows, arr)
        serialize_(cls.diana_setattr_ilshifts, arr)
        serialize_(cls.diana_setattr_irshifts, arr)
        serialize_(cls.diana_setattr_ibitors, arr)
        serialize_(cls.diana_setattr_ibitands, arr)
        serialize_(cls.diana_setattr_ibitxors, arr)
        serialize_(cls.diana_mkdicts, arr)
        serialize_(cls.diana_mksets, arr)
        serialize_(cls.diana_mklists, arr)
        serialize_(cls.diana_calls, arr)
        serialize_(cls.diana_formats, arr)
        serialize_(cls.diana_consts, arr)
        serialize_(cls.diana_mktuples, arr)
        serialize_(cls.diana_packs, arr)
        serialize_(cls.diana_replicates, arr)
    inspect = {
        Diana_FunctionDef.TAG : (Diana_FunctionDef, diana_functiondefs),
        Diana_LoadGlobalRef.TAG : (Diana_LoadGlobalRef, diana_loadglobalrefs),
        Diana_DelVar.TAG : (Diana_DelVar, diana_delvars),
        Diana_LoadVar.TAG : (Diana_LoadVar, diana_loadvars),
        Diana_StoreVar.TAG : (Diana_StoreVar, diana_storevars),
        Diana_Action.TAG : (Diana_Action, diana_actions),
        Diana_ControlIf.TAG : (Diana_ControlIf, diana_controlifs),
        Diana_JumpIfNot.TAG : (Diana_JumpIfNot, diana_jumpifnots),
        Diana_JumpIf.TAG : (Diana_JumpIf, diana_jumpifs),
        Diana_Jump.TAG : (Diana_Jump, diana_jumps),
        Diana_Control.TAG : (Diana_Control, diana_controls),
        Diana_Try.TAG : (Diana_Try, diana_trys),
        Diana_Loop.TAG : (Diana_Loop, diana_loops),
        Diana_For.TAG : (Diana_For, diana_fors),
        Diana_With.TAG : (Diana_With, diana_withs),
        Diana_GetAttr.TAG : (Diana_GetAttr, diana_getattrs),
        Diana_SetAttr.TAG : (Diana_SetAttr, diana_setattrs),
        Diana_SetAttr_Iadd.TAG : (Diana_SetAttr_Iadd, diana_setattr_iadds),
        Diana_SetAttr_Isub.TAG : (Diana_SetAttr_Isub, diana_setattr_isubs),
        Diana_SetAttr_Imul.TAG : (Diana_SetAttr_Imul, diana_setattr_imuls),
        Diana_SetAttr_Itruediv.TAG : (Diana_SetAttr_Itruediv, diana_setattr_itruedivs),
        Diana_SetAttr_Ifloordiv.TAG : (Diana_SetAttr_Ifloordiv, diana_setattr_ifloordivs),
        Diana_SetAttr_Imod.TAG : (Diana_SetAttr_Imod, diana_setattr_imods),
        Diana_SetAttr_Ipow.TAG : (Diana_SetAttr_Ipow, diana_setattr_ipows),
        Diana_SetAttr_Ilshift.TAG : (Diana_SetAttr_Ilshift, diana_setattr_ilshifts),
        Diana_SetAttr_Irshift.TAG : (Diana_SetAttr_Irshift, diana_setattr_irshifts),
        Diana_SetAttr_Ibitor.TAG : (Diana_SetAttr_Ibitor, diana_setattr_ibitors),
        Diana_SetAttr_Ibitand.TAG : (Diana_SetAttr_Ibitand, diana_setattr_ibitands),
        Diana_SetAttr_Ibitxor.TAG : (Diana_SetAttr_Ibitxor, diana_setattr_ibitxors),
        Diana_DelItem.TAG : (Diana_DelItem, None),
        Diana_GetItem.TAG : (Diana_GetItem, None),
        Diana_SetItem.TAG : (Diana_SetItem, None),
        Diana_SetItem_Iadd.TAG : (Diana_SetItem_Iadd, None),
        Diana_SetItem_Isub.TAG : (Diana_SetItem_Isub, None),
        Diana_SetItem_Imul.TAG : (Diana_SetItem_Imul, None),
        Diana_SetItem_Itruediv.TAG : (Diana_SetItem_Itruediv, None),
        Diana_SetItem_Ifloordiv.TAG : (Diana_SetItem_Ifloordiv, None),
        Diana_SetItem_Imod.TAG : (Diana_SetItem_Imod, None),
        Diana_SetItem_Ipow.TAG : (Diana_SetItem_Ipow, None),
        Diana_SetItem_Ilshift.TAG : (Diana_SetItem_Ilshift, None),
        Diana_SetItem_Irshift.TAG : (Diana_SetItem_Irshift, None),
        Diana_SetItem_Ibitor.TAG : (Diana_SetItem_Ibitor, None),
        Diana_SetItem_Ibitand.TAG : (Diana_SetItem_Ibitand, None),
        Diana_SetItem_Ibitxor.TAG : (Diana_SetItem_Ibitxor, None),
        Diana_add.TAG : (Diana_add, None),
        Diana_sub.TAG : (Diana_sub, None),
        Diana_mul.TAG : (Diana_mul, None),
        Diana_truediv.TAG : (Diana_truediv, None),
        Diana_floordiv.TAG : (Diana_floordiv, None),
        Diana_mod.TAG : (Diana_mod, None),
        Diana_pow.TAG : (Diana_pow, None),
        Diana_lshift.TAG : (Diana_lshift, None),
        Diana_rshift.TAG : (Diana_rshift, None),
        Diana_bitor.TAG : (Diana_bitor, None),
        Diana_bitand.TAG : (Diana_bitand, None),
        Diana_bitxor.TAG : (Diana_bitxor, None),
        Diana_gt.TAG : (Diana_gt, None),
        Diana_lt.TAG : (Diana_lt, None),
        Diana_ge.TAG : (Diana_ge, None),
        Diana_le.TAG : (Diana_le, None),
        Diana_eq.TAG : (Diana_eq, None),
        Diana_ne.TAG : (Diana_ne, None),
        Diana_in.TAG : (Diana_in, None),
        Diana_notin.TAG : (Diana_notin, None),
        Diana_UnaryOp_invert.TAG : (Diana_UnaryOp_invert, None),
        Diana_UnaryOp_not.TAG : (Diana_UnaryOp_not, None),
        Diana_UnaryOp_neg.TAG : (Diana_UnaryOp_neg, None),
        Diana_MKDict.TAG : (Diana_MKDict, diana_mkdicts),
        Diana_MKSet.TAG : (Diana_MKSet, diana_mksets),
        Diana_MKList.TAG : (Diana_MKList, diana_mklists),
        Diana_Call.TAG : (Diana_Call, diana_calls),
        Diana_Format.TAG : (Diana_Format, diana_formats),
        Diana_Const.TAG : (Diana_Const, diana_consts),
        Diana_MKTuple.TAG : (Diana_MKTuple, diana_mktuples),
        Diana_Pack.TAG : (Diana_Pack, diana_packs),
        Diana_Replicate.TAG : (Diana_Replicate, diana_replicates),
        Diana_Pop.TAG : (Diana_Pop, None),
    }

DianaIR = Catch | FuncMeta | Block | Diana_FunctionDef | Diana_LoadGlobalRef | Diana_DelVar | Diana_LoadVar | Diana_StoreVar | Diana_Action | Diana_ControlIf | Diana_JumpIfNot | Diana_JumpIf | Diana_Jump | Diana_Control | Diana_Try | Diana_Loop | Diana_For | Diana_With | Diana_GetAttr | Diana_SetAttr | Diana_SetAttr_Iadd | Diana_SetAttr_Isub | Diana_SetAttr_Imul | Diana_SetAttr_Itruediv | Diana_SetAttr_Ifloordiv | Diana_SetAttr_Imod | Diana_SetAttr_Ipow | Diana_SetAttr_Ilshift | Diana_SetAttr_Irshift | Diana_SetAttr_Ibitor | Diana_SetAttr_Ibitand | Diana_SetAttr_Ibitxor | Diana_MKDict | Diana_MKSet | Diana_MKList | Diana_Call | Diana_Format | Diana_Const | Diana_MKTuple | Diana_Pack | Diana_Replicate
