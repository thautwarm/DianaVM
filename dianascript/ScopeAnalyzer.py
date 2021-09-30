"""
The original of this resolver work was done
in the project YAPyPy.
It has been slightly modified to support 3.10 features and,
C# 'out' keyword.
"""
from __future__ import annotations
import ast
import typing
from typing import NamedTuple, List, Optional, Union
from pprint import pformat
from enum import Enum, auto as _auto
from dataclasses import dataclass, replace
from collections import OrderedDict    


class ContextType(Enum):
    """
    Generator
    Coroutine
    """

    Module = _auto()
    Generator = _auto()  # yield
    Coroutine = _auto()  # async
    Annotation = _auto()
    ClassDef = _auto()


@dataclass
class AnalyzedSymTable:
    bounds: set[str]
    freevars: set[str]
    cellvars: set[str]


@dataclass
class SymTable:
    requires: set[str]
    entered: set[str]
    explicit_nonlocals: set[str]
    explicit_globals: set[str]
    parent: SymTable | None
    children: list[SymTable]
    depth: int  # to test if it is global context
    analyzed: AnalyzedSymTable
    cts: set[ContextType]

    @staticmethod
    def global_context():
        return SymTable(
            requires=set(),
            entered=set(),
            explicit_globals=set(),
            explicit_nonlocals=set(),
            parent=None,
            children=[],
            depth=0,
            analyzed=None,
            cts={ContextType.Module},
        )

    def enter_new(self):
        new = replace(
            self,
            requires=set(),
            entered=set(),
            explicit_globals=set(),
            explicit_nonlocals=set(),
            parent=self,
            children=[],
            depth=self.depth + 1,
            cts=set(),
        )
        self.children.append(new)
        return new

    def can_resolve_by_parents(self, symbol: str):
        assert self.analyzed
        return (
            symbol in self.analyzed.bounds
            or self.parent
            and self.parent.can_resolve_by_parents(symbol)
        )

    def resolve_bounds(self):
        enters = self.entered
        nonlocals = self.explicit_nonlocals
        globals_ = self.explicit_globals
        # split bounds
        bounds = {
            each for each in enters if each not in nonlocals and each not in globals_
        }
        self.analyzed = AnalyzedSymTable(bounds, set(), set())
        return bounds

    def resolve_freevars(self):
        assert self.analyzed
        assert self.parent
        enters = self.entered
        requires = self.requires - enters
        nonlocals = self.explicit_nonlocals
        freevars = self.analyzed.freevars
        freevars.update(
            nonlocals.union(
                {each for each in requires if self.parent.can_resolve_by_parents(each)}
            )
        )
        return freevars

    def resolve_cellvars(self):
        def fetched_from_outside(sym_tb: SymTable):
            assert sym_tb.analyzed

            return sym_tb.analyzed.freevars.union(
                *(each.analyze().analyzed.freevars for each in sym_tb.children),
            )

        analyzed = self.analyzed
        assert analyzed
        cellvars = analyzed.cellvars
        bounds = analyzed.bounds

        requires_from_sub_contexts = fetched_from_outside(self)

        cellvars.update(requires_from_sub_contexts.intersection(bounds))
        borrowed_freevars = requires_from_sub_contexts - cellvars
        # bounds.difference_update(cellvars)
        analyzed.freevars.update(borrowed_freevars)
        return cellvars

    def is_global(self):
        return self.depth == 0

    def analyze(self):
        if self.analyzed is not None:
            return self

        if self.is_global():
            # global context
            self.analyzed = AnalyzedSymTable(set(), set(), set())
            for each in self.children:
                each.analyze()
            return self

        # analyze local table.
        self.resolve_bounds()
        self.resolve_freevars()
        self.resolve_cellvars()
        return self

    def show_resolution(self):
        def show_resolution(this):
            return [this.analyzed, [show_resolution(each) for each in this.children]]

        return pformat(show_resolution(self))


def _visit_name(self, node: ast.Name):
    symtable = self.symtable
    name = node.id
    if isinstance(node.ctx, ast.Store):
        symtable.entered.add(name)
    elif isinstance(node.ctx, (ast.Load, ast.Del)):
        symtable.requires.add(name)
    
    return node


def _visit_import(self, node: ast.ImportFrom):
    for each in node.names:
        name = each.asname or each.name
        self.symtable.entered.add(name)

    return node


def _visit_try(self, node: ast.ExceptHandler):
    if node.type:
        self.visit(node.type)
    if node.name:
        self.symtable.entered.add(node.name)
    visit_suite(self.visit, node.body)
    return node



def _visit_global(self, node: ast.Global):
    self.symtable.explicit_globals.update(node.names)
    return node


def _visit_nonlocal(self, node: ast.Nonlocal):
    self.symtable.explicit_nonlocals.update(node.names)
    return node


def visit_suite(visit_fn, suite: list):
    return [visit_fn(each) for each in suite]


def _visit_cls(self: "ASTTagger", node: ast.ClassDef):
    bases = visit_suite(self.visit, node.bases)
    keywords = visit_suite(self.visit, node.keywords)
    decorator_list = visit_suite(self.visit, node.decorator_list)

    self.symtable.entered.add(node.name)

    new = self.symtable.enter_new()

    new.entered.add("__module__")
    new.entered.add("__qualname__")  # pep-3155 nested name.

    new_tagger = ASTTagger(new)
    new.cts.add(ContextType.ClassDef)
    body = visit_suite(new_tagger.visit, node.body)

    node.bases = bases
    node.keywords = keywords
    node.decorator_list = decorator_list
    node.body = body
    setattr(node, "_tag", new)
    return node


def _visit_await(self: "ASTTagger", node: ast.Await):
    cts = self.symtable.cts
    if ContextType.Coroutine not in cts:
        cts.add(ContextType.Coroutine)
    return self.generic_visit(node)


def _visit_list_set_gen_comp(self: "ASTTagger", node: ast.ListComp):
    new = self.symtable.enter_new()
    new.entered.add(".0")
    new_tagger = ASTTagger(new)
    node.elt = new_tagger.visit(node.elt)
    head, *tail = node.generators
    head.iter = self.visit(head.iter)
    head.target = new_tagger.visit(head.target)
    if head.ifs:
        head.ifs = [new_tagger.visit(each) for each in head.ifs]

    if any(each.is_async for each in node.generators):
        new.cts.add(ContextType.Coroutine)

    node.generators = [head, *[new_tagger.visit(each) for each in tail]]
    setattr(node, "_tag", new)
    return node


def _visit_dict_comp(self: "ASTTagger", node: ast.DictComp):
    new = self.symtable.enter_new()
    new.entered.add(".0")
    new_tagger = ASTTagger(new)
    node.key = new_tagger.visit(node.key)
    node.value = new_tagger.visit(node.value)

    head, *tail = node.generators
    head.iter = self.visit(head.iter)
    head.target = new_tagger.visit(head.target)
    if head.ifs:
        head.ifs = [new_tagger.visit(each) for each in head.ifs]

    if any(each.is_async for each in node.generators):
        new.cts.add(ContextType.Coroutine)
    node.generators = [head, *[new_tagger.visit(each) for each in tail]]
    setattr(node, "_tag", new)
    return node


def _visit_yield(self: "ASTTagger", node: ast.Yield):
    self.symtable.cts.add(ContextType.Generator)
    return node

def _visit_yield_from(self: "ASTTagger", node: ast.YieldFrom):
    self.symtable.cts.add(ContextType.Generator)
    return node


def _visit_ann_assign(self: "ASTTagger", node: ast.AnnAssign):
    self.symtable.cts.add(ContextType.Annotation)
    return node


def _visit_fn_def(
    self: "ASTTagger", node: Union[ast.FunctionDef, ast.AsyncFunctionDef]
):
    self.symtable.entered.add(node.name)
    args = node.args
    visit_suite(self.visit, node.decorator_list)
    visit_suite(self.visit, args.defaults)
    visit_suite(self.visit, args.kw_defaults)

    if node.returns:
        node.returns = self.visit(node.returns)

    new = self.symtable.enter_new()

    if isinstance(node, ast.AsyncFunctionDef):
        new.cts.add(ContextType.Coroutine)

    arguments = args.args + args.kwonlyargs
    if args.vararg:
        arguments.append(args.vararg)
    if args.kwarg:
        arguments.append(args.kwarg)
    for arg in arguments:
        annotation = arg.annotation
        if annotation:
            self.visit(annotation)
        new.entered.add(arg.arg)

    new_tagger = ASTTagger(new)
    node.body = [new_tagger.visit(each) for each in node.body]
    setattr(node, "_tag", new)
    return node


def _visit_lam(self: "ASTTagger", node: ast.Lambda):
    args = node.args
    new = self.symtable.enter_new()
    arguments = args.args + args.kwonlyargs
    if args.vararg:
        arguments.append(args.vararg)
    if args.kwarg:
        arguments.append(args.kwarg)
    for arg in arguments:
        # lambda might be able to annotated in the future?
        annotation = arg.annotation
        if annotation:
            self.visit(annotation)
        new.entered.add(arg.arg)

    new_tagger = ASTTagger(new)
    node.body = new_tagger.visit(node.body)
    setattr(node, "_tag", new)
    return node


def _visit_getitem(self: ASTTagger, node: ast.Subscript):
    match node:
        case ast.Subscript(ctx=ast.Load(), value=ast.Name(id='out'), slice=slice):
            assert isinstance(slice, ast.Name), "ouy keyword must be used for a simple variable."
            self.symtable.entered.add(slice.id)
            new = self.symtable.enter_new()
            new.requires.add(slice.id)
    node.value = self.visit(node.value)
    node.slice = self.visit(node.slice)
    return node


class ASTTagger(ast.NodeTransformer):
    def __init__(self, symtable: SymTable):
        self.symtable = symtable
    
    visit_Subscript = _visit_getitem
    
    visit_Name = _visit_name
    visit_Import = _visit_import
    visit_ImportFrom = _visit_import

    visit_Global = _visit_global
    visit_Nonlocal = _visit_nonlocal
    visit_FunctionDef = _visit_fn_def
    visit_AsyncFunctionDef = _visit_fn_def
    visit_Lambda = _visit_lam
    visit_ListComp = visit_SetComp = visit_GeneratorExp = _visit_list_set_gen_comp
    visit_DictComp = _visit_dict_comp

    visit_Yield = _visit_yield
    visit_YieldFrom = _visit_yield_from
    visit_AnnAssign = _visit_ann_assign
    visit_ClassDef = _visit_cls
    visit_Await = _visit_await
    visit_Try = _visit_try


def get_symtable(
    x: ast.ClassDef
    | ast.DictComp
    | ast.SetComp
    | ast.Lambda
    | ast.AsyncFunctionDef
    | ast.FunctionDef,
) -> SymTable:
    return getattr(x, "_tag")


def to_tagged_ast(node: ast.Module):
    global_table = SymTable.global_context()
    # transform ast node to tagged. visit is an proxy method to spec method.
    node = ASTTagger(global_table).visit(node)
    global_table.analyze()
    return node

if __name__ == "__main__":
    import ast

    mod = """
lambda : f(a, out[b])
"""
    print(mod)
    mod = ast.parse(mod)

    g = SymTable.global_context()
    ASTTagger(g).visit(mod)

    g.analyze()
    print(g.show_resolution())

