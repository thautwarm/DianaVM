from __future__ import annotations
from collections import defaultdict
from functools import wraps
from lark import Lark, Transformer, v_args
from dataclasses import dataclass
from contextlib import contextmanager
import io

grammar = r"""
%import common.WS
%import common.CNAME
%import common.ESCAPED_STRING
%import common.LETTER
%import common.DIGIT
%ignore WS
%ignore COMMENT

COMMENT: /\s*/ "//" /[^\n]/*
TNAME: ("_"|"$"|LETTER) ("_"|"$"|LETTER|DIGIT)*

id : TNAME -> asstr
template_id : TNAME -> asstr

seplist{sep, e} : e (sep e)* -> many
                | -> empty

type : id                              -> typename
     | type "[" "]"                    -> array_type
     | type "<" seplist{",", type} ">" -> generic_type

param : id ":" type -> param

from  : -> none
      | "from" "{" id+ "}" -> many
node : template_id "(" seplist{",", param} ")"  from -> node
category : id "=" "{" node+ "}" -> category
start : category* -> many
"""



@v_args(inline=True)
class Trans(Transformer):
    def param(self, a, b):
        return a, b
    def asstr(self, s):
        return str(s)
    def typename(self, s):
        return s
    def array_type(self, t):
        return t + "[]"
    def generic_type(self, t, args):
        return f"{t}<{', '.join(args)}>"
    def empty(self):
        return ()
    def many(self, *args):
        return list(args)
    def none(self):
        return None
    def node(self, template_id, params, from_):
        return Node(template_id, params, from_)
    def category(self, id, *nodes):
        return Category(id, list(nodes))


@dataclass
class Node:
    template_id: str
    params: list[tuple[str, str]]
    from_: list[str] | None

@dataclass
class Category:
    typename: str
    nodes: list[Node]


class Codegen:
    IO: io.TextIOWrapper
    def __init__(self):
        self.indent = ""
        self.enums = defaultdict(list)
    
    @contextmanager
    def tab(self, do=True):

        indent = self.indent
        try:
            if do:
                self.indent += "    "
            yield
        finally:
            self.indent = indent
    
    def __lshift__(self, other: str):
        self.IO.write(self.indent)
        self.IO.write(other)
        self.IO.write('\n')

    @property
    def newline(self):
        self.IO.write("\n")
    def __call__(self, categories: list[Category]):
        self << "using System;"
        self << "using System.IO;"
        self << "using System.Collections.Generic;"
        self << "namespace DianaScript"
        self << "{"

        assert categories
        gen_seqs : list[tuple[str, list[tuple[str, str]]]]= []
        for each in categories:
            match each:
                case Category(typename, nodes):
                    pass
                case _:
                    raise
            
            
            for node in nodes:
                match node:
                    case Node(template_id, params, temp_args) if temp_args:
                        for temp_arg in temp_args:
                            kind_name = template_id.replace('$', temp_arg)
                            gen_seqs.append((kind_name, params))
                            self.enums[typename].append(kind_name)


                    case Node(kind_name, params, None):
                        self.enums[typename].append(kind_name)
                        gen_seqs.append((kind_name, params))
                    case _:
                        raise
        for kind_name, params in gen_seqs:
            
            self << f"public class {kind_name}"
            self << "{"
            with self.tab():
                
                for field, type in params:
                    self << f"public {type} {field};"
                    self.newline
                
                def_p = ', '.join(f"{type} {field}" for field, type in params)
                self << f"public static {kind_name} Make({def_p}) => new {kind_name}"
                self << "{"
                with self.tab():
                    for field, type in params:
                        self << f"{field} = {field},"
                self << "};"

            self << "}"
            self.newline
        
        for enum_category,  kinds in self.enums.items():
            self << f"public enum {enum_category}: int"
            self << "{"
            with self.tab():
                for kind in kinds:
                    self << f"{kind},"
            self << "}"
            self.newline
        
        self << "public partial class DFlatGraphCode"
        self << "{"
        with self.tab():
            for kind,  params in gen_seqs:
                
                self << f"public {kind}[] {kind.lower()}s;"
                self.newline
                    
        self << "}"

        self << "public partial class AIRParser"
        self << "{"

        with self.tab():
            for enum_category,  kinds in self.enums.items():

                self << f"public {enum_category} Read{enum_category}Tag()"
                self << "{"
                self << "    fileStream.Read(cache_4byte, 0, 1);"
                self << f"    return ({enum_category})cache_4byte[0];"
                self << "}"
                self.newline

                
                self << f"public Ptr<{enum_category}> Read(THint<Ptr<{enum_category}>> _)"
                self << "{"
                with self.tab():
                    self << f"var tag = Read{enum_category}Tag();"
                    self << f"switch(tag)"
                    self << "{"
                    with self.tab():
                        for kind in kinds:
                            self << f"case {enum_category}.{kind}: return new Ptr<{enum_category}>(tag, ReadInt());"
                        self << f'default: throw new InvalidDataException($"invalid tag {{tag}} for {enum_category}.");'
                    self << "}"
                self << "}"

            for kind, params in gen_seqs:
                self << f"public {kind} Read(THint<{kind}> _) => new {kind}"
                self << "{"
                with self.tab():
                    for field, type in params:
                        self << f"{field} = Read(THint<{type}>.val),"
                self << "};"
                self.newline
            
            self << f"public DFlatGraphCode Read(THint<DFlatGraphCode> _) => new DFlatGraphCode"
            self << "{"
            for kind, params in gen_seqs:
                with self.tab():
                    self << f"{kind.lower()}s = Read(THint<{kind}[]>.val),"
            self << "};"
            self.newline
            
            for t in ["int", "string", "float", "bool", "DFlatGraphCode", *(x[0] for x in gen_seqs),
                        *(f"Ptr<{x}>" for x in self.enums.keys()),
                    ]:
                generate_array_read(self, t)
                    
        self << "}"
        
        
        self << "}"
    
    
    
def generate_array_read(self: Codegen, tname: str):
    hint_name = tname.replace('<', '__').replace('>', '__')
    self << f"public static readonly THint<{tname}> {hint_name}_hint = THint<{tname}>.val;"
    self << f"public {tname}[] Read(THint<{tname}[]> _)"
    self << "{"
    with self.tab():
        self << f"{tname}[] src = new {tname}[ReadInt()];"
        self << "for (var i = 0; i < src.Length; i++)"
        self << "{"
        self << f"    src[i] = Read({hint_name}_hint);"
        self << "}"
        self << "return src;"
    self << "}"


parser = Lark(
    grammar,
    transformer=Trans(),
    parser="lalr",
    start="start",
    debug=True,
)


seq = parser.parse(open("fgraph.spec").read())
from prettyprinter import pprint, install_extras
install_extras(['dataclasses'])
# pprint(seq)

cg = Codegen()
cg.IO = open("src/FlatGraph.cs", 'w', encoding='utf8')
cg(seq)