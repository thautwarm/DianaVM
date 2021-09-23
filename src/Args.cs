namespace DianaScript{
using System.Collections.Generic;
public interface Args
    {
        public int NArgs { get; }
        public DObj this[int i] { get; }

        public Args Prepend(DObj o);

    }

    public class StackViewArgs : Args
    {
        private int narg;
        public List<DObj> vstack;


        public StackViewArgs(int narg, List<DObj> vstack)
        {
            this.narg = narg;
            this.vstack = vstack;
        }
        public int NArgs { get => narg; }

        public DObj this[int i]
        {
            get => vstack[vstack.Count - narg + i];
        }

        public Args Prepend(DObj o){
            return new PrependArgs(narg, vstack, new List<DObj> {o});
        }
    }

    public class PrependArgs : Args
    {
        public List<DObj> vstack;
        public int _narg;
        public List<DObj> prepend;

        public PrependArgs(int narg, List<DObj> vstack)
        {
            this._narg = narg;
            this.vstack = vstack;
            this.prepend = new List<DObj> { };
        }
        public PrependArgs(int narg, List<DObj> prepend, List<DObj> vstack)
        {
            this._narg = narg;
            this.vstack = vstack;
            this.prepend = prepend;
        }
        public int NArgs { get => _narg + prepend.Count; }
        public DObj this[int i]
        {
            //  prepend | vstack  | vstack args
            //  3 2 1 0 | ....... | 4 5 6 7
            get
            {
                if (i < prepend.Count)
                {
                    return prepend[prepend.Count - i];
                }
                // vstack.Count - (narg - prepend.Count) + (i - prepend.Count);
                return vstack[vstack.Count - NArgs + i];
            }
        }

        public Args Prepend(DObj o){
            this.prepend.Push(o);
            return this;
        }

    
}
}