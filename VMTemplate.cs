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
    public static void check_argcount(DFunc f, int argcount){
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

public class DirectRef
{
    public DObj cell_contents;
}

public partial class DVM{

    // stack of (blockind, offset)
    public Stack<(int, int)> ErrorFrames;
    
    public DFlatGraphCode flatGraph;
}


// like frame
public partial class BlockExecutor{
    
    public DVM virtual_machine;
    public int offset;
    public int token;
    DFunc cur_func;
    DObj[] vstack;
    public DObj @return;
    public DFlatGraphCode flatGraph;
    public const int bit_global = 0b01 << 30;
    public const int bit_free = 0b01 << 31;
    public const int bit_nonlocal = bit_global & bit_free;
    
    DObj loadvar(int slot){
        if ((slot & bit_nonlocal) == 0){
            return vstack[slot];
        }
        else if ((slot & bit_free) == 0){
                return cur_func.nameSpace[
                    flatGraph.internstrings[slot << 2 >> 2]];
            }
        else{
            // assert
            return cur_func.freevals[slot << 2 >> 2].cell_contents;
        }
    }

    void storevar(int slot, DObj val){
        if ((slot & bit_nonlocal) == 0){
            vstack[slot] = val;
        }
        else if ((slot & bit_free) == 0){
            cur_func.nameSpace[
                flatGraph.internstrings[slot << 2 >> 2]] = val;
            }
        else{
            // assert
            cur_func.freevals[slot << 2 >> 2].cell_contents = val;
        }
    }

    string loadstr(int slot) => flatGraph.strings[slot];
    InternString loadistr(int slot) => flatGraph.internstrings[slot];
    void set_return(DObj val) => @return = val;
    DObj[] create_vstack(DFunc func, int[] s_args){
        var locals = new DObj[func.nlocal];
        for(var i = 0; i < func.narg; i++){
            locals[i] = loadvar(s_args[i]);
        }
        if(func.is_vararg){
            var vararg = new DObj[s_args.Length - func.narg];
            for(var i = func.narg; i < s_args.Length; i ++){
                    vararg[i-func.narg] = loadvar(i);
            }
            locals[func.narg] = DTuple.Make(vararg);
        }
        return locals;
    }
    public void exec_block(int blockind){
        int old_offset = offset;
        var codes = flatGraph.blocks[blockind].codes;
        try{
            for(var i = 0; i< codes.Length; i++){
                exec_code(codes[i]);
                if(token != (int) TOKEN.GO_AHEAD)
                    return;
            }
        }
        catch{
            virtual_machine.ErrorFrames.Push((blockind, offset));
            throw;
        }
    }
    public void assert(bool val, DObj msg){
        if (!val)
            throw new Exception("assertion failed" + msg.__str__);
    }
    public void exec_code(Ptr curPtr){    
//REPLACE
    }
}
}