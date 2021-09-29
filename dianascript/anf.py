from ast import *
from contextlib import contextmanager
from typing import Any, Generator
class ANFTransformer(NodeVisitor):
    def __init__(self) -> None:
        self.vstack = []
        self.tmp_to_slot = {}
        self.block = []
        self.current_node: stmt | expr = Name("", Load())


    def __lshift__(self, s: stmt):
        self.block.append(s)
        
    @contextmanager
    def enter_block(self):
        block = self.block
        try:
            yield
        finally:
            self.block = block

    def alloc_(self, lhs=None) -> str:
        match lhs: 
            case Name(id=id):
                pass
            case _:
                id = None
        if id is not None:
            return id
        for i in range(len(self.vstack)):
            id, is_used = self.vstack[i]
            if is_used:
                continue
            self.vstack[i] = id, True
        else:
            i  = len(self.vstack)
            id = f".{i}"
            self.vstack.append((id, True))
            self.tmp_to_slot[id] = i
        return id
    def dealloc_(self, n: str):
        if n.startswith('.'):
            i = self.tmp_to_slot[n]
            n_, used = self.vstack[i]
            assert n_ == n, f"allocation for {n_} and {n} error."
            assert not used, f"{n_} is not used, cannot dealloc."
            self.vstack[i] = n, False
    
    def visit(self, node):
        return getattr(self, node.__class__.__name__)(node)
    
    def visit_Block(self, stmts: list[stmt]) -> Generator[stmt, None, None]:
        yield stmts[0]
        
    def visit_FunctionDef(self, node: FunctionDef) -> Any:
        names = []
        for each in node.decorator_list:
            names.append(self.visit(each))
        
        new = ANFTransformer()
        node.body = list(new.visit_Block(node.body))
        self << node
    
    def create_name(self, n: str, node: expr | stmt):
        return Name(n, lineno=node.lineno, col_offset=node.col_offset)
    
    def visitExpr(self, a: expr, res: str):
        pass

    
    def alloc_lhs(self, targets: list[stmt | expr] | list[stmt] | list[expr]):
        s = next((each.id for each in targets if isinstance(each, Name)), None)
        if s is None:
            s = self.alloc_()
        return s
    def target_anf(self, target: expr, s):
        match target:
            case Name(id=nid) if nid == s:
                return False
            case Name(id=nid) as lhs:
                return True
            case Attribute(value=value, attr=attr) as lhs:
                ss = self.alloc_(value)
                self.visitExpr(value, ss)
                lhs.value = self.create_name(ss, value)
                self.dealloc_(ss)
                return True
            case Subscript(value=value, slice=slice) as lhs:
                ss = self.alloc_(value)
                self.visitExpr(value, ss)
                sss = self.alloc_(slice)
                self.visitExpr(value, sss)
                lhs.value=self.create_name(ss, value)
                lhs.slice = self.create_name(sss, slice)
                self.dealloc_(ss)
                self.dealloc_(sss)
                return True
            case Starred():
                raise
            case (List(elts=elts) | Tuple(elts=elts)) as lhs:
                assert elts
                s_list = []
                s_list = [self.alloc_(elt) for elt in elts]
                new_elts: list[expr] = [self.create_name(s, elts[i]) for i, s in enumerate(s_list)]
                lhs.elts = new_elts
                for i, each in enumerate(elts):
                    s_list.append(self.alloc_(each))
                    self.visitExpr(
                        Subscript(value=self.create_name(s, lhs), slice=Constant(i), lineno=each.lineno, col_offset=each.col_offset), 
                        s_list[-1]
                    )
                for each in s_list:
                    self.dealloc_(each)
                return True
            case _:
                raise
    def visit_Assign(self, node: Assign) -> Any:
        s = self.alloc_lhs(node.targets)
        self.visitExpr(node.value, s)
        node.value = self.create_name(s, node.value)
        new_targets = []
        for each in node.targets:
           if self.target_anf(each, s):
               new_targets.append(each)
        node.targets = new_targets
        if node.targets:
            self << node
        self.dealloc_(s)
    
    def visit_AugAssign(self, node: AugAssign) -> Any:
        s = self.alloc_lhs([node.value])
        self.visitExpr(node.value, s)
        node.value = self.create_name(s, node)
        self.target_anf(node.target, s)
        self << node
        self.dealloc_(s)
    
    def visit_AnnAssign(self, node: AnnAssign) -> Any:
        if not node.value:
            return
        s = self.alloc_lhs([node.target])
        self.visitExpr(node.value, s)
        node.value = self.create_name(s, node.value)
        if self.target_anf(node.target, s):
            self << node
        self.dealloc_(s)
    
    def visit_For(self, node: For) -> Any:
        ss = self.alloc_lhs([node.iter])
        self.visitExpr(node.iter, ss)
        node.iter = self.create_name(ss, node.iter)
        with self.enter_block():
            s = self.alloc_lhs([node.target])
            self.target_anf(node.target, s)
            for each in node.body:
                self.visit(each)
            node.body = self.block
        self.dealloc_(s)
        self.dealloc_(ss)
        self << node
    
    def visit_While(self, node: While) -> Any:
        s = self.alloc_lhs([node.test])
        with self.enter_block():
            self.visitExpr(node.test, s)
            test = self.create_name(s, node.test)
            self << If(test=test, body=[], orelse=[Break()], lineno = node.lineno, col_offset=node.col_offset)
            for each in node.body:
                self.visit(each)
        
            node.test = Constant(True)
            node.body = self.block
        self << node
        self.dealloc_(s)
    
    def visit_If(self, node: If) -> Any:
        s = self.alloc_lhs([node.test])
        self.visitExpr(node.test, s)
        node.test = self.create_name(s, node.test)
        
        with self.enter_block():
            for each in node.body:
                self.visit(each)
            node.body = self.block
        with self.enter_block():
            for each in node.orelse:
                self.visit(each)
            node.orelse = self.block
        self << node
        self.dealloc_(s)
        
    
    def visit_With(self, node: With) -> Any:
        s_list = []
        for item in node.items:
            s = self.alloc_lhs([])
            s_list.append(s)
            self.visitExpr(item.context_expr, s)
            item.context_expr = self.create_name(s, item.context_expr)
        with self.enter_block():
            for s, item in zip(s_list, node.items):
                if item.optional_vars:
                    if self.target_anf(item.optional_vars, s):
                        self << Assign(targets=[item.optional_vars], value=self.create_name(s, item.optional_vars))
                        item.optional_vars = None
            for each in node.body:
                self.visit(each)
            node.body = self.block
        self << node
        for each in s_list:
            self.dealloc_(each)
    
    def visit_Raise(self, node: Raise) -> Any:
        if node.cause:
            raise
        if node.exc:
            s = self.alloc_lhs([node.exc])
            self.visitExpr(node.exc, s)
            node.exc = self.create_name(s, node.exc)
            self.dealloc_(s)
        self << node
    
    # Try
    # Assert

    def visit_Global(self, node: Global) -> Any:
        self << node
    
    def visit_Nonlocal(self, node: Nonlocal) -> Any:
        self << node
    
    def visit_Expr(self, node: Expr) -> Any:
        s = self.alloc_lhs([node.value])
        self.visitExpr(node.value, s)
        # node.value = self.create_name(s, node.value)
    
    def visit_Pass(self, node: Pass) -> Any:
        self << node

    def visit_Break(self, node: Pass) -> Any:
        self << node

    def visit_Continue(self, node: Continue) -> Any:
        self << node
    
    def visit_BoolOp(self, node: BoolOp) -> Any:
        s = self.alloc_lhs([])
        for each in node.values:
            self.visitExpr(each, s)

    


        
        
        
        
        

            

            
                        
                        
                    

                    
        
        
        



        
        
        

        
        
    