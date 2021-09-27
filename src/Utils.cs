using System;
using System.Collections.Generic;
namespace DianaScript
{

public static class InternStringExtensions
{

    public static  Dictionary<string, InternString> strToId = new Dictionary<string, InternString>();
    public static  Dictionary<int, string> idToStr = new Dictionary<int, string>();

    public static InternString ToIStr(this string s){
        if (strToId.TryGetValue(s, out var id)){
            return id;
        }
        id = new InternString { identity = strToId.Count } ;
        strToId[s] = id;
        idToStr[id.identity] = s;
        return id;
    }
    
    public static string ToIStr(this InternString s){
        return idToStr[s.identity];
    }
}
public struct InternString{
    public int identity;

    public override string ToString() => InternStringExtensions.idToStr[identity];
}

}