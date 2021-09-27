using System;
using System.Collections.Generic;
namespace DianaScript
{
    public partial class GlobalNamespace
    {

        public static void Print(params DObj[] objs)
        {
            foreach (var obj in objs)
                Console.Write(obj.__repr__);
            Console.WriteLine("");
        }

        public static Dictionary<InternString, DObj> Globals => throw new NotImplementedException();
        public static Dictionary<string, DObj> GetGlonal()
        {
            var ns = new Dictionary<string, DObj>();
            foreach (var kv in GlobalNamespace.Cls.unique.Getters)
            {
                ns[kv.Key] = kv.Value.Item2;
            }
            foreach (var cls in new DClsObj[] {
                 DStr.Cls.unique,
                 DInt.Cls.unique,
                 DDict.Cls.unique,
                 DList.Cls.unique,
                 DNil.Cls.unique,
                 DFloat.Cls.unique,
                 DWrap.Cls.unique,
                 DTuple.Cls.unique,
                 DBool.Cls.unique,
                 DArray.Cls.unique
                })
                ns[cls.name] = cls;
            return ns;
        }
    }

};