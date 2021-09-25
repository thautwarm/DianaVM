from __future__ import annotations
from lark import Lark, Transformer, v_args
from dataclasses import dataclass
from json.decoder import scanstring


grammar = r'''
%import common.WS
%import common.CNAME
%import common.ESCAPED_STRING
%import common.COMMENT
%ignore WS
%ignore COMMENT

COMMENT: /\s*/ "//" /[^\n]/*
SPECIAL :  "__getitem__" | "__setitem__"

OP  : "+" | "-" | "*" | "/" | "%"
    | "&" | "|" | "^" | "<<" | ">>"
    | "==" | "!="

bindname : (SPECIAL | CNAME | OP) -> asstr

seplist{sep, e} : e (sep e)* -> many
                | -> empty


typelist : seplist{",", toptype} -> just

type : CNAME                 -> typename
     | type "[" "]"          -> array_type
     | type "<" seplist{",", type} ">" -> generic_type

type2_part1 : "*" type   -> varg
            | "out" type -> outarg
            |     type   -> simplearg

type2_part2 : "?" -> true
            |     -> false

type2_part3 : "from" type -> just
            | -> none

toptype : type2_part1 type2_part2 type2_part3 -> toptype

methodname : "this" "." (CNAME|SPECIAL)   -> instmeth
           | CNAME  "." (CNAME|SPECIAL)   -> extmeth
           | CNAME                        -> clsmeth


bindnames : seplist{",", bindname} -> just

op_special : (OP|SPECIAL) -> asstr

ops    : op_special ("," op_special)* -> many

method :  toptype methodname "(" typelist ")" "as" bindnames -> bindmeth
       |  toptype methodname "as" bindnames -> bindproperty
       |  "operator" "{" ops "}" -> autu_operator
       |  "operator" "all" -> all_operator


boxedtypes : "boxedtypes" "{" CNAME* "}" -> boxedtypes
methods : method* -> many
clsbind : CNAME CNAME type "{" methods "}" -> clsbind
emit : ESCAPED_STRING -> emit
each : emit* clsbind -> each
start : boxedtypes each+ -> start
'''

@dataclass
class TName:
    _ : str

    def __post_init__(self):
        assert isinstance(self._, str)
    def as_str(self):
        return self._

@dataclass
class TArray:
    _ : Type
    def as_str(self):
        return f"{self._.as_str()}[]"


@dataclass
class TGeneric:
    base : Type
    params: tuple[Type, ...]
    def __post_init__(self):
        assert all(isinstance(e, (TName, TArray, TGeneric)) for e in self.params)
    def as_str(self):
        args = ', '.join(map(lambda x: x.as_str(), self.params))
        return f"{self.base.as_str()}<{args}>"


Type = TName | TArray | TGeneric

@dataclass
class TParam:
    t : Type
    is_out : bool = False
    is_varg : bool = False
    is_optional : bool = False
    cast_from   : Type | None = None


@dataclass
class InstMethName:
    _: str

@dataclass
class ClassMethName:
    _: str

@dataclass
class ExtMethName:
    classname: str
    _: str

MethName = ExtMethName | ClassMethName | InstMethName


@dataclass
class MethodDecl:
    ret: TParam
    methname: MethName
    params : tuple[TParam, ...] | None
    bindings: tuple[str]

all_ops = (
    "+" , "-" , "*" , "/" , "%"
    , "&" , "|" , "^" , "<<" , ">>"
    , "==" , "!="
)
@dataclass
class AutoOperatorDecl:
    ops : tuple[str, ...] = all_ops

@dataclass
class ClassBind:
    bindname: str
    clswraptype: str
    clstype: Type
    methods: tuple[MethodDecl | AutoOperatorDecl, ...]
    emit: str = ""



boxed_types = {}
@v_args(inline=True)
class Trans(Transformer):
    def __init__(self):
        self.current_emit = ""

    def start(self, _, *ast_seq):
        return ast_seq
    def boxedtypes(self, *args):
        for k in args:
            boxed_types[str(k)] = None

    def asstr(self, a):
        return str(a)
    def just(self, a):
        return a
    def many(self, *args):
        return args
    def empty(self):
        return ()
    def true(self):
        return True
    def false(self):
        return False
    def typename(self, a):
        return TName(str(a))
    def array_type(self, a):
        return TArray(a)
    def generic_type(self, t, args):
        return TGeneric(t, args)
    def varg(self, a):
        return TParam(a, is_varg=True)
    def outarg(self, a):
        return TParam(a, is_out=True)
    def simplearg(self, a):
        return TParam(a)
    def none(self):
        return None
    def toptype(self, p1: TParam, p2: bool, p3: Type | None):
        p1.is_optional = p2
        p1.cast_from = p3
        return p1
    def instmeth(self, name):
        return InstMethName(str(name))
    def extmeth(self, srcclass, name):
        return ExtMethName(str(srcclass), str(name))
    def clsmeth(self, name):
        return ClassMethName(str(name))
    def bindmeth(self, ret, methname, params, bindnames):
        return MethodDecl(ret, methname, params, bindnames)

    def bindproperty(self, ret, methname, bindnames):
        return MethodDecl(ret, methname, None, bindnames)

    def auto_operator(self, a):
        return AutoOperatorDecl(a)

    def all_operator(self):
        return AutoOperatorDecl()

    def clsbind(self, a, b, c, d):
        return ClassBind(str(a), str(b), c, d)

    def emit(self, a):
        self.current_emit += scanstring(a, 1)[0]

    def each(self, *args):
        a = args[-1]
        a.emit = self.current_emit
        self.current_emit = ""

        return a


tbnf_parser = Lark(
    grammar,
    transformer=Trans(),
    parser="lalr",
    start="start",
    debug=True,
)

from prettyprinter import install_extras, pprint
install_extras(["dataclasses"])

ast = tbnf_parser.parse(open("gen.spec").read())
pprint(ast)



from pathlib import Path
from io import StringIO
from contextlib import contextmanager
from json import dumps





def accept_arg(in_t: str | Type, arg_repr: str):
    if not isinstance(in_t, str):
        in_t = in_t.as_str()
    return f"MK.unbox<{in_t}>({arg_repr})"

def cast(target_t: str | Type, arg_repr: str):
    if not isinstance(target_t, str):
        target_t = target_t.as_str()
    if target_t == 'void':
        return arg_repr
    return f"MK.cast(THelper<{target_t}>.val, {arg_repr})"

@dataclass
class TypedExpr:
    t: Type
    _: str

    def __repr__(self):
        return self._

class Codegen:
    def __init__(self):
        self.IO = StringIO()
        self.wrap_type = ""
        self.impl_type = ""
        self.cls_name = "" # name that user see in the script language
        self.indent = ""
        self.getters = []
    def __lshift__(self, other: str):
        self.IO.write(self.indent)
        self.IO.write(other)
        return self

    @contextmanager
    def tab(self, do=True):

        indent = self.indent
        try:
            if do:
                self.indent += "  "
            yield
        finally:
            self.indent = indent

    def declare(self, name: str, func):
        self.getters.append('{ "%s", (false, MK.CreateFunc(%s)) },\n' % (name, func) )
        return self

    def bind_method(self, x: MethodDecl):
        bind_name = x.bindings[0]
        name = f"bind_{x.bindings[0]}"
        for each in x.bindings:
            self.declare(each, name)
        self  << f"public static DObj {name}(Args _args)\n"
        self << "{\n"
        variadic = False
        with self.tab():
            self << "var nargs = _args.NArgs;\n"
            arg_check = '!='
            arg_desc = ""
            valid_argc = [0]
            access_field_from_first = False
            field_from_class = None
            match x.methname:
                case InstMethName(meth_name):
                    params = [TParam(TName(self.impl_type)), *x.params]
                    access_field_from_first = True
                    meth_name
                case ClassMethName(meth_name):
                    field_from_class = self.impl_type
                    params = x.params
                case ExtMethName(field_from_class, meth_name):
                    params = x.params

            for param in params:
                if param.is_optional:
                    valid_argc.append(valid_argc[-1] + 1)
                    arg_check = "<"
                elif param.is_varg:
                    variadic = True
                    arg_check = "<"
                    arg_desc = ">="
                else:
                    valid_argc[-1] += 1
            self << f"if (nargs {arg_check} {valid_argc[0]})\n"
            error_msg = f"calling {self.cls_name}.{bind_name}; needs at least {arg_desc} ({','.join(map(str, valid_argc))}) arguments, got {{nargs}}."
            self << f"  throw new D_TypeError(${dumps(error_msg)});\n"

            arguments = []
            i = -1

            out_args: list[tuple[str, str]] = []
            def try_return(check=True):
                if check:
                    if i + 1 not in valid_argc:
                        return
                    if i + 1 == valid_argc[-1] and not variadic:
                        pass
                    else:
                        self << f"if (nargs == {i + 1})\n"

                if access_field_from_first:
                    call = f"{arguments[0]}.{meth_name}({','. join(arguments[1:])})"
                elif meth_name == "new":
                    call = f"{meth_name}({','.join(arguments)})"
                else:
                    call = f"{field_from_class}.{meth_name}({','.join(arguments)})"
                if x.ret.cast_from:
                    call = cast(x.ret.t, call)
                self << "{\n"
                with self.tab():
                    if x.ret.t.as_str() == "void":
                        self << f"{call};\n"
                        for changed, each in out_args:
                            self << f"{each}.set_contents({cast('DObj', changed)});\n"
                        self << "return MK.Nil();\n"
                    else:
                        self << f"var _return = {call};\n"
                        for changed, each in out_args:
                            self << f"{each}.set_contents({cast('DObj', changed)});\n"
                        self << f"return MK.create(_return);\n"
                self << "}\n"

            try_return()
            for i, param in enumerate(params):
                param: TParam = param
                expect_t : Type = param.t
                if param.cast_from is None:
                    in_t: Type = param.t
                else:
                    in_t: Type = param.cast_from
                argn = f"_arg{i}"
                if param.is_varg:
                    self << f"var {argn} = new {expect_t.as_str()}[nargs - {i} - 1];\n"
                    self << f"for(var _i = {i}; _i < nargs; _i++)\n"
                    self << f"  {argn}[_i - {i}] = _args[_i];\n"
                    arguments.append(argn)
                    break
                arg_repr = f"_args[{i}]"
                if param.is_out:
                    out_arg = f"_out_{i}"
                    out_args.append((argn, out_arg))
                    self << f"var {out_arg} = {accept_arg(TName('DRef'), arg_repr)};\n"
                    arg_repr = out_arg + ".get_contents()"
                arg_repr = accept_arg(in_t, arg_repr)
                if in_t.as_str() != expect_t.as_str():
                    arg_repr = cast(expect_t, arg_repr)
                self << f"var {argn} = {arg_repr};\n"

                if param.is_out:
                    argn = "out " + argn
                arguments.append(argn)
                try_return()

            if variadic:
                try_return(check=False)
            else:
                error_msg = f"call {self.cls_name}.{bind_name}; needs at most ({valid_argc[-1]}) arguments, got {{nargs}}."
                self << f"throw new D_TypeError(${dumps(error_msg)});\n"

        self << "}\n"

    def bind_cls_property(self, cls_name, x : MethodDecl):
        bind_name = x.bindings[0]
        name = f"bind_{x.bindings[0]}"
        for each in x.bindings:
            self.declare(each, name)
        self  << f"public static DObj {name}(Args _args)\n"
        self << "{\n"
        with self.tab():
            self << "var nargs = _args.NArgs;\n"
            self << "if (nargs != 0)\n"
            error_msg = f"accessing {self.cls_name}.{bind_name}; needs 0 arguments, got {{nargs}}."
            self << f"  throw new D_TypeError(${dumps(error_msg)});\n"
            self << (f"var ret = "
                    f"{cls_name}.{x.methname._};\n")
            ret = 'ret';

            if x.ret.cast_from:
                ret = cast(x.ret.t, ret)
            if x.ret.t.as_str() == "void":
                self << f"{ret};\n"
                self << "return MK.Nil();\n"
            else:
                self << f"return MK.create({ret});"
        self << "}\n"


    def bind_this_property(self,  x : MethodDecl):
        bind_name = x.bindings[0]
        name = f"bind_{x.bindings[0]}"
        for each in x.bindings:
            self.declare(each, name)
        self  << f"public static DObj {name}(Args _args)\n"
        self << "{\n"
        with self.tab():
            self << "var nargs = _args.NArgs;\n"
            self << "if (nargs != 1)\n"
            error_msg = f"accessing {self.cls_name}.{bind_name}; needs only 1 argument, got {{nargs}}."
            self << f"  throw new D_TypeError(${dumps(error_msg)});\n"
            self << f"var arg = _args[0];\n"
            self << f"var ret = {accept_arg(self.impl_type, 'arg')}.{x.methname._};\n"
            if x.ret.cast_from:
                self << f"return MK.create({cast(x.ret.t, 'ret')});\n"
            else:
                self << f"return MK.create(ret);\n"
        self << "}\n"

    def bind_property(self, x: MethodDecl):
        assert x.params is None
        match x.methname:
            case InstMethName():
                self.bind_this_property(x)

            case ClassMethName(_):
                self.bind_cls_property(self.impl_type, x)

            case ExtMethName(ext, _):
                self.bind_cls_property(ext, x)



    def bind_class(self, x: ClassBind):
        bind_name = x.bindname
        self.getters = []
        # TODO use bind_name instead of cls_name
        self.bind_name = self.cls_name = bind_name
        type_struct = f"d{x.bindname}"
        wrap_type = x.clswraptype
        impl_type = x.clstype.as_str()
        self.wrap_type = wrap_type
        self.impl_type = impl_type

        self << "using System;" << "\n"
        self <<  x.emit << "\n"
        self << "namespace DianaScript\n"
        self << "{\n"
        self << f"public partial class {wrap_type}\n"
        self << "{\n"
        with self.tab():
            self << f"public DClsObj GetCls => Cls.unique;\n"
            for method in x.methods:
                    match method:
                        case MethodDecl() if method.params is None:
                            self.bind_property(method)
                        case MethodDecl():
                            self.bind_method(method)

            self << f"public partial class Cls : DClsObj"
            self << "{\n"
            self << f"public string name => {dumps(bind_name)};\n"
            self << f"public static Cls unique = new Cls();\n"
            self << f"public Type NativeType => typeof({wrap_type});\n"
            self << f"public System.Collections.Generic.Dictionary<string, (bool, DObj)> Getters {{get; set;}}\n"
            self << f"public Cls()\n"
            self << "{\n"
            
            with self.tab():
                self << "DWrap.RegisterTypeMap(NativeType, this);\n"
                self << f"Getters = new System.Collections.Generic.Dictionary<string, (bool, DObj)>\n"
                self << "{\n"
                with self.tab():
                    for each in self.getters:
                        self << each
                self << "};\n"
            self << "}\n"
            self << "}\n"
        self << "}\n"
        self << "}\n"

cg = Codegen()

root = Path("src") / "structures"
root.mkdir(exist_ok=True)
for cls in ast:
    cls: ClassBind
    file = root / (cls.clswraptype + ".cs")
    with file.open('w') as f:
        cg.IO = f
        cg.bind_class(cls)
