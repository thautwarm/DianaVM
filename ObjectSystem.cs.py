from __future__ import annotations
from csharp_prog import *
from dianascript.operator_resolve import Doubly
from dianascript.serialize import DObj

G = Generator(globals())
myio = IO()

void = TName("void")
obj = TName("DObj")
Args = TName("Args")
sym = TName("InternString")
IEnumerable = TName("IEnumerable")
InvalidOperationException = TName("InvalidOperationException")

binary_op = (obj, [obj])
compare_op = (bool, [obj])


def binary(op: str):
    def default_compare(self, other: DObj) -> DObj:
        msg = asexpr(f"class '{self.Class.name}' does not support {op}.")
        G.append(Throw(InvalidOperationException, msg))
    default_compare.__name__ = op
    return default_compare

def compare(op: str):
    def default_compare(self, other: DObj) -> bool:
        msg = asexpr(f"class '{self.Class.name}' does not support {op} comparison.")
        G.append(Throw(InvalidOperationException, msg))
    default_compare.__name__ = op
    return default_compare

def __eq__(self, other: DObj) -> bool:
    G.return_(self == other)
    return whatever(bool)

def __ne__(self, other: DObj) -> bool:
    G.return_(self.__ne__(other))
    return whatever(bool)

def __ge__(self, other: DObj) -> bool:
    expr: Expr = self.__eq__(other)
    expr = expr.Or(self.__gt__(other))
    G.return_(expr)
    return whatever(bool)

def __le__(self, other: DObj) -> bool:
    expr: Expr = self.__eq__(other)
    expr = expr.Or(self.__lt__(other))
    G.return_(expr)
    return whatever(bool)

__lt__ =  compare("__lt__")
__gt__ =  compare("__gt__")



object_methods = {
    "__call__": (obj, [Args]),
    **{
        f"__{m}__": binary_op
        for m in (
            "add",
            "sub",
            "mul",
            "truediv",
            "floordiv",
            "mul",
            "pow",
            "lshift",
            "rshift",
            "bitand",
            "bitor",
            "bitxor",
            "getitem",
        )
    },
    **{
        f"__{m}__": compare_op
        for m in ("subclasscheck", "contains", "gt", "ge", "lt", "le", "eq", "ne")
    },
    "__setitem__": (void, [obj, obj]),
    "__delitem__": (void, [obj]),
    "__getattr__": (obj, [sym]),
    "__setattr__": (void, [sym, obj]),
    "__enter__": (obj, []),
    "__exit__": (void, [obj, obj, obj]),
    "__iter__": (IEnumerable[obj], []),
    "__repr__": (str, []),
    "__str__": (str, []),
    "__len__": (int, []),
    "__hash__": (bool, []),
    "__not__": (bool, []),
    "__bool__": (bool, []),
}



    

ns = {}
for k, (ret, params) in object_methods.items():
        ns["s" + k] = Method(
            k,
            [Param(f"_arg{i}", astype(t)) for i, t in enumerate(params)],
            [],
            astype(ret),
            Modifier.public,
        )

helpers = ["", Method("", [], [])]
@G
@G.modifier(is_interface=True, modifier=Modifier.public)
class DObj:
    # s1 = Emit("public DClsObj Class { get; }")
    locals().update(ns)
    

    # @staticmethod
    # def p(x: int) -> int:
    #     a = G.decl("a")
    #     G[a] = TName("List")[int]()
    #     G(a.Add(x))
    #     G.return_(a)
    #     return whatever(int)

    # def f(self, a: tuple[int, int]):
    #     b = G.decl("b", int)
    #     G[b] = 0
    #     with G.loop(a[1] < 2):
    #         G[b] += a[0]
    #     G.return_(b)

@G
@G.modifier(modifier=Modifier.public, bases=[DObj])
class DClsObj:
    s = mk("__add__")


G.build()

for each in G.block:
    each.cg(myio)

print(myio.x.getvalue())
