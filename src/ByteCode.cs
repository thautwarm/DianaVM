using System;

namespace DianaScript
{

    public enum CODE
    {
        PEEK = 1,
        LOAD_CELL = 2,
        LOAD_LOCAL = 3,
        STORE_CELL = 4,
        STORE_LOCAL = 5,
        LOAD_CONST = 6,
        POP = 7,
        CALL = 8,
        PUSH_BLOCK = 9,
        POP_BLOCK = 10,
        JUMP = 11,
        JUMP_IF = 12,
        RETURN = 13,
        ERR_CLEAR = 14,
        YIELD = 15,

        // TODO: load_global/store_global
    }
}
