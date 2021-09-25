using System;

namespace DianaScript
{
public partial class DFrame
{
  public DClsObj GetCls => Cls.unique;
  public static DObj bind_get_func(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"accessing frame.get_func; needs only 1 argument, got {nargs}.");
    var arg = _args[0];
    var ret = MK.unbox<DFrame>(arg).func;
    return MK.create(ret);  }
  public partial class Cls : DClsObj  {
  public string name => "frame";
  public static Cls unique = new Cls();
  public Type NativeType => typeof(DFrame);
  public System.Collections.Generic.Dictionary<string, (bool, DObj)> Getters {get; set;}
  public Cls()
  {
    DWrap.RegisterTypeMap(NativeType, this);
    Getters = new System.Collections.Generic.Dictionary<string, (bool, DObj)>
    {
      { "get_func", (false, MK.CreateFunc(bind_get_func)) },
    };
  }
  }
}
}
