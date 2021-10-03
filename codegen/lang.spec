language Diana

data string
data InternString
data DObj

dataclass FuncMeta(
    is_vararg: bool,
    freeslots: int[],
    nonargcells: int[], 
    narg: int,
    nlocal: int,
    name: InternString,
    filename: string,
    lineno: int,
    linenos: (int, int)[],
    freenames: string[],
    localnames: string[],
    bytecode: Bytecode
)

FunctionDef(metadataInd: int)
[%
    var meta = loadmetadata(metadataInd);
    int nfree = meta.freeslots.Length;
    var new_freevals = new DRef[meta.freeslots.Length];
    for(var i = 0; i < nfree; i++)
        new_freevals[i] = loadref(meta.freeslots[i]);
    
    var dfunc = DFunc.Make(
        body:meta.bytecode,
        freevals:new_freevals,
        is_vararg: meta.is_vararg,
        narg: meta.narg,
        nlocal: meta.nlocal,
        nonargcells: meta.nonargcells,
        metadataInd: metadataInd,
        nameSpace: cur_func.nameSpace
    );
    push(dfunc);
%]
LoadGlobalRef(istr: InternString)
[%
    push(new DRefGlobal(cur_func.nameSpace, istr));
%]
DelVar(target: int)
[%
    storevar(target, null);
%]
LoadVar(i: int)
[%
    push(loadvar(i));
%]
StoreVar(i: int)
[%
    storevar(i, pop());
%]
Action(kind: int)
[%
    switch (kind)
    {
        case (int) ACTION.ASSERT: 
            assert(pop().__bool__(), pop());
            break;
        case (int) ACTION.RAISE:
            throw MK.unbox<Exception>(pop());
        default:
            throw new Exception("unknown action " + ((ACTION) kind));
    }
%]
Return()
[%
    
    token = (int) TOKEN.RETURN;
    return;
%]
Break()
[%
    token = (int) TOKEN.LOOP_BREAK;
    return;
%]
Continue()
[%
    token = (int) TOKEN.LOOP_CONTINUE;
    return;
%]
JumpIfNot(off: int)
[%
    if(!(pop().__bool__()))
    {
        offset = off; 
        goto AFTER_EXEC_CHECK_CFG;
    }

%]
JumpIf(off: int)
[%
    if(pop().__bool__())
    {
        offset = off; 
        goto AFTER_EXEC_CHECK_CFG;
    }
%]
Jump(off: int)
[%
    offset = off; 
    goto AFTER_EXEC_CHECK_CFG;
%]
GetAttr(attr: InternString)
[%
    var value = pop();
    push(value.__getattr__(attr));
%]

SetAttr(attr: InternString)
[%
    var (value, subject) = pop2();
    subject.__setattr__(
        attr,
        value
    );
%]
SetAttr_I$$(attr: InternString)  from {
    add sub mul truediv floordiv mod pow lshift rshift bitor bitand bitxor
}
[%
    var (value, subject) = pop2();
    subject.__setattr__(
        attr,
        subject.__getattr__(attr).__$$__(value)
    );
%]
DelItem()
[%
    var (subject, item) = pop2();
    subject.__delitem__(item);
%]
GetItem()
[%
    var (subject, item) = pop2();
    push(subject.__getitem__(item));
%]
SetItem()
[%
    var (value, subject, item) = pop3();
    subject.__setitem__(
        item,
        value
    );
%]
SetItem_I$$()  from {
    add sub mul truediv floordiv mod pow lshift rshift bitor bitand bitxor
}
[%
    var (value, subject, item) = pop3();
    subject.__setitem__(
        item,
        subject.__getitem__(item).__$$__(value)
    );
    push(item);
%]
$$() from {
    add sub mul truediv floordiv mod pow lshift rshift bitor bitand bitxor
    gt lt ge le eq ne
}
[%
    var (left, right) = pop2();
    push(MK.create(left.__$$__(right)));
%]
in() 
[%
    var (left, right) = pop2();
    push(MK.create(right.__contains__(left)));
%]
notin() 
[%
    var (left, right) = pop2();
    push(MK.create(!(right.__contains__(left))));
%]
UnaryOp_$$() from { invert not neg }
[%
    var val = pop();
    push(MK.create(val.__$$__()));
%]
MKDict(n: int)
[%
    var dict = new Dictionary<DObj, DObj>(n);
    for(var i = 0; i < n; i++){
        var (k, v) = pop2();
        dict[k] = v;
    }
    push(MK.Dict(dict));
%]
MKSet(n: int)
[%
    var hset = new HashSet<DObj>(n);
    popNTo(n, hset);
    push(MK.Set(hset));
%]
MKList(n: int)
[%
    var list = popNToList(n);
    push(MK.List(list));
%]
Call(n: int)
[%
    var f = peek(n);
    if (f is DFunc dfunc)
    {
        check_argcount(dfunc, n);
        var locals = create_locals(dfunc, n);
        pop(); // f
        push(virtual_machine.exec_func(dfunc, locals));
    }
    else
    {
        var args = new Args();
        reversePopNTo(n, args);
        pop(); // f
        push(f.__call__(args));
    }
%]
Format(format: int, argn: int)
[%
    var argvals = new string[argn];
    for(var i = 0; i < argn; i++)
        argvals[argn - i - 1] = pop().__str__(); // TODO: format style
    var str = String.Format(loadstr(format), argvals);
    push(MK.String(str));
%]
Const(p_const: int)
[%
    push(loadconst(p_const));
%]
MKTuple(n: int)
[%
    var tuple_elts = new DObj[n];
    popNTo(n, tuple_elts);
    push(MK.Tuple(tuple_elts));
%]
Pack(n: int)
[%
    var tuple = (DTuple) pop();
    var tuple_elts = tuple.elts;
    // TODO check exact
    for(var i = 0; i < n; i++)
        push(tuple_elts[i]);
%]
Replicate(n: int)
[%
    var val = pop();
    for(var i = 0; i < n; i++)
        push(val);
%]
Pop()
[%
    pop();
%]


// block instructions:

TryCatch(unwind_bound: int, catch_start: int, catch_bound: int)
[%
    offset += OFFSET_INC;
    try
    {
        exec(BYTECODE, unwind_bound);
    }
    catch (Exception e)
    {
        clearstack();
        var e_boxed = MK.create(e);
        push(e_boxed);
        offset = catch_start;
        exec(BYTECODE, catch_bound);
        virtual_machine.errorFrames.Clear();
    }
    offset = catch_bound;
    goto AFTER_EXEC_CHECK_CFG;
%]
TryFinally(unwind_bound: int, final_start: int, final_bound: int)
[%
    offset += OFFSET_INC;
    try
    {
        exec(BYTECODE, unwind_bound);
    }
    finally
    {
        clearstack();
        offset = final_start;
        exec(BYTECODE, final_bound);
    }
    offset = final_bound;
    goto AFTER_EXEC_CHECK_CFG;
%]
TryCatchFinally(
    unwind_bound: int,
    catch_start: int,
    catch_bound: int,
    final_start: int,
    final_bound: int)
[%

    offset += OFFSET_INC;
    try
    {
        exec(BYTECODE, unwind_bound);
        // if succeed, offset = unwind_bound or returned;
    }
    catch (Exception e)
    {
        clearstack();
        var e_boxed = MK.create(e);
        push(e_boxed);
        offset = catch_start;
        exec(BYTECODE, catch_bound);
        virtual_machine.errorFrames.Clear();
    }
    finally
    {
        clearstack();
        offset = final_start;
        exec(BYTECODE, final_bound);
    }
    offset = final_bound;
    goto AFTER_EXEC_CHECK_CFG;
%]
Loop(loop_bound: int)
[%
    var loop_start = offset + OFFSET_INC;
    while(true)
    {
        offset = loop_start;
        exec(BYTECODE, loop_bound);
        switch(token)
        {
            case (int) TOKEN.LOOP_BREAK:
                token = (int) TOKEN.GO_AHEAD;
                goto loop_end;
            case (int) TOKEN.RETURN:
                return;
            default:
                token = (int) TOKEN.GO_AHEAD;
                break;
        }
    }
    loop_end:
    offset = loop_bound;
    goto AFTER_EXEC_NO_CFG_CHECK;
%]
For(loop_bound: int)
[%
    var iter = pop();
    var loop_start = offset + OFFSET_INC;
    foreach(var it in iter.__iter__())
    {
        offset = loop_start;
        push(it);
        exec(BYTECODE, loop_bound);
        switch(token)
        {
            case (int) TOKEN.LOOP_BREAK:
                token = (int) TOKEN.GO_AHEAD;
                goto for_end;
            case (int) TOKEN.RETURN:
                return;
            default:
                token = (int) TOKEN.GO_AHEAD;
                break;
        }
        
    }
    for_end:
    offset = loop_bound;
    goto AFTER_EXEC_NO_CFG_CHECK;
%]
With(with_bound: int)
[%
    var resource = pop();
    var item = resource.__enter__();
    offset += OFFSET_INC;
    try
    {
        push(item);
        exec(BYTECODE, with_bound);
    }
    catch (Exception e)
    {
        var e_boxed = MK.create(e);
        var e_trace = MK.create(virtual_machine.errorFrames);
        resource.__exit__(
            e_boxed.Class,
            e_boxed,
            e_trace
        );
    }
    resource.__exit__(
            MK.Nil(),
            MK.Nil(),
            MK.Nil()
    );
    goto AFTER_EXEC_CHECK_CFG;
%]
