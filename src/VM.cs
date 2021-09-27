using System;
using System.Collections.Generic;

namespace DianaScript
{

    using CallStack = Stack<DBigFrame>;
    using MiniFrame = Stack<Ptr>;
    using NameSpace = Dictionary<InternString, DObj>;
    public static class StackClassExtensions
    {
        public static void Add<T>(this Stack<T> self, T a) => self.Push(a);

    }


    public class Ref : DObj
    {
        public DObj cell_contents;
        public Ref(DObj c)
        {
            this.cell_contents = cell_contents;
        }
    }

    public class Frame
    {

        public DObj[] vstack;

        public NameSpace nameSpace;
        public void Push(DObj v) => throw new NotImplementedException();

        public DObj Pop() => throw new NotImplementedException();

        public DObj Access(int i) => throw new NotImplementedException();


    }
    public class VM
    {


        // these bits are used for case switch.
        // the left 24 bits are used to represent a mini-frame's state
        public static int instr_mask = 0b11111111;
        public static int instr_nbits = 8;

        public static int state_bit0 = 0b01 << 8;
        public static int state_bit1 = 0b01 << 9;
        public static int state_bit2 = 0b01 << 10;
        public static int state_bit3 = 0b01 << 11;
        public static int state_bit4 = 0b01 << 12;

        public DCode code;
        public DFlatGraphCode flatGraph;
        public Dictionary<InternString, Dictionary<InternString, DObj>> modules;

        public static void assert(bool a, string msg)
        {
            if (!a)
                throw new Exception($"assertion failed, {msg}.");
        }

        public Ptr ptr;
        public DObj val;

        public IEnumerable<Ptr> Execute(Frame frame, Ptr ptr)
        {
            switch (ptr.code)
            {
                case CODE.Stmt_FunctionDef:

                    int metadataInd = flatGraph.stmt_functiondefs[ptr.ind].metadataInd;
                    var funcMeta = code.funcMetas[metadataInd];
                    if (funcMeta.freeslots.Length == 0)
                    {
                        frame.Push(
                            DFunc.Make(flatGraph.stmt_functiondefs[ptr.ind].code, metadataInd, name_space: frame.nameSpace));
                    }
                    else
                    {
                        frame.Push(
                            DFunc.Make(flatGraph.stmt_functiondefs[ptr.ind].code, metadataInd, name_space: frame.nameSpace));
                    }
                    yield break;


                case CODE.Stmt_Return:
                    yield return flatGraph.stmt_returns[ptr.ind].value;
                    yield break;
                case CODE.Stmt_DelLocalName:
                    frame.vstack[flatGraph.stmt_dellocalnames[ptr.ind].slot] = null;
                    yield break;
                
                    
                    

            }


        }
    }
}