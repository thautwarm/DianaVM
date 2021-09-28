using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DianaScript
{



    
    public class ExceptionWithFrames : Exception
    {

        public Stack<MiniFrame> frames;
        public Exception e;

        public DFlatGraphCode flatGraph;
        public ExceptionWithFrames(Stack<MiniFrame> frames, DFlatGraphCode flatGraph, Exception e) : base("Unhandled exception in the VM side.")
        {
            this.frames = frames;
            this.e = e;
            this.flatGraph = flatGraph;
        }

        public override string StackTrace
        {
            get
            {
                var tmp = new string[frames.Count];

                var i = 0;
                foreach(var frame in frames){

                    var best_lineno = -1;
                    var block = flatGraph.blocks[frame.blockind];
                    var meta = flatGraph.funcmetas[frame.metadataInd];
                    var func_filename = meta.filename;
                    var func_name = meta.name;
                    var func_lineno = meta.lineno;

                
                    foreach (var (offset, lineno) in block.location_data)
                    {
                        best_lineno = lineno;
                        if (frame.offset < offset)
                        {
                            break;
                        }
                    }
                    
                    var kind = ((CODE) block.codes[frame.offset].kind).ToString();
                    

                    
                    // TODO: show source code?
                    if (best_lineno == -1)
                        tmp[i] = $"    calling {func_name}, fail at {kind}.\n" +
                                 $"       callsite: {block.filename}, line unknown.\n"+
                                 $"       function defined at: {func_filename}, line {func_lineno}";
                                
                    else
                        tmp[i] = $"    calling {func_name}, fail at {kind}.\n" +
                                 $"       callsite: {block.filename}, line {best_lineno}."+
                                 $"       function defined at: {func_filename}, line {func_lineno}";
                        
                    i++;
                }
                return e.Message + "\n" + String.Join("\n", tmp);
            }
        }

        public static ExceptionWithFrames Make(Stack<MiniFrame> frames, DFlatGraphCode flatGraph, Exception e)
        {
            return new ExceptionWithFrames(frames, flatGraph, e);
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
