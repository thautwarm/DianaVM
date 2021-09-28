// eval stmt return
// 0 : return
// 1 : go ahead
// 2 : call frame
// 3 : continue loop
// 4 : break loop
// 5 : reraise exception

// loadmetadata
// storevar, loadvar, loadstr, loadistr
// set_return, execute_block
// check_argcount
// check_argcount(dfunc, argcount: int)
// create_vstack(dfunc, s_args:int[])

// exec type   | frame share |  offset share
// exec_block  | yes         |  no
// exec_code   | yes         |  yes
// exec_func   | no          |  no

language Diana


data string as strings
data DObj as objects
data InternString as istrings

dataclass Catch(exc_target: int, exc_type: int, body: Block)
dataclass FuncMeta(
    is_vararg: bool,
    freeslots: int[],
    narg: int,
    nlocal: int,
    name: InternString,
    modname: InternString,
    filename: string
)
dataclass Loc(location_data: (int, int)[])
dataclass Block(codes: Ptr[], location_data: int)

FunctionDef(target: int, metadataInd: int, code: Block)
[%
    var meta = loadmetadata($metadataInd);
    int nfree = meta.freeslots.Length;

    var new_freevals = new DObj[meta.freeslots.Length);
    for(var i = 0; i < nfree; i)
        new_freevals[i] = loadvar(meta.freeslots[i]);

    var dfunc = DFunc.Make($code,
        freevals:new_freevals,
        is_vararg: meta.is_vararg,
        narg: meta.narg,
        nlocal: meta.nlocal,
        metadataInd: $metadataInd);
        
    storevar(target, dfunc);
%]
Return(reg: int)
[%
    set_return(loadvar(reg));
    token = (int) TOKEN.RETURN;
%]
DelVar(target: int)
[%
    storevar($target, null);
%]
SetVar(target: int, s_val: int)
[%
    storevar($target, loadvar($s_val));
%]
JumpIf(s_val: int, offset: int)
[%
    if (loadvar(s_val).__bool__)
        offset = $offset;
%]
Jump(offset: int)
[%
    offset = $offset;
%]
Raise(s_exc: int)
[%
    var e = MK.unbox<Exception>(loadvar(s_exc));
    throw e;
%]
Assert(value: int, s_msg: int)
[%
    assert(storevar(value).__bool__, loadvar($s_msg));
%]
Control(token: int) // break, continue, reraise
[%
    token = $token;
%]
Try(body: Block, except_handlers: Catch[], final_body: Block)
[%
        try
        {
            execute_block(body);
        }
        catch (Exception e)
        {
            foreach(var handler in $except_handlers){
                var exc_type = loadvar(handler.exc_type);
                var e_boxed = MK.create(e);
                if (exc_type.__subclasscheck__(e_boxed.GetCls == exc_type))
                {
                    if (handler.exc_target)
                        storevar(exc_target, e_boxed);
                    execute_block(body);
                    if (handler.exc_target)
                        storevar(exc_target, null);
                    virtual_machine.ErrorFrames.Clear();
                    goto end_handled;
                }
            }
            throw;
            end_handled:
        }
        finally{
            execute_block(final_body)
        }
%]
For(target: int, s_iter: int, body: Block)
[%
    var iter = loadvar($s_iter);
    foreach(var it in iter.__iter__){
        storevar($target, it);
        execute_block($body);
        switch(token)
        {
            case (int) TOKEN.BREAK:
                break;
            case (int) TOKEN.RETURN:
                return;
            token = (int) TOKEN.GO_AHEAD;
        }
        
    }
%]
With(s_resource: int, s_as: int, body: Block)
[%
    var resource = loadvar($s_resource);
    var val = resource.__enter__()
    storevar($s_as, val)
    try
    {
        execute_block(body)
        resource.__exit__(
            MK.Nil(),
            MK.Nil(),
            MK.Nil()
        );
    }
    catch (Exception e)
    {
        var e_boxed = MK.create(e);
        var e_trace = MK.create(error_frames);
        resource.__exit__(
            e_boxed.GetCls,
            e,
            error_frames
        );
    }
%]
DelItem(s_value: int,s_item: int)
[%
    var value = loadvar($s_value);
    var item = loadvar($s_item);
    value.__delitem__(item);
%]
GetItem(target_and_value: int, s_item: int)
[%
    var value = loadvar($target_and_value);
    var item = loadvar($s_item);
    value = value.__getitem__(item);
    storevar($target_and_value, item);
%]
BinaryOp_$T(target_and_left: int, right: int) from {
    add sub mul truediv floordiv mod pow lshift rshift bitor bitand bitxor
}
[%
    var left = loadvar($target_and_left);
    var right = loadvar($right);
    storevar($target_and_left, left.__${T}__(right));
%]
UnaryOp_$T(target_and_value: int) from { invert not }
[%
    var val = loadvar($target_and_value);
    storevar(target_and_value, val.__${T}__);
%]
Dict(target: int, s_kvs: Tuple<int, int>[])
[%
    var dict = new Dictionary<DObj>($s_kvs.Length);
    for(var i = 0; i < $s_kvs.Length; i++){
        var kv = $s_kvs[i];
        dict[loadvar(kv.Item1)] = loadvar(kv.Item2);
    }
    storevar($target, MK.Set(dict));
%]
Set(target: int, s_elts: int[])
[%
    var hset = new HashSet<DObj>($s_elts.Length);
    for(var i = 0; i < $s_elts.Length; i++)
        hset.Add(loadvar(i));
    storevar($target, MK.Set(hset));
%]
List(target: int, s_elts: int[])
[%
    var list = new List<DObj>($s_elts.Length);
    for(var i = 0; i < $s_elts.Length; i++)
        list.Add(loadvar(i));
    storevar($target, MK.List(list));
%]
Generator(target_and_func: int)
[%
    var f = (DFunc) loadvar(target_and_func)
    storevar($target_and_func, create_generator(f));
%]
Call(target: int, s_f: int, s_args: int[])
[%
    var f = loadvar($target);
    if (f is DFunc dfunc){
        check_argcount(dfunc, $s_args.Length);
        var vstack = create_vstack(dfunc, $s_args);
        storevar($target, execute_func(dfunc, vstack));
    }
    var args = new Args();
    for(var i = $s_args - 1; i >= 0; i--)
        args.Prepend(loadvar($s_args[i]));
    storevar($target, f.__call__(args));
%]
Format(target: int, format: int, args: int[])
[%
    var argvals = new string[$args.Length];
    for(var i = 0; i < $args.Length; i++)
        argvals[i] = loadvar($args[i]);
    var str = String.Format(loadstr(format), argvals);
    storevar($target, MK.String(str));
%]
Const(target: int, p_const: int)
[%
    storevar($target, loadconst($p_const));
%]
GetAttr(target_and_value: int, p_attr: int)
[%
    var value = loadvar($target_and_value);
    storevar(target_and_value, value.Get(loadistr($p_attr)));
%]
MoveVar(target: int, slot: int)
[%
    storevar($target, loadvar($slot))
%]
Tuple(target: int, s_elts: int[])
[%
    var tuple = DTuple.Create($s_elts.Length);
    var tuple_elts = tuple.elts;
    for(var i = 0; i < $s_elts.Length; i++)
        tuple_elts[i] = loadvar($s_elts[i]);
    storevar($target, tuple);
%]
PackTuple(targets: int[], s_value: int)
[%
    var tuple = (DTuple) loadvar(s_value);
    var tuple_elts = tuple.elts;
    for(var i = 0; i < tuple_elts.Length; i++)
        storevar(targets[i], tuple_elts[i]);
%]
