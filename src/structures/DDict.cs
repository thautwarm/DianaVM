using System;
using System.Collections.Generic;
namespace DianaScript
{
public partial class DDict
{
  public DClsObj GetCls => Cls.unique;
  public static DObj bind___contains__(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling dict.__contains__; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<Dictionary<DObj, DObj>>(_args[0]);
    var _arg1 = MK.unbox<DObj>(_args[1]);
    {
      var _return = _arg0.ContainsKey(_arg1);
      return MK.create(_return);
    }
    throw new D_TypeError($"call dict.__contains__; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind___len__(Args _args) // bind `this` prop 
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"accessing dict.__len__; needs only 1 argument, got {nargs}.");
    var arg = _args[0];
    var ret = MK.unbox<Dictionary<DObj, DObj>>(arg).Count;
    return MK.create(ret);
  }
  public static DObj bind___delitem__(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling dict.__delitem__; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<Dictionary<DObj, DObj>>(_args[0]);
    var _arg1 = MK.unbox<DObj>(_args[1]);
    {
      _arg0.Remove(_arg1);
      return MK.Nil();
    }
    throw new D_TypeError($"call dict.__delitem__; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind_clear(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"calling dict.clear; needs at least  (1) arguments, got {nargs}.");
    var _arg0 = MK.unbox<Dictionary<DObj, DObj>>(_args[0]);
    {
      _arg0.Clear();
      return MK.Nil();
    }
    throw new D_TypeError($"call dict.clear; needs at most (1) arguments, got {nargs}.");
  }
  public static DObj bind_search(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs != 3)
      throw new D_TypeError($"calling dict.search; needs at least  (3) arguments, got {nargs}.");
    var _arg0 = MK.unbox<Dictionary<DObj, DObj>>(_args[0]);
    var _arg1 = MK.unbox<DObj>(_args[1]);
    var _out_2 = MK.unbox<Ref>(_args[2]);
    var _arg2 = MK.unbox<DObj>(_out_2.get_contents());
    {
      var _return = _arg0.TryGetValue(_arg1,out _arg2);
      _out_2.set_contents(MK.cast(THint<DObj>.val, _arg2));
      return MK.create(_return);
    }
    throw new D_TypeError($"call dict.search; needs at most (3) arguments, got {nargs}.");
  }
  public static DObj bind___setitem__(Args _args) // bind this.[ind]=val 
  {
    var nargs = _args.NArgs;
    if (nargs != 3)
      throw new D_ValueError("calling dict.__setitem__; needs 3 arguments, got {nargs}.");
    var _arg0 = MK.unbox<Dictionary<DObj, DObj>>(_args[0]);
    var _arg1 = MK.unbox<DObj>(_args[1]);
    var _arg2 = MK.unbox<DObj>(_args[2]);
    _arg0[_arg1] = _arg2;
    return MK.Nil();
  }
  public static DObj bind___getitem__(Args _args) // bind this.[ind] 
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_ValueError("calling dict.__getitem__; needs 2 arguments, got {nargs}.");
    var _arg0 = MK.unbox<Dictionary<DObj, DObj>>(_args[0]);
    var _arg1 = MK.unbox<DObj>(_args[1]);
    var _return = _arg0[_arg1];
    return MK.create(_return);
  }
  public partial class Cls : DClsObj  {
  public string name => "dict";
  public static Cls unique = new Cls();
  public Type NativeType => typeof(DDict);
  public System.Collections.Generic.Dictionary<InternString, (bool, DObj)> Getters {get; set;}
  public Cls()
  {
    DWrap.RegisterTypeMap(NativeType, this);
    Getters = new System.Collections.Generic.Dictionary<InternString, (bool, DObj)>
    {
      { "__contains__".ToIStr(), (false, MK.CreateFunc(bind___contains__)) },
      { "__len__".ToIStr(), (false, MK.CreateFunc(bind___len__)) },
      { "__delitem__".ToIStr(), (false, MK.CreateFunc(bind___delitem__)) },
      { "clear".ToIStr(), (false, MK.CreateFunc(bind_clear)) },
      { "search".ToIStr(), (false, MK.CreateFunc(bind_search)) },
      { "__setitem__".ToIStr(), (false, MK.CreateFunc(bind___setitem__)) },
      { "__getitem__".ToIStr(), (false, MK.CreateFunc(bind___getitem__)) },
    };
  }
  }
}
}
