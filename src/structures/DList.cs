using System;
using System.Collections.Generic;
namespace DianaScript
{
public partial class DList
{
  public DClsObj GetCls => Cls.unique;
  public static DObj bind___contains__(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling list.__contains__; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<List<DObj>>(_args[0]);
    var _arg1 = MK.unbox<DObj>(_args[1]);
    {
      var _return = _arg0.Contains(_arg1);
      return MK.create(_return);
    }
    throw new D_TypeError($"call list.__contains__; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind_append(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling list.append; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<List<DObj>>(_args[0]);
    var _arg1 = MK.unbox<DObj>(_args[1]);
    {
      _arg0.Add(_arg1);
      return MK.Nil();
    }
    throw new D_TypeError($"call list.append; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind_extend(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling list.extend; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<List<DObj>>(_args[0]);
    var _arg1 = MK.cast(THelper<IEnumerable<DObj>>.val, MK.unbox<DObj>(_args[1]));
    {
      _arg0.AddRange(_arg1);
      return MK.Nil();
    }
    throw new D_TypeError($"call list.extend; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind_insert(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 3)
      throw new D_TypeError($"calling list.insert; needs at least  (3) arguments, got {nargs}.");
    var _arg0 = MK.unbox<List<DObj>>(_args[0]);
    var _arg1 = MK.unbox<Int32>(_args[1]);
    var _arg2 = MK.unbox<DObj>(_args[2]);
    {
      _arg0.Insert(_arg1,_arg2);
      return MK.Nil();
    }
    throw new D_TypeError($"call list.insert; needs at most (3) arguments, got {nargs}.");
  }
  public static DObj bind_remove(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling list.remove; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<List<DObj>>(_args[0]);
    var _arg1 = MK.unbox<DObj>(_args[1]);
    {
      _arg0.Remove(_arg1);
      return MK.Nil();
    }
    throw new D_TypeError($"call list.remove; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind_find(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling list.find; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<List<DObj>>(_args[0]);
    var _arg1 = MK.unbox<Predicate<DObj>>(_args[1]);
    {
      _arg0.Find(_arg1);
      return MK.Nil();
    }
    throw new D_TypeError($"call list.find; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind_index(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs < 2)
      throw new D_TypeError($"calling list.index; needs at least  (2,3) arguments, got {nargs}.");
    var _arg0 = MK.unbox<List<DObj>>(_args[0]);
    var _arg1 = MK.unbox<DObj>(_args[1]);
    if (nargs == 2)
    {
      _arg0.IndexOf(_arg1);
      return MK.Nil();
    }
    var _arg2 = MK.unbox<Int32>(_args[2]);
    if (nargs == 3)
    {
      _arg0.IndexOf(_arg1,_arg2);
      return MK.Nil();
    }
    throw new D_TypeError($"call list.index; needs at most (3) arguments, got {nargs}.");
  }
  public static DObj bind___delitem__(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling list.__delitem__; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<List<DObj>>(_args[0]);
    var _arg1 = MK.unbox<Int32>(_args[1]);
    {
      _arg0.RemoveAt(_arg1);
      return MK.Nil();
    }
    throw new D_TypeError($"call list.__delitem__; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind_sort(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"calling list.sort; needs at least  (1) arguments, got {nargs}.");
    var _arg0 = MK.unbox<List<DObj>>(_args[0]);
    {
      _arg0.Sort();
      return MK.Nil();
    }
    throw new D_TypeError($"call list.sort; needs at most (1) arguments, got {nargs}.");
  }
  public static DObj bind_array(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"calling list.array; needs at least  (1) arguments, got {nargs}.");
    var _arg0 = MK.unbox<List<DObj>>(_args[0]);
    {
      var _return = _arg0.ToArray();
      return MK.create(_return);
    }
    throw new D_TypeError($"call list.array; needs at most (1) arguments, got {nargs}.");
  }
  public static DObj bind___len__(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"accessing list.__len__; needs only 1 argument, got {nargs}.");
    var arg = _args[0];
    var ret = MK.unbox<List<DObj>>(arg).Count;
    return MK.create(ret);  }
  public static DObj bind_clear(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"calling list.clear; needs at least  (1) arguments, got {nargs}.");
    var _arg0 = MK.unbox<List<DObj>>(_args[0]);
    {
      _arg0.Clear();
      return MK.Nil();
    }
    throw new D_TypeError($"call list.clear; needs at most (1) arguments, got {nargs}.");
  }
  public partial class Cls : DClsObj  {
  public string name => "list";
  public static Cls unique = new Cls();
  public Type NativeType => typeof(DList);
  public System.Collections.Generic.Dictionary<string, (bool, DObj)> Getters {get; set;}
  public Cls()
  {
    Getters = new System.Collections.Generic.Dictionary<string, (bool, DObj)>
    {
      { "__contains__", (false, MK.CreateFunc(bind___contains__)) },
      { "append", (false, MK.CreateFunc(bind_append)) },
      { "extend", (false, MK.CreateFunc(bind_extend)) },
      { "insert", (false, MK.CreateFunc(bind_insert)) },
      { "remove", (false, MK.CreateFunc(bind_remove)) },
      { "find", (false, MK.CreateFunc(bind_find)) },
      { "index", (false, MK.CreateFunc(bind_index)) },
      { "__delitem__", (false, MK.CreateFunc(bind___delitem__)) },
      { "sort", (false, MK.CreateFunc(bind_sort)) },
      { "array", (false, MK.CreateFunc(bind_array)) },
      { "__len__", (false, MK.CreateFunc(bind___len__)) },
      { "clear", (false, MK.CreateFunc(bind_clear)) },
    };
  }
  }
}
}
