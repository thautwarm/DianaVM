using System;

namespace DianaScript
{

    public enum CODE
    {


        POP,
        RETURN,
        ERR_CLEAR,
        LOAD_ITEM,
        STORE_ITEM,

        NO_ARG,
        JUMP,
        JUMP_IF,
        POP_BLOCK,

        LOAD_ATTR,
        STORE_ATTR,
        LOAD_GLOBAL,
        STORE_GLOBAL,
        LOAD_CELL,
        LOAD_LOCAL,
        STORE_CELL,
        STORE_LOCAL,
        LOAD_CONST,
        PEEK,
        CALL,
        PUSH_BLOCK,
        
        YIELD, // TODO

        MAKE_FUNCTION,        

    }
}
