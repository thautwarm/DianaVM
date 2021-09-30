from __future__ import annotations
from collections import defaultdict
import dataclasses
from functools import wraps
import functools
import textwrap
from typing import is_typeddict
from lark import Lark, Transformer, v_args
from dataclasses import dataclass, field
from contextlib import contextmanager
from json.decoder import scanstring
from string import Template
from textwrap import indent

import io

CODE = "CODE"
grammar = open("lang-generator.lark")

@v_args(inline=True)
class Trans(Transformer):
    def data(self, a):
        return Data(a)
    def just(self, a):
        return a
    def unesc(self, s):
        return scanstring(scanstring(s, 1)[0])
    def param(self, a, b):
        return a, b
    def asstr(self, s):
        return str(s)
    def typename(self, s):
        return s
    def array_type(self, t):
        return t + "[]"
    def tuple_type(self, *args):
        return f"({','.join(args)})"
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
    def node(self, template_id, params, from_, code):
        return Node(template_id, params, from_, code)
    def code(self, code):
        return code[2:-2]
    def localmethod(self, code):
        return Code(code)
    def staticmethod(self, code):
        return Code(code, is_static=True)
    def dataclass(self, id, params):
        return Dataclass(id, params)
    def language(self, id, staticmethods, *nodes):
        return Language(id, staticmethods, list(nodes))

@dataclass
class Dataclass:
    name: str
    params: list[tuple[str, str]]

@dataclass
class Node:
    template_id: str
    params: list[tuple[str, str]]
    from_: list[str] | None
    code: str

@dataclass
class Language:
    name: str
    nodes: list[Node]
    emits: list[str] = field(default_factory=list)

@dataclass
class Code:
    code: str
    is_static : bool = False

@dataclass
class Data:
    type: str

parser = Lark(
    grammar,
    transformer=Trans(),
    parser="lalr",
    start="start",
    debug=True,
)


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
     | "(" type ("," type)* ")"        -> tuple_type
     | type "<" seplist{",", type} ">" -> generic_type

start : type -> just
"""


@v_args(inline=True)
class TypeTrans(Trans):
    def typename(self, s):
        return {"List": 'list', 'Tuple': 'tuple', 'string': 'str'}.get(s, s)
    def array_type(self, t):
        return f"tuple[{t}, ...]"
    def generic_type(self, t, args):
        return f"{t}[{', '.join(args)}]"
    def tuple_type(self, *args):
        return f'tuple[{", ".join(args)}]'

type_parser = Lark(
    type_grammar,
    transformer=TypeTrans(),
    parser="lalr",
    start="start",
    debug=True,
)

def make_parameters(params: list[tuple[str, str]]):
    return ', '.join(f"{type} {name}" for name, type in params)

def make_arg_init_pairs(params: list[tuple[str, str]]):
    for name, _ in params: yield f"{name} = {name}" 

def fst(d): return d[0]
def attr(attr): return lambda x: getattr(x, attr)

class Codegen:
    IO: io.TextIOWrapper
    datagen_seqs : list[tuple[str, list[tuple[str, str]]]] = []

    def __init__(self):
        self.indent = ""
        self.enums = {}
        self.codes: dict[str, str] = {}
        self.dataclasses = []
        self.data = []
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
        self.IO.write(self.indent)
        self.IO.write(other)
        self.IO.write('\n')
        return self

    @property
    def newline(self):
        self.IO.write("\n")
    
    
    
    def generate_data_class(self, name: str, params: list[tuple[str, str]], generate_constructor: bool):
        self << f"public struct {name}"
        self << "{"
        with self.tab():
            for field, type in params:
                self << f"public {type} {field};"
            self.newline
            if generate_constructor:
                self << f"public static {name} Make({make_parameters(params)}) => new {name}"
                self << "{"
                for name  in self.tab_iter(map(fst, params)):
                    self << f"{name} = {name},\n"
                self << "};"
        self << "}"

    def graph_code_def(self, language: Language):
        self << "using System;"
        self << "using System.Collections.Generic;"
        self << "namespace DianaScript"
        self << "{"
        
        datagen_seqs = self.datagen_seqs
        match language:
            case Language(langname, nodes):
                pass
            case _:
                raise
        for node in nodes:
            match node:
                case Node(template_id, params, temp_args, code) if temp_args:
                    tcode = Template(code)
                    for temp_arg in temp_args:
                        kind_name = langname + '_' + Template(template_id).substitute(T=temp_arg)
                        if params:
                            datagen_seqs.append((kind_name, params))
                        # print(params)
                        code = tcode.substitute(T=temp_arg)
                        self.enums[kind_name] = params
                        self.codes[kind_name] = code
                        
                

                case Node(kind_name, params, None, code):
                    kind_name = langname + "_" + kind_name
                    self.enums[kind_name] = params
                    if params:
                        datagen_seqs.append((kind_name, params))
                    self.codes[kind_name] = code
                
                case Dataclass(classname, params):
                    self.dataclasses.append(classname)
                    datagen_seqs.append((classname, params))
                case Data(type):
                    self.data.append(type)
                    datagen_seqs.append((type, []))
                case a:
                    raise Exception(a)
        print(list(map(fst, datagen_seqs)))
        for kind_name, params in datagen_seqs:
            if params:
                self.generate_data_class(kind_name, params, generate_constructor=kind_name in self.dataclasses)
        
        self << f"public enum {CODE}"
        self << "{"
        for kind in self.tab_iter(self.enums):
            self << f"{kind},"
        self << "}"
        self.newline
        
        self << "public partial class DFlatGraphCode"
        self << "{"
        for kind,  params in self.tab_iter(datagen_seqs):
            self << f"public {kind}[] {kind.lower()}s;"
            self.newline
        self << "}"
        self << "}"

    def gen_parser(self, language: Language):
        self << "using System;"
        self << "using System.IO;"
        self << "using System.Collections.Generic;"
        self << "namespace DianaScript"
        self << "{"
        
        datagen_seqs = self.datagen_seqs
        
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
            for kind, params in datagen_seqs:
                if not params: continue
                self << f"public {kind} Read(THint<{kind}> _) => new {kind}"
                self << "{"
                for field, type in self.tab_iter(params):
                    self << f"{field} = Read(THint<{type}>.val),"
                self << "};"
                self.newline
            
            self << f"public DFlatGraphCode Read(THint<DFlatGraphCode> _) => new DFlatGraphCode"
            self << "{"
            for kind, params in self.tab_iter(datagen_seqs):    
                self << f"{kind.lower()}s = Read(THint<{kind}[]>.val),"
            self << "};"
            self.newline
            for t in ["int", "(int, int)", "float", "bool", *(map(fst, datagen_seqs)), "Ptr" ]:
                generate_array_read(self, t)
                    
        self << "}" << "}"
    
    
    def gen_python_code_builder(self, language: Language):
        self << "from __future__ import annotations"
        self << "from dataclasses import dataclass"
        self << "from dianascript.serialize import *"
        self.newline
        self.newline
        
        @functools.lru_cache()
        def py_parse(s) -> str:
            return type_parser.parse(s)
    
        params_lookup = dict(self.datagen_seqs)
        for i, kind in enumerate(self.enums):
            params = params_lookup.get(kind, [])
            self << "@dataclass(frozen=True)"
            self << f"class {kind}:"
            with self.tab():
                for field, type in params:
                    py_type = py_parse(type)
                    self << f"{field}: {py_type}"
                self << f"TAG = {i}"
                
                self.newline
                self << "def serialize_(self, arr: bytearray):"
                with self.tab():
                    self << "arr.append(self.TAG)"
                    for field, type in params:
                        self << f"serialize_(self.{field}, arr)"
                self.newline
                if params:
                    self << "def as_ptr(self) -> Ptr:"
                    with self.tab():
                        self << f"return Ptr(self.TAG, DFlatGraphCode.{kind.lower()}s.cache(self))"
                else:
                    self << "def as_ptr(self) -> Ptr:"
                    with self.tab():
                        self << f"return Ptr(self.TAG, 0)"
            self.newline
            self.newline
        
        
        for i, kind in enumerate(self.dataclasses):
            params = params_lookup[kind]
            self << "@dataclass(frozen=True)"
            self << f"class {kind}:"
            with self.tab():
                for field, type in params:
                    py_type = py_parse(type)
                    self << f"{field}: {py_type}"
                
                self.newline
                self << "def serialize_(self, arr: bytearray):"
                with self.tab():
                    for field, type in params:
                        self << f"serialize_(self.{field}, arr)"
                self.newline
                
                self << "def as_int(self) -> int:"
                with self.tab():
                    self << f"return DFlatGraphCode.{kind.lower()}s.cache(self)"                
            self.newline
            self.newline
        
        kind = "DFlatGraphCode"
        params =  [(f"{kind.lower()}s", kind) for kind, _ in self.datagen_seqs]
        self << f"class {kind}:"
        with self.tab():
            for field, type in params:
                py_type = py_parse(type)
                self << f"{field} : Builder[{py_type}] = Builder()"
            
            self.newline
            self << "@classmethod"
            self << "def serialize_(cls, arr: bytearray):"
            with self.tab():
                for field, type in params:
                    self << f"serialize_(cls.{field}, arr)"
        self.newline
        self << f"{language.name}IR" + " = " + ' | '.join(py_parse(type) for type, params in self.datagen_seqs if params)
    

    def generate_interpreter(self, language: Language):
        template_file = open('VMTemplate.cs.in')
        for each in template_file:
            if each.strip().startswith("//REPLACE"):
                break
            self << each[:-1].replace('@__', '')
        
        INDENT = "            "
        with self.tab(n=2):
            self << f"switch(curPtr.kind)"
            self << "{"
            for kind_name, code in self.tab_iter(self.codes.items()):
                params = self.enums[kind_name]
                self << f"case (int) {CODE}.{kind_name}:"
                self << "{"
                for field, _ in self.tab_iter(params):
                    self << f"var {field} = flatGraph.{kind_name.lower()}s[curPtr.ind].{field};"
                self << indent(code, INDENT)
                
                self << "    break;"
                self << "}"
            else:
                self << "default:"
                with self.tab():
                    self << "throw new Exception(\"unknown code\" + (CODE) curPtr.kind);"
            self << "}"
        for each in template_file:
            self << each[:-1].replace('@__', '')
        
        
            

        
    
def generate_array_read(self: Codegen, tname: str):
    hint_name = tname.replace('<', '__').replace('>', '__')
    if not hint_name.isidentifier():
        hint_name = '___' +''.join(c.isidentifier() and c or '__' for c in hint_name)

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


lang = parser.parse(open("lang.spec").read())
from prettyprinter import pprint, install_extras
install_extras(['dataclasses'])
# pprint(seq)

cg = Codegen()
cg.IO = open("src/FlatGraph.cs", 'w', encoding='utf8')
cg.graph_code_def(lang)


cg.IO = open("src/FlatGraphParser.cs", 'w', encoding='utf8')
cg.gen_parser(lang)

cg.IO = open(f"dianascript/code_cons.py", 'w', encoding='utf8')
cg.gen_python_code_builder(lang)

cg.IO = open(f"src/DVM.cs", 'w', encoding='utf8')
cg.generate_interpreter(lang)
