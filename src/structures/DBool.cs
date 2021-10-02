using System;

namespace DianaScript
{
public partial class DBool
{
  public DClsObj Class => Cls.unique;
  public static DObj bind_parse(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"calling bool.parse; needs at least  (1) arguments, got {nargs}.");
    var _arg0 = MK.unbox<String>(_args[0]);
    {
      var _return = Boolean.Parse(_arg0);
      return MK.create(_return);
    }
    throw new D_TypeError($"call bool.parse; needs at most (1) arguments, got {nargs}.");
  }
  public static DObj bind_try_parse(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling bool.try_parse; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<String>(_args[0]);
    var _out_1 = MK.unbox<Ref>(_args[1]);
    var _arg1 = MK.unbox<Boolean>(_out_1.get_contents());
    {
      var _return = Boolean.TryParse(_arg0,out _arg1);
      _out_1.set_contents(MK.cast(THint<DObj>.val, _arg1));
      return MK.create(_return);
    }
    throw new D_TypeError($"call bool.try_parse; needs at most (2) arguments, got {nargs}.");
  }
  public partial class Cls : DClsObj  {
  public string name => "bool";
  public static Cls unique = new Cls();
  public Type NativeType => typeof(DBool);
  public System.Collections.Generic.Dictionary<InternString, DObj> Dict {get; set;}
  public Cls()
  {
    DWrap.RegisterTypeMap(NativeType, this);
    Dict = new System.Collections.Generic.Dictionary<InternString, DObj>
    {
      { "parse".ToIStr(), MK.CreateFunc(bind_parse) },
      { "try_parse".ToIStr(), MK.CreateFunc(bind_try_parse) },
    };
  }
  }
}
}
