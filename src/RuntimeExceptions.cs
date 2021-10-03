using System;
using System.Collections.Generic;

namespace DianaScript
{

    public class ExceptionWithFrames : Exception
    {

        public Stack<MiniFrame> frames;
        public Exception e;

        public ExceptionWithFrames(Stack<MiniFrame> frames, Exception e) : base("Unhandled exception in the VM side.")
        {
            this.frames = frames;
            this.e = e;
        }

        public override string StackTrace
        {
            get
            {
                var tmp = new string[frames.Count];

                var i = 0;
                foreach (var frame in frames)
                {

                    var best_lineno = -1;
                    var meta = AWorld.funcmetas[frame.metadataInd];
                    int[] block = meta.bytecode;
                    var func_filename = meta.filename;
                    var func_name = meta.name;
                    var func_lineno = meta.lineno;

                    foreach (var (offset, lineno) in meta.linenos)
                    {
                        best_lineno = lineno;
                        if (frame.offset < offset)
                        {
                            break;
                        }
                    }

                    var kind = ((CODETAG)block[frame.offset]).ToString();



                    // TODO: show source code?
                    if (best_lineno == -1)
                        tmp[i] = $"    calling {func_name}, fail at {kind}: line unknown.\n" +
                                 $"       function defined at: {func_filename}, line {func_lineno}";

                    else
                        tmp[i] = $"    calling {func_name}, fail at {kind}: line {best_lineno}.\n" +
                                 $"       function defined at: {func_filename}, line {func_lineno}";

                    i++;
                }
                return e.Message + "\n" + String.Join("\n", tmp);
            }
        }

        public static ExceptionWithFrames Make(Stack<MiniFrame> frames, Exception e)
        {
            return new ExceptionWithFrames(frames, e);
        }
    }
    public class D_AttributeError : Exception
    {
        public D_AttributeError(DClsObj t, DStr attr, string message) : base(message)
        {
        }
        public D_AttributeError(DClsObj t, DStr attr) : base($"{t.__repr__()} has no attribute {attr.value}.")
        {
        }
        public D_AttributeError(string message) : base(message)
        {
        }
    }

    public class D_TypeError : Exception
    {
        public DClsObj expect;
        public DObj got;
        public D_TypeError(DClsObj expect, DObj o) : base($"expect an instance of {expect.__repr__()}, but got {o.__repr__()}.")
        {
        }

        public D_TypeError(string expect, string o) : base($"expect an instance of {expect}, but got {o}.")
        {
        }


        public D_TypeError(DClsObj expect, DObj o, string message) : base(message)
        {
        }
        public D_TypeError(string message) : base(message)
        {
        }
    }

    public class D_ValueError : Exception
    {
        public D_ValueError(DObj expect, DObj o) : base($"expect the value {expect.__repr__()}, but got {o.__repr__()}.")
        {
        }
        public D_ValueError(DObj expect, DObj o, string message) : base(message)
        {
        }

        public D_ValueError(string message) : base(message)
        {
        }
    }

    public class D_NameError : Exception
    {
        public D_NameError(string s) : base($"Name {s} not found.")
        {
        }
    }

    public class D_InvalidComparison : Exception
    {

        public D_InvalidComparison(DClsObj cls) : base($"{cls.name} produces invalid comparsion.")
        {
        }
    }


};
