

namespace DianaScript
{
    public struct Ptr<T>
    {

        public T kind;
        public int ind;

        public Ptr(T kind, int ind){
            this.kind = kind;
            this.ind = ind;
        }
    }
}