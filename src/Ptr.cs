using System;

namespace DianaScript
{
    public struct Ptr
    {

        public int kind;
        public int ind;

        public Ptr(CODE code, int ind)
        {
            this.ind = ind;
            this.kind = (int)code;
        }
    }
}
