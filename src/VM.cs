using System;
using System.Collections.Generic;
namespace DianaScript
{

    public static class Utils
    {
    
        public static IEnumerable<B>  Map<A, B>(this IEnumerable<A> lst, Func<A, B> f){
            foreach(var e in lst){
                yield return f(e);
            }
        }
        public static B Then<A, B>(this A a, Func<A, B> f)
        {
            return f(a);
        }
        public static void Do<A>(this A a, Action<A> f)
        {
            f(a);
        }
    }
    public static class ListAsStack
    {
        public static A Pop<A>(this List<A> self)
        {
            var i = self.Count - 1;
            var r = self[i];
            self.RemoveAt(i);
            return r;
        }
        public static void Push<A>(this List<A> self, A a)
        {
            self.Add(a);
        }
        public static void Pop_<A>(this List<A> self)
        {
            var i = self.Count - 1;
            self.RemoveAt(i);
        }
        public static A Peek<A>(this List<A> self, int i)
        {
            return self[self.Count - 1 - i];
        }
        public static void PopN_<A>(this List<A> self, int n)
        {
            for (var i = 0; i < n; i++)
            {
                self.Pop_();
            }
        }

        public static void PopNTo<A>(this List<A> self, int n, A[] ret)
        {
            for (var i = 0; i < n; i++)
            {
                ret[n - i - 1] = self.Pop();
            }
        }
    }

    public enum CallSplit
    {
        Finished = 0,
        Error = 1,
        SubRoutine = 2
    }

    public interface VM
    {
        public DClsObj FuncCls { get; }
        public DClsObj IntCls { get; }
        public DClsObj BoolCls { get; }
        public DClsObj FrameCls { get; }
        public DClsObj GtorCls { get; }
        public DGenerator CreateGenerator(DFrame frame, DObj val);


        public DTuple CreateTuple(DObj[] elts);
        public DNil Nil { get; }

        public List<DFrame> Frames { get; }
        public List<DFrame> ErrorFrames { get; }
        public DError CurrentError { get; set; }
        public DObj CurrentReturn { get; set; }

        public DError Err_InvalidJump(DObj d);
        public DError Err_NotBool(DObj d);
        public DError Err_ArgMismatchForUserFunc(DFunc f, int n);

        public DFrame new_frame(DFunc func) => new DFrame
        {
            func = func,
            localvals = new DObj[] { },
            GetCls = FrameCls,
            vstack = new List<DObj>(),
            estack = new List<(int, int)>(),
            offset = 0
        };

        public DFrame new_frame(DFunc func, DObj[] localvals) => new DFrame
        {
            func = func,
            localvals = localvals,
            GetCls = FrameCls,
            vstack = new List<DObj>(),
            estack = new List<(int, int)>(),
            offset = 0
        };

        public void restore_frame(DFrame frame)
        {

            var (offset, vdep) = frame.estack.Pop();
            var ndiff = frame.vstack.Count - vdep;
            if (ndiff < 0)
                throw new InvalidOperationException("Fatal: Negative stack length. Your bytecode is malformed.");
            else frame.vstack.PopN_(ndiff);

            frame.offset = offset;
        }

        public DObj Run(DCode code)
        {

            var func = new DFunc { code = code, freevals = new DObj[] { }, GetCls = FuncCls };
            var aug_frame = new_frame(func);
            var root_frame = new_frame(func);
            Frames.Add(aug_frame);
            Frames.Add(root_frame);
            aug_frame.estack.Push((0, 0));
            var cur_frame = root_frame;
            while (Frames.Count != 1)
            {
                switch (Scheduler(cur_frame))
                {
                    case CallSplit.Error:
                        do
                        {
                            ErrorFrames.Push(cur_frame);
                            Frames.Pop_();
                            cur_frame = Frames.Peek(0);
                        } while (cur_frame.estack.Count == 0);
                        restore_frame(cur_frame);
                        break;
                    case CallSplit.SubRoutine:
                        cur_frame = Frames.Peek(0);
                        break;
                    case CallSplit.Finished:
                        Frames.Pop_();
                        cur_frame = Frames.Peek(0);
                        cur_frame.vstack.Push(CurrentReturn);
                        break;
                }
            }
            Frames.Pop_();
            if (aug_frame.estack.Count == 0)
            {
                return null;
            }
            return aug_frame.vstack.Pop();
        }
        public CallSplit Scheduler(DFrame frame)
        {


            var bc = frame.code.bc;
            var vstack = frame.vstack;
            var estack = frame.estack;
            Args args;
            int operand;
            while (true)
            {

                switch ((CODE)bc[frame.offset])
                {
                    case CODE.LOAD_CELL:
                        operand = bc[frame.offset + 1];
                        vstack.Push(frame.freevals[operand]);
                        frame.offset += 2;
                        break;
                    case CODE.STORE_CELL:
                        operand = bc[frame.offset + 1];
                        frame.freevals[operand] = vstack.Pop();
                        frame.offset += 2;
                        break;
                    case CODE.LOAD_LOCAL:
                        operand = bc[frame.offset + 1];
                        vstack.Push(frame.localvals[operand]);
                        frame.offset += 2;
                        break;
                    case CODE.STORE_LOCAL:
                        operand = bc[frame.offset + 1];
                        frame.localvals[operand] = vstack.Pop();
                        frame.offset += 2;
                        break;
                    case CODE.LOAD_CONST:
                        // Console.WriteLine(frame.offset);
                        operand = bc[frame.offset + 1];
                        // Console.WriteLine(operand);
                        vstack.Push(frame.code.consts[operand]);
                        frame.offset += 2;
                        break;
                    case CODE.PEEK:
                        operand = bc[frame.offset + 1];
                        vstack.Push(vstack.Peek(operand));
                        frame.offset += 2;
                        break;
                    case CODE.POP:
                        vstack.Pop_();
                        frame.offset += 1;
                        break;
                    case CODE.CALL:
                        {
                            operand = bc[frame.offset + 1];
                            var fn = vstack.Peek(operand);
                            DObj[] new_locals = null;
                            if (fn is DFunc dfunc)
                            {

                                if (dfunc.code.varg)
                                {
                                    if (operand < dfunc.code.narg)
                                    {
                                        CurrentError = Err_ArgMismatchForUserFunc(dfunc, operand);
                                        return CallSplit.Error;
                                    }
                                    var narg = dfunc.code.narg;
                                    new_locals = new DObj[dfunc.code.nlocal];
                                    for (var i = 0; i < narg; i++)
                                    {
                                        new_locals[i] = vstack[vstack.Count - operand + i];
                                    }
                                    var left = operand - narg;
                                    var vararg = CreateTuple(new DObj[operand - narg]);
                                    for (var i = 0; i < left; i++)
                                    {
                                        vararg.elts[i] = vstack[vstack.Count - left + i];
                                    }
                                    new_locals[narg] = vararg;
                                }
                                else if (dfunc.code.narg == operand)
                                {
                                    new_locals = new DObj[dfunc.code.nlocal];
                                    for (var i = 0; i < operand; i++)
                                    {
                                        new_locals[i] = vstack[vstack.Count - operand + i];
                                    }
                                }
                                vstack.Pop_(); // pop func
                                Frames.Push(new_frame(dfunc, new_locals));
                                frame.offset += 2;
                                return CallSplit.SubRoutine;
                            }
                            args = new StackViewArgs(operand, vstack);
                            var fastcalled = fn.GetCls.call(fn, args);
                            vstack.PopN_(operand + 1);
                            if (fastcalled == null)
                            {
                                if (estack.Count == 0)
                                {
                                    return CallSplit.Error;
                                }
                                restore_frame(frame);
                                vstack.Push(CurrentError);
                                CurrentError = null;
                            }
                            else
                            {
                                vstack.Push(fastcalled);
                                frame.offset += 2;
                            }
                            break;
                        }
                    case CODE.PUSH_BLOCK:
                        operand = bc[frame.offset + 1];
                        estack.Push((operand, vstack.Count));
                        frame.offset += 2;
                        break;
                    case CODE.POP_BLOCK:
                        estack.Pop_();
                        frame.offset += 1;
                        break;
                    case CODE.JUMP:
                        {
                            var tos = vstack.Pop();
                            if (tos is DInt dint)
                            {
                                frame.offset = dint.value;
                            }
                            else if (estack.Count == 0)
                            {
                                CurrentError = Err_InvalidJump(tos);
                                return CallSplit.Error;
                            }
                            else
                            {
                                restore_frame(frame);
                                vstack.Push(Err_InvalidJump(tos));
                            }
                            break;
                        }
                    case CODE.JUMP_IF:
                        {
                            var target = vstack.Pop();
                            var test = vstack.Pop();

                            if (test is DBool dbool)
                            {
                                if (target is DInt dint)
                                {
                                    frame.offset = dbool.value ? dint.value : frame.offset + 2;
                                    continue;
                                }
                                else
                                {
                                    CurrentError = Err_InvalidJump(target);
                                }

                            }
                            else
                            {
                                CurrentError = Err_NotBool(test);
                            }
                            if (estack.Count == 0)
                            {
                                return CallSplit.Error;
                            }

                            restore_frame(frame);
                            vstack.Push(CurrentError);
                            CurrentError = null;
                            break;
                        }
                    case CODE.RETURN:
                        CurrentReturn = vstack.Pop();
                        frame.offset += 1;
                        return CallSplit.Finished;
                    case CODE.ERR_CLEAR:
                        ErrorFrames.Clear();
                        frame.offset += 1;
                        break;
                    case CODE.YIELD:
                        throw new NotImplementedException();
                        // CurrentReturn = CreateGenerator(frame, vstack.Pop());
                        // frame.offset += 1;
                        // return CallSplit.Finished;
                }
            }
        }

    }





}
