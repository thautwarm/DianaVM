using System.Collections;
using System.Collections.Generic;
namespace DianaScript
{

public class Args : IEnumerable<DObj>{
    public List<DObj> inner;
    public int Count => inner.Count;
    public int NArgs => inner.Count;
    public Args(){
        inner = new List<DObj> ();
    }

    public Args(IEnumerable<DObj> xs){
        
        inner = new List<DObj> (xs);
    }
    public DObj this[int i] {
        get => inner[inner.Count - i - 1];
        set => inner[inner.Count - i - 1] = value;
    }
    
    
    public void Add(DObj o) => inner.Add(o);
    public void Prepend(DObj o){
        inner.Add(o);
    }

        public IEnumerator<DObj> GetEnumerator()
        {
            return ((IEnumerable<DObj>)inner).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)inner).GetEnumerator();
        }
    }

}
    
    
    