using System;

namespace DianaScript
{
public partial class DTuple
{
  public DClsObj GetCls => Cls.unique;
  public static DObj bind___len__(Args _args) // bind `this` prop 
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"accessing tuple.__len__; needs only 1 argument, got {nargs}.");
    var arg = _args[0];
    var ret = MK.unbox<DTuple>(arg).Length;
    return MK.create(ret);
  }
  public static DObj bind___contains__(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling tuple.__contains__; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<DTuple>(_args[0]);
    var _arg1 = MK.unbox<DObj>(_args[1]);
    {
      var _return = _arg0.__contains__(_arg1);
      return MK.create(_return);
    }
    throw new D_TypeError($"call tuple.__contains__; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind___eq__(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling tuple.__eq__; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<DTuple>(_args[0]);
    var _arg1 = MK.unbox<DTuple>(_args[1]);
    {
      var _return = _arg0.__eq__(_arg1);
      return MK.create(_return);
    }
    throw new D_TypeError($"call tuple.__eq__; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind___add__(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling tuple.__add__; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<DTuple>(_args[0]);
    var _arg1 = MK.unbox<DTuple>(_args[1]);
    {
      var _return = _arg0.__add__(_arg1);
      return MK.create(_return);
    }
    throw new D_TypeError($"call tuple.__add__; needs at most (2) arguments, got {nargs}.");
  }
  public partial class Cls : DClsObj  {
  public string name => "tuple";
  public static Cls unique = new Cls();
  public Type NativeType => typeof(DTuple);
  public System.Collections.Generic.Dictionary<string, (bool, DObj)> Getters {get; set;}
  public Cls()
  {
    DWrap.RegisterTypeMap(NativeType, this);
    Getters = new System.Collections.Generic.Dictionary<string, (bool, DObj)>
    {
      { "__len__", (false, MK.CreateFunc(bind___len__)) },
      { "__contains__", (false, MK.CreateFunc(bind___contains__)) },
      { "__eq__", (false, MK.CreateFunc(bind___eq__)) },
      { "__add__", (false, MK.CreateFunc(bind___add__)) },
    };
  }
  }
}
}
