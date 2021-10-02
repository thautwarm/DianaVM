using System;
using System.Collections.Generic;

namespace DianaScript
{
    using static CallDFuncExtensions;

    public static class UserObjectCreation

    {
        public static DUserObj.Cls Make(string name, Dictionary<DObj, DObj> d)
        {
            var m_methods = new Dictionary<InternString, DObj>();
            var m_ops = Ops.defaultOps;
            foreach (var (key, obj) in d)
            {
                var s = (DStr)key;
                var meth = obj;
                switch (s.value)
                {
                    case "__iter__":
                        {
                            m_ops.__iter__ = (x) => dcall1<IEnumerable<DObj>>(meth, x);
                            break;
                        }
                    case "__add__":
                        {
                            m_ops.__add__ = (x, y) => dcall2(meth, x, y);
                            break;

                        }
                    case "__sub__":
                        {
                            m_ops.__sub__ = (x, y) => dcall2(meth, x, y);
                            break;

                        }
                    default:
                        throw new NotImplementedException();

                }
            }
            return new DUserObj.Cls(name, m_ops, m_methods);
        }
    }

}