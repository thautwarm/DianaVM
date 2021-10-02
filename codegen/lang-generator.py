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
from pathlib import Path

import io

CODE = "CODE"
current_path = Path(__file__)
grammar = Path(__file__).with_name("lang-generator.lark").open().read()

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
        for i, kind in self.tab_iter(enumerate(self.enums)):
            self << f"{kind} = {i},"
        self << "}"
        self.newline
        
        self << "public partial class AWorld"
        self << "{"
        self << "private static readonly object _loaderSync = new object();"
        for kind,  params in self.tab_iter(datagen_seqs):
            self << f"public static List<{kind}> {kind.lower()}s = new List<{kind}>(200);"
            self << f"private static int Num_{kind} = 0;"
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
        
        self << "public partial class AWorld"
        self << "{"

        
        with self.tab():
            self << f"private {CODE} Read{CODE}()"
            self << "{"
            self << "    fileStream.Read(cache_4byte, 0, 1);"
            self << f"    return ({CODE})cache_4byte[0];"
            self << "}"
            self.newline
            
            self << f"private Ptr ReadFromCode({CODE} code) => code switch"
            self << "{"
            for  kind, params in self.tab_iter(self.enums.items()):
                if params:
                    ind = f"Num_{kind} + ReadInt()"
                else:
                    ind = "0"
                self << f"{CODE}.{kind} => new Ptr(code, {ind}),"
            self << "    _ => throw new ArgumentOutOfRangeException(\"unknown code {code}.\")"
            self << "};"
            
            self << "private Ptr Read(THint<Ptr> _) => ReadFromCode(ReadCODE());"
            for kind, params in datagen_seqs:
                if params:
                    self << f"private {kind} Read{kind}() => new {kind}"
                    self << "{"
                    for field, type in self.tab_iter(params):
                        self << f"{field} = Read(THint<{type}>.val),"
                    self << "};"
                    self.newline
                else:
                    self << f"private void Load_{kind.lower()}s()"
                    self << "{"
                    with self.tab():
                        self << "int len = ReadInt();"
                        self << "for(var i = 0; i < len; i++)"
                        with self.tab():
                            self << f"{kind.lower()}s.Add(Read(THint<{kind}>.val));"
                    self << "}"
                    continue
                                    
                if kind in self.dataclasses:
                    self << f"private {kind} Read(THint<{kind}> _) => Read{kind}();"
                self << f"private void Load_{kind.lower()}s()"
                self << "{"
                with self.tab():
                    self << "int len = ReadInt();"
                    self << "for(var i = 0; i < len; i++)"
                    with self.tab():
                        self << f"{kind.lower()}s.Add(Read{kind}());"
                self << "}"
                self.newline
            
            self << f"public void LoadCode()"
            self << "{"
            with self.tab():
                self << "lock(_loaderSync)"
                self << "{"
                for kind, params in self.tab_iter(datagen_seqs):
                    self << f"Load_{kind.lower()}s();"
                self << "}"
            self << "}"
            self.newline
            for t in ["int", "(int, int)", "float", "bool", "Ptr", "string", *self.dataclasses]:
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
                        self << f"return Ptr(self.TAG, None)"
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
        template_file = current_path.with_name('VMTemplate.cs.in').open()
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
                    self << f"var {field} = AWorld.{kind_name.lower()}s[curPtr.ind].{field};"
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


lang = parser.parse(current_path.with_name("lang.spec").open().read())
from prettyprinter import pprint, install_extras
install_extras(['dataclasses'])
# pprint(seq)

root = current_path.parent.parent
cg = Codegen()
cg.IO = (root / "src" / "FlatGraph.cs").open('w', encoding='utf8')
cg.graph_code_def(lang)


cg.IO = (root / "src" / "FlatGraphParser.cs").open('w', encoding='utf8')
cg.gen_parser(lang)

cg.IO = (root / f"dianascript" / "code_cons.py").open('w', encoding='utf8')
cg.gen_python_code_builder(lang)

cg.IO = (root / "src" / "DVM.cs").open('w', encoding='utf8')
cg.generate_interpreter(lang)
