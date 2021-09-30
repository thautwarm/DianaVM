from __future__ import annotations
from enum import Enum, auto as _auto
import typing
from dataclasses import dataclass


from dianascript.chexpr import *


from dianascript.chlhs import *


@dataclass
class SFunc:
    name:str
    args:list[str]
    body:list[Chstmt]
    loc: tuple[int, int] | None = None


    def __or__(self, loc):
        self.loc = loc
        return self


@dataclass
class SDecl:
    vars:list[str]
    loc: tuple[int, int] | None = None


    def __or__(self, loc):
        self.loc = loc
        return self


@dataclass
class SAssign:
    targets:list[Chlhs]
    value:Chexpr
    loc: tuple[int, int] | None = None


    def __or__(self, loc):
        self.loc = loc
        return self


@dataclass
class SExpr:
    expr:Chexpr
    loc: tuple[int, int] | None = None


    def __or__(self, loc):
        self.loc = loc
        return self


@dataclass
class SFor:
    target:Chlhs
    iter:Chexpr
    body:list[Chstmt]
    loc: tuple[int, int] | None = None


    def __or__(self, loc):
        self.loc = loc
        return self


@dataclass
class SLoop:
    block:list[Chstmt]
    loc: tuple[int, int] | None = None


    def __or__(self, loc):
        self.loc = loc
        return self


@dataclass
class SIf:
    cond:Chexpr
    then:list[Chstmt]
    loc: tuple[int, int] | None = None


    def __or__(self, loc):
        self.loc = loc
        return self




@dataclass
class SBreak:
    loc: tuple[int, int] | None = None


    def __or__(self, loc):
        self.loc = loc
        return self


@dataclass
class SContinue:
    loc: tuple[int, int] | None = None


    def __or__(self, loc):
        self.loc = loc
        return self


Chstmt = SFunc | SDecl | SAssign | SExpr | SFor | SLoop | SIf | SBreak | SContinue