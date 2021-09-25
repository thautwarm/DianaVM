using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DianaScript
{

    public static class Utils
    {

        public static IEnumerable<B> Map<A, B>(this IEnumerable<A> lst, Func<A, B> f)
        {
            foreach (var e in lst)
            {
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

    public class VM
    {

        public DNil Nil = MK.Nil();


        [NotNull]
        public List<DFrame> Frames;

        [NotNull]

        public List<DFrame> ErrorFrames;

        public Exception CurrentError;
        public DObj CurrentReturn;

        public VM()
        {
            Frames = new List<DFrame>();
            ErrorFrames = new List<DFrame>();
        }

        public Exception Err_InvalidCode(DObj d)
        {
            return new D_TypeError(DCode.Cls.unique, d);
        }
        public Exception Err_Name(string d)
        {
            return new D_NameError(d);
        }
        public Exception Err_InvalidJump(DObj d)
        {
            return new InvalidProgramException($"{d.__repr__} cannot be a jump target.");
        }
        public Exception Err_NotBool(DObj d)
        {
            return new InvalidProgramException($"{d.__repr__} is required to be bool.");
        }
        public Exception Err_ArgMismatchForUserFunc(DFunc f, int n)
        {

            string f_repr = ((DObj)f).__repr__;
            if (f_repr == null) return null;
            var expect = (f.code.varg ? ">=" : "") + $"{f.code.narg}";
            return new ArgumentException($"{f_repr} takes expect {expect} arguments, got {n}.");
        }


        public DFrame new_frame(DFunc func, DObj[] localvals) => DFrame.Make(func, localvals: localvals);


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

            var ns = GlobalNamespace.GetGlonal();
            var func = DFunc.Make(code, freevals: new DObj[0], name_space: ns);
            var aug_frame = DFrame.Make(func);
            var root_frame = DFrame.Make(func);
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
                        if (CurrentError == null)
                        {
                            throw new InvalidProgramException("system fatal error: error not set.");
                        }
                        cur_frame.vstack.Push(MK.Wrap(CurrentError));
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
                var err = aug_frame.vstack.Pop();
                if (err.Native is Exception e) throw ExceptionWithFrames.Make(ErrorFrames, e);
                throw new InvalidProgramException($"Fatal: non-exception object raised, got {err.__repr__}.");
            }
            return aug_frame.vstack.Pop();


        }
        public CallSplit Scheduler(DFrame frame)
        {


            int[] bytecode = frame.code.bc;
            List<DObj> vstack = frame.vstack;
            List<(int, int)> estack = frame.estack;
            Args args;
            int operand;
            string attr;
            DObj tos, tos2, tos3;
            while (true)
            {
                switch ((CODE)bytecode[frame.offset])
                {
                    case CODE.LOAD_ATTR:
                        operand = bytecode[frame.offset + 1];
                        attr = frame.strings[operand];
                        tos = vstack.Pop();
                        try
                        {
                            tos = tos.Get(attr);
                        }
                        catch (Exception e)
                        {
                            CurrentError = e;
                            goto HANDLE_ERROR;
                        }
                        vstack.Push(tos);
                        frame.offset += 2;
                        continue;

                    case CODE.STORE_ATTR:
                        operand = bytecode[frame.offset + 1];
                        attr = frame.strings[operand];
                        tos = vstack.Pop();
                        tos2 = vstack.Pop();
                        try
                        {
                            tos.Set(attr, tos2);
                        }
                        catch (Exception e)
                        {
                            CurrentError = e;
                            goto HANDLE_ERROR;
                        }
                        frame.offset += 2;
                        continue;
                    case CODE.LOAD_ITEM:
                        tos = vstack.Pop();
                        tos2 = vstack.Pop();
                        try
                        {
                            tos = tos.__getitem__(tos2);
                        }
                        catch (Exception e)
                        {
                            CurrentError = e;
                            goto HANDLE_ERROR;
                        }
                        vstack.Push(tos);
                        frame.offset += 1;
                        continue;
                    case CODE.STORE_ITEM:
                        tos = vstack.Pop();
                        tos2 = vstack.Pop();
                        tos3 = vstack.Pop();
                        try
                        {
                            tos.__setitem__(tos2, tos3);
                        }
                        catch (Exception e)
                        {
                            CurrentError = e;
                            goto HANDLE_ERROR;
                        }
                        frame.offset += 1;
                        continue;
                    case CODE.LOAD_GLOBAL:
                        operand = bytecode[frame.offset + 1];
                        attr = frame.strings[operand];

                        if (frame.name_space.TryGetValue(attr, out tos))
                        {
                            vstack.Push(tos);
                            frame.offset += 2;
                            continue;
                        }
                        CurrentError = Err_Name(attr);
                        goto HANDLE_ERROR;
                    case CODE.STORE_GLOBAL:
                        operand = bytecode[frame.offset + 1];
                        attr = frame.strings[operand];
                        tos = vstack.Pop();
                        frame.name_space[attr] = tos;
                        break;
                    case CODE.LOAD_CELL:
                        operand = bytecode[frame.offset + 1];
                        vstack.Push(frame.freevals[operand]);
                        frame.offset += 2;
                        break;
                    case CODE.STORE_CELL:
                        operand = bytecode[frame.offset + 1];
                        frame.freevals[operand] = vstack.Pop();
                        frame.offset += 2;
                        break;
                    case CODE.LOAD_LOCAL:
                        operand = bytecode[frame.offset + 1];
                        vstack.Push(frame.localvals[operand]);
                        frame.offset += 2;
                        break;
                    case CODE.STORE_LOCAL:
                        operand = bytecode[frame.offset + 1];
                        frame.localvals[operand] = vstack.Pop();
                        frame.offset += 2;
                        break;
                    case CODE.LOAD_CONST:
                        // Console.WriteLine(frame.offset);
                        operand = bytecode[frame.offset + 1];
                        // Console.WriteLine(operand);
                        vstack.Push(frame.code.consts[operand]);
                        frame.offset += 2;
                        break;
                    case CODE.PEEK:
                        operand = bytecode[frame.offset + 1];
                        vstack.Push(vstack.Peek(operand));
                        frame.offset += 2;
                        break;
                    case CODE.POP:
                        vstack.Pop_();
                        frame.offset += 1;
                        break;
                    case CODE.MAKE_FUNCTION:
                        operand = bytecode[frame.offset + 1];
                        // operand & 0x001 defaults
                        // operand & 0x010 free
                        DObj[] freevals = null, defaults = null;
                        DObj code = code = vstack.Pop();
                        if (code is DCode unwrap_code)
                        {
                        }
                        else
                        {
                            CurrentError = Err_InvalidCode(code);
                            goto HANDLE_ERROR;
                        }
                        if ((operand & 0b001) == 1)
                        {
                            try
                            {
                                defaults = MK.unbox<DTuple>(vstack.Pop()).elts;
                            }
                            catch (Exception e)
                            {
                                CurrentError = e;
                                goto HANDLE_ERROR;
                            }
                        }
                        if ((operand & 0b010) == 1)
                        {
                            try
                            {
                                freevals = MK.unbox<DTuple>(vstack.Pop()).elts;
                            }
                            catch (Exception e)
                            {
                                CurrentError = e;
                                goto HANDLE_ERROR;
                            }
                        }
                        vstack.Push(
                            DFunc.Make(
                                unwrap_code,
                                freevals,
                                default,
                                frame.name_space
                            ));
                        frame.offset += 2;
                        break;

                    case CODE.CALL:
                        {
                            operand = bytecode[frame.offset + 1];
                            var fn = vstack.Peek(operand);
                            DObj[] new_locals = null;
                            if (fn is DFunc dfunc)
                            {

                                if (dfunc.code.varg)
                                {
                                    if (operand < dfunc.code.narg)
                                    {
                                        CurrentError = Err_ArgMismatchForUserFunc(dfunc, operand);
                                        goto HANDLE_ERROR;
                                    }
                                    var narg = dfunc.code.narg;
                                    var left = operand - narg;

                                    var vararg = MK.Tuple(new DObj[operand - narg]);
                                    for (var i = 0; i < left; i++)
                                    {
                                        vararg.elts[left - i - 1] = vstack.Pop();
                                    }
                                    new_locals = new DObj[dfunc.code.nlocal];
                                    new_locals[narg] = vararg;

                                    for (var i = 0; i < narg; i++)
                                    {
                                        new_locals[narg - i - 1] = vstack.Pop();
                                    }
                                }
                                else if (dfunc.code.narg == operand)
                                {
                                    new_locals = new DObj[dfunc.code.nlocal];
                                    for (var i = 0; i < operand; i++)
                                    {
                                        new_locals[i] = vstack.Pop();
                                    }
                                }
                                else
                                {
                                    CurrentError = Err_ArgMismatchForUserFunc(dfunc, operand);
                                    goto HANDLE_ERROR;
                                }
                                vstack.Pop_(); // pop func
                                Frames.Push(new_frame(dfunc, new_locals));
                                frame.offset += 2;
                                return CallSplit.SubRoutine;
                            }
                            args = new Args();
                            for (var i = 0; i < operand; i++)
                            {
                                args.Add(vstack.Pop());
                            }
                            vstack.Pop_();
                            try
                            {
                                var fastcalled = fn.__call__(args);
                                vstack.Push(fastcalled);
                                frame.offset += 2;
                            }
                            catch (Exception e)
                            {
                                CurrentError = e;
                                goto HANDLE_ERROR;
                            }
                            break;
                        }
                    case CODE.PUSH_BLOCK:
                        operand = bytecode[frame.offset + 1];
                        estack.Push((operand, vstack.Count));
                        frame.offset += 2;
                        break;
                    case CODE.POP_BLOCK:
                        estack.Pop_();
                        frame.offset += 1;
                        break;
                    case CODE.JUMP:
                        {
                            tos = vstack.Pop();
                            if (tos is DInt dint)
                            {
                                frame.offset = dint.value;
                                continue;
                            }
                            CurrentError = Err_InvalidJump(tos);
                            goto HANDLE_ERROR;
                        }
                    case CODE.JUMP_IF:
                        {
                            var target = vstack.Pop();
                            var test = vstack.Pop();

                            if (test is DBool dbool)
                            {
                                if (target is DInt dint)
                                {
                                    frame.offset = dbool.value ? dint.value : frame.offset + 1;
                                    continue;
                                }

                                CurrentError = Err_InvalidJump(target);
                            }
                            else
                            {
                                CurrentError = Err_NotBool(test);
                            }
                            goto HANDLE_ERROR;
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
                    case CODE.NO_ARG:
                        throw new InvalidProgramException("NO_ARG instruction cannot appear here!");
                    default:
                        throw new NotImplementedException();
                    HANDLE_ERROR:
                        if (estack.Count == 0)
                        {
                            return CallSplit.Error;
                        }
                        restore_frame(frame);
                        vstack.Push(MK.Wrap(CurrentError));
                        CurrentError = null;
                        break;
                }
            }
        }

    }





}
