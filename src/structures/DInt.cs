using System;

namespace DianaScript
{
public partial class DInt
{
  public DClsObj GetCls => Cls.unique;
  public static DObj bind_parse(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"calling int.parse; needs at least  (1) arguments, got {nargs}.");
    var _arg0 = MK.unbox<String>(_args[0]);
    {
      var _return = Int32.Parse(_arg0);
      return MK.create(_return);
    }
    throw new D_TypeError($"call int.parse; needs at most (1) arguments, got {nargs}.");
  }
  public static DObj bind_try_parse(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling int.try_parse; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<String>(_args[0]);
    var _out_1 = MK.unbox<DRef>(_args[1]);
    var _arg1 = MK.unbox<Int32>(_out_1.get_contents());
    {
      var _return = Int32.TryParse(_arg0,out _arg1);
      _out_1.set_contents(MK.cast(THelper<DObj>.val, _arg1));
      return MK.create(_return);
    }
    throw new D_TypeError($"call int.try_parse; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind_max(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 0)
      throw new D_TypeError($"accessing int.max; needs 0 arguments, got {nargs}.");
    var ret = Int32.MaxValue;
    return MK.create(ret);  }
  public static DObj bind_min(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 0)
      throw new D_TypeError($"accessing int.min; needs 0 arguments, got {nargs}.");
    var ret = Int32.MinValue;
    return MK.create(ret);  }
  public partial class Cls : DClsObj  {
  public string name => "int";
  public static Cls unique = new Cls();
  public Type NativeType => typeof(DInt);
  public System.Collections.Generic.Dictionary<string, (bool, DObj)> Getters {get; set;}
  public Cls()
  {
    Getters = new System.Collections.Generic.Dictionary<string, (bool, DObj)>
    {
      { "parse", (false, MK.CreateFunc(bind_parse)) },
      { "try_parse", (false, MK.CreateFunc(bind_try_parse)) },
      { "max", (false, MK.CreateFunc(bind_max)) },
      { "min", (false, MK.CreateFunc(bind_min)) },
    };
  }
  }
}
}
