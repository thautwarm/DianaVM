// begin
using System;
using System.Collections.Generic;

namespace DianaScript
{
    using NameSpace = Dictionary<InternString, DObj>;
    using static TOKEN;
    using static VMHelper;

    public static class VMHelper
    {
        public static void check_argcount(DFunc f, int argcount)
        {
            if (argcount >= f.narg)
                return;
            string f_repr = ((DObj)f).__repr__;
            var expect = (f.is_vararg ? ">=" : "") + $"{f.narg}";
            throw new ArgumentException($"{f_repr} takes expect {expect} arguments, got {argcount}.");
        }
    }
    public enum TOKEN
    {
        RETURN,
        GO_AHEAD,
        LOOP_BREAK,
        LOOP_CONTINUE
    }

    public struct MiniFrame
    {
        public int blockind;
        public int offset;

        public int metadataInd;

    }


    public partial class DVM
    {

        // stack of (blockind, offset)
        public Stack<MiniFrame> errorFrames;

        public DFlatGraphCode flatGraph;

        public static DFlatGraphCode FlatGraph; // should be the same as the instance field

        public DVM(DFlatGraphCode flatGraph)
        {
            this.flatGraph = flatGraph;
            DVM.FlatGraph = flatGraph;
            this.errorFrames = new Stack<MiniFrame>();
        }

        public DObj exec_func(DFunc dfunc, DObj[] vstack)
        {
            var executor = new BlockExecutor
            {
                virtual_machine = this,
                offset = 0,
                token = (int)GO_AHEAD,
                vstack = vstack,
                cur_func = dfunc,
                @return = null,
                flatGraph = flatGraph
            };
            executor.exec_block(dfunc.body);
            if (executor.@return == null)
                return MK.Nil();
            return executor.@return;
        }
    }


    // like frame
    public partial class BlockExecutor
    {

        public DVM virtual_machine;
        public int offset;
        public int token;
        public DFunc cur_func;
        public DObj[] vstack;
        public DObj @return;
        public DFlatGraphCode flatGraph;

        public const int bit_nonlocal = 0b01;
        public const int bit_classify = 0b01 << 1;

        DRef loadref(int slot)
        {
            if ((slot & bit_nonlocal) == 0) // slot & bit_classify must be true
                return vstack[slot >> 2] as DRef;
            else
                return cur_func.freevals[slot >> 2];
        }

        DObj loadconst(int slot)
        {
            return flatGraph.dobjs[slot];
        }

        DObj loadvar(int slot)
        {
            DObj c;
            if ((slot & bit_nonlocal) == 0) 
            {
                if ((slot & bit_classify) == 0)
                    c = vstack[slot >> 2]; // local
                else
                {
                    DRef r = vstack[slot >> 2] as DRef; 
                    c = r.cell_contents; // local cell
                    
                }
                if (c == null)
                {
                    var name = flatGraph.funcmetas[cur_func.metadataInd].localnames[slot >> 2];
                    throw new NullReferenceException($"undefined variable {name}.");
                }
            }
            else
            {
                if ((slot & bit_classify) == 0) // freevals
                {
                    c = cur_func.freevals[slot >> 2].cell_contents; 
                    if (c == null)
                    {
                        var name = flatGraph.funcmetas[cur_func.metadataInd].freenames[slot >> 2];
                        throw new NullReferenceException($"undefined cell variable {name}.");
                    }
                }
                else // global
                {
                   c = cur_func.nameSpace[flatGraph.internstrings[slot >> 2]];
                    
                }
            }
            return c;
        }
        

        void storevar(int slot, DObj o)
        {
            if ((slot & bit_nonlocal) == 0) 
            {
                if ((slot & bit_classify) == 0)
                    vstack[slot >> 2] = o; // local
                else
                {
                    DRef r = vstack[slot >> 2] as DRef; 
                    r.cell_contents = o; // local cell
                    
                }
            }
            else
            {
                if ((slot & bit_classify) == 0) // freevals
                {
                    cur_func.freevals[slot >> 2].cell_contents = o; // local
                }
                else // global
                {
                   cur_func.nameSpace[flatGraph.internstrings[slot >> 2]] = o;
                    
                }
            }
        }

        FuncMeta loadmetadata(int slot) => flatGraph.funcmetas[slot];
        string loadstr(int slot) => flatGraph.strings[slot];
        InternString loadistr(int slot) => flatGraph.internstrings[slot];
        void set_return(DObj val) => @return = val;
        DObj[] create_vstack(DFunc func, int[] s_args)
        {
            var locals = new DObj[func.nlocal];
            for (var i = 0; i < func.narg; i++)
            {
                locals[i] = loadvar(s_args[i]);
            }
            if (func.is_vararg)
            {
                var vararg = new DObj[s_args.Length - func.narg];
                for (var i = func.narg; i < s_args.Length; i++)
                {
                    vararg[i - func.narg] = loadvar(i);
                }
                locals[func.narg] = DTuple.Make(vararg);
            }
            return locals;
        }
        public void exec_block(int blockind)
        {
            int old_offset = offset;
            var codes = flatGraph.blocks[blockind].codes;
            try
            {
                for (var i = 0; i < codes.Length; i++)
                {
                    offset = i;
                    exec_code(codes[i]);
                    if (token != (int)GO_AHEAD)
                        return;
                }
            }
            catch
            {
                virtual_machine.errorFrames.Push(new MiniFrame { 
                    metadataInd = cur_func.metadataInd,
                    blockind = blockind,
                    offset = offset
                });
                throw;
            }
        }
        public void assert(bool val, DObj msg)
        {
            if (!val)
                throw new Exception("assertion failed" + msg.__str__);
        }
        public void exec_code(Ptr curPtr)
        {
        switch(curPtr.kind)
        {
            case (int) CODE.Diana_FunctionDef:
            {
            
                var meta = loadmetadata(flatGraph.diana_functiondefs[curPtr.ind].metadataInd);
                int nfree = meta.freeslots.Length;

                var new_freevals = new DRef[meta.freeslots.Length];
                for(var i = 0; i < nfree; i++)
                    new_freevals[i] = loadref(meta.freeslots[i]);

                var dfunc = DFunc.Make(flatGraph.diana_functiondefs[curPtr.ind].code,
                    freevals:new_freevals,
                    is_vararg: meta.is_vararg,
                    narg: meta.narg,
                    nlocal: meta.nlocal,
                    metadataInd: flatGraph.diana_functiondefs[curPtr.ind].metadataInd);
        
                storevar(flatGraph.diana_functiondefs[curPtr.ind].target, dfunc);

                break;
            }
            case (int) CODE.Diana_Return:
            {
            
                set_return(loadvar(flatGraph.diana_returns[curPtr.ind].reg));
                token = (int) TOKEN.RETURN;

                break;
            }
            case (int) CODE.Diana_DelVar:
            {
            
                storevar(flatGraph.diana_delvars[curPtr.ind].target, null);

                break;
            }
            case (int) CODE.Diana_LoadAsCell:
            {
            
                var refobj = new DRef();
                refobj.cell_contents = loadvar(flatGraph.diana_loadascells[curPtr.ind].target);
                storevar(flatGraph.diana_loadascells[curPtr.ind].target, refobj);

                break;
            }
            case (int) CODE.Diana_LoadGlobalRef:
            {
            
                storevar(flatGraph.diana_loadglobalrefs[curPtr.ind].target, new DRefGlobal(cur_func.nameSpace, loadistr(flatGraph.diana_loadglobalrefs[curPtr.ind].p_val)));

                break;
            }
            case (int) CODE.Diana_LoadVar:
            {
            
                storevar(flatGraph.diana_loadvars[curPtr.ind].target, loadvar(flatGraph.diana_loadvars[curPtr.ind].p_val));

                break;
            }
            case (int) CODE.Diana_Raise:
            {
            
                var e = MK.unbox<Exception>(loadvar(flatGraph.diana_raises[curPtr.ind].p_exc));
                throw e;

                break;
            }
            case (int) CODE.Diana_Assert:
            {
            
                assert(loadvar(flatGraph.diana_asserts[curPtr.ind].value).__bool__, loadvar(flatGraph.diana_asserts[curPtr.ind].p_msg));

                break;
            }
            case (int) CODE.Diana_Control:
            {
            
                token = flatGraph.diana_controls[curPtr.ind].token;

                break;
            }
            case (int) CODE.Diana_Try:
            {
            
                    try
                    {
                        exec_block(flatGraph.diana_trys[curPtr.ind].body);
                    }
                    catch (Exception e)
                    {
                        foreach(var handler in flatGraph.diana_trys[curPtr.ind].except_handlers){
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
                        exec_block(flatGraph.diana_trys[curPtr.ind].final_body);
                    }

                break;
            }
            case (int) CODE.Diana_While:
            {
            
                while(loadvar(flatGraph.diana_whiles[curPtr.ind].p_cond).__bool__)
                {
                    exec_block(flatGraph.diana_whiles[curPtr.ind].body);
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

                break;
            }
            case (int) CODE.Diana_For:
            {
            
                var iter = loadvar(flatGraph.diana_fors[curPtr.ind].p_iter);
                foreach(var it in iter.__iter__){
                    storevar(flatGraph.diana_fors[curPtr.ind].target, it);
                    exec_block(flatGraph.diana_fors[curPtr.ind].body);
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

                break;
            }
            case (int) CODE.Diana_With:
            {
            
                var resource = loadvar(flatGraph.diana_withs[curPtr.ind].p_resource);
                var val = resource.__enter__();
                storevar(flatGraph.diana_withs[curPtr.ind].p_as, val);
                try
                {
                    exec_block(flatGraph.diana_withs[curPtr.ind].body);
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

                break;
            }
            case (int) CODE.Diana_DelItem:
            {
            
                var value = loadvar(flatGraph.diana_delitems[curPtr.ind].p_value);
                var item = loadvar(flatGraph.diana_delitems[curPtr.ind].p_item);
                value.__delitem__(item);

                break;
            }
            case (int) CODE.Diana_GetItem:
            {
            
                var value = loadvar(flatGraph.diana_getitems[curPtr.ind].p_value);
                var item = loadvar(flatGraph.diana_getitems[curPtr.ind].p_item);
                value = value.__getitem__(item);
                storevar(flatGraph.diana_getitems[curPtr.ind].target, item);

                break;
            }
            case (int) CODE.Diana_BinaryOp_add:
            {
            
                var left = loadvar(flatGraph.diana_binaryop_adds[curPtr.ind].left);
                var right = loadvar(flatGraph.diana_binaryop_adds[curPtr.ind].right);
                storevar(flatGraph.diana_binaryop_adds[curPtr.ind].target, left.__add__(right));

                break;
            }
            case (int) CODE.Diana_BinaryOp_sub:
            {
            
                var left = loadvar(flatGraph.diana_binaryop_subs[curPtr.ind].left);
                var right = loadvar(flatGraph.diana_binaryop_subs[curPtr.ind].right);
                storevar(flatGraph.diana_binaryop_subs[curPtr.ind].target, left.__sub__(right));

                break;
            }
            case (int) CODE.Diana_BinaryOp_mul:
            {
            
                var left = loadvar(flatGraph.diana_binaryop_muls[curPtr.ind].left);
                var right = loadvar(flatGraph.diana_binaryop_muls[curPtr.ind].right);
                storevar(flatGraph.diana_binaryop_muls[curPtr.ind].target, left.__mul__(right));

                break;
            }
            case (int) CODE.Diana_BinaryOp_truediv:
            {
            
                var left = loadvar(flatGraph.diana_binaryop_truedivs[curPtr.ind].left);
                var right = loadvar(flatGraph.diana_binaryop_truedivs[curPtr.ind].right);
                storevar(flatGraph.diana_binaryop_truedivs[curPtr.ind].target, left.__truediv__(right));

                break;
            }
            case (int) CODE.Diana_BinaryOp_floordiv:
            {
            
                var left = loadvar(flatGraph.diana_binaryop_floordivs[curPtr.ind].left);
                var right = loadvar(flatGraph.diana_binaryop_floordivs[curPtr.ind].right);
                storevar(flatGraph.diana_binaryop_floordivs[curPtr.ind].target, left.__floordiv__(right));

                break;
            }
            case (int) CODE.Diana_BinaryOp_mod:
            {
            
                var left = loadvar(flatGraph.diana_binaryop_mods[curPtr.ind].left);
                var right = loadvar(flatGraph.diana_binaryop_mods[curPtr.ind].right);
                storevar(flatGraph.diana_binaryop_mods[curPtr.ind].target, left.__mod__(right));

                break;
            }
            case (int) CODE.Diana_BinaryOp_pow:
            {
            
                var left = loadvar(flatGraph.diana_binaryop_pows[curPtr.ind].left);
                var right = loadvar(flatGraph.diana_binaryop_pows[curPtr.ind].right);
                storevar(flatGraph.diana_binaryop_pows[curPtr.ind].target, left.__pow__(right));

                break;
            }
            case (int) CODE.Diana_BinaryOp_lshift:
            {
            
                var left = loadvar(flatGraph.diana_binaryop_lshifts[curPtr.ind].left);
                var right = loadvar(flatGraph.diana_binaryop_lshifts[curPtr.ind].right);
                storevar(flatGraph.diana_binaryop_lshifts[curPtr.ind].target, left.__lshift__(right));

                break;
            }
            case (int) CODE.Diana_BinaryOp_rshift:
            {
            
                var left = loadvar(flatGraph.diana_binaryop_rshifts[curPtr.ind].left);
                var right = loadvar(flatGraph.diana_binaryop_rshifts[curPtr.ind].right);
                storevar(flatGraph.diana_binaryop_rshifts[curPtr.ind].target, left.__rshift__(right));

                break;
            }
            case (int) CODE.Diana_BinaryOp_bitor:
            {
            
                var left = loadvar(flatGraph.diana_binaryop_bitors[curPtr.ind].left);
                var right = loadvar(flatGraph.diana_binaryop_bitors[curPtr.ind].right);
                storevar(flatGraph.diana_binaryop_bitors[curPtr.ind].target, left.__bitor__(right));

                break;
            }
            case (int) CODE.Diana_BinaryOp_bitand:
            {
            
                var left = loadvar(flatGraph.diana_binaryop_bitands[curPtr.ind].left);
                var right = loadvar(flatGraph.diana_binaryop_bitands[curPtr.ind].right);
                storevar(flatGraph.diana_binaryop_bitands[curPtr.ind].target, left.__bitand__(right));

                break;
            }
            case (int) CODE.Diana_BinaryOp_bitxor:
            {
            
                var left = loadvar(flatGraph.diana_binaryop_bitxors[curPtr.ind].left);
                var right = loadvar(flatGraph.diana_binaryop_bitxors[curPtr.ind].right);
                storevar(flatGraph.diana_binaryop_bitxors[curPtr.ind].target, left.__bitxor__(right));

                break;
            }
            case (int) CODE.Diana_UnaryOp_invert:
            {
            
                var val = loadvar(flatGraph.diana_unaryop_inverts[curPtr.ind].p_value);
                storevar(flatGraph.diana_unaryop_inverts[curPtr.ind].target, MK.create(val.__invert__));

                break;
            }
            case (int) CODE.Diana_UnaryOp_not:
            {
            
                var val = loadvar(flatGraph.diana_unaryop_nots[curPtr.ind].p_value);
                storevar(flatGraph.diana_unaryop_nots[curPtr.ind].target, MK.create(val.__not__));

                break;
            }
            case (int) CODE.Diana_Dict:
            {
            
                var dict = new Dictionary<DObj, DObj>(flatGraph.diana_dicts[curPtr.ind].p_kvs.Length);
                for(var i = 0; i < flatGraph.diana_dicts[curPtr.ind].p_kvs.Length; i++){
                    var kv = flatGraph.diana_dicts[curPtr.ind].p_kvs[i];
                    dict[loadvar(kv.Item1)] = loadvar(kv.Item2);
                }
                storevar(flatGraph.diana_dicts[curPtr.ind].target, MK.Dict(dict));

                break;
            }
            case (int) CODE.Diana_Set:
            {
            
                var hset = new HashSet<DObj>(flatGraph.diana_sets[curPtr.ind].p_elts.Length);
                for(var i = 0; i < flatGraph.diana_sets[curPtr.ind].p_elts.Length; i++)
                    hset.Add(loadvar(i));
                storevar(flatGraph.diana_sets[curPtr.ind].target, MK.Set(hset));

                break;
            }
            case (int) CODE.Diana_List:
            {
            
                var list = new List<DObj>(flatGraph.diana_lists[curPtr.ind].p_elts.Length);
                for(var i = 0; i < flatGraph.diana_lists[curPtr.ind].p_elts.Length; i++)
                    list.Add(loadvar(i));
                storevar(flatGraph.diana_lists[curPtr.ind].target, MK.List(list));

                break;
            }
            case (int) CODE.Diana_Call:
            {
            
                var f = loadvar(flatGraph.diana_calls[curPtr.ind].p_f);
                if (f is DFunc dfunc){
                    check_argcount(dfunc, flatGraph.diana_calls[curPtr.ind].p_args.Length);
                    var vstack = create_vstack(dfunc, flatGraph.diana_calls[curPtr.ind].p_args);
                    storevar(flatGraph.diana_calls[curPtr.ind].target, virtual_machine.exec_func(dfunc, vstack));
                }
                var args = new Args();
                for(var i = flatGraph.diana_calls[curPtr.ind].p_args.Length - 1; i >= 0; i--)
                    args.Prepend(loadvar(flatGraph.diana_calls[curPtr.ind].p_args[i]));
                storevar(flatGraph.diana_calls[curPtr.ind].target, f.__call__(args));

                break;
            }
            case (int) CODE.Diana_Format:
            {
            
                var argvals = new string[flatGraph.diana_formats[curPtr.ind].args.Length];
                for(var i = 0; i < flatGraph.diana_formats[curPtr.ind].args.Length; i++)
                    argvals[i] = loadvar(flatGraph.diana_formats[curPtr.ind].args[i]).__str__; // TODO: format repr
                var str = String.Format(loadstr(flatGraph.diana_formats[curPtr.ind].format), argvals);
                storevar(flatGraph.diana_formats[curPtr.ind].target, MK.String(str));

                break;
            }
            case (int) CODE.Diana_Const:
            {
            
                storevar(flatGraph.diana_consts[curPtr.ind].target, loadconst(flatGraph.diana_consts[curPtr.ind].p_const));

                break;
            }
            case (int) CODE.Diana_GetAttr:
            {
            
                var value = loadvar(flatGraph.diana_getattrs[curPtr.ind].p_value);
                storevar(flatGraph.diana_getattrs[curPtr.ind].target, value.Get(loadistr(flatGraph.diana_getattrs[curPtr.ind].p_attr)));

                break;
            }
            case (int) CODE.Diana_MoveVar:
            {
            
                storevar(flatGraph.diana_movevars[curPtr.ind].target, loadvar(flatGraph.diana_movevars[curPtr.ind].slot));

                break;
            }
            case (int) CODE.Diana_Tuple:
            {
            
                var tuple_elts = new DObj[flatGraph.diana_tuples[curPtr.ind].p_elts.Length];
                for(var i = 0; i < flatGraph.diana_tuples[curPtr.ind].p_elts.Length; i++)
                    tuple_elts[i] = loadvar(flatGraph.diana_tuples[curPtr.ind].p_elts[i]);
                storevar(flatGraph.diana_tuples[curPtr.ind].target, MK.Tuple(tuple_elts));

                break;
            }
            case (int) CODE.Diana_PackTuple:
            {
            
                var tuple = (DTuple) loadvar(flatGraph.diana_packtuples[curPtr.ind].p_value);
                var tuple_elts = tuple.elts;
                for(var i = 0; i < tuple_elts.Length; i++)
                    storevar(flatGraph.diana_packtuples[curPtr.ind].targets[i], tuple_elts[i]);

                break;
            }
        default:
            throw new Exception("unknown code" + (CODE) curPtr.kind);
        }
        }
    }
}
