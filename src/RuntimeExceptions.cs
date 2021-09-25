using System;
namespace DianaScript
{


public class D_AttributeError : Exception, DObj
{
    public D_AttributeError(DClsObj t, DStr attr, string message) : base(message){
    }
    public D_AttributeError(DClsObj t, DStr attr) : base($"{t.Repr} has no attribute {attr.value}.")
    {
    }
    public D_AttributeError(string message): base(message)
    {
    }

    
}

public class D_TypeError : Exception, DObj
{
    public DClsObj expect;
    public DObj got;
    public D_TypeError(DClsObj expect, DObj o): base($"expect an instance of {expect.Repr}, but got {o.Repr}.")
    {
    }

    public D_TypeError(string expect, string o): base($"expect an instance of {expect}, but got {o}.")
    {
    }


    public D_TypeError(DClsObj expect, DObj o, string message): base(message)
    {
    }
    public D_TypeError(string message): base(message)
    {
    }
}

public class D_ValueError : Exception, DObj
{
    public D_ValueError(DObj expect, DObj o): base($"expect the value {expect.Repr}, but got {o.Repr}.")
    {
    }
    public D_ValueError(DObj expect, DObj o, string message): base(message)
    {
    }

    public D_ValueError(string message): base(message)
    {
    }
}


};
