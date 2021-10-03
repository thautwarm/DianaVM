# Generated from lark-action.


from __future__ import annotations

def _get_location(token):
    return (token.line, token.column)

def _get_value(token):
    return token.value



from dataclasses import dataclass, replace
from pyrsistent import PVector, pvector

def append(self, x):
    self.append(x)
    return self

HEAD = "Diana"

@dataclass(frozen=True)
class TArr:
    eltype : Type

@dataclass(frozen=True)
class TTup:
    eltypes : PVector[Type]

@dataclass(frozen=True)
class TName:
    name: str


@dataclass(frozen=True)
class Field:
    type: Type
    name: str

Type = TName | TTup | TArr
    
@dataclass(frozen=True)
class Def:
    name: str
    fields: PVector[Field] = pvector([])
    is_external : bool = False
    is_bytecode : bool = False
    action: str = ""
    tag: int = -1

    def __iter__(self):
        yield from self.fields
    def __len__(self):
        return len(self.fields)

tag_counter = [0]
def next_tag():
    x = tag_counter[0]
    tag_counter[0] += 1
    return x


def _rep(s: str, x):
    return s.replace("$$", x)
def _template(defi: Def, xs: list[str]):
    hd, *tl = xs
    defhd = replace(defi, name=defi.name.replace("$$", hd), action=defi.action.replace("$$", hd))
    deftl = [
        replace(
            defi,
            name=defi.name.replace("$$", each),
            action=defi.action.replace("$$", each),
            tag=defi.tag if not defi.is_bytecode else next_tag())
        for each in tl
    ]
    return [defhd, *deftl]

def _join_defs(xs):
    res = []
    for x in xs:
        if isinstance(x, list):
            res.extend(x)
        else:
            res.append(x)
    return res


from gen_lang2_raw import Transformer, Lark_StandAlone, Tree
class gen_lang2_Transformer(Transformer):

    def name_0(self, __args):
        return  str(__args[1-1])
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
    def type_0(self, __args):
        return  TTup(__args[2-1])
    def type_1(self, __args):
        return  TArr(__args[1-1])
    def type_2(self, __args):
        return  TName(__args[1-1])
    def type_3(self, __args):
        return  TGen(TName(__args[1-1]), __args[2-1])
    def field_0(self, __args):
        return  Field(__args[3-1], __args[1-1])
    def field_1(self, __args):
        return  Field(__args[1-1], __args[2-1])
    def byte_type_0(self, __args):
        return  TName(_get_value(__args[1-1]))
    def operand_field_0(self, __args):
        return  Field(__args[3-1], __args[1-1])
    def operand_field_1(self, __args):
        return  Field(__args[1-1], __args[2-1])
    def topl_0(self, __args):
        return  Def(name=__args[2-1], is_external=True)
    def topl_1(self, __args):
        return Def(name=__args[2-1], fields=__args[4-1])
    def topl_2(self, __args):
        return Def(name= HEAD + "_" + __args[1-1], fields=__args[3-1], is_bytecode=True, action=_get_value(__args[5-1])[2:-2], tag=next_tag())
    def topl_3(self, __args):
        return [ Def(name= HEAD + "_" + _rep(__args[1-1], x), fields=__args[3-1], is_bytecode=True, action=_rep(_get_value(__args[6-1])[2:-2], x), tag=next_tag()) for x in __args[5-1] ]
    def from_list_0(self, __args):
        return  __args[3-1]
    def langname_0(self, __args):
        return  globals().update(HEAD=__args[2-1])
    def start_0(self, __args):
        return  _join_defs(__args[2-1])


parser = Lark_StandAlone(transformer=gen_lang2_Transformer())
if __name__ == '__main__':

        import prettyprinter
        prettyprinter.install_extras(["dataclasses"])
        while True:
            print("input q and exit.")
            source = input("> ")
            if source.strip() == "q":
                break
            if not source.strip():
                continue
            res = parser.parse(source)
            if not isinstance(res, Tree):
                prettyprinter.pprint(res)
            else:
                print(res)
            
