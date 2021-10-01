# dotnet tool install -g dotnet-format
from __future__ import annotations
from attr import attr, attrs, attrib
from enum import Enum, auto
from typing import Generic, TypeVar
from contextlib import contextmanager
from pyrsistent import pmap, PMap
import abc
import json

T = TypeVar("T")


def pack(cls):
    return cls + 1

@pack
class Array(Generic[T]):
    pass


class Modifier(Enum):
    internal = auto()
    private = auto()
    public = auto()


@attrs(frozen=True)
class TName:
    _: str

    def gen(self):
        return self._


@attrs(frozen=True)
class TGen:
    f: Type
    args: list[Type] = attrib(converter=tuple)

    def gen(self):
        args = ",".join(map(gen, self.args))
        return f"{self.f.gen()}<{args}>"


@attrs(frozen=True)
class TArr:
    eltype: Type

    def gen(self):
        return f"{self.eltype.gen()}[]"


@attrs(frozen=True)
class TTup:
    eltypes: list[Type] = attrib(converter=tuple)

    def __attrs_post_init__(self):
        assert len(self.eltypes) > 1

    def gen(self):
        args = ",".join(map(gen, self.eltypes))
        return f"({args})"


def gen(t: Type):
    return t.gen()


Type = TName | TGen | TArr | TTup


@attrs(frozen=True)
class Class:
    name: str
    bases: Type
    modifier: Modifier = Modifier.public

@attrs(frozen=True)
class Param:
    name: str
    type: Type


@attrs(frozen=True)
class Method:  # method def and decl
    name: str
    params: list[Param] = attrib(converter=tuple)
    ret: Type = TName("void")
    body: list[Stmt] = attrib(converter=tuple, factory=tuple)



class ExprBase(Generic[T]):


    @property
    def S(self):
        return self.cg()
    
    def cg(self):
        raise NotImplementedError
    
    def __str__(self) -> str:
        return f"{{{self.S}}}"
    

class StmtBase:
    pass


def S(x: Expr):
    return x.cg()

@attrs(frozen=True)
class Const(ExprBase):
    o: object
    
    def cg(self):
        o = self.o
        if isinstance(o, str):
            return json.dumps("$@" + o)
        return str(o)


@attrs(frozen=True, )
class Call(ExprBase):
    f: Expr
    args: list[Expr] = attrib(converter=tuple)
    keyword_args: dict[str, Expr] = attrib(converter=pmap)
    
    def cg(self):
        f = self.f.S
        args = ', '.join(map(S, self.args))
        if args:
            args += ", "
        kwargs = ', '.join(f"{name}: {each.S}" for name, each in self.keyword_args.items())
        if kwargs:
            kwargs += ", "
        return f"{f}({args}{kwargs})"


@attrs(frozen=True)
class BinOp(ExprBase):
    operator: str
    left: Expr
    right: Expr


    def cg(self):
        return f"{self.left.S} {self.operator} {self.right.S}"

@attrs(frozen=True)
class UOp(ExprBase):
    operator: str
    left: Expr
    right: Expr

@attrs(frozen=True)
class New(ExprBase):
    cls: Type
    fields: dict[str, Expr] = attrib(converter=pmap)
    
    def cg(self):
        kwargs = ', '.join(f"{name} = {each.S}" for name, each in self.fields.items())
        return f"new {self.cls} {{ {kwargs} }}"


@attrs(frozen=True)
class Attr(ExprBase):
    value: Expr
    attr: str

    def cg(self):
        return f"{self.value.S}.{attr}"

@attrs(frozen=True)
class Item(ExprBase):
    value: Expr
    item: Expr
    
    def cg(self):
        return f"{self.value.S}[{self.item.S}]"


@attrs(frozen=True)
class Var(ExprBase):
    name: str

    def cg(self):
        return self.name


@attrs(frozen=True)
class Tuple(ExprBase):
    elts: list[Expr] = attrib(converter=tuple)

    def cg(self):
        args = ', '.join(map(S, self.elts))
        return f"({args})"
    


@attrs(frozen=True)
class Assign(StmtBase):
    lhs: Tuple | Var | Attr | Item
    rhs: Expr
    
    def cg(self):
        return f"{self.lhs.S} = {self.rhs.S}"
    


@attrs(frozen=True)
class If(StmtBase):
    cond: Expr
    then: list[Stmt] = attrib(converter=tuple)
    orelse: list[Stmt] = attrib(converter=tuple)
    
    def cg(self):
        return f"if {}"


@attrs(frozen=True)
class While(StmtBase):
    cond: Expr
    body: list[Stmt] = attrib(converter=tuple)


@attrs(frozen=True)
class Forange(StmtBase):
    target: Var
    lower: Expr
    upper: Expr
    body: list[Stmt] = attrib(converter=tuple)
    descend: bool = False

@attrs(frozen=True)
class Decl(StmtBase):
    var: Var
    type: Type


Stmt = int
Expr = Const

# switch
# assign
# attr
# call
# getitem

# scope[Name] =

G = TypeVar("G")


class ClassBase:
    def __setitem__(self, x: G, y: G):
        pass

    @contextmanager
    def forange(self, start: int, end: int):
        raise NotImplementedError

    def f(self):
        return 1


class E(Enum):
    pass


        
