from __future__ import annotations
from collections import defaultdict
from functools import wraps
import functools
from lark import Lark, Transformer, v_args
from dataclasses import dataclass, field
from contextlib import contextmanager
from json.decoder import scanstring
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
meth : type id "(" seplist{",", param} ")" -> meth

topl : meth -> just
     | node -> just
category : id "=" "{" topl* "}" -> category

start : category* -> many
"""


type_grammar = r"""
%import common.WS
%import common.CNAME
%ignore WS
%ignore COMMENT

COMMENT: /\s*/ "//" /[^\n]/*

id : CNAME -> asstr

seplist{sep, e} : e (sep e)* -> many
                | -> empty

type : id                              -> typename
     | type "[" "]"                    -> array_type
     | type "<" seplist{",", type} ">" -> generic_type

start : type -> just
"""



@v_args(inline=True)
class Trans(Transformer):
    def just(self, a):
        return a
    def unesc(self, s):
        return scanstring(scanstring(s, 1)[0])
    def param(self, a, b):
        return a, b
    def meth(self, a, b, c):
        r = Method(a, b, c)
        return r
    def asstr(self, s):
        return str(s)
    def typename(self, s):
        return s
    def array_type(self, t):
        return t + "[]"
    def generic_type(self, t, args):
        if t == 'Tuple':
            return f"({', '.join(args)})"
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
    emits = []

@dataclass
class Method:
    ret: str
    name: str
    params: list[tuple[str, str]]
    support_case: list[str] = field(default_factory=list)


parser = Lark(
    grammar,
    transformer=Trans(),
    parser="lalr",
    start="start",
    debug=True,
)


@v_args(inline=True)
class TypeTrans(Trans):
    def typename(self, s):
        return {"List": 'list', 'Tuple': 'tuple'}.get(s, s)
    def array_type(self, t):
        return f"list[{t}]"
    def generic_type(self, t, args):
        return f"{t}[{', '.join(args)}]"
type_parser = Lark(
    type_grammar,
    transformer=TypeTrans(),
    parser="lalr",
    start="start",
    debug=True,
)



CODE = "CODE"


class Codegen:
    IO: io.TextIOWrapper
    gen_seqs : list[tuple[str, list[tuple[str, str]]]] = []

    def __init__(self):
        self.indent = ""
        self.enums = defaultdict(list)
        self.methods: dict[str, list[Method]] = defaultdict(list)
    
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
        return self

    @property
    def newline(self):
        self.IO.write("\n")
    
    

    def graph_code_def(self, categories: list[Category]):
        self << "using System;"
        self << "using System.Collections.Generic;"
        self << "namespace DianaScript"
        self << "{"
        
        
        
        assert categories
        gen_seqs = self.gen_seqs
        for each in categories:
            for e in each.emits:
                self << e
            match each:
                case Category(typename, nodes):
                    pass
                case _:
                    raise
            
            
            for node in nodes:
                match node:
                    case Node(template_id, params, temp_args) if temp_args:
                        meths = self.methods[typename]
                        for temp_arg in temp_args:
                            kind_name = typename + '_' + template_id.replace('$', temp_arg)
                            gen_seqs.append((kind_name, params))
                            self.enums[typename].append(kind_name)
                            for meth in meths:
                                meth.support_case.append(kind_name)
                                print("+", typename)


                    case Node(kind_name, params, None):
                        kind_name = typename + "_" + kind_name
                        self.enums[typename].append(kind_name)
                        gen_seqs.append((kind_name, params))
                        meths = self.methods[typename]
                        for meth in meths:
                                meth.support_case.append(kind_name)
                                

                    case Method() as a:
                        self.methods[typename].append(a)
                        pass
                    case a:
                        raise Exception(a)
        for kind_name, params in gen_seqs:
            
            self << f"public class {kind_name}"
            self << "{"
            with self.tab():
                
                for field, type in params:
                    self << f"public {type} {field};"
                    self.newline
                
                # def_p = ', '.join(f"{type} {field}" for field, type in params)
                # self << f"public static {kind_name} Make({def_p}) => new {kind_name}"
                # self << "{"
                # with self.tab():
                #     for field, type in params:
                #         self << f"{field} = {field},"
                # self << "};"

            self << "}"
            self.newline
        
        self << f"public enum {CODE}"
        self << "{"
        for _, kinds in self.enums.items():
            with self.tab():
                for kind in kinds:
                    self << f"{kind},"
        self << "}"
        self.newline
        
        self << "public class DFlatGraphCode"
        self << "{"
        with self.tab():
            for kind,  params in gen_seqs:
                self << f"public {kind}[] {kind.lower()}s;"
                self.newline
        self << "}"
        self << "}"
    def gen_parser(self, categories):
        self << "using System;"
        self << "using System.IO;"
        self << "using System.Collections.Generic;"
        self << "namespace DianaScript"
        self << "{"
        for each in categories:
            for e in each.emits:
                self << e
        gen_seqs = self.gen_seqs
        
        self << "public partial class AIRParser"
        self << "{"

        
        with self.tab():
            self << f"public {CODE} Read{CODE}()"
            self << "{"
            self << "    fileStream.Read(cache_4byte, 0, 1);"
            self << f"    return ({CODE})cache_4byte[0];"
            self << "}"
            self.newline

            self << f"public Ptr Read(THint<Ptr> _) => new Ptr(Read{CODE}(), ReadInt());"
            
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
            
            for t in ["int", "string", "float", "bool", "DFlatGraphCode", *(x[0] for x in gen_seqs), "Ptr" ]:
                generate_array_read(self, t)
                    
        self << "}" << "}"
    
    
    def gen_python_code_builder(self, _):
        self << "from __future__ import annotations"
        self << "from dataclasses import dataclass"
        self << "from dianascript.serialize import Ptr, serialize_"
        self.newline
        self.newline
        
        @functools.lru_cache()
        def py_parse(s):
            return type_parser.parse(s)

        for i, (kind, params) in enumerate(self.gen_seqs):
            self << "@dataclass(frozen=True)"
            self << f"class {kind}:"
            with self.tab():
                for field, type in params:
                    py_type = py_parse(type)
                    self << f"{field}: {py_type}"
                self << f"TAG : int = {i}"
                
                self.newline
                self << "def serialize_(self, arr: bytearray):"
                with self.tab():
                    self << "arr.append(self.TAG)"
                    for field, type in params:
                        self << f"serialize_(self.{field}, arr)"

            self.newline
            self.newline
        
    
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





seq = parser.parse(open("fgraph.spec").read())
from prettyprinter import pprint, install_extras
install_extras(['dataclasses'])
# pprint(seq)

cg = Codegen()
cg.IO = open("src/FlatGraph.cs", 'w', encoding='utf8')
cg.graph_code_def(seq)


cg.IO = open("src/FlatGraphParser.cs", 'w', encoding='utf8')
cg.gen_parser(seq)

cg.IO = open(f"dianascript/code_cons.py", 'w', encoding='utf8')
cg.gen_python_code_builder(seq)