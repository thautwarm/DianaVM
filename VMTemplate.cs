// begin
using System;
using System.Collections.Generic;

namespace DianaScript
{
    

using static TOKEN;
public enum TOKEN
{
    RETURN,
    GO_AHEAD,
    CALL_FRAME,
    LOOP_CONTINUE,
    LOOP_BREAK,
    RERAISE
}

public class DirectRef
{
    public DObj cell_contents;
}
public partial class BlockExecution {
    // begin pseudocode
    public DirectRef[] curFreevars;

    public DFlatGraphCode flatGraph;
    public DCode bigcode;
    public int offset;
    public Stack<(int, int)> error_frames;
    
    public const int @case = 1;
    public int @b() => 1;
    // end pseudocode
    public const int bit_global = 0b01 << 30;
    public const int bit_free = 0b01 << 31;

    public const int bit_nonlocal = bit_global & bit_free;
    int @funcname(Frame frame, Block block){
        int token = (int) GO_AHEAD;
        
        DObj loadvar(int slot){
            if ((slot & bit_nonlocal) == 0){
                return frame.vstack[slot];
            }
            else if ((slot & bit_free) == 0){
                    return frame.nameSpace[
                        bigcode.constStrPool[slot << 2 >> 2]];
                }
            else{
                // assert
                return frame.freevals[slot << 2 >> 2].cell_contents;
            }
        }
        void storevar(int slot, DObj val){
            if ((slot & bit_nonlocal) == 0){
                frame.vstack[slot] = val;
            }
            else if ((slot & bit_free) == 0){
                frame.nameSpace[
                    bigcode.constStrPool[slot << 2 >> 2]] = val;
                }
            else{
                // assert
                frame.freevals[slot << 2 >> 2].cell_contents = val;
            }
        }
        //

        return token;

    }
}

// end

}