// begin
using System;
using System.Collections.Generic;
using System.Linq;

namespace DianaScript
{
    using NameSpace = Dictionary<InternString, DObj>;
    using static @__TOKEN;
    using static @__VMHelper;

    public static class @__VMHelper
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
    }
    public enum @__TOKEN
    {
        RETURN = 0,
        GO_AHEAD = 1,
        LOOP_BREAK = 2,
        LOOP_CONTINUE = 3
    }

    public enum @__ACTION
    {
        RAISE,
        ASSERT
    }
    public struct @__MiniFrame
    {
        public int blockind;
        public int offset;

        public int metadataInd;

    }

    public partial class @__DVM
    {

        // stack of (blockind, offset)
        public Stack<@__MiniFrame> errorFrames;
        public DFunc resuable;
        private static DObj[] emptyDObjArray = new DObj[0];
        public @__DVM()
        {
            this.resuable = DFunc.Make(0, 0, 0, 0, null);
            this.errorFrames = new Stack<@__MiniFrame>();
        }

        public DObj call_func(DFunc dfunc, params DObj [] args){
            var argcount = args.Length;
            check_argcount(dfunc, argcount);
            var locals = new DObj[dfunc.nlocal];
            if (dfunc.is_vararg)
            {
                var nvararg = argcount - dfunc.narg;
                var vararg = new DObj[nvararg];
                for (var i = 0; i < nvararg; i++)
                {
                    vararg[i] = args[dfunc.narg + i];
                }
                locals[dfunc.narg] = DTuple.Make(vararg);
            }
            for (var i = 0; i < dfunc.narg; i++)
            {
                locals[i] = args[i];
            }
        
            var nonargcells = dfunc.nonargcells;
            if (nonargcells != null)
                for (var i = 0; i < nonargcells.Length; i++)
                    locals[i] = new DRef();
    
            return exec_func(dfunc, locals);
        }
        
        public void exec_block(int metadataInd, int block, NameSpace globals)
        {
            this.resuable.metadataInd = metadataInd;
            this.resuable.body = block;
            this.resuable.nameSpace = globals;
            exec_func(this.resuable, emptyDObjArray);
        }
    
    
        public DObj exec_func(DFunc dfunc, DObj[] localvars)
        {
#if A_DBG
            Console.WriteLine($"call {dfunc.__repr__()}; COUNT(localvars)={localvars.Length}");
#endif
            var executor = new @__BlockExecutor
            {
                virtual_machine = this,
                offset = 0,
                token = (int)GO_AHEAD,
                vstack = new List<DObj>(),
                cur_func = dfunc,
                localvars = localvars
            };
            executor.exec_block(dfunc.body);
            if (executor.vstack.Count == 0)
                return MK.Nil();
            return executor.vstack[executor.vstack.Count - 1];
        }
    }


    // like frame
    public partial class @__BlockExecutor
    {

        public @__DVM virtual_machine;
        public int offset;
        public int token;
        public List<DObj> vstack;
        public DObj[] localvars;
        public DFunc cur_func;

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
            return AWorld.dobjs[slot];
        }
        public DObj check_vstack()
        {
            Console.WriteLine(String.Join("|", vstack.Select(x => x.__repr__())));
            return MK.Nil();
        }
        DObj loadvar(int slot)
        {
            
            DObj c;
            if ((slot & bit_nonlocal) == 0)
            {
#if A_DBG
            Console.WriteLine($"localvar {slot >> 2}; stacksize->{vstack.Count}");
#endif
                if ((slot & bit_classify) == 0)
                    c = localvars[slot >> 2]; // local
                else
                {
                    DRef r = localvars[slot >> 2] as DRef;
                    c = r.cell_contents; // local cell

                }
                if (c == null)
                {
                    var name = AWorld.funcmetas[cur_func.metadataInd].localnames[slot >> 2];
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
                        var name = AWorld.funcmetas[cur_func.metadataInd].freenames[slot >> 2];
                        throw new NullReferenceException($"undefined cell variable {name}.");
                    }
                }
                else // global
                {
                    c = cur_func.nameSpace[AWorld.internstrings[slot >> 2]];

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
                    cur_func.nameSpace[AWorld.internstrings[slot >> 2]] = o;

                }
            }
        }

        FuncMeta loadmetadata(int slot) => AWorld.funcmetas[slot];
        string loadstr(int slot) => AWorld.strings[slot];
        
        DObj peek(int n){
#if A_DBG
            Console.WriteLine($"peek {n}; stacksize->{vstack.Count}");
#endif
            return vstack[vstack.Count - n - 1];
        }
        void push(DObj o)
        {
#if A_DBG
            Console.WriteLine($"push ; stacksize->{vstack.Count} + 1");
#endif
            vstack.Add(o);
        }

        DObj pop()
        {
#if A_DBG
            Console.WriteLine($"pop(); stacksize -> {vstack.Count}-1");
#endif
            var i = vstack.Count - 1;
            var r = vstack[i];
            vstack.RemoveAt(i);
            return r;
        }

        (DObj, DObj) pop2()
        {
#if A_DBG
            Console.WriteLine($"pop2(); stacksize->{vstack.Count}-2");
#endif
            var i = vstack.Count - 1;
            var r = (vstack[i - 1], vstack[i]);
            vstack.RemoveAt(i);
            vstack.RemoveAt(i - 1);
            return r;
        }

        (DObj, DObj, DObj) pop3()
        {
#if A_DBG
            Console.WriteLine($"pop2(); stacksize->{vstack.Count}-3");
#endif

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

#if A_DBG
            Console.WriteLine($"pop {n}; stacksize->{vstack.Count} - {n}");
#endif

            for(var i = 0; i < n; i++)
                set.Add(pop());
        }

        List<DObj> popNToList(int n){
#if A_DBG
            Console.WriteLine($"pop {n}; stacksize->{vstack.Count} - {n}");
#endif
            List<DObj> lst = list_init(n);
            for(var i = 0; i < n; i++)
                lst[n - i -1]  = pop();
            return lst;
        }
        
        void reversePopNTo(int n, Args args){
#if A_DBG
            Console.WriteLine($"pop {n}; stacksize->{vstack.Count} - {n}");
#endif
            for(var i = 0; i < n; i++)
                args.Add(pop());
        }

        void popNTo(int n, DObj[] lst){
#if A_DBG
            Console.WriteLine($"pop {n}; stacksize->{vstack.Count} - {n}");
#endif
            for(var i = 0; i < n; i++)
                lst[n - i -1]  = pop();
        }


        void reversePopNTo(int n, List<DObj> lst){
#if A_DBG
            Console.WriteLine($"pop {n}; stacksize->{vstack.Count} - {n}");
#endif
            for(var i = 0; i < n; i++)
                lst.Add(pop());
        }

        DObj[] create_locals(DFunc func, int argcount)
        {
#if A_DBG
            Console.WriteLine($"call {func.__repr__()} with {argcount} arguments.");
            Console.WriteLine($"nlocal = {func.nlocal}; argcount = {argcount}.");
#endif
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
            var codes = AWorld.blocks[blockind].codes;
            offset = 0;
            try
            {
                while (offset < codes.Length)
                {
                    exec_code(codes[offset]);
                    if (token != (int)GO_AHEAD)
                        return;
                    offset++;
                }
                offset = old_offset + 1;
            }
            catch
            {
                virtual_machine.errorFrames.Push(new @__MiniFrame
                {
                    metadataInd = cur_func.metadataInd,
                    blockind = blockind,
                    offset = old_offset
                });
                throw;
            }
        }
        public void assert(bool val, DObj msg)
        {
            if (!val)
                throw new Exception("assertion failed:" + msg.__str__());
        }
        public void exec_code(Ptr curPtr)
        {
            //REPLACE
        }
    }
}