using System;

namespace DianaScript
{

    public enum BIN_OP{
        ADD, SUB, MUL, POW,
        FLOORDIV, TRUEDIV, MOD, 
        LSHIFT, RSHIFT, AND, 
        OR, XOR,
        EQ, NE, IN
    }
    public enum CODE
    {


        POP,
        RETURN,
        ERR_CLEAR,
        LOAD_ITEM,
        STORE_ITEM,
        DEL_ITEM,
        INVERT,

        NO_ARG,
        JUMP,
        JUMP_IF,
        POP_BLOCK,

        LOAD_ATTR,
        STORE_ATTR,
        LOAD_GLOBAL,
        STORE_GLOBAL,
        REF_GLOBAL,
        LOAD_CELL,
        LOAD_LOCAL,
        STORE_CELL,
        STORE_LOCAL,
        REF_LOCAL,
        LOAD_CONST,
        PEEK,
        CALL,
        PUSH_BLOCK,
        
        YIELD, // TODO

        MAKE_FUNCTION,   

        BIN     

    }
}
