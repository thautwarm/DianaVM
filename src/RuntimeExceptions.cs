using System;
namespace DianaScript
{


public class D_AttributeError : Exception
{
    public D_AttributeError(DClsObj t, DStr attr, string message) : base(message){
    }
    public D_AttributeError(DClsObj t, DStr attr) : base($"{t.__repr__} has no attribute {attr.value}.")
    {
    }
    public D_AttributeError(string message): base(message)
    {
    }

    
}

public class D_TypeError : Exception
{
    public DClsObj expect;
    public DObj got;
    public D_TypeError(DClsObj expect, DObj o): base($"expect an instance of {expect.__repr__}, but got {o.__repr__}.")
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

public class D_ValueError : Exception
{
    public D_ValueError(DObj expect, DObj o): base($"expect the value {expect.__repr__}, but got {o.__repr__}.")
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
