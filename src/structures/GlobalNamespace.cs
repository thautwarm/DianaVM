using System;

namespace DianaScript
{
public partial class GlobalNamespace
{
  public DClsObj Class => Cls.unique;
  public static DObj bind_print(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs < 0)
      throw new D_TypeError($"calling GlobalNamespace.print; needs at least >= (0) arguments, got {nargs}.");
    if (nargs == 0)
    {
      GlobalNamespace.Print();
      return MK.Nil();
    }
    var _arg0 = new DObj[nargs - 0];
    for(var _i = 0; _i < nargs; _i++)
      _arg0[_i - 0] = _args[_i];
    {
      GlobalNamespace.Print(_arg0);
      return MK.Nil();
    }
  }
  public static DObj bind_time(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs != 0)
      throw new D_TypeError($"calling GlobalNamespace.time; needs at least  (0) arguments, got {nargs}.");
    {
      var _return = GlobalNamespace.Time();
      return MK.create(_return);
    }
    throw new D_TypeError($"call GlobalNamespace.time; needs at most (0) arguments, got {nargs}.");
  }
  public static DObj bind_assert(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs < 1)
      throw new D_TypeError($"calling GlobalNamespace.assert; needs at least  (1,2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<bool>(_args[0]);
    if (nargs == 1)
    {
      GlobalNamespace.Assert(_arg0);
      return MK.Nil();
    }
    var _arg1 = MK.unbox<string>(_args[1]);
    {
      GlobalNamespace.Assert(_arg0,_arg1);
      return MK.Nil();
    }
    throw new D_TypeError($"call GlobalNamespace.assert; needs at most (2) arguments, got {nargs}.");
  }
  public partial class Cls : DClsObj  {
  public string name => "GlobalNamespace";
  public static Cls unique = new Cls();
  public Type NativeType => typeof(GlobalNamespace);
  public System.Collections.Generic.Dictionary<InternString, DObj> Dict {get; set;}
  public Cls()
  {
    DWrap.RegisterTypeMap(NativeType, this);
    Dict = new System.Collections.Generic.Dictionary<InternString, DObj>
    {
      { "print".ToIStr(), MK.CreateFunc(bind_print) },
      { "time".ToIStr(), MK.CreateFunc(bind_time) },
      { "assert".ToIStr(), MK.CreateFunc(bind_assert) },
    };
  }
  }
}
}
