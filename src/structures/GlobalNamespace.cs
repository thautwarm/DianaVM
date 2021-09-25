using System;

namespace DianaScript
{
public partial class GlobalNamespace
{
  public DClsObj GetCls => Cls.unique;
  public static DObj bind_print(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs < 0)
      throw new D_TypeError($"calling GlobalNamespace.print; needs at least >= (0) arguments, got {nargs}.");
    if (nargs == 0)
    {
      GlobalNamespace.Print();
      return MK.Nil();
    }
    var _arg0 = new DObj[nargs - 0 - 1];
    for(var _i = 0; _i < nargs; _i++)
      _arg0[_i - 0] = _args[_i];
    {
      GlobalNamespace.Print(_arg0);
      return MK.Nil();
    }
  }
  public partial class Cls : DClsObj  {
  public string name => "GlobalNamespace";
  public static Cls unique = new Cls();
  public Type NativeType => typeof(GlobalNamespace);
  public System.Collections.Generic.Dictionary<string, (bool, DObj)> Getters {get; set;}
  public Cls()
  {
    DWrap.RegisterTypeMap(NativeType, this);
    Getters = new System.Collections.Generic.Dictionary<string, (bool, DObj)>
    {
      { "print", (false, MK.CreateFunc(bind_print)) },
    };
  }
  }
}
}
