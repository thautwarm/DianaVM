using System;
using System.Collections.Generic;
namespace DianaScript
{



    public class ExceptionWithFrames : Exception
    {
        public System.Collections.Generic.List<DFrame> frames;
        public Exception e;
        public ExceptionWithFrames(List<DFrame> frames, Exception e) : base("Unhandled exception in the VM side.")
        {
            this.frames = frames;
            this.e = e;
        }

        public override string StackTrace
        {
            get
            {
                var tmp = new string[frames.Count];
                for (var i = 0; i < frames.Count; i++)
                {
                    var frame = frames[i];
                    var best_lineno = -1;
                    foreach (var (offset, lineno) in frame.code.locs)
                    {
                        best_lineno = lineno;
                        if (frame.offset < offset)
                        {
                            break;
                        }
                    }
                    // TODO: show source code?
                    if (best_lineno == -1)
                        tmp[i] = $"    at calling {frame.code.name} at {frame.code.filename}, line unknown.";
                    else
                        tmp[i] = $"    at calling {frame.code.name} at {frame.code.filename}, line {best_lineno}.";
                }
                return e.Message + "\n" + String.Join("\n", tmp);
            }
        }

        public static ExceptionWithFrames Make(List<DFrame> frames, Exception e)
        {

            return new ExceptionWithFrames(frames, e);
        }
    }
    public class D_AttributeError : Exception
    {
        public D_AttributeError(DClsObj t, DStr attr, string message) : base(message)
        {
        }
        public D_AttributeError(DClsObj t, DStr attr) : base($"{t.__repr__} has no attribute {attr.value}.")
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
        public D_TypeError(DClsObj expect, DObj o) : base($"expect an instance of {expect.__repr__}, but got {o.__repr__}.")
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
        public D_ValueError(DObj expect, DObj o) : base($"expect the value {expect.__repr__}, but got {o.__repr__}.")
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


};
