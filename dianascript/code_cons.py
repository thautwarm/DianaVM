from __future__ import annotations
from dataclasses import dataclass
from dianascript.serialize import Ptr, serialize_


@dataclass(frozen=True)
class Stmt_FunctionDef:
    metadataInd: int
    code: Ptr
    TAG : int = 0

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.metadataInd, arr)
        serialize_(self.code, arr)


@dataclass(frozen=True)
class Stmt_Return:
    value: Ptr
    TAG : int = 1

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.value, arr)


@dataclass(frozen=True)
class Stmt_DelGlobalName:
    slot: int
    TAG : int = 2

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.slot, arr)


@dataclass(frozen=True)
class Stmt_DelLocalName:
    slot: int
    TAG : int = 3

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.slot, arr)


@dataclass(frozen=True)
class Stmt_DelDerefName:
    slot: int
    TAG : int = 4

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.slot, arr)


@dataclass(frozen=True)
class Stmt_DeleteItem:
    value: Ptr
    item: Ptr
    TAG : int = 5

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.value, arr)
        serialize_(self.item, arr)


@dataclass(frozen=True)
class Stmt_Assign:
    targets: Ptr
    value: Ptr
    TAG : int = 6

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.targets, arr)
        serialize_(self.value, arr)


@dataclass(frozen=True)
class Stmt_AddAssign:
    target: Ptr
    value: Ptr
    TAG : int = 7

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.value, arr)


@dataclass(frozen=True)
class Stmt_SubAssign:
    target: Ptr
    value: Ptr
    TAG : int = 8

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.value, arr)


@dataclass(frozen=True)
class Stmt_MutAssign:
    target: Ptr
    value: Ptr
    TAG : int = 9

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.value, arr)


@dataclass(frozen=True)
class Stmt_TrueDivAssign:
    target: Ptr
    value: Ptr
    TAG : int = 10

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.value, arr)


@dataclass(frozen=True)
class Stmt_FloorDivAssign:
    target: Ptr
    value: Ptr
    TAG : int = 11

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.value, arr)


@dataclass(frozen=True)
class Stmt_ModAssign:
    target: Ptr
    value: Ptr
    TAG : int = 12

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.value, arr)


@dataclass(frozen=True)
class Stmt_PowAssign:
    target: Ptr
    value: Ptr
    TAG : int = 13

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.value, arr)


@dataclass(frozen=True)
class Stmt_LShiftAssign:
    target: Ptr
    value: Ptr
    TAG : int = 14

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.value, arr)


@dataclass(frozen=True)
class Stmt_RShiftAssign:
    target: Ptr
    value: Ptr
    TAG : int = 15

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.value, arr)


@dataclass(frozen=True)
class Stmt_BitOrAssign:
    target: Ptr
    value: Ptr
    TAG : int = 16

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.value, arr)


@dataclass(frozen=True)
class Stmt_BitAndAssign:
    target: Ptr
    value: Ptr
    TAG : int = 17

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.value, arr)


@dataclass(frozen=True)
class Stmt_BitXorAssign:
    target: Ptr
    value: Ptr
    TAG : int = 18

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.value, arr)


@dataclass(frozen=True)
class Stmt_For:
    target: Ptr
    iter: Ptr
    body: Ptr
    TAG : int = 19

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.iter, arr)
        serialize_(self.body, arr)


@dataclass(frozen=True)
class Stmt_While:
    cond: Ptr
    body: Ptr
    TAG : int = 20

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.cond, arr)
        serialize_(self.body, arr)


@dataclass(frozen=True)
class Stmt_If:
    cond: Ptr
    then: Ptr
    orelse: Ptr
    TAG : int = 21

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.cond, arr)
        serialize_(self.then, arr)
        serialize_(self.orelse, arr)


@dataclass(frozen=True)
class Stmt_With:
    expr: Ptr
    body: Ptr
    TAG : int = 22

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.expr, arr)
        serialize_(self.body, arr)


@dataclass(frozen=True)
class Stmt_Raise:
    value: Ptr
    TAG : int = 23

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.value, arr)


@dataclass(frozen=True)
class Stmt_RaiseFrom:
    value: Ptr
    from_: Ptr
    TAG : int = 24

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.value, arr)
        serialize_(self.from_, arr)


@dataclass(frozen=True)
class Stmt_Try:
    body: Ptr
    err_slot: int
    except_handlers: Ptr
    final_body: Ptr
    TAG : int = 25

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.body, arr)
        serialize_(self.err_slot, arr)
        serialize_(self.except_handlers, arr)
        serialize_(self.final_body, arr)


@dataclass(frozen=True)
class Stmt_Assert:
    value: Ptr
    TAG : int = 26

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.value, arr)


@dataclass(frozen=True)
class Stmt_ExprStmt:
    value: Ptr
    TAG : int = 27

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.value, arr)


@dataclass(frozen=True)
class Stmt_Control:
    kind: int
    TAG : int = 28

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.kind, arr)


@dataclass(frozen=True)
class Stmt_Block:
    stmts: list[Ptr]
    TAG : int = 29

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.stmts, arr)


@dataclass(frozen=True)
class Expr_AddOp:
    left: Ptr
    right: Ptr
    TAG : int = 30

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Expr_SubOp:
    left: Ptr
    right: Ptr
    TAG : int = 31

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Expr_MutOp:
    left: Ptr
    right: Ptr
    TAG : int = 32

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Expr_TrueDivOp:
    left: Ptr
    right: Ptr
    TAG : int = 33

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Expr_FloorDivOp:
    left: Ptr
    right: Ptr
    TAG : int = 34

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Expr_ModOp:
    left: Ptr
    right: Ptr
    TAG : int = 35

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Expr_PowOp:
    left: Ptr
    right: Ptr
    TAG : int = 36

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Expr_LShiftOp:
    left: Ptr
    right: Ptr
    TAG : int = 37

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Expr_RShiftOp:
    left: Ptr
    right: Ptr
    TAG : int = 38

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Expr_BitOrOp:
    left: Ptr
    right: Ptr
    TAG : int = 39

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Expr_BitAndOp:
    left: Ptr
    right: Ptr
    TAG : int = 40

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Expr_BitXorOp:
    left: Ptr
    right: Ptr
    TAG : int = 41

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Expr_GlobalBinder:
    slot: int
    TAG : int = 42

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.slot, arr)


@dataclass(frozen=True)
class Expr_LocalBinder:
    slot: int
    TAG : int = 43

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.slot, arr)


@dataclass(frozen=True)
class Expr_DerefBinder:
    slot: int
    TAG : int = 44

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.slot, arr)


@dataclass(frozen=True)
class Expr_AndOp:
    left: Ptr
    right: Ptr
    TAG : int = 45

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Expr_OrOp:
    left: Ptr
    right: Ptr
    TAG : int = 46

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.left, arr)
        serialize_(self.right, arr)


@dataclass(frozen=True)
class Expr_InvertOp:
    value: Ptr
    TAG : int = 47

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.value, arr)


@dataclass(frozen=True)
class Expr_NotOp:
    value: Ptr
    TAG : int = 48

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.value, arr)


@dataclass(frozen=True)
class Expr_Lambda:
    frees: list[int]
    code: int
    TAG : int = 49

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.frees, arr)
        serialize_(self.code, arr)


@dataclass(frozen=True)
class Expr_IfExpr:
    cond: Ptr
    then: Ptr
    orelse: Ptr
    TAG : int = 50

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.cond, arr)
        serialize_(self.then, arr)
        serialize_(self.orelse, arr)


@dataclass(frozen=True)
class Expr_Dict:
    keys: list[Ptr]
    values: list[Ptr]
    TAG : int = 51

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.keys, arr)
        serialize_(self.values, arr)


@dataclass(frozen=True)
class Expr_Set:
    elts: list[Ptr]
    TAG : int = 52

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.elts, arr)


@dataclass(frozen=True)
class Expr_List:
    elts: list[Ptr]
    TAG : int = 53

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.elts, arr)


@dataclass(frozen=True)
class Expr_Generator:
    target: Ptr
    iter: Ptr
    body: Ptr
    TAG : int = 54

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.target, arr)
        serialize_(self.iter, arr)
        serialize_(self.body, arr)


@dataclass(frozen=True)
class Expr_Comprehension:
    adder: Ptr
    target: Ptr
    iter: Ptr
    body: Ptr
    TAG : int = 55

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.adder, arr)
        serialize_(self.target, arr)
        serialize_(self.iter, arr)
        serialize_(self.body, arr)


@dataclass(frozen=True)
class Expr_Call:
    f: Ptr
    args: Ptr
    TAG : int = 56

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.f, arr)
        serialize_(self.args, arr)


@dataclass(frozen=True)
class Expr_Format:
    format: int
    args: Ptr
    TAG : int = 57

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.format, arr)
        serialize_(self.args, arr)


@dataclass(frozen=True)
class Expr_Const:
    constInd: int
    TAG : int = 58

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.constInd, arr)


@dataclass(frozen=True)
class Expr_Attr:
    value: Ptr
    attr: int
    TAG : int = 59

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.value, arr)
        serialize_(self.attr, arr)


@dataclass(frozen=True)
class Expr_GlobalName:
    slot: int
    TAG : int = 60

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.slot, arr)


@dataclass(frozen=True)
class Expr_LocalName:
    slot: int
    TAG : int = 61

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.slot, arr)


@dataclass(frozen=True)
class Expr_DerefName:
    slot: int
    TAG : int = 62

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.slot, arr)


@dataclass(frozen=True)
class Expr_Item:
    value: Ptr
    item: Ptr
    TAG : int = 63

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.value, arr)
        serialize_(self.item, arr)


@dataclass(frozen=True)
class Expr_Tuple:
    elts: list[Ptr]
    TAG : int = 64

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.elts, arr)


@dataclass(frozen=True)
class Arg_GlobalNameOut:
    ind: int
    TAG : int = 65

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.ind, arr)


@dataclass(frozen=True)
class Arg_LocalNameOut:
    ind: int
    TAG : int = 66

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.ind, arr)


@dataclass(frozen=True)
class Arg_DerefNameOut:
    ind: int
    TAG : int = 67

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.ind, arr)


@dataclass(frozen=True)
class Arg_ItemOut:
    value: Ptr
    item: Ptr
    TAG : int = 68

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.value, arr)
        serialize_(self.item, arr)


@dataclass(frozen=True)
class Arg_AttrOut:
    value: Ptr
    attr: int
    TAG : int = 69

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.value, arr)
        serialize_(self.attr, arr)


@dataclass(frozen=True)
class Arg_Val:
    value: Ptr
    TAG : int = 70

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.value, arr)


@dataclass(frozen=True)
class ExceptHandler_ArbitraryCatch:
    assign_slot: int
    body: Ptr
    TAG : int = 71

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.assign_slot, arr)
        serialize_(self.body, arr)


@dataclass(frozen=True)
class ExceptHandler_TypeCheckCatch:
    type: Ptr
    assign_slot: int
    body: Ptr
    TAG : int = 72

    def serialize_(self, arr: bytearray):
        arr.append(self.TAG)
        serialize_(self.type, arr)
        serialize_(self.assign_slot, arr)
        serialize_(self.body, arr)


