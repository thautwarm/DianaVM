using System;
using System.Collections.Generic;
namespace DianaScript
{
public partial class BlockExecutor
{
    public static void check_argcount(DFunc f, int argcount)
    {
        if (argcount == f.narg)
            return;
        if (argcount > f.narg && f.is_vararg)
            return;
        string f_repr = f.__repr__();
        var expect = (f.is_vararg ? ">=" : "") + $"{f.narg}";
        throw new ArgumentException($"{f_repr} takes {expect} arguments, got {argcount}.");
    }

    int FromIndex_int(int a) => a;
    InternString FromIndex_InternString(int a) => new InternString { identity = a };
    public void exec_code(int[] codes, int bound)
    {
        LOOP_HEAD:
        while (offset < bound)
        {
        var instruction = codes[offset];
        switch(instruction)
        {
            case (int) CODETAG.Diana_FunctionDef:
            {
                var metadataInd = FromIndex_int(codes[offset + 1]);
                var code = FromIndex_int(codes[offset + 2]);
                
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

                
                offset += 3;
                break;
            }
            case (int) CODETAG.Diana_LoadGlobalRef:
            {
                var istr = FromIndex_InternString(codes[offset + 1]);
                
                push(new DRefGlobal(cur_func.nameSpace, istr));

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_DelVar:
            {
                var target = FromIndex_int(codes[offset + 1]);
                
                storevar(target, null);

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_LoadVar:
            {
                var i = FromIndex_int(codes[offset + 1]);
                
                push(loadvar(i));

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_StoreVar:
            {
                var i = FromIndex_int(codes[offset + 1]);
                
                storevar(i, pop());

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_Action:
            {
                var kind = FromIndex_int(codes[offset + 1]);
                
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

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_ControlIf:
            {
                var arg = FromIndex_int(codes[offset + 1]);
                
                if(pop().__bool__())
                    token = arg;

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_JumpIfNot:
            {
                var off = FromIndex_int(codes[offset + 1]);
                
                if(!(pop().__bool__()))
                    offset = off; 

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_JumpIf:
            {
                var off = FromIndex_int(codes[offset + 1]);
                
                if(pop().__bool__())
                    offset = off; 

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_Jump:
            {
                var off = FromIndex_int(codes[offset + 1]);
                
                offset = off; 

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_Control:
            {
                var arg = FromIndex_int(codes[offset + 1]);
                
                token = arg;

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_Try:
            {
                var unwind_start = FromIndex_int(codes[offset + 1]);
                var unwind_stop = FromIndex_int(codes[offset + 2]);
                var errorlabel = FromIndex_int(codes[offset + 3]);
                var finallabel = FromIndex_int(codes[offset + 4]);
                   
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

                
                offset += 5;
                break;
            }
            case (int) CODETAG.Diana_Loop:
            {
                var body = FromIndex_int(codes[offset + 1]);
                
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

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_For:
            {
                var body = FromIndex_int(codes[offset + 1]);
                
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

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_With:
            {
                var body = FromIndex_int(codes[offset + 1]);
                
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

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_GetAttr:
            {
                var attr = FromIndex_InternString(codes[offset + 1]);
                
                var value = pop();
                push(value.__getattr__(attr));

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_SetAttr:
            {
                var attr = FromIndex_InternString(codes[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    value
                );

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Iadd:
            {
                var attr = FromIndex_InternString(codes[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__add__(value)
                );

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Isub:
            {
                var attr = FromIndex_InternString(codes[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__sub__(value)
                );

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Imul:
            {
                var attr = FromIndex_InternString(codes[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__mul__(value)
                );

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Itruediv:
            {
                var attr = FromIndex_InternString(codes[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__truediv__(value)
                );

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Ifloordiv:
            {
                var attr = FromIndex_InternString(codes[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__floordiv__(value)
                );

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Imod:
            {
                var attr = FromIndex_InternString(codes[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__mod__(value)
                );

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Ipow:
            {
                var attr = FromIndex_InternString(codes[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__pow__(value)
                );

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Ilshift:
            {
                var attr = FromIndex_InternString(codes[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__lshift__(value)
                );

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Irshift:
            {
                var attr = FromIndex_InternString(codes[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__rshift__(value)
                );

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Ibitor:
            {
                var attr = FromIndex_InternString(codes[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__bitor__(value)
                );

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Ibitand:
            {
                var attr = FromIndex_InternString(codes[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__bitand__(value)
                );

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Ibitxor:
            {
                var attr = FromIndex_InternString(codes[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__bitxor__(value)
                );

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_DelItem:
            {
                
                var (subject, item) = pop2();
                subject.__delitem__(item);

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_GetItem:
            {
                
                var (subject, item) = pop2();
                push(subject.__getitem__(item));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_SetItem:
            {
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    value
                );

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Iadd:
            {
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__add__(value)
                );
                push(item);

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Isub:
            {
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__sub__(value)
                );
                push(item);

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Imul:
            {
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__mul__(value)
                );
                push(item);

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Itruediv:
            {
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__truediv__(value)
                );
                push(item);

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Ifloordiv:
            {
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__floordiv__(value)
                );
                push(item);

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Imod:
            {
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__mod__(value)
                );
                push(item);

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Ipow:
            {
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__pow__(value)
                );
                push(item);

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Ilshift:
            {
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__lshift__(value)
                );
                push(item);

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Irshift:
            {
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__rshift__(value)
                );
                push(item);

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Ibitor:
            {
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__bitor__(value)
                );
                push(item);

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Ibitand:
            {
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__bitand__(value)
                );
                push(item);

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Ibitxor:
            {
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__bitxor__(value)
                );
                push(item);

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_add:
            {
                
                var (left, right) = pop2();
                push(MK.create(left.__add__(right)));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_sub:
            {
                
                var (left, right) = pop2();
                push(MK.create(left.__sub__(right)));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_mul:
            {
                
                var (left, right) = pop2();
                push(MK.create(left.__mul__(right)));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_truediv:
            {
                
                var (left, right) = pop2();
                push(MK.create(left.__truediv__(right)));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_floordiv:
            {
                
                var (left, right) = pop2();
                push(MK.create(left.__floordiv__(right)));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_mod:
            {
                
                var (left, right) = pop2();
                push(MK.create(left.__mod__(right)));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_pow:
            {
                
                var (left, right) = pop2();
                push(MK.create(left.__pow__(right)));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_lshift:
            {
                
                var (left, right) = pop2();
                push(MK.create(left.__lshift__(right)));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_rshift:
            {
                
                var (left, right) = pop2();
                push(MK.create(left.__rshift__(right)));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_bitor:
            {
                
                var (left, right) = pop2();
                push(MK.create(left.__bitor__(right)));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_bitand:
            {
                
                var (left, right) = pop2();
                push(MK.create(left.__bitand__(right)));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_bitxor:
            {
                
                var (left, right) = pop2();
                push(MK.create(left.__bitxor__(right)));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_gt:
            {
                
                var (left, right) = pop2();
                push(MK.create(left.__gt__(right)));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_lt:
            {
                
                var (left, right) = pop2();
                push(MK.create(left.__lt__(right)));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_ge:
            {
                
                var (left, right) = pop2();
                push(MK.create(left.__ge__(right)));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_le:
            {
                
                var (left, right) = pop2();
                push(MK.create(left.__le__(right)));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_eq:
            {
                
                var (left, right) = pop2();
                push(MK.create(left.__eq__(right)));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_ne:
            {
                
                var (left, right) = pop2();
                push(MK.create(left.__ne__(right)));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_in:
            {
                
                var (left, right) = pop2();
                push(MK.create(right.__contains__(left)));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_notin:
            {
                
                var (left, right) = pop2();
                push(MK.create(!(right.__contains__(left))));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_UnaryOp_invert:
            {
                
                var val = pop();
                push(MK.create(val.__invert__()));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_UnaryOp_not:
            {
                
                var val = pop();
                push(MK.create(val.__not__()));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_UnaryOp_neg:
            {
                
                var val = pop();
                push(MK.create(val.__neg__()));

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_MKDict:
            {
                var n = FromIndex_int(codes[offset + 1]);
                
                var dict = new Dictionary<DObj, DObj>(n);
                for(var i = 0; i < n; i++){
                    var (k, v) = pop2();
                    dict[k] = v;
                }
                push(MK.Dict(dict));

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_MKSet:
            {
                var n = FromIndex_int(codes[offset + 1]);
                
                var hset = new HashSet<DObj>(n);
                popNTo(n, hset);
                push(MK.Set(hset));

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_MKList:
            {
                var n = FromIndex_int(codes[offset + 1]);
                
                var list = popNToList(n);
                push(MK.List(list));

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_Call:
            {
                var n = FromIndex_int(codes[offset + 1]);
                
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

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_Format:
            {
                var format = FromIndex_int(codes[offset + 1]);
                var argn = FromIndex_int(codes[offset + 2]);
                
                var argvals = new string[argn];
                for(var i = 0; i < argn; i++)
                    argvals[argn - i - 1] = pop().__str__(); // TODO: format style
                var str = String.Format(loadstr(format), argvals);
                push(MK.String(str));

                
                offset += 3;
                break;
            }
            case (int) CODETAG.Diana_Const:
            {
                var p_const = FromIndex_int(codes[offset + 1]);
                
                push(loadconst(p_const));

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_MKTuple:
            {
                var n = FromIndex_int(codes[offset + 1]);
                
                var tuple_elts = new DObj[n];
                popNTo(n, tuple_elts);
                push(MK.Tuple(tuple_elts));

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_Pack:
            {
                var n = FromIndex_int(codes[offset + 1]);
                
                var tuple = (DTuple) pop();
                var tuple_elts = tuple.elts;
                // TODO check exact
                for(var i = 0; i < n; i++)
                    push(tuple_elts[i]);

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_Replicate:
            {
                var n = FromIndex_int(codes[offset + 1]);
                
                var val = pop();
                for(var i = 0; i < n; i++)
                    push(val);

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_Pop:
            {
                
                pop();

                
                offset += 1;
                break;
            }
            default:
                throw new InvalidOperationException($"unknown bytecode {instruction}");
            
        }
        }
    }
}
}