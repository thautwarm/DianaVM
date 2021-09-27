using System;
using System.Collections.Generic;

namespace DianaScript
{

    using CallStack = Stack<DBigFrame>;
    using MiniFrame = Stack<Ptr>;
    public static class StackClassExtensions
    {
        public static void Add<T>(this Stack<T> self, T a) => self.Push(a);

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

        public static DCode code;
        public Dictionary<InternString, Dictionary<InternString, DObj>> modules;

        public static void assert(bool a, string msg)
        {
            if (!a)
                throw new Exception($"assertion failed, {msg}.");
        }
        public void Execute(int func_ind)
        {
            DFlatGraphCode flatGraph = code.flatGraph;
            DObj[] constObjPool = code.constObjPool;
            InternString[] constStrPool = code.constStrPool;
            FuncMeta[] funcMetas = code.funcMetas;

            var augmentedFrame = DBigFrame.Make(GlobalNamespace.Globals, new DObj[1], default(Ptr), default(int));
            augmentedFrame.catchingException = true;
            var curPtr = new Ptr(CODE.Stmt_FunctionDef, func_ind);
            CallStack callStack = new CallStack { };
            CallStack errorStack = new CallStack { };
            MiniFrame curMiniFrame = augmentedFrame.miniFrame;
            Dictionary<InternString, DObj> curGlobal = augmentedFrame.globals;
            DObj[] curVStack = augmentedFrame.valueStack;
            DObj[] curFreeVals = augmentedFrame.freeVals;
            Stack<int> curSrcPosInds = augmentedFrame.srcPosIndices;
            DBigFrame curBigFrame = augmentedFrame;
            int vstackOffset = augmentedFrame.vstackOffset;
            Exception e = null;

        handle_frames:
        frame_return:

        void popBigFrame_(){ throw new NotImplementedException(); }

        handle_exception:
        while (!curBigFrame.catchingException)
            popBigFrame_();

        graph_code_maybe_exec:

        graph_code_must_exec:
            curPtr = curMiniFrame.Pop();

            switch (curPtr.code & instr_mask)
            {
                case (int)CODE.Stmt_FunctionDef:
                    
                    int metadataInd = flatGraph.stmt_functiondefs[curPtr.ind].metadataInd;
                    var funcMeta = funcMetas[metadataInd];
                    var newFreevals = new DObj[funcMeta.freeslots.Length];
                    for (var i = 0; i < funcMeta.freeslots.Length; i++)
                    {
                        var slot = funcMeta.freeslots[i];
                        newFreevals[i] = (slot < 0) ? curFreeVals[-i - 1] : curVStack[i];
                    }

                    curVStack[vstackOffset++] = DFunc.Make(flatGraph.stmt_functiondefs[curPtr.ind].code, metadataInd, newFreevals, curGlobal);
                    goto graph_code_must_exec;
                case (int)CODE.Stmt_Return:
                    if (e != null){
                        goto graph_code_must_exec;
                    }
                        
                    if ((curPtr.code & state_bit0) == 1)
                    {
                        var valuePtr = flatGraph.stmt_returns[curPtr.ind].value;
                        curMiniFrame.Push(valuePtr);
                        goto graph_code_must_exec;
                    }
                    else
                    {
                        assert((curPtr.code >> instr_nbits) == 0, "invalid return state");
                        curPtr.code |= state_bit0;
                        curMiniFrame.Push(curPtr);
                        goto frame_return;
                    }
                case (int)CODE.Stmt_DelLocalName:
                    curVStack[flatGraph.stmt_dellocalnames[curPtr.ind].slot] = null;
                    goto graph_code_must_exec;
                
                case (int)CODE.Stmt_DelGlobalName:
                    var globalname = code.constStrPool[curPtr.ind];
                    if (!curGlobal.Remove(globalname)){
                        e = new KeyNotFoundException(globalname.ToString());
                        goto handle_exception;
                    }
                    goto graph_code_must_exec;
                




            }




        }
    }
}