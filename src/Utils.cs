using System;
using System.Collections.Generic;
namespace DianaScript
{

    public static class CallDFuncExtensions
    {

        public static T dcall1<T>(DObj f, DObj arg)
        {
            return MK.unbox<T>(f.__call__(new Args { arg }));
        }
        
        public static DObj dcall1(DObj f, DObj arg)
        {
            return f.__call__(new Args { arg });
        }
        public static T dcall2<T>(DObj f, DObj arg1, DObj arg2)
        {
            return MK.unbox<T>(f.__call__(new Args { arg2, arg1 }));
        }

        public static DObj dcall2(DObj f, DObj arg1, DObj arg2)
        {
            return f.__call__(new Args { arg2, arg1 });
        }

        public static T dcall3<T>(DObj f, DObj arg1, DObj arg2)
        {
            return MK.unbox<T>(f.__call__(new Args { arg2, arg1 }));
        }
        public static DObj dcall3(DObj f, DObj arg1, DObj arg2)
        {
            return f.__call__(new Args { arg2, arg1 });
        }
    }
    public static class InternStringExtensions
    {

        public static Dictionary<string, InternString> strToId = new Dictionary<string, InternString>();
        public static Dictionary<int, string> idToStr = new Dictionary<int, string>();

        public static InternString ToIStr(this string s)
        {
            if (strToId.TryGetValue(s, out var id))
            {
                return id;
            }
            id = new InternString { identity = strToId.Count };
            strToId[s] = id;
            idToStr[id.identity] = s;

            return id;
        }

        public static string ToIStr(this InternString s)
        {
            return idToStr[s.identity];
        }
    }
    public struct InternString : IEquatable<InternString>, IComparable<InternString>
    {
        public int identity;

        public int CompareTo(InternString other) => identity.CompareTo(other.identity);

        public bool Equals(InternString other) => identity == other.identity;

        public override string ToString() => InternStringExtensions.idToStr[identity];
    }


}