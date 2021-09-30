from __future__ import annotations
from enum import Enum, auto as _auto
import typing
from dataclasses import dataclass


from dianascript.chexpr import *


@dataclass
class LVar:
    var:str
    loc: tuple[int, int] | None = None


    def __or__(self, loc):
        self.loc = loc
        return self


@dataclass
class LIt:
    value:Chexpr
    item:Chexpr
    loc: tuple[int, int] | None = None


    def __or__(self, loc):
        self.loc = loc
        return self


@dataclass
class LAttr:
    value:Chexpr
    attr:str
    loc: tuple[int, int] | None = None


    def __or__(self, loc):
        self.loc = loc
        return self


Chlhs = LVar | LIt | LAttr