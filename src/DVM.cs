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

    public enum ACTION
    {
        RAISE,
        ASSERT
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

        public DObj exec_func(DFunc dfunc, DObj[] localvars)
        {
            var executor = new BlockExecutor
            {
                virtual_machine = this,
                offset = 0,
                token = (int)GO_AHEAD,
                vstack = new List<DObj>(),
                cur_func = dfunc,
                flatGraph = flatGraph
            };
            executor.exec_block(dfunc.body);
            if (executor.vstack.Count == 0)
                return MK.Nil();
            return executor.vstack[executor.vstack.Count - 1];
        }
    }


    // like frame
    public partial class BlockExecutor
    {

        public DVM virtual_machine;
        public int offset;
        public int token;
        public List<DObj> vstack;
        public DObj[] localvars;
        public DFunc cur_func;
        public DFlatGraphCode flatGraph;

        public const int bit_nonlocal = 0b01;
        public const int bit_classify = 0b01 << 1;

        DRef loadref(int slot)
        {
            if ((slot & bit_nonlocal) == 0) // slot & bit_classify must be true
                return localvars[slot >> 2] as DRef;
            else
                return cur_func.freevals[slot >> 2];
        }

        void clearstack()
        {
            vstack.Clear();
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
                    c = localvars[slot >> 2]; // local
                else
                {
                    DRef r = localvars[slot >> 2] as DRef;
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
                    localvars[slot >> 2] = o; // local
                else
                {
                    DRef r = localvars[slot >> 2] as DRef;
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
        
        DObj peek(int n){
            return vstack[vstack.Count - n - 1];
        }
        void push(DObj o)
        {
            vstack.Add(o);
        }

        DObj pop()
        {
            var i = vstack.Count - 1;
            var r = vstack[i];
            vstack.RemoveAt(i);
            return r;
        }

        (DObj, DObj) pop2()
        {
            var i = vstack.Count - 1;
            var r = (vstack[i - 1], vstack[i]);
            vstack.RemoveAt(i);
            vstack.RemoveAt(i - 1);
            return r;
        }

        (DObj, DObj, DObj) pop3()
        {
            var i = vstack.Count - 1;
            var r = (vstack[i - 2], vstack[i - 1], vstack[i]);
            vstack.RemoveAt(i);
            vstack.RemoveAt(i - 1);
            vstack.RemoveAt(i - 2);
            return r;
        }
        
        List<DObj> list_init(int n){
            var lst = new List<DObj>(n);
            for(var i = 0; i < n; i ++)
            {
                lst.Add(null);
            }
            return lst;
        }
        void popNTo(int n, HashSet<DObj> set){
            for(var i = 0; i < n; i++)
                set.Add(pop());
        }

        List<DObj> popNToList(int n){
            List<DObj> lst = new List<DObj>();
            for(var i = 0; i < n; i++)
                lst[n - i -1]  = pop();
            return lst;
        }
        
        void reversePopNTo(int n, Args args){
            for(var i = 0; i < n; i++)
                args.Add(pop());
        }

        void popNTo(int n, DObj[] lst){
            for(var i = 0; i < n; i++)
                lst[n - i -1]  = pop();
        }


        void reversePopNTo(int n, List<DObj> lst){
            for(var i = 0; i < n; i++)
                lst.Add(pop());
        }

        DObj[] create_locals(DFunc func, int argcount)
        {
            var locals = new DObj[func.nlocal];
            if (func.is_vararg)
            {
                var nvararg = argcount - func.narg;
                var vararg = new DObj[nvararg];
                for (var i = nvararg - 1; i >= 0; i--)
                {
                    vararg[i] = pop();
                }
                locals[func.narg] = DTuple.Make(vararg);
            }
            for (var i = func.narg - 1; i >= 0; i--)
            {
                locals[i] = pop();
            }
        
            var nonargcells = func.nonargcells;
            if (nonargcells != null)
                for (var i = 0; i < nonargcells.Length; i++)
                    locals[i] = new DRef();
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
                virtual_machine.errorFrames.Push(new MiniFrame
                {
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
                var metadataInd = flatGraph.diana_functiondefs[curPtr.ind].metadataInd;
                var code = flatGraph.diana_functiondefs[curPtr.ind].code;
            
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
                    metadataInd: metadataInd);
                push(dfunc);

                break;
            }
            case (int) CODE.Diana_LoadGlobalRef:
            {
                var istr = flatGraph.diana_loadglobalrefs[curPtr.ind].istr;
            
                push(new DRefGlobal(cur_func.nameSpace, istr));

                break;
            }
            case (int) CODE.Diana_DelVar:
            {
                var targets = flatGraph.diana_delvars[curPtr.ind].targets;
            
                for(var i = 0; i < targets.Length; i++)
                    storevar(targets[i], null);

                break;
            }
            case (int) CODE.Diana_LoadVar:
            {
                var i = flatGraph.diana_loadvars[curPtr.ind].i;
            
                push(loadvar(i));

                break;
            }
            case (int) CODE.Diana_StoreVar:
            {
                var i = flatGraph.diana_storevars[curPtr.ind].i;
            
                storevar(i, pop());

                break;
            }
            case (int) CODE.Diana_Action:
            {
                var kind = flatGraph.diana_actions[curPtr.ind].kind;
            
                switch (kind)
                {
                    case (int) ACTION.ASSERT: 
                        assert(pop().__bool__, pop());
                        break;
                    case (int) ACTION.RAISE:
                        throw MK.unbox<Exception>(pop());
                    default:
                        throw new Exception("unknown action " + ((ACTION) kind));
                }

                break;
            }
            case (int) CODE.Diana_ControlIf:
            {
                var arg = flatGraph.diana_controlifs[curPtr.ind].arg;
            
                if(pop().__bool__)
                    token = arg;

                break;
            }
            case (int) CODE.Diana_JumpIfNot:
            {
                var off = flatGraph.diana_jumpifnots[curPtr.ind].off;
            
                if(!(pop().__bool__))
                    offset = off; 

                break;
            }
            case (int) CODE.Diana_JumpIf:
            {
                var off = flatGraph.diana_jumpifs[curPtr.ind].off;
            
                if(pop().__bool__)
                    offset = off; 

                break;
            }
            case (int) CODE.Diana_Jump:
            {
                var off = flatGraph.diana_jumps[curPtr.ind].off;
            
                offset = off; 

                break;
            }
            case (int) CODE.Diana_Control:
            {
                var arg = flatGraph.diana_controls[curPtr.ind].arg;
            
                token = arg;

                break;
            }
            case (int) CODE.Diana_Try:
            {
                var body = flatGraph.diana_trys[curPtr.ind].body;
                var except_handlers = flatGraph.diana_trys[curPtr.ind].except_handlers;
                var final_body = flatGraph.diana_trys[curPtr.ind].final_body;
               
                try
                {
                    exec_block(body);
                }
                catch (Exception e)
                {
                    clearstack();
                    foreach(var handler in except_handlers){
                        exec_block(handler.exc_type);
                        var exc_type = pop();
                        var e_boxed = MK.create(e);
                        if (exc_type.__subclasscheck__(e_boxed.GetCls))
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

                break;
            }
            case (int) CODE.Diana_Loop:
            {
                var body = flatGraph.diana_loops[curPtr.ind].body;
            
                while(true)
                {
                    exec_block(body);
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
                var body = flatGraph.diana_fors[curPtr.ind].body;
            
                var iter = pop();
                foreach(var it in iter.__iter__){
                    push(it);
                    exec_block(body);
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
                var body = flatGraph.diana_withs[curPtr.ind].body;
            
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
                        e_boxed.GetCls,
                        e_boxed,
                        e_trace
                    );
                }

                break;
            }
            case (int) CODE.Diana_GetAttr:
            {
                var attr = flatGraph.diana_getattrs[curPtr.ind].attr;
            
                var value = pop();
                push(value.Get(attr));

                break;
            }
            case (int) CODE.Diana_SetAttr:
            {
                var attr = flatGraph.diana_setattrs[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    value
                );

                break;
            }
            case (int) CODE.Diana_SetAttr_Iadd:
            {
                var attr = flatGraph.diana_setattr_iadds[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    subject.Get(attr).__add__(value)
                );

                break;
            }
            case (int) CODE.Diana_SetAttr_Isub:
            {
                var attr = flatGraph.diana_setattr_isubs[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    subject.Get(attr).__sub__(value)
                );

                break;
            }
            case (int) CODE.Diana_SetAttr_Imul:
            {
                var attr = flatGraph.diana_setattr_imuls[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    subject.Get(attr).__mul__(value)
                );

                break;
            }
            case (int) CODE.Diana_SetAttr_Itruediv:
            {
                var attr = flatGraph.diana_setattr_itruedivs[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    subject.Get(attr).__truediv__(value)
                );

                break;
            }
            case (int) CODE.Diana_SetAttr_Ifloordiv:
            {
                var attr = flatGraph.diana_setattr_ifloordivs[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    subject.Get(attr).__floordiv__(value)
                );

                break;
            }
            case (int) CODE.Diana_SetAttr_Imod:
            {
                var attr = flatGraph.diana_setattr_imods[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    subject.Get(attr).__mod__(value)
                );

                break;
            }
            case (int) CODE.Diana_SetAttr_Ipow:
            {
                var attr = flatGraph.diana_setattr_ipows[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    subject.Get(attr).__pow__(value)
                );

                break;
            }
            case (int) CODE.Diana_SetAttr_Ilshift:
            {
                var attr = flatGraph.diana_setattr_ilshifts[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    subject.Get(attr).__lshift__(value)
                );

                break;
            }
            case (int) CODE.Diana_SetAttr_Irshift:
            {
                var attr = flatGraph.diana_setattr_irshifts[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    subject.Get(attr).__rshift__(value)
                );

                break;
            }
            case (int) CODE.Diana_SetAttr_Ibitor:
            {
                var attr = flatGraph.diana_setattr_ibitors[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    subject.Get(attr).__bitor__(value)
                );

                break;
            }
            case (int) CODE.Diana_SetAttr_Ibitand:
            {
                var attr = flatGraph.diana_setattr_ibitands[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    subject.Get(attr).__bitand__(value)
                );

                break;
            }
            case (int) CODE.Diana_SetAttr_Ibitxor:
            {
                var attr = flatGraph.diana_setattr_ibitxors[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    subject.Get(attr).__bitxor__(value)
                );

                break;
            }
            case (int) CODE.Diana_SetAttr_Igt:
            {
                var attr = flatGraph.diana_setattr_igts[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    subject.Get(attr).__gt__(value)
                );

                break;
            }
            case (int) CODE.Diana_SetAttr_Ilt:
            {
                var attr = flatGraph.diana_setattr_ilts[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    subject.Get(attr).__lt__(value)
                );

                break;
            }
            case (int) CODE.Diana_SetAttr_Ige:
            {
                var attr = flatGraph.diana_setattr_iges[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    subject.Get(attr).__ge__(value)
                );

                break;
            }
            case (int) CODE.Diana_SetAttr_Ile:
            {
                var attr = flatGraph.diana_setattr_iles[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    subject.Get(attr).__le__(value)
                );

                break;
            }
            case (int) CODE.Diana_SetAttr_Ieq:
            {
                var attr = flatGraph.diana_setattr_ieqs[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    subject.Get(attr).__eq__(value)
                );

                break;
            }
            case (int) CODE.Diana_SetAttr_Ineq:
            {
                var attr = flatGraph.diana_setattr_ineqs[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    subject.Get(attr).__neq__(value)
                );

                break;
            }
            case (int) CODE.Diana_SetAttr_Iin:
            {
                var attr = flatGraph.diana_setattr_iins[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    subject.Get(attr).__in__(value)
                );

                break;
            }
            case (int) CODE.Diana_SetAttr_Inotin:
            {
                var attr = flatGraph.diana_setattr_inotins[curPtr.ind].attr;
            
                var (value, subject) = pop2();
                subject.Set(
                    attr,
                    subject.Get(attr).__notin__(value)
                );

                break;
            }
            case (int) CODE.Diana_DelItem:
            {
            
                var (subject, item) = pop2();
                subject.__delitem__(item);

                break;
            }
            case (int) CODE.Diana_GetItem:
            {
            
                var (subject, item) = pop2();
                push(subject.__getitem__(item));

                break;
            }
            case (int) CODE.Diana_SetItem:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    value
                );

                break;
            }
            case (int) CODE.Diana_SetItem_Iadd:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__add__(value)
                );
                push(item);

                break;
            }
            case (int) CODE.Diana_SetItem_Isub:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__sub__(value)
                );
                push(item);

                break;
            }
            case (int) CODE.Diana_SetItem_Imul:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__mul__(value)
                );
                push(item);

                break;
            }
            case (int) CODE.Diana_SetItem_Itruediv:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__truediv__(value)
                );
                push(item);

                break;
            }
            case (int) CODE.Diana_SetItem_Ifloordiv:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__floordiv__(value)
                );
                push(item);

                break;
            }
            case (int) CODE.Diana_SetItem_Imod:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__mod__(value)
                );
                push(item);

                break;
            }
            case (int) CODE.Diana_SetItem_Ipow:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__pow__(value)
                );
                push(item);

                break;
            }
            case (int) CODE.Diana_SetItem_Ilshift:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__lshift__(value)
                );
                push(item);

                break;
            }
            case (int) CODE.Diana_SetItem_Irshift:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__rshift__(value)
                );
                push(item);

                break;
            }
            case (int) CODE.Diana_SetItem_Ibitor:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__bitor__(value)
                );
                push(item);

                break;
            }
            case (int) CODE.Diana_SetItem_Ibitand:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__bitand__(value)
                );
                push(item);

                break;
            }
            case (int) CODE.Diana_SetItem_Ibitxor:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__bitxor__(value)
                );
                push(item);

                break;
            }
            case (int) CODE.Diana_SetItem_Igt:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__gt__(value)
                );
                push(item);

                break;
            }
            case (int) CODE.Diana_SetItem_Ilt:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__lt__(value)
                );
                push(item);

                break;
            }
            case (int) CODE.Diana_SetItem_Ige:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__ge__(value)
                );
                push(item);

                break;
            }
            case (int) CODE.Diana_SetItem_Ile:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__le__(value)
                );
                push(item);

                break;
            }
            case (int) CODE.Diana_SetItem_Ieq:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__eq__(value)
                );
                push(item);

                break;
            }
            case (int) CODE.Diana_SetItem_Ineq:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__neq__(value)
                );
                push(item);

                break;
            }
            case (int) CODE.Diana_SetItem_Iin:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__in__(value)
                );
                push(item);

                break;
            }
            case (int) CODE.Diana_SetItem_Inotin:
            {
            
                var (value, subject, item) = pop3();
                subject.__setitem__(
                    item,
                    subject.__getitem__(item).__notin__(value)
                );
                push(item);

                break;
            }
            case (int) CODE.Diana_add:
            {
            
                var (left, right) = pop2();
                push(MK.create(left.__add__(right)));

                break;
            }
            case (int) CODE.Diana_sub:
            {
            
                var (left, right) = pop2();
                push(MK.create(left.__sub__(right)));

                break;
            }
            case (int) CODE.Diana_mul:
            {
            
                var (left, right) = pop2();
                push(MK.create(left.__mul__(right)));

                break;
            }
            case (int) CODE.Diana_truediv:
            {
            
                var (left, right) = pop2();
                push(MK.create(left.__truediv__(right)));

                break;
            }
            case (int) CODE.Diana_floordiv:
            {
            
                var (left, right) = pop2();
                push(MK.create(left.__floordiv__(right)));

                break;
            }
            case (int) CODE.Diana_mod:
            {
            
                var (left, right) = pop2();
                push(MK.create(left.__mod__(right)));

                break;
            }
            case (int) CODE.Diana_pow:
            {
            
                var (left, right) = pop2();
                push(MK.create(left.__pow__(right)));

                break;
            }
            case (int) CODE.Diana_lshift:
            {
            
                var (left, right) = pop2();
                push(MK.create(left.__lshift__(right)));

                break;
            }
            case (int) CODE.Diana_rshift:
            {
            
                var (left, right) = pop2();
                push(MK.create(left.__rshift__(right)));

                break;
            }
            case (int) CODE.Diana_bitor:
            {
            
                var (left, right) = pop2();
                push(MK.create(left.__bitor__(right)));

                break;
            }
            case (int) CODE.Diana_bitand:
            {
            
                var (left, right) = pop2();
                push(MK.create(left.__bitand__(right)));

                break;
            }
            case (int) CODE.Diana_bitxor:
            {
            
                var (left, right) = pop2();
                push(MK.create(left.__bitxor__(right)));

                break;
            }
            case (int) CODE.Diana_gt:
            {
            
                var (left, right) = pop2();
                push(MK.create(left.__gt__(right)));

                break;
            }
            case (int) CODE.Diana_lt:
            {
            
                var (left, right) = pop2();
                push(MK.create(left.__lt__(right)));

                break;
            }
            case (int) CODE.Diana_ge:
            {
            
                var (left, right) = pop2();
                push(MK.create(left.__ge__(right)));

                break;
            }
            case (int) CODE.Diana_le:
            {
            
                var (left, right) = pop2();
                push(MK.create(left.__le__(right)));

                break;
            }
            case (int) CODE.Diana_eq:
            {
            
                var (left, right) = pop2();
                push(MK.create(left.__eq__(right)));

                break;
            }
            case (int) CODE.Diana_neq:
            {
            
                var (left, right) = pop2();
                push(MK.create(left.__neq__(right)));

                break;
            }
            case (int) CODE.Diana_in:
            {
            
                var (left, right) = pop2();
                push(MK.create(right.__contains__(left)));

                break;
            }
            case (int) CODE.Diana_notin:
            {
            
                var (left, right) = pop2();
                push(MK.create(!(right.__contains__(left))));

                break;
            }
            case (int) CODE.Diana_UnaryOp_invert:
            {
            
                var val = pop();
                push(MK.create(val.__invert__));

                break;
            }
            case (int) CODE.Diana_UnaryOp_not:
            {
            
                var val = pop();
                push(MK.create(val.__not__));

                break;
            }
            case (int) CODE.Diana_UnaryOp_neg:
            {
            
                var val = pop();
                push(MK.create(val.__neg__));

                break;
            }
            case (int) CODE.Diana_MKDict:
            {
                var n = flatGraph.diana_mkdicts[curPtr.ind].n;
            
                var dict = new Dictionary<DObj, DObj>(n);
                for(var i = 0; i < n; i++){
                    var (k, v) = pop2();
                    dict[k] = v;
                }
                push(MK.Dict(dict));

                break;
            }
            case (int) CODE.Diana_MKSet:
            {
                var n = flatGraph.diana_mksets[curPtr.ind].n;
            
                var hset = new HashSet<DObj>(n);
                popNTo(n, hset);
                push(MK.Set(hset));

                break;
            }
            case (int) CODE.Diana_MKList:
            {
                var n = flatGraph.diana_mklists[curPtr.ind].n;
            
                var list = popNToList(n);
                push(MK.List(list));

                break;
            }
            case (int) CODE.Diana_Call:
            {
                var n = flatGraph.diana_calls[curPtr.ind].n;
            
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

                break;
            }
            case (int) CODE.Diana_Format:
            {
                var format = flatGraph.diana_formats[curPtr.ind].format;
                var argn = flatGraph.diana_formats[curPtr.ind].argn;
            
                var argvals = new string[argn];
                for(var i = 0; i < argn; i++)
                    argvals[argn - i - 1] = pop().__str__; // TODO: format style
                var str = String.Format(loadstr(format), argvals);
                push(MK.String(str));

                break;
            }
            case (int) CODE.Diana_Const:
            {
                var p_const = flatGraph.diana_consts[curPtr.ind].p_const;
            
                push(loadconst(p_const));

                break;
            }
            case (int) CODE.Diana_MKTuple:
            {
                var n = flatGraph.diana_mktuples[curPtr.ind].n;
            
                var tuple_elts = new DObj[n];
                popNTo(n, tuple_elts);
                push(MK.Tuple(tuple_elts));

                break;
            }
            case (int) CODE.Diana_Pack:
            {
                var n = flatGraph.diana_packs[curPtr.ind].n;
            
                var tuple = (DTuple) pop();
                var tuple_elts = tuple.elts;
                // TODO check exact
                for(var i = 0; i < n; i++)
                    push(tuple_elts[i]);

                break;
            }
            case (int) CODE.Diana_Replicate:
            {
                var n = flatGraph.diana_replicates[curPtr.ind].n;
            
                var val = pop();
                for(var i = 0; i < n; i++)
                    push(val);

                break;
            }
            case (int) CODE.Diana_Pop:
            {
            
                pop();

                break;
            }
        default:
            throw new Exception("unknown code" + (CODE) curPtr.kind);
        }
        }
    }
}
