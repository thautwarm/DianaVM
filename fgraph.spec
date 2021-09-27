Stmt = {
    FunctionDef(metadataInd: int, code: Ptr)
        
    end
    Return(value: Ptr)
    Del$Name(slot: int) from { Global Local Deref }
    DeleteItem(value: Ptr, item: Ptr)
    Assign(targets: Ptr, value: Ptr)
        x1 = eval.rhs value
        for each in targets{
            push x1 to valuestack
            eval.rhs each
        }
    end    

    $Assign(target: Ptr, value: Ptr) from {
        Add Sub Mut TrueDiv FloorDiv Mod Pow LShift RShift BitOr BitAnd BitXor
    }
        eval.rhs target
        pop valuestack to x1
        eval.rhs value
        pop valuestack to x2
        r  <- extern.op_$(x2, x1) (check exception)
        push r to valuestack
    end

    For(target: Ptr, iter: Ptr, body: Ptr)
    While(cond: Ptr, body: Ptr)
    If(cond: Ptr, then: Ptr, orelse: Ptr)
    With(expr: Ptr, body: Ptr)
    Raise(value: Ptr)
    RaiseFrom(value: Ptr, from_: Ptr)
    Try(body: Ptr, err_slot: int, except_handlers: Ptr, final_body: Ptr)
    Assert(value: Ptr)
    ExprStmt(value: Ptr)
    Control(kind: int) // break, continue, reraise
    Block(stmts: Ptr[])
}

// RHS case: (Frame) -> DObj
// LHS case: (Frame, DObj) -> void

Expr = {

    $Op(left: Ptr, right: Ptr) from {
        Add Sub Mut TrueDiv FloorDiv Mod Pow LShift RShift BitOr BitAnd BitXor
    }
    $Binder(slot: int) from { Global Local Deref }
    $Op(left: Ptr, right: Ptr) from {And Or}
    $Op(value: Ptr) from { Invert Not }
    Lambda(frees : int[], code: int)
    IfExpr(cond: Ptr, then: Ptr, orelse: Ptr)
    Dict(keys: Ptr[], values: Ptr[])
    Set(elts: Ptr[])
    List(elts: Ptr[])
    Generator(target: Ptr, iter: Ptr, body: Ptr)
    Comprehension(adder: Ptr, target: Ptr, iter: Ptr, body: Ptr)
    Call(f: Ptr, args: Ptr)
    Format(format: int, args: Ptr)
    Const(constInd: int)

    Attr(value: Ptr, attr: int)
    $Name(slot: int) from { Global Local Deref }
    Item(value: Ptr, item: Ptr)
    Tuple(elts: Ptr[])
} 


// Arg(Frame) -> DObj
Arg = {
    // lhs expr

    $NameOut(ind: int) from { Global Local Deref }
    ItemOut(value: Ptr, item: Ptr)
    AttrOut(value: Ptr, attr: int)
    Val(value: Ptr)
}

// ExceptHandler(Frame, exc: Exception) -> bool
ExceptHandler = {
    // assign_slot = -1 no store
    ArbitraryCatch(assign_slot: int, body: Ptr)
    TypeCheckCatch(type: Ptr, assign_slot: int, body: Ptr)
}