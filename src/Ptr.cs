using System;

namespace DianaScript
{
    public struct Ptr
    {

        public int code;
        public int ind;
        
        public Ptr(CODE code, int ind){
            this.ind = ind;
            this.code = (int) code;
        }
    }
}
