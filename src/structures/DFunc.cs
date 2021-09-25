using System;

namespace DianaScript
{
public partial class DFunc
{
  public DClsObj GetCls => Cls.unique;
  public static DObj bind_get_code(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"accessing function.get_code; needs only 1 argument, got {nargs}.");
    var arg = _args[0];
    var ret = MK.unbox<DFunc>(arg).code;
    return MK.create(ret);  }
  public partial class Cls : DClsObj  {
  public string name => "function";
  public static Cls unique = new Cls();
  public Type NativeType => typeof(DFunc);
  public System.Collections.Generic.Dictionary<string, (bool, DObj)> Getters {get; set;}
  public Cls()
  {
    DWrap.RegisterTypeMap(NativeType, this);
    Getters = new System.Collections.Generic.Dictionary<string, (bool, DObj)>
    {
      { "get_code", (false, MK.CreateFunc(bind_get_code)) },
    };
  }
  }
}
}
