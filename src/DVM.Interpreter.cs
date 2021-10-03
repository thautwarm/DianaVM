using System;
using System.Collections.Generic;
namespace DianaScript
{
public partial class BlockExecutor
{

    int FromIndex_int(int a) => a;
    InternString FromIndex_InternString(int a) => new InternString { identity = a };

    public void exec(int[] codes) => exec(codes, codes.Length);
    public void exec(int[] BYTECODE, int bound)
    {
        LOOP_HEAD:
        while (offset < bound)
        {
        var instruction = BYTECODE[offset];
        switch(instruction)
        {
            case (int) CODETAG.Diana_FunctionDef:
            {
                var metadataInd = FromIndex_int(BYTECODE[offset + 1]);
                
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

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_LoadGlobalRef:
            {
                var istr = FromIndex_InternString(BYTECODE[offset + 1]);
                
                push(new DRefGlobal(cur_func.nameSpace, istr));

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_DelVar:
            {
                var target = FromIndex_int(BYTECODE[offset + 1]);
                
                storevar(target, null);

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_LoadVar:
            {
                var i = FromIndex_int(BYTECODE[offset + 1]);
                
                push(loadvar(i));

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_StoreVar:
            {
                var i = FromIndex_int(BYTECODE[offset + 1]);
                
                storevar(i, pop());

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_Action:
            {
                var kind = FromIndex_int(BYTECODE[offset + 1]);
                
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
            case (int) CODETAG.Diana_Return:
            {
                
    
                token = (int) TOKEN.RETURN;
                return;

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_Break:
            {
                
                token = (int) TOKEN.LOOP_BREAK;
                return;

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_Continue:
            {
                
                token = (int) TOKEN.LOOP_CONTINUE;
                return;

                
                offset += 1;
                break;
            }
            case (int) CODETAG.Diana_JumpIfNot:
            {
                var off = FromIndex_int(BYTECODE[offset + 1]);
                
                if(!(pop().__bool__()))
                {
                    offset = off; 
                    goto LOOP_HEAD;
                }


                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_JumpIf:
            {
                var off = FromIndex_int(BYTECODE[offset + 1]);
                
                if(pop().__bool__())
                {
                    offset = off; 
                    goto LOOP_HEAD;
                }

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_Jump:
            {
                var off = FromIndex_int(BYTECODE[offset + 1]);
                
                offset = off; 
                goto LOOP_HEAD;

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_TryCatch:
            {
                var unwind_bound = FromIndex_int(BYTECODE[offset + 1]);
                var catch_start = FromIndex_int(BYTECODE[offset + 2]);
                var catch_bound = FromIndex_int(BYTECODE[offset + 3]);
                
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

                
                offset += 4;
                break;
            }
            case (int) CODETAG.Diana_TryFinally:
            {
                var unwind_bound = FromIndex_int(BYTECODE[offset + 1]);
                var final_start = FromIndex_int(BYTECODE[offset + 2]);
                var final_bound = FromIndex_int(BYTECODE[offset + 3]);
                
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

                
                offset += 4;
                break;
            }
            case (int) CODETAG.Diana_TryCatchFinally:
            {
                var unwind_bound = FromIndex_int(BYTECODE[offset + 1]);
                var catch_start = FromIndex_int(BYTECODE[offset + 2]);
                var catch_bound = FromIndex_int(BYTECODE[offset + 3]);
                var final_start = FromIndex_int(BYTECODE[offset + 4]);
                var final_bound = FromIndex_int(BYTECODE[offset + 5]);
                

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
                finally
                {
                    clearstack();
                    offset = final_start;
                    exec(BYTECODE, final_bound);
                }

                
                offset += 6;
                break;
            }
            case (int) CODETAG.Diana_Loop:
            {
                var loop_bound = FromIndex_int(BYTECODE[offset + 1]);
                
                while(true)
                {
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
                loop_end: ;

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_For:
            {
                var for_bound = FromIndex_int(BYTECODE[offset + 1]);
                
                var iter = pop();
                foreach(var it in iter.__iter__())
                {
                    push(it);
                    exec(BYTECODE, for_bound);
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
                var with_bound = FromIndex_int(BYTECODE[offset + 1]);
                
                var resource = pop();
                var item = resource.__enter__();
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

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_GetAttr:
            {
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
                var value = pop();
                push(value.__getattr__(attr));

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_SetAttr:
            {
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
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
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
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
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
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
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
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
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
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
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
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
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
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
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
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
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
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
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
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
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
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
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
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
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
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
                var n = FromIndex_int(BYTECODE[offset + 1]);
                
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
                var n = FromIndex_int(BYTECODE[offset + 1]);
                
                var hset = new HashSet<DObj>(n);
                popNTo(n, hset);
                push(MK.Set(hset));

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_MKList:
            {
                var n = FromIndex_int(BYTECODE[offset + 1]);
                
                var list = popNToList(n);
                push(MK.List(list));

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_Call:
            {
                var n = FromIndex_int(BYTECODE[offset + 1]);
                
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
                var format = FromIndex_int(BYTECODE[offset + 1]);
                var argn = FromIndex_int(BYTECODE[offset + 2]);
                
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
                var p_const = FromIndex_int(BYTECODE[offset + 1]);
                
                push(loadconst(p_const));

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_MKTuple:
            {
                var n = FromIndex_int(BYTECODE[offset + 1]);
                
                var tuple_elts = new DObj[n];
                popNTo(n, tuple_elts);
                push(MK.Tuple(tuple_elts));

                
                offset += 2;
                break;
            }
            case (int) CODETAG.Diana_Pack:
            {
                var n = FromIndex_int(BYTECODE[offset + 1]);
                
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
                var n = FromIndex_int(BYTECODE[offset + 1]);
                
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