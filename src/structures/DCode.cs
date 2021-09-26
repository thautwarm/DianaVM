using System;

namespace DianaScript
{
public partial class DCode
{
  public DClsObj GetCls => Cls.unique;
  public static DObj bind_get_bc(Args _args) // bind `this` prop 
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"accessing code.get_bc; needs only 1 argument, got {nargs}.");
    var arg = _args[0];
    var ret = MK.unbox<DCode>(arg).bc;
    return MK.create(ret);
  }
  public static DObj bind_get_consts(Args _args) // bind `this` prop 
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"accessing code.get_consts; needs only 1 argument, got {nargs}.");
    var arg = _args[0];
    var ret = MK.unbox<DCode>(arg).consts;
    return MK.create(ret);
  }
  public static DObj bind_get_nfree(Args _args) // bind `this` prop 
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"accessing code.get_nfree; needs only 1 argument, got {nargs}.");
    var arg = _args[0];
    var ret = MK.unbox<DCode>(arg).nfree;
    return MK.create(ret);
  }
  public static DObj bind_get_narg(Args _args) // bind `this` prop 
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"accessing code.get_narg; needs only 1 argument, got {nargs}.");
    var arg = _args[0];
    var ret = MK.unbox<DCode>(arg).narg;
    return MK.create(ret);
  }
  public partial class Cls : DClsObj  {
  public string name => "code";
  public static Cls unique = new Cls();
  public Type NativeType => typeof(DCode);
  public System.Collections.Generic.Dictionary<string, (bool, DObj)> Getters {get; set;}
  public Cls()
  {
    DWrap.RegisterTypeMap(NativeType, this);
    Getters = new System.Collections.Generic.Dictionary<string, (bool, DObj)>
    {
      { "get_bc", (false, MK.CreateFunc(bind_get_bc)) },
      { "get_consts", (false, MK.CreateFunc(bind_get_consts)) },
      { "get_nfree", (false, MK.CreateFunc(bind_get_nfree)) },
      { "get_narg", (false, MK.CreateFunc(bind_get_narg)) },
    };
  }
  }
}
}
