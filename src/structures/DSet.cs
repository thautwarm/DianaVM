using System;
using System.Collections.Generic;
namespace DianaScript
{
public partial class DSet
{
  public DClsObj Class => Cls.unique;
  public static DObj bind___or__(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling set.__or__; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<HashSet<DObj>>(_args[0]);
    var _arg1 = MK.unbox<IEnumerable<DObj>>(_args[1]);
    {
      _arg0.UnionWith(_arg1);
      return MK.Nil();
    }
    throw new D_TypeError($"call set.__or__; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind___and__(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling set.__and__; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<HashSet<DObj>>(_args[0]);
    var _arg1 = MK.unbox<IEnumerable<DObj>>(_args[1]);
    {
      _arg0.IntersectWith(_arg1);
      return MK.Nil();
    }
    throw new D_TypeError($"call set.__and__; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind_subset(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling set.subset; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<HashSet<DObj>>(_args[0]);
    var _arg1 = MK.unbox<IEnumerable<DObj>>(_args[1]);
    {
      var _return = _arg0.IsSubsetOf(_arg1);
      return MK.create(_return);
    }
    throw new D_TypeError($"call set.subset; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind_superset(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling set.superset; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<HashSet<DObj>>(_args[0]);
    var _arg1 = MK.unbox<IEnumerable<DObj>>(_args[1]);
    {
      var _return = _arg0.IsSupersetOf(_arg1);
      return MK.create(_return);
    }
    throw new D_TypeError($"call set.superset; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind___contains__(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling set.__contains__; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<HashSet<DObj>>(_args[0]);
    var _arg1 = MK.unbox<DObj>(_args[1]);
    {
      var _return = _arg0.Contains(_arg1);
      return MK.create(_return);
    }
    throw new D_TypeError($"call set.__contains__; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind_add(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling set.add; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<HashSet<DObj>>(_args[0]);
    var _arg1 = MK.unbox<DObj>(_args[1]);
    {
      var _return = _arg0.Add(_arg1);
      return MK.create(_return);
    }
    throw new D_TypeError($"call set.add; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind_remove(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling set.remove; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<HashSet<DObj>>(_args[0]);
    var _arg1 = MK.unbox<DObj>(_args[1]);
    {
      var _return = _arg0.Remove(_arg1);
      return MK.create(_return);
    }
    throw new D_TypeError($"call set.remove; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind___len__(Args _args) // bind `this` prop 
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"accessing set.__len__; needs only 1 argument, got {nargs}.");
    var arg = _args[0];
    var ret = MK.unbox<HashSet<DObj>>(arg).Count;
    return MK.create(ret);
  }
  public static DObj bind_clear(Args _args) // bind method 
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"calling set.clear; needs at least  (1) arguments, got {nargs}.");
    var _arg0 = MK.unbox<HashSet<DObj>>(_args[0]);
    {
      _arg0.Clear();
      return MK.Nil();
    }
    throw new D_TypeError($"call set.clear; needs at most (1) arguments, got {nargs}.");
  }
  public partial class Cls : DClsObj  {
  public string name => "set";
  public static Cls unique = new Cls();
  public Type NativeType => typeof(DSet);
  public System.Collections.Generic.Dictionary<InternString, DObj> Dict {get; set;}
  public Cls()
  {
    DWrap.RegisterTypeMap(NativeType, this);
    Dict = new System.Collections.Generic.Dictionary<InternString, DObj>
    {
      { "__or__".ToIStr(), MK.CreateFunc(bind___or__) },
      { "__and__".ToIStr(), MK.CreateFunc(bind___and__) },
      { "subset".ToIStr(), MK.CreateFunc(bind_subset) },
      { "superset".ToIStr(), MK.CreateFunc(bind_superset) },
      { "__contains__".ToIStr(), MK.CreateFunc(bind___contains__) },
      { "add".ToIStr(), MK.CreateFunc(bind_add) },
      { "remove".ToIStr(), MK.CreateFunc(bind_remove) },
      { "__len__".ToIStr(), MK.CreateFunc(bind___len__) },
      { "clear".ToIStr(), MK.CreateFunc(bind_clear) },
    };
  }
  }
}
}
