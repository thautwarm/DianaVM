
Stmt = {
    FunctionDef(frees: int[], code: int)
    Return(value: Ptr<Expr>)
    Del$Name(slot: int) from { Global Local Deref }
    DeleteItem(value: Ptr<Expr>, item: Ptr<Expr>)
    Assign(targets: Ptr<Expr>, value: Ptr<Expr>)
    $Assign(target: Ptr<Expr>, value: Ptr<Expr>) from {
        Add Sub Mut TrueDiv FloorDiv Mod Pow LShift RShift BitOr BitAnd BitXor
    }
    For(target: Ptr<Expr>, iter: Ptr<Expr>, body: Ptr<Stmt>[])
    While(cond: Ptr<Expr>, body: Ptr<Stmt>[])
    If(cond: Ptr<Expr>, then: Ptr<Stmt>[], orelse: Ptr<Stmt>[])
    With(expr: Ptr<Expr>, body: Ptr<Stmt>[])
    Raise(value: Ptr<Expr>, from: Ptr<Expr>)
    Try(body: Ptr<Stmt>[], except_handlers: Ptr<ExceptHandler>[], final_body: Ptr<Stmt>[])
    Assert(value: Ptr<Expr>)
    ExprStmt(value: Ptr<Expr>)
    Control(kind: int) // break, continue
}

Expr = {
    $Op(left: Ptr<Expr>, right: Ptr<Expr>) from {
        Add Sub Mut TrueDiv FloorDiv Mod Pow LShift RShift BitOr BitAnd BitXor
    }
    $Binder(slot: int) from { Global Local Deref }
    $Op(left: Ptr<Expr>, right: Ptr<Expr>) from {And Or}
    $Op(value: Ptr<Expr>) from { Invert Not }
    Lambda(frees : int[], code: int)
    IfExpr(cond: Ptr<Expr>, then: Ptr<Stmt>[], orelse: Ptr<Stmt>[])
    Dict(keys: Ptr<Expr>[], values: Ptr<Expr>[])
    Set(elts: Ptr<Expr>[])
    List(elts: Ptr<Expr>[])
    Generator(target: Ptr<Expr>, iter: Ptr<Expr>, body: Ptr<Expr>)
    Comprehension(adder: Ptr<Expr>, target: Ptr<Expr>, iter: Ptr<Expr>, body: Ptr<Expr>)
    Call(f: Ptr<Expr>, args: Ptr<Arg>)
    Format(format: int, args: Ptr<Expr>)
    Const(constInd: int)
    Attr(value: Ptr<Expr>, attr: int)
    $Name(slot: int) from { Global Local Deref }
    Item(value: Ptr<Expr>, item: Ptr<Expr>)
    Tuple(elts: Ptr<Expr>[])
}

Arg = {
    // lhs expr
    $NameOut(ind: int) from { Global Local Deref }
    ItemOut(value: Ptr<Expr>, item: Ptr<Expr>)
    AttrOut(value: Ptr<Expr>, attr: int)
    Val(value: Ptr<Expr>)
}

ExceptHandler = {

    // assign_slot = -1 no store
    ArbitraryCatch(assign_slot: int, body: Ptr<Stmt>[])
    TypeCheckCatch(type: Ptr<Expr>, assign_slot: int, body: Ptr<Stmt>[])

}