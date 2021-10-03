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
        
        while (offset < bound)
        {
        

        var instruction = BYTECODE[offset];
        switch(instruction)
        {
            case (int) CODETAG.Diana_FunctionDef:
            {
                var OFFSET_INC = 2;
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

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_LoadGlobalRef:
            {
                var OFFSET_INC = 2;
                var istr = FromIndex_InternString(BYTECODE[offset + 1]);
                
                push(new DRefGlobal(cur_func.nameSpace, istr));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_DelVar:
            {
                var OFFSET_INC = 2;
                var target = FromIndex_int(BYTECODE[offset + 1]);
                
                storevar(target, null);

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_LoadVar:
            {
                var OFFSET_INC = 2;
                var i = FromIndex_int(BYTECODE[offset + 1]);
                
                push(loadvar(i));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_StoreVar:
            {
                var OFFSET_INC = 2;
                var i = FromIndex_int(BYTECODE[offset + 1]);
                
                storevar(i, pop());

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_Action:
            {
                var OFFSET_INC = 2;
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

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_Return:
            {
                var OFFSET_INC = 1;
                
    
                token = (int) TOKEN.RETURN;
                return;

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_Break:
            {
                var OFFSET_INC = 1;
                
                token = (int) TOKEN.LOOP_BREAK;
                return;

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_Continue:
            {
                var OFFSET_INC = 1;
                
                token = (int) TOKEN.LOOP_CONTINUE;
                return;

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_JumpIfNot:
            {
                var OFFSET_INC = 2;
                var off = FromIndex_int(BYTECODE[offset + 1]);
                
                if(!(pop().__bool__()))
                {
                    offset = off; 
                    goto AFTER_EXEC_CHECK_CFG;
                }

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_JumpIfNot_OrPop:
            {
                var OFFSET_INC = 2;
                var off = FromIndex_int(BYTECODE[offset + 1]);
                
                if(!(peek(0).__bool__()))
                {
                    offset = off; 
                    goto AFTER_EXEC_CHECK_CFG;
                }
                else
                {
                    pop();

                }

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_JumpIf:
            {
                var OFFSET_INC = 2;
                var off = FromIndex_int(BYTECODE[offset + 1]);
                
                if(pop().__bool__())
                {
                    offset = off; 
                    goto AFTER_EXEC_CHECK_CFG;
                }

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_JumpIf_OrPop:
            {
                var OFFSET_INC = 2;
                var off = FromIndex_int(BYTECODE[offset + 1]);
                
                if(peek(0).__bool__())
                {
                    offset = off; 
                    goto AFTER_EXEC_CHECK_CFG;
                }
                else
                {
                    pop();
                }

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_Jump:
            {
                var OFFSET_INC = 2;
                var off = FromIndex_int(BYTECODE[offset + 1]);
                
                offset = off; 
                goto AFTER_EXEC_CHECK_CFG;

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_GetAttr:
            {
                var OFFSET_INC = 2;
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
                var value = pop();
                push(value.__getattr__(attr));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetAttr:
            {
                var OFFSET_INC = 2;
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    value
                );

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Iadd:
            {
                var OFFSET_INC = 2;
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__add__(value)
                );

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Isub:
            {
                var OFFSET_INC = 2;
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__sub__(value)
                );

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Imul:
            {
                var OFFSET_INC = 2;
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__mul__(value)
                );

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Itruediv:
            {
                var OFFSET_INC = 2;
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__truediv__(value)
                );

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Ifloordiv:
            {
                var OFFSET_INC = 2;
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__floordiv__(value)
                );

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Imod:
            {
                var OFFSET_INC = 2;
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__mod__(value)
                );

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Ipow:
            {
                var OFFSET_INC = 2;
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__pow__(value)
                );

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Ilshift:
            {
                var OFFSET_INC = 2;
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__lshift__(value)
                );

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Irshift:
            {
                var OFFSET_INC = 2;
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__rshift__(value)
                );

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Ibitor:
            {
                var OFFSET_INC = 2;
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__bitor__(value)
                );

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Ibitand:
            {
                var OFFSET_INC = 2;
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__bitand__(value)
                );

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetAttr_Ibitxor:
            {
                var OFFSET_INC = 2;
                var attr = FromIndex_InternString(BYTECODE[offset + 1]);
                
                var (value, subject) = pop2();
                subject.__setattr__(
                    attr,
                    subject.__getattr__(attr).__bitxor__(value)
                );

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_DelItem:
            {
                var OFFSET_INC = 1;
                
                var (subject, item) = pop2();
                subject.__delitem__(item);

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_GetItem:
            {
                var OFFSET_INC = 1;
                
                var (subject, item) = pop2();
                push(subject.__getitem__(item));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetItem:
            {
                var OFFSET_INC = 1;
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    value
                );

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Iadd:
            {
                var OFFSET_INC = 1;
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__add__(value)
                );
                push(item);

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Isub:
            {
                var OFFSET_INC = 1;
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__sub__(value)
                );
                push(item);

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Imul:
            {
                var OFFSET_INC = 1;
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__mul__(value)
                );
                push(item);

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Itruediv:
            {
                var OFFSET_INC = 1;
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__truediv__(value)
                );
                push(item);

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Ifloordiv:
            {
                var OFFSET_INC = 1;
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__floordiv__(value)
                );
                push(item);

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Imod:
            {
                var OFFSET_INC = 1;
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__mod__(value)
                );
                push(item);

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Ipow:
            {
                var OFFSET_INC = 1;
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__pow__(value)
                );
                push(item);

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Ilshift:
            {
                var OFFSET_INC = 1;
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__lshift__(value)
                );
                push(item);

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Irshift:
            {
                var OFFSET_INC = 1;
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__rshift__(value)
                );
                push(item);

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Ibitor:
            {
                var OFFSET_INC = 1;
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__bitor__(value)
                );
                push(item);

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Ibitand:
            {
                var OFFSET_INC = 1;
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__bitand__(value)
                );
                push(item);

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_SetItem_Ibitxor:
            {
                var OFFSET_INC = 1;
                
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__bitxor__(value)
                );
                push(item);

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_add:
            {
                var OFFSET_INC = 1;
                
                var (left, right) = pop2();
                push(MK.create(left.__add__(right)));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_sub:
            {
                var OFFSET_INC = 1;
                
                var (left, right) = pop2();
                push(MK.create(left.__sub__(right)));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_mul:
            {
                var OFFSET_INC = 1;
                
                var (left, right) = pop2();
                push(MK.create(left.__mul__(right)));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_truediv:
            {
                var OFFSET_INC = 1;
                
                var (left, right) = pop2();
                push(MK.create(left.__truediv__(right)));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_floordiv:
            {
                var OFFSET_INC = 1;
                
                var (left, right) = pop2();
                push(MK.create(left.__floordiv__(right)));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_mod:
            {
                var OFFSET_INC = 1;
                
                var (left, right) = pop2();
                push(MK.create(left.__mod__(right)));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_pow:
            {
                var OFFSET_INC = 1;
                
                var (left, right) = pop2();
                push(MK.create(left.__pow__(right)));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_lshift:
            {
                var OFFSET_INC = 1;
                
                var (left, right) = pop2();
                push(MK.create(left.__lshift__(right)));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_rshift:
            {
                var OFFSET_INC = 1;
                
                var (left, right) = pop2();
                push(MK.create(left.__rshift__(right)));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_bitor:
            {
                var OFFSET_INC = 1;
                
                var (left, right) = pop2();
                push(MK.create(left.__bitor__(right)));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_bitand:
            {
                var OFFSET_INC = 1;
                
                var (left, right) = pop2();
                push(MK.create(left.__bitand__(right)));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_bitxor:
            {
                var OFFSET_INC = 1;
                
                var (left, right) = pop2();
                push(MK.create(left.__bitxor__(right)));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_gt:
            {
                var OFFSET_INC = 1;
                
                var (left, right) = pop2();
                push(MK.create(left.__gt__(right)));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_lt:
            {
                var OFFSET_INC = 1;
                
                var (left, right) = pop2();
                push(MK.create(left.__lt__(right)));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_ge:
            {
                var OFFSET_INC = 1;
                
                var (left, right) = pop2();
                push(MK.create(left.__ge__(right)));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_le:
            {
                var OFFSET_INC = 1;
                
                var (left, right) = pop2();
                push(MK.create(left.__le__(right)));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_eq:
            {
                var OFFSET_INC = 1;
                
                var (left, right) = pop2();
                push(MK.create(left.__eq__(right)));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_ne:
            {
                var OFFSET_INC = 1;
                
                var (left, right) = pop2();
                push(MK.create(left.__ne__(right)));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_in:
            {
                var OFFSET_INC = 1;
                
                var (left, right) = pop2();
                push(MK.create(right.__contains__(left)));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_notin:
            {
                var OFFSET_INC = 1;
                
                var (left, right) = pop2();
                push(MK.create(!(right.__contains__(left))));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_UnaryOp_invert:
            {
                var OFFSET_INC = 1;
                
                var val = pop();
                push(MK.create(val.__invert__()));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_UnaryOp_not:
            {
                var OFFSET_INC = 1;
                
                var val = pop();
                push(MK.create(val.__not__()));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_UnaryOp_neg:
            {
                var OFFSET_INC = 1;
                
                var val = pop();
                push(MK.create(val.__neg__()));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_MKDict:
            {
                var OFFSET_INC = 2;
                var n = FromIndex_int(BYTECODE[offset + 1]);
                
                var dict = new Dictionary<DObj, DObj>(n);
                for(var i = 0; i < n; i++){
                    var (k, v) = pop2();
                    dict[k] = v;
                }
                push(MK.Dict(dict));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_MKSet:
            {
                var OFFSET_INC = 2;
                var n = FromIndex_int(BYTECODE[offset + 1]);
                
                var hset = new HashSet<DObj>(n);
                popNTo(n, hset);
                push(MK.Set(hset));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_MKList:
            {
                var OFFSET_INC = 2;
                var n = FromIndex_int(BYTECODE[offset + 1]);
                
                var list = popNToList(n);
                push(MK.List(list));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_Call:
            {
                var OFFSET_INC = 2;
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

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_Format:
            {
                var OFFSET_INC = 3;
                var format = FromIndex_int(BYTECODE[offset + 1]);
                var argn = FromIndex_int(BYTECODE[offset + 2]);
                
                var argvals = new string[argn];
                for(var i = 0; i < argn; i++)
                    argvals[argn - i - 1] = pop().__str__(); // TODO: format style
                var str = String.Format(loadstr(format), argvals);
                push(MK.String(str));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_Const:
            {
                var OFFSET_INC = 2;
                var p_const = FromIndex_int(BYTECODE[offset + 1]);
                
                push(loadconst(p_const));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_MKTuple:
            {
                var OFFSET_INC = 2;
                var n = FromIndex_int(BYTECODE[offset + 1]);
                
                var tuple_elts = new DObj[n];
                popNTo(n, tuple_elts);
                push(MK.Tuple(tuple_elts));

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_Pack:
            {
                var OFFSET_INC = 2;
                var n = FromIndex_int(BYTECODE[offset + 1]);
                
                var tuple = (DTuple) pop();
                var tuple_elts = tuple.elts;
                // TODO check exact
                for(var i = 0; i < n; i++)
                    push(tuple_elts[i]);

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_Replicate:
            {
                var OFFSET_INC = 2;
                var n = FromIndex_int(BYTECODE[offset + 1]);
                
                var val = pop();
                for(var i = 0; i < n; i++)
                    push(val);

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_Pop:
            {
                var OFFSET_INC = 1;
                
                pop();

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_TryCatch:
            {
                var OFFSET_INC = 4;
                var unwind_bound = FromIndex_int(BYTECODE[offset + 1]);
                var catch_start = FromIndex_int(BYTECODE[offset + 2]);
                var catch_bound = FromIndex_int(BYTECODE[offset + 3]);
                
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

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_TryFinally:
            {
                var OFFSET_INC = 4;
                var unwind_bound = FromIndex_int(BYTECODE[offset + 1]);
                var final_start = FromIndex_int(BYTECODE[offset + 2]);
                var final_bound = FromIndex_int(BYTECODE[offset + 3]);
                
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

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_TryCatchFinally:
            {
                var OFFSET_INC = 6;
                var unwind_bound = FromIndex_int(BYTECODE[offset + 1]);
                var catch_start = FromIndex_int(BYTECODE[offset + 2]);
                var catch_bound = FromIndex_int(BYTECODE[offset + 3]);
                var final_start = FromIndex_int(BYTECODE[offset + 4]);
                var final_bound = FromIndex_int(BYTECODE[offset + 5]);
                

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

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_Loop:
            {
                var OFFSET_INC = 2;
                var loop_bound = FromIndex_int(BYTECODE[offset + 1]);
                
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

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_For:
            {
                var OFFSET_INC = 2;
                var loop_bound = FromIndex_int(BYTECODE[offset + 1]);
                
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

                
                offset += OFFSET_INC;
                break;
            }
            case (int) CODETAG.Diana_With:
            {
                var OFFSET_INC = 2;
                var with_bound = FromIndex_int(BYTECODE[offset + 1]);
                
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

                
                offset += OFFSET_INC;
                break;
            }
            default:
                throw new InvalidOperationException($"unknown bytecode {instruction}");
            
        }
        AFTER_EXEC_CHECK_CFG:
            if (token != (int) TOKEN.GO_AHEAD)
                return;
        AFTER_EXEC_NO_CFG_CHECK:
            ;
            
        }
    }
}
}