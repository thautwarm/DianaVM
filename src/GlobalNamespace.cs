using System;
using System.Collections.Generic;

namespace DianaScript
{
    public class  AssertionException: Exception
    {
        public AssertionException(string message) : base(message){}
    }
    public partial class GlobalNamespace: DObj
    {

        public object Native => this;
        public static void Print(params DObj[] objs)
        {
            foreach (var obj in objs)
                Console.Write(obj.__repr__());
            Console.WriteLine("");
        }

        public static int Time()
        {
            return (int) (System.DateTime.Now.Ticks % int.MaxValue);
        }

        
        public static void Assert(bool a, string msg = "Assertion failed")
        {
            if (!a)
            {
                throw new AssertionException(msg);
                
            }
        }
        public static Dictionary<InternString, DObj> Globals => throw new NotImplementedException();
        public static Dictionary<InternString, DObj> GetGlonal()
        {
            var ns = new Dictionary<InternString, DObj>();
            foreach (var kv in GlobalNamespace.Cls.unique.Dict)
            {
                ns[kv.Key] = kv.Value;
            }
            foreach (var cls in new DClsObj[] {
                 DStr.Cls.unique,
                 DInt.Cls.unique,
                 DDict.Cls.unique,
                 DList.Cls.unique,
                 DNil.Cls.unique,
                 DFloat.Cls.unique,
                 DTuple.Cls.unique,
                 DBool.Cls.unique,
                 DArray.Cls.unique,
                })
                ns[cls.name.ToIStr()] = cls;
            return ns;
        }
    }

};