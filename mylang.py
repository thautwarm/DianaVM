# Generated from lark-action.
def _get_location(token):
    return (token.line, token.column)

def _get_value(token):
    return token.value


from contextlib import contextmanager
from dianascript.operator_resolve import Operator, binop_reduce
from dianascript.chexpr import *
from dianascript.chlhs import *
from dianascript.chstmt import *
def cons(op):
    def ap(l, r):
        loc, opname = op.val
        return EOp(l, opname, r)
    return ap

def resolve_binop(args):
    if len(args) == 1:
        return args[0]

    for i in range(1, len(args), 2):
        assert isinstance(args[i], tuple)
        op = args[i]
        args[i] = Operator(op[1], op)
    return binop_reduce(cons, args)

def append(xs, x):
    xs.append(x)
    return xs

BLOCKS = [[]]

def enter_block():
    global BLOCKS
    BLOCKS.append([])

def emit(x):
    BLOCKS[-1].append(x)



from mylang_raw import Transformer, Lark_StandAlone
class mylang_Transformer(Transformer):

    def start_0(self, __args):
        return  __args[1-1]
    def list_0(self, __args):
        return  [__args[1-1]]
    def list_1(self, __args):
        return  append(__args[1-1], __args[2-1])
    def seplist_0(self, __args):
        return  [__args[1-1]]
    def seplist_1(self, __args):
        return  append(__args[1-1], __args[3-1])
    def nullable_0(self, __args):
        return  []
    def nullable_1(self, __args):
        return  __args[1-1]
    def stmt_0(self, __args):
        return  SDecl(__args[2-1]) | _get_location(__args[1-1])
    def stmt_1(self, __args):
        return  SAssign(__args[1-1], __args[3-1]) | _get_location(__args[2-1])
    def stmt_2(self, __args):
        return  SExpr(__args[1-1])
    def stmt_3(self, __args):
        return  SLoop(__args[2-1]) | _get_location(__args[1-1])
    def stmt_4(self, __args):
        return  SFor(__args[2-1], __args[4-1], __args[6-1]) | _get_location(__args[1-1])
    def stmt_5(self, __args):
        return  SIf(__args[2-1], __args[4-1], None) | _get_location(__args[1-1])
    def stmt_6(self, __args):
        return  SIf(__args[2-1], __args[4-1], __args[6-1]) | _get_location(__args[1-1])
    def stmt_7(self, __args):
        return  SBreak()
    def stmt_8(self, __args):
        return  SContinue()
    def func_0(self, __args):
        return   SFunc(__args[2-1], __args[4-1], __args[6-1]) | _get_location(__args[1-1])
    def block_0(self, __args):
        return   __args[1-1]
    def or_expr_0(self, __args):
        return  EOr(__args[1-1], __args[2-1]) | _get_location(__args[1-1])
    def or_expr_1(self, __args):
        return  __args[1-1]
    def and_expr_0(self, __args):
        return  EAnd(__args[1-1], __args[2-1]) | _get_location(__args[1-1])
    def and_expr_1(self, __args):
        return  __args[1-1]
    def binop_0(self, __args):
        return  (_get_location(__args[1-1]), _get_value(__args[1-1]))
    def binop_1(self, __args):
        return  (_get_location(__args[1-1]), _get_value(__args[1-1]) + "_" + _get_value(__args[2-1]))
    def bin_0(self, __args):
        return  resolve_binop(__args)
    def not_0(self, __args):
        return  ENot(__args[2-1]) | _get_location(__args[1-1])
    def not_1(self, __args):
        return  __args[1-1]
    def trailer_0(self, __args):
        return  True
    def trailer_1(self, __args):
        return  False
    def lhs_0(self, __args):
        return  LVar(_get_value(__args[1-1])) | _get_location(__args[1-1])
    def lhs_1(self, __args):
        return  LIt(__args[1-1], __args[4-1]) | _get_location(__args[2-1])
    def lhs_2(self, __args):
        return  LAttr(__args[1-1], __args[3-1]) | _get_location(__args[2-1])
    def pair_0(self, __args):
        return  (__args[1-1], __args[3-1])
    def atom_0(self, __args):
        return  EIt(__args[1-1], __args[4-1]) | _get_location(__args[2-1])
    def atom_1(self, __args):
        return  EAttr(__args[1-1], __args[3-1]) | _get_location(__args[2-1])
    def atom_2(self, __args):
        return  EApp(__args[1-1], __args[3-1]) | _get_location(__args[2-1])
    def atom_3(self, __args):
        return  EList(__args[2-1]) | _get_location(__args[1-1])
    def atom_4(self, __args):
        return  EPar(__args[2-1], __args[3-1]) | _get_location(__args[1-1])
    def atom_5(self, __args):
        return  EDict(__args[2-1], __args[3-1]) | _get_location(__args[1-1])
    def atom_6(self, __args):
        return  EVal(eval(_get_value(__args[1-1]))) | _get_location(__args[1-1])
    def atom_7(self, __args):
        return  EVal(eval(_get_value(__args[1-1]))) | _get_location(__args[1-1])
    def atom_8(self, __args):
        return  EVal(None)  | _get_location(__args[1-1])
    def atom_9(self, __args):
        return  EVal(True)  | _get_location(__args[1-1])
    def atom_10(self, __args):
        return  EVal(False) | _get_location(__args[1-1])
    def atom_11(self, __args):
        return  EVar(_get_value(__args[1-1]))    | _get_location(__args[1-1])
    def atom_12(self, __args):
        return  ENeg(__args[2-1])    | _get_location(__args[1-1])
    def atom_13(self, __args):
        return  EInv(__args[2-1])    | _get_location(__args[1-1])
    def string_0(self, __args):
        return  __args[1-1]
    def name_0(self, __args):
        return  _get_value(__args[1-1])
    def number_0(self, __args):
        return  __args[1-1]


parser = Lark_StandAlone(transformer=mylang_Transformer())
if __name__ == '__main__':

        while True:
            print("input q and exit.")
            source = input("> ")
            if source.strip() == "q":
                break
            if not source.strip():
                continue
            print(parser.parse(source))
