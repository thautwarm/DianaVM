using System;

namespace DianaScript
{
public partial class DFloat
{
  public DClsObj GetCls => Cls.unique;
  public static DObj bind_parse(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"calling float.parse; needs at least  (1) arguments, got {nargs}.");
    var _arg0 = MK.unbox<String>(_args[0]);
    {
      var _return = Single.Parse(_arg0);
      return MK.create(_return);
    }
    throw new D_TypeError($"call float.parse; needs at most (1) arguments, got {nargs}.");
  }
  public static DObj bind_try_parse(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling float.try_parse; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<String>(_args[0]);
    var _out_1 = MK.unbox<Ref>(_args[1]);
    var _arg1 = MK.unbox<Single>(_out_1.get_contents());
    {
      var _return = Single.TryParse(_arg0,out _arg1);
      _out_1.set_contents(MK.cast(THint<DObj>.val, _arg1));
      return MK.create(_return);
    }
    throw new D_TypeError($"call float.try_parse; needs at most (2) arguments, got {nargs}.");
  }
  public partial class Cls : DClsObj  {
  public string name => "float";
  public static Cls unique = new Cls();
  public Type NativeType => typeof(DFloat);
  public System.Collections.Generic.Dictionary<InternString, (bool, DObj)> Getters {get; set;}
  public Cls()
  {
    DWrap.RegisterTypeMap(NativeType, this);
    Getters = new System.Collections.Generic.Dictionary<InternString, (bool, DObj)>
    {
      { "parse".ToIStr(), (false, MK.CreateFunc(bind_parse)) },
      { "try_parse".ToIStr(), (false, MK.CreateFunc(bind_try_parse)) },
    };
  }
  }
}
}
