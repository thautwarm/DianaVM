using System;
using System.Collections.Generic;
namespace DianaScript
{
    public partial class DList
    {
        public DObj __getitem__(DObj item)
        {
            var it = (DInt) item;
            return this.src[(int) it.value];
        }
        public void __setitem__(DObj item, DObj value)
        {
            var it = (DInt) item;
            this.src[(int) it.value] = value;
        }

        public void __delitem__(DObj item)
        {
            var it = (DInt) item;
            this.src.RemoveAt((int) it.value);
        }

        public IEnumerable<DObj> __iter__()
        {
            var n = this.src.Count;
            var src = this.src;
            for(var i = 0; i < n;  i++)
            {
                yield return src[i];
            }
        }
    }
}