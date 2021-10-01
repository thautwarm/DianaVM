using System;
using System.Collections.Generic;

namespace DianaScript
{
    public partial class DArrayEx : DObjEx
    {
        public DClsObj eltype;
        public Array src;
        public object Native => src;

        public static Ops MakeMethodStruct(){
            var ops = Ops.defaultOps;
            IEnumerable<DObj> __iter__2(DArray self)
            {
            int n = self.Count;
            for (var i = 0; i < n; i++)
            {
                yield return self[i];
            }
            };
            ops.__iter__ = __iter__2;
        }
        public static DArray Make(DClsObj eltype, int n)
        {
            return new DArray
            {
                eltype = eltype,
                src = System.Array.CreateInstance(eltype.NativeType, n)
            };
        }
        public static DArray Make<T>(T[] array)
        {
            var t = typeof(T);
            var eltype = DWrap.TypeMapOrCache(t);

            return new DArray { eltype = eltype, src = array as Array };
        }

        public DObj this[int i]
        {
            get =>
(DObj)src.GetValue(i);
            set
            {
                var native = value.Native.GetType();
                if (native == eltype.NativeType)
                {
                    src.SetValue(value, i);
                }
                else
                {
                    throw new D_TypeError(eltype.NativeType.Name, value.Native.GetType().Name);
                }
            }
        }
        public int Count => src.GetLength(0);
        public string __repr__ => $"Array({this.eltype.__repr__}, [{String.Join(", ", ((this as DObj).__iter__))}])";
        public static IEnumerable<DObj> __iter__(DObj self_)
        {    
            var self = (DArray) self_;
            int n = self.Count;
            for (var i = 0; i < n; i++)
            {
                yield return self[i];
            }
        }

    }
    
}