// eval stmt return
// 0 : return
// 1 : go ahead
// 2 : call frame
// 3 : continue loop
// 4 : break loop
// 5 : reraise exception
// IEquatable<T>
// IComparable<T>
// IFormattable 

// loadmetadata
// storevar, loadvar, loadstr, loadistr
// exec_block
// check_argcount
// check_argcount(dfunc, argcount: int)
// create_vstack(dfunc, p_args:int[])

// exec type   | frame share |  offset share
// exec_block  | yes         |  no
// exec_code   | yes         |  yes
// exec_func   | no          |  no

language Diana


data string
data DObj
data InternString

dataclass FuncMeta(
    is_vararg: bool,
    freeslots: int[],
    nonargcells: int[], 
    narg: int,
    nlocal: int,
    name: InternString,
    filename: string,
    lineno: int,
    freenames: string[],
    localnames: string[]
)
dataclass Block(codes: Ptr[], location_data: (int, int)[], filename: string)

FunctionDef(metadataInd: int, code: int)
[%
    var meta = loadmetadata(metadataInd);
    int nfree = meta.freeslots.Length;
    var new_freevals = new DRef[meta.freeslots.Length];
    for(var i = 0; i < nfree; i++)
        new_freevals[i] = loadref(meta.freeslots[i]);

    var dfunc = DFunc.Make(code,
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
ControlIf(arg: int) // break, continue, reraise
[%
    if(pop().__bool__())
        token = arg;
%]
JumpIfNot(off: int)
[%
    if(!(pop().__bool__()))
        offset = off; 
%]
JumpIf(off: int)
[%
    if(pop().__bool__())
        offset = off; 
%]
Jump(off: int)
[%
    offset = off; 
%]
Control(arg: int) // break, continue, reraise
[%
    token = arg;
%]
Try(unwind_start: int, unwind_stop: int, errorlabel: int, finallabel: int)
[%   
    try
    {
        exec_block(body);
    }
    catch (Exception e)
    {
        clearstack();
        var except_handlers = load_handlers(except_handler_id);
        foreach(var handler in except_handlers){
            exec_block(handler.exc_type);
            var exc_type = pop();
            var e_boxed = MK.create(e);
            if (exc_type.__subclasscheck__(e_boxed.Class))
            {
                push(e_boxed);
                exec_block(handler.body);   
                virtual_machine.errorFrames.Clear();
                goto end_handled;
            }
        }
        throw;
        end_handled: ;
    }
    finally
    {
        clearstack();
        exec_block(final_body);
    }
%]
Loop(body: int)
[%
    while(true)
    {
        exec_block(body);
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
    loop_end: ;
%]
For(body: int)
[%
    var iter = pop();
    foreach(var it in iter.__iter__()){
        push(it);
        exec_block(body);
        switch(token)
        {
            case (int) TOKEN.LOOP_BREAK:
                goto for_end;
            case (int) TOKEN.RETURN:
                return;
            default:
                token = (int) TOKEN.GO_AHEAD;
                break;
        }
        for_end: ;
    }
%]
With(body: int)
[%
    var resource = pop();
    try
    {
        push(resource.__enter__());
        exec_block(body);
        resource.__exit__(
            MK.Nil(),
            MK.Nil(),
            MK.Nil()
        );
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