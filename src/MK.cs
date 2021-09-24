using System;
namespace DianaScript{

using Args = System.Collections.Generic.List<DObj>;
public static class MK{

    
    
    public static DStr String(string s) => throw new NotImplementedException();
    public static DInt Int(int s) => throw new NotImplementedException();
    public static DBool Bool(bool b) => throw new NotImplementedException();
    public static DFloat Float(float b) => throw new NotImplementedException();

    public static DNil Nil() => throw new NotImplementedException();

    
    public static BFunc2 FuncN(Func<Args, DObj> f) => throw new NotImplementedException();

    public static BFunc3 Func3(Func<DObj, DObj, DObj, DObj> f) => throw new NotImplementedException();
    public static BFunc2 Func2(Func<DObj, DObj, DObj> f) => throw new NotImplementedException();
    public static BFunc1 Func1(Func<DObj, DObj> f) => throw new NotImplementedException();
    public static BFunc0 Func0(Func<DObj> f) => throw new NotImplementedException();

}
}