// eval stmt return
// 0 : return
// 1 : go ahead
// 2 : call frame
// 3 : continue loop
// 4 : break loop
// 5 : reraise exception

// loadmetadata
// storevar, loadvar, loadstr, loadistr
// set_return, exec_block
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

dataclass Catch(exc_target: int, exc_type: int, body: int)
dataclass FuncMeta(
    is_vararg: bool,
    freeslots: int[],
    narg: int,
    nlocal: int,
    name: InternString,
    modname: InternString,
    filename: string,
    lineno: int,
    freenames: string[],
    localnames: string[]
)
dataclass Loc(location_data: (int, int)[])
dataclass Block(codes: Ptr[], location_data: (int, int)[], filename: string)

FunctionDef(target: int, metadataInd: int, code: int)
[%
    var meta = loadmetadata($metadataInd);
    int nfree = meta.freeslots.Length;

    var new_freevals = new DRef[meta.freeslots.Length];
    for(var i = 0; i < nfree; i++)
        new_freevals[i] = loadref(meta.freeslots[i]);

    var dfunc = DFunc.Make($code,
        freevals:new_freevals,
        is_vararg: meta.is_vararg,
        narg: meta.narg,
        nlocal: meta.nlocal,
        metadataInd: $metadataInd);
        
    storevar($target, dfunc);
%]
Return(reg: int)
[%
    set_return(loadvar($reg));
    token = (int) TOKEN.RETURN;
%]
DelVar(target: int)
[%
    storevar($target, null);
%]
SetVar(target: int, p_val: int)
[%
    storevar($target, loadvar($p_val));
%]
JumpIf(p_val: int, offset: int)
[%
    if (loadvar($p_val).__bool__)
        offset = $offset;
%]
Jump(offset: int)
[%
    offset = $offset;
%]
Raise(p_exc: int)
[%
    var e = MK.unbox<Exception>(loadvar($p_exc));
    throw e;
%]
Assert(value: int, p_msg: int)
[%
    assert(loadvar($value).__bool__, loadvar($p_msg));
%]
Control(token: int) // break, continue, reraise
[%
    token = $token;
%]
Try(body: int, except_handlers: Catch[], final_body: int)
[%
        try
        {
            exec_block($body);
        }
        catch (Exception e)
        {
            foreach(var handler in $except_handlers){
                var exc_type = loadvar(handler.exc_type);
                var e_boxed = MK.create(e);
                if (exc_type.__subclasscheck__(e_boxed.GetCls))
                {
                    storevar(handler.exc_target, e_boxed);
                    exec_block(handler.body);
                    storevar(handler.exc_target, null);
                    virtual_machine.errorFrames.Clear();
                    goto end_handled;
                }
            }
            throw;
            end_handled: ;
        }
        finally{
            exec_block($final_body);
        }
%]
For(target: int, p_iter: int, body: int)
[%
    var iter = loadvar($p_iter);
    foreach(var it in iter.__iter__){
        storevar($target, it);
        exec_block($body);
        switch(token)
        {
            case (int) TOKEN.LOOP_BREAK:
                break;
            case (int) TOKEN.RETURN:
                return;
            default:
                token = (int) TOKEN.GO_AHEAD;
                break;
        }
        
    }
%]
With(p_resource: int, p_as: int, body: int)
[%
    var resource = loadvar($p_resource);
    var val = resource.__enter__();
    storevar($p_as, val);
    try
    {
        exec_block($body);
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
            e_boxed.GetCls,
            e_boxed,
            e_trace
        );
    }
%]
DelItem(p_value: int,p_item: int)
[%
    var value = loadvar($p_value);
    var item = loadvar($p_item);
    value.__delitem__(item);
%]
GetItem(target: int, p_value: int, p_item: int)
[%
    var value = loadvar($p_value);
    var item = loadvar($p_item);
    value = value.__getitem__(item);
    storevar($target, item);
%]
BinaryOp_$T(target: int, left: int, right: int) from {
    add sub mul truediv floordiv mod pow lshift rshift bitor bitand bitxor
}
[%
    var left = loadvar($left);
    var right = loadvar($right);
    storevar($target, left.__${T}__(right));
%]
UnaryOp_$T(target: int, p_value: int) from { invert not }
[%
    var val = loadvar($p_value);
    storevar($target, MK.create(val.__${T}__));
%]
Dict(target: int, p_kvs: Tuple<int, int>[])
[%
    var dict = new Dictionary<DObj, DObj>($p_kvs.Length);
    for(var i = 0; i < $p_kvs.Length; i++){
        var kv = $p_kvs[i];
        dict[loadvar(kv.Item1)] = loadvar(kv.Item2);
    }
    storevar($target, MK.Dict(dict));
%]
Set(target: int, p_elts: int[])
[%
    var hset = new HashSet<DObj>($p_elts.Length);
    for(var i = 0; i < $p_elts.Length; i++)
        hset.Add(loadvar(i));
    storevar($target, MK.Set(hset));
%]
List(target: int, p_elts: int[])
[%
    var list = new List<DObj>($p_elts.Length);
    for(var i = 0; i < $p_elts.Length; i++)
        list.Add(loadvar(i));
    storevar($target, MK.List(list));
%]
Call(target: int, p_f: int, p_args: int[])
[%
    var f = loadvar($p_f);
    if (f is DFunc dfunc){
        check_argcount(dfunc, $p_args.Length);
        var vstack = create_vstack(dfunc, $p_args);
        storevar($target, virtual_machine.exec_func(dfunc, vstack));
    }
    var args = new Args();
    for(var i = $p_args.Length - 1; i >= 0; i--)
        args.Prepend(loadvar($p_args[i]));
    storevar($target, f.__call__(args));
%]
Format(target: int, format: int, args: int[])
[%
    var argvals = new string[$args.Length];
    for(var i = 0; i < $args.Length; i++)
        argvals[i] = loadvar($args[i]).__str__; // TODO: format repr
    var str = String.Format(loadstr($format), argvals);
    storevar($target, MK.String(str));
%]
Const(target: int, p_const: int)
[%
    storevar($target, loadconst($p_const));
%]
GetAttr(target: int, p_value: int,  p_attr: int)
[%
    var value = loadvar($p_value);
    storevar($target, value.Get(loadistr($p_attr)));
%]
MoveVar(target: int, slot: int)
[%
    storevar($target, loadvar($slot));
%]
Tuple(target: int, p_elts: int[])
[%
    var tuple_elts = new DObj[$p_elts.Length];
    for(var i = 0; i < $p_elts.Length; i++)
        tuple_elts[i] = loadvar($p_elts[i]);
    storevar($target, MK.Tuple(tuple_elts));
%]
PackTuple(targets: int[], p_value: int)
[%
    var tuple = (DTuple) loadvar($p_value);
    var tuple_elts = tuple.elts;
    for(var i = 0; i < tuple_elts.Length; i++)
        storevar($targets[i], tuple_elts[i]);
%]
