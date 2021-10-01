# dotnet tool install -g dotnet-format
from __future__ import annotations
from io import StringIO
from enum import Enum, auto
from typing import Generic, TypeVar, Iterable, final
import typing
from typing_extensions import Protocol
from contextlib import contextmanager
from numpy.core.shape_base import block
from dataclasses import dataclass, field
from pyrsistent import pmap, PMap
import json


from dianascript.parser import append

T = TypeVar("T")


def args_as_exprs(f):
    def ap(self, *args, **kwargs):
        return f(self, *map(asexpr, args), **{k: asexpr(v) for k, v in kwargs.items()})

    return ap


class IO:
    def __init__(self):
        self.x = StringIO()
        self.indent = ""

    @contextmanager
    def tab(self, do=True, n=1):

        indent = self.indent
        try:
            if do:
                self.indent += "    " * n
            yield
        finally:
            self.indent = indent

    def tab_iter(self, iterable):
        with self.tab():
            for each in iterable:
                yield each

    def __lshift__(self, other: str):
        print(self.indent + other, file=self.x)
        return self


class Modifier(Enum):
    none = ""
    internal = "internal "
    private = "private "
    public = "public "


class TypeBase:
    def gen(self) -> str:
        raise NotImplementedError

    def __repr__(self):
        return self.gen()
    
    def __str__(self):
        return self.gen()


@dataclass
class TName(TypeBase):
    name: str

    def __getitem__(self, arg):
        if isinstance(arg, tuple):
            return TGen(self, list(map(astype, arg)))
        
        return TGen(self, [astype(arg)])

    def gen(self):
        return self.name

    @args_as_exprs
    def __call__(self, *args, **kwargs):
        if kwargs:
            assert not args
            return New2(self, kwargs)
        return New(self, list(args))


@dataclass
class TGen(TypeBase):
    f: Type
    args: list[Type]

    def gen(self):
        args = ",".join(map(gen, self.args))
        return f"{self.f.gen()}<{args}>"

    @args_as_exprs
    def __call__(self, *args, **kwargs):
        if kwargs:
            assert not args
            return New2(self, kwargs)
        return New(self, list(args))


@dataclass
class TArr(TypeBase):
    eltype: Type

    def gen(self):
        return f"{self.eltype.gen()}[]"

    @args_as_exprs
    def __call__(self, *args, **kwargs):
        if kwargs:
            assert not args
            return New2(self, kwargs)
        return New(self, list(args))


@dataclass
class TTup(TypeBase):
    eltypes: list[Type]

    def __attrs_post_init__(self):
        assert len(self.eltypes) > 1

    def gen(self):
        args = ",".join(map(gen, self.eltypes))
        return f"({args})"


def gen(t: Type):
    return t.gen()


Type = TName | TGen | TArr | TTup | TypeBase


class Array(Generic[T]):
    def __getitem__(self, i) -> T:
        raise NotImplementedError


globals()["Array"] = TName("Array")


@dataclass
class Param:
    name: str
    type: Type

    def __repr__(self):
        return f"{self.type} {self.name}"


_tmap = {
    "str": "str",
    "int": "int",
    "float": "float",
    "bool": "bool",
}


def asexpr(x) -> Expr:
    if isinstance(x, ExprBase):
        return x
    if isinstance(x, tuple):
        return Tuple([asexpr(e) for e in x])
    if isinstance(x, str):
        return Const(x)
    if isinstance(x, (int, bool)):
        return Const(x)
    if isinstance(x, bool):
        return Named(("false", "true")[x])
    if x is None:
        return Named("null")
    if x is type:
        return Named("typeof(Type)")
    if isinstance(x, type):
        return Named(_tmap[f"typeof({x.__class__.__name__})"])
    raise ValueError(x)


class ExprBase:
    @property
    def S(self) -> str:
        return self.cg()

    def cg(self):
        raise NotImplementedError

    @args_as_exprs
    def __eq__(self, other: Expr | Value):
        return BOp("==", self, other)  # type: ignore

    @args_as_exprs
    def __ne__(self, other: Expr | Value):
        return BOp("!=", self, other)  # type: ignore

    @args_as_exprs
    def __lt__(self, other: Expr | Value):
        return BOp("<", self, other)  # type: ignore

    @args_as_exprs
    def __gt__(self, other: Expr | Value):
        return BOp(">", self, other)  # type: ignore

    @args_as_exprs
    def __le__(self, other: Expr | Value):
        return BOp("<=", self, other)  # type: ignore

    @args_as_exprs
    def __ge__(self, other: Expr | Value):
        return BOp(">=", self, other)  # type: ignore

    @args_as_exprs
    def __add__(self, other: Expr | Value):
        return BOp("+", self, other)  # type: ignore

    @args_as_exprs
    def __sub__(self, other: Expr | Value):
        return BOp("-", self, other)  # type: ignore

    @args_as_exprs
    def __mul__(self, other: Expr | Value):
        return BOp("*", self, other)  # type: ignore

    @args_as_exprs
    def __truediv__(self, other: Expr | Value):
        return BOp("/", self, other)  # type: ignore

    @args_as_exprs
    def __mod__(self, other: Expr | Value):
        return BOp("%", self, other)  # type: ignore

    @args_as_exprs
    def And(self, other: Expr | Value):
        return BOp("&&", self, other)  # type: ignore

    @args_as_exprs
    def Or(self, other: Expr | Value):
        return BOp("||", self, other)  # type: ignore

    @property
    def Not(self):
        return UOp("!", self)

    @property
    def __neg__(self):
        return UOp("-", self)

    def __str__(self) -> str:
        return f"{{{self.S}}}"

    @args_as_exprs
    def __call__(self: Expr | Value, *args: Expr | Value, **kwargs: Expr | Value):
        return Call(self, args, kwargs)  # type: ignore

    def __getattr__(self: Expr | Value, attr: str):
        return Attr(asexpr(self), attr)

    @args_as_exprs
    def __getitem__(self: Expr | Value, item: Expr | Value):
        return Item(self, item)  # type: ignore


class StmtBase:
    def cg(self, io: IO):
        raise NotImplementedError


def S(x: Expr) -> str:
    return x.cg()  # type: ignore


@dataclass
class Const(ExprBase):
    o: object

    def cg(self):
        o = self.o
        if isinstance(o, str):
            return "$@" + json.dumps(o)
        return str(o)


@dataclass
class Call(ExprBase):
    f: Expr
    args: list[Expr]
    keyword_args: dict[str, Expr]

    def cg(self):
        f = self.f.S
        args = ", ".join(map(S, self.args))
        if args:
            args += ", "
        kwargs = ", ".join(
            f"{name}: {each.S}" for name, each in self.keyword_args.items()
        )
        if kwargs:
            kwargs += ", "
        return f"{f}({args}{kwargs})"


@dataclass
class UOp(ExprBase):
    operator: str
    val: Expr

    def cg(self):
        return f"{self.operator}({self.val.S})"


@dataclass
class BOp(ExprBase):
    operator: str
    left: Expr
    right: Expr

    def cg(self):
        return f"{self.left.S} {self.operator} {self.right.S}"


@dataclass
class New(ExprBase):
    cls: Type
    args: list[Expr] = field(default_factory=list)

    def cg(self):
        args = ", ".join(map(S, self.args))

        return f"new {self.cls}({args})"


@dataclass
class New2(ExprBase):
    cls: Type
    fields: dict[str, Expr]

    def cg(self):

        kwargs = ", ".join(f"{name} = {each.S}" for name, each in self.fields.items())
        return f"new {self.cls}{{ {kwargs} }}"


@dataclass
class Attr(ExprBase):
    value: Expr
    attr: str

    def cg(self):
        return f"{self.value.S}.{self.attr}"


@dataclass
class Item(ExprBase):
    value: Expr
    item: Expr

    def cg(self):
        return f"{self.value.S}[{self.item.S}]"


@dataclass
class Var(ExprBase):
    name: str

    def cg(self):
        return self.name


@dataclass
class Named(ExprBase):
    n: str

    def cg(self):
        return self.n


@dataclass
class Tuple(ExprBase):
    elts: list[Expr]

    def cg(self):
        args = ", ".join(map(S, self.elts))
        return f"({args})"


import types


def astype2(x, g: dict):
    if isinstance(x, str):
        x = eval(x, g)
    return astype(x)

def astype(x) -> Type:
    if isinstance(x, TypeBase):
        return x
    if isinstance(x, types.GenericAlias):
        orig = x.__origin__
        args = list(map(astype, x.__args__))
        if orig is tuple:
            return TTup(args)
        if orig is Array or orig is list:
            assert len(args) == 1
            return TArr(args[0])
        return TGen(astype(orig), args)

    assert isinstance(x, type)
    return TName(_tmap[x.__name__])


Value = tuple | int | str | None | float | bool | type
Expr = Tuple | Var | Item | Attr | New | BOp | UOp | Call | Const | ExprBase


@dataclass
class Method(StmtBase):  # method def and decl
    name: str
    params: list[Param]
    body: list[Stmt]
    ret: Type = TName("void")
    modifier: Modifier = Modifier.public
    is_static: bool = False

    def cg(self, io: IO):
        params = ", ".join(map(repr, self.params))
        
        if not self.body:
            io << f"{self.modifier.value}{self.ret} {self.name}({params});"
            return
        
        io << f"{self.modifier.value}{self.ret} {self.name}({params})"
        
        io << "{"
        for each in io.tab_iter(self.body):
            each.cg(io)
        io << "}"


@dataclass
class Assign(StmtBase):
    lhs: Expr
    rhs: Expr

    def cg(self, io: IO):
        io << f"{self.lhs.S} = {self.rhs.S};"

@dataclass
class Throw(StmtBase):
    type: Type
    msg: Expr
    def cg(self, io: IO):
         io << f"throw new {self.type}({self.msg.S});"

@dataclass
class If(StmtBase):
    cond: Expr
    then: list[Stmt]
    orelse: list[Stmt]

    def cg(self, io: IO):
        io << f"if ({self.cond.S})"
        io << "{"
        for each in io.tab_iter(self.then):
            each.cg(io)
        io << "}"
        if not self.orelse:
            return
        for each in io.tab_iter(self.orelse):
            each.cg(io)


@dataclass
class While(StmtBase):
    cond: Expr
    body: list[Stmt]

    def cg(self, io: IO):
        io << f"while ({self.cond.S})"
        io << "{"
        for each in io.tab_iter(self.body):
            each.cg(io)
        io << "}"


_cnt = [0]  # for iter variable auto renaming


@dataclass
class Forange(StmtBase):
    lower: Expr
    upper: Expr
    body: list[Stmt]

    def cg(self, io: IO):
        _cnt[0] += 1
        i = _cnt[0]
        var = f"_x__{i}"
        io << f"for(var {var} = {self.lower.S}; {var} < {self.upper.S}; {var}++)"
        io << "{"
        for each in io.tab_iter(self.body):
            each.cg(io)
        io << "}"


@dataclass
class Foreach(StmtBase):
    target: Expr
    iter: Expr
    body: list[Stmt]

    def cg(self, io: IO):
        io << f"foreach(var {self.target.S} in {self.iter.S})"
        io << "{"
        for each in io.tab_iter(self.body):
            each.cg(io)
        io << "}"


@dataclass
class Decl(StmtBase):
    var: Var
    type: Type | None

    def cg(self, io: IO):
        t = self.type and str(self.type) or "var"
        io << f"{self.type} {self.var.S};"


@dataclass
class Return(StmtBase):
    val: Expr | None

    def cg(self, io: IO):
        if self.val is None:
            io << "return;"
            return
        io << f"return {self.val.S};"


@dataclass
class Yield(StmtBase):
    val: Expr

    def cg(self, io: IO):
        io << f"yield return {self.val.S};"


@dataclass
class Break(StmtBase):
    def cg(self, io: IO):
        io << f"break;"


@dataclass
class Continue(StmtBase):
    def cg(self, io: IO):
        io << f"break;"


@dataclass
class Ignore(StmtBase):
    expr: Expr
    def cg(self, io: IO):
        io << f"{self.expr.S};"

@dataclass
class Field(StmtBase):
    param: Param
    value: Expr | None = None

    def cg(self, io: IO):
        name = self.param.name
        if name.startswith("_"):
            if name.startswith("_static"):
                s = f"private static {self.param!r}"
            else:
                s = f"private {self.param!r}"
        else:
            if name.startswith("_static"):
                s = f"public static {self.param!r}"
            else:
                s = f"public {self.param!r}"
        if self.value is None:
            io << (s + ";")
            return
        io << f"{s} = {self.value.S};"

@dataclass
class Emit(StmtBase):
    code: str
    def cg(self, io: IO):
        io << self.code

@dataclass
class Class(StmtBase):
    name: str
    bases: list[Type]
    body: list[Stmt]
    modifier: Modifier = Modifier.public
    is_interface: bool = False
    is_static: bool = False

    def cg(self, io: IO):
        static = self.is_static and "static " or ""
        kind = self.is_interface and "interface " or "class "
        bases = ', '.join(map(str, self.bases))
        if bases:
            bases = ": " + bases
        io << f"{self.modifier.value}{static}{kind}{self.name}{bases}"
        io << "{"
        for each in io.tab_iter(self.body):
            each.cg(io)
        io << "}"


Stmt = (
    Decl
    | Forange
    | Foreach
    | While
    | If
    | Assign
    | Return
    | Yield
    | Break
    | Continue
    | Method
    | Class
    | Field
    | Ignore
    | Emit
    | Throw
)



global _current_modifier
_current_modifier = Modifier.none


def public():
    global _current_modifier
    _current_modifier = Modifier.public


def internal():
    global _current_modifier
    _current_modifier = Modifier.internal


def private():
    global _current_modifier
    _current_modifier = Modifier.private


def static(meth):
    pass


class This:
    def __init__(self, clsname: str, d: dict):
        self.dict = d
        self.name = clsname

    def __getattr__(self, attr: str):
        x = self.dict.get(attr, None)
        if x is None:
            return Attr(Named("this"), attr)
        if isinstance(x, staticmethod):
            return Attr(Named(self.name), attr)
        if isinstance(x, types.FunctionType):
            return Attr(Named("this"), attr)
        raise
    
    def __repr__(self):
        return "{this}"


class Generator:
    def __init__(self, globals: dict):
        self.block: list[Stmt] = []
        self.modifiers = {}
        self.statics = {}
        self.classes = []
        self.interfaces = {}
        self.bases = {}
        self.globals = globals
    
    def append(self, *args: Stmt):
        self.block.extend(args)
    
    def __setitem__(self, lhs: Expr | tuple, rhs: Expr | Value):
        assert isinstance(lhs, Expr)
        rhs = asexpr(rhs)
        self.block.append(Assign(lhs, rhs))
    
    def __getitem__(self, lhs: Expr):
        return lhs

    def expr(self, expr: Expr | Value):
        self.block.append(Ignore(asexpr(expr)))

    @contextmanager
    def forange(self, low: Expr | Value, high: Expr | Value):
        old_block = self.block
        self.block = []
        try:
            yield
        finally:
            old_block.append(Forange(asexpr(low), asexpr(high), self.block))
            self.block = old_block

    @contextmanager
    def foreach(self, target: Expr | tuple, iter: Expr | Value):
        old_block = self.block
        self.block = []
        try:
            yield
        finally:
            old_block.append(Foreach(asexpr(target), asexpr(iter), self.block))
            self.block = old_block

    @contextmanager
    def loop(self, when: Expr | Value):
        old_block = self.block
        self.block = []
        try:
            yield
        finally:
            old_block.append(While(asexpr(when), self.block))
            self.block = old_block

    @contextmanager
    def if_(self, cond: Expr | Value):
        old_block = self.block
        then_block = []
        else_block = []

        # disable emit code tentatively
        self.block = None  # type: ignore
        try:
            yield (CodePack(self, then_block), CodePack(self, else_block))
        finally:
            old_block.append(If(asexpr(cond), then_block, else_block))
            self.block = old_block

    def decl(self, name: str, t: Type | type | None = None):
        if t and not isinstance(t, TypeBase):
            t = astype(t)
        self.block.append(Decl(Var(name), t))
        return Var(name)

    def return_(self, expr: Expr | Value | None = None):
        if expr is not None:
            expr = asexpr(expr)
        self.block.append(Return(expr))

    def break_(self):
        self.block.append(Break())

    def continue_(self):
        self.block.append(Continue())

    def yield_(self, val: Expr | Value):
        val = asexpr(val)
        self.block.append(Yield(val))

    def modifier(self, static=False, modifier=Modifier.none, is_interface=False, bases=()):
        def ap(f):
            self.statics[f] = static
            self.modifiers[f] = modifier
            self.interfaces[f] = is_interface
            self.bases[f] = bases
            return f

        return ap

    def class_(self, cls: type):
        old_block = self.block
        self.block = []
        for k, v in getattr(cls, "__annotations__", {}).items():
            t = astype2(v, self.globals)
            field = Field(Param(k, t), cls.__dict__.get(k))
            self.block.append(field)
        for k, v in cls.__dict__.items():
            if not k.startswith("__"):
                if isinstance(v, staticmethod):
                    self.method_(v.__func__, cls, use_self=False)
                elif isinstance(v, types.FunctionType):
                    self.method_(v, cls, use_self=True)
                elif isinstance(v, StmtBase):
                    self.block.append(v)
                else:
                    print(f"{cls.__name__}.{k} = {v} unknown")
        cls_code = Class(
            name=cls.__name__,
            bases=self.bases.get(cls, ()),
            body=self.block,
            modifier=self.modifiers.get(cls, Modifier.none),
            is_static=self.statics.get(cls, False),
            is_interface=self.interfaces.get(cls, False)
        )

        self.block = old_block
        old_block.append(cls_code)

    def method_(self, f: types.FunctionType, cls: type, use_self=False):
        old_block = self.block
        self.block = []
        g = f.__globals__
        f.__annotations__.update(
            {k: astype2(v, g) for k, v in f.__annotations__.items()}
        )
        params = []
        args = []
        for n in f.__code__.co_varnames[use_self : f.__code__.co_argcount]:
            t = f.__annotations__[n]
            params.append(Param(n, t))
            args.append(Var(n))
        if use_self:
            this = This(cls.__name__, cls.__dict__)
            f(this, *args)
        else:
            f(*args)
        cls_code = Method(
            name=f.__name__,
            params=params,
            ret=f.__annotations__.get("return", TName("void")),
            body=self.block,
            modifier=self.modifiers.get(f, Modifier.none),
            is_static=self.statics.get(f, False),
        )
        self.block = old_block
        old_block.append(cls_code)

    def __call__(self, cls: type | Expr) -> type:
        if isinstance(cls, type):
            _tmap[cls.__name__] = cls.__name__
            self.classes.append(cls)
            return cls
        else:
            self.expr(asexpr(cls))
            return whatever(type)

    def build(self):
        for cls in self.classes:
            self.class_(cls)


class CodePack:
    def __init__(self, gen: Generator, block):
        self.gen = gen
        self.old_block = block

    def __enter__(self):
        self.gen.block, self.old_block = self.old_block, self.gen.block

    def __exit__(self):
        self.gen.block = self.old_block






def whatever(_: typing.Type[T]) -> T:
    return # type: ignore


if __name__ == '__main__':
    G = Generator(globals())
    @G
    class SS:
        x: int

        @staticmethod
        def p(x: int) -> int:
            a = G.decl("a")
            G[a] = TName("List")[int]()
            G(a.Add(x))
            G.return_(a)
            return whatever(int)
        
        def f(self, a: tuple[int, int]):
            b = G.decl("b", int)
            G[b] = 0
            with G.loop(a[1] < 2):
                G[b] += a[0]
            G.return_(b)


    G.build()

    for each in G.block:
        each.cg(myio)

    print(myio.x.getvalue())

