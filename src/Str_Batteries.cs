using System;
using System.Collections.Generic;
namespace DianaScript{

public partial class DianaVM{

    
    
    public void AddMethod(string meth, DObj o){
        StrCls.dict[meth] = o;
    }

    public void IniStr(){
        StrCls.dict = new();

            // String.Join;
            // String.Empty;
            // String.Format
            // String.Concat
            String a = "2";
            // a.Replace
            // a.EndsWith
            // a.StartsWith
            // a.SubString
            // a.ToLower
            // a.ToUpper
            var k = a.Split();

    }
}
};