using System;

namespace DianaScript
{
public partial class DStr
{
  public DClsObj GetCls => Cls.unique;
  public static DObj bind_join(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling str.join; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<String>(_args[0]);
    var _arg1 = MK.cast(THelper<String[]>.val, MK.unbox<DObj>(_args[1]));
    {
      var _return = String.Join(_arg0,_arg1);
      return MK.create(_return);
    }
    throw new D_TypeError($"call str.join; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind_concat(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"calling str.concat; needs at least  (1) arguments, got {nargs}.");
    var _arg0 = MK.cast(THelper<String[]>.val, MK.unbox<DObj>(_args[0]));
    {
      var _return = String.Concat(_arg0);
      return MK.create(_return);
    }
    throw new D_TypeError($"call str.concat; needs at most (1) arguments, got {nargs}.");
  }
  public static DObj bind_endswith(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling str.endswith; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<String>(_args[0]);
    var _arg1 = MK.unbox<String>(_args[1]);
    {
      var _return = _arg0.EndsWith(_arg1);
      return MK.create(_return);
    }
    throw new D_TypeError($"call str.endswith; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind_startswith(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling str.startswith; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<String>(_args[0]);
    var _arg1 = MK.unbox<String>(_args[1]);
    {
      var _return = _arg0.StartsWith(_arg1);
      return MK.create(_return);
    }
    throw new D_TypeError($"call str.startswith; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind___len__(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"accessing str.__len__; needs only 1 argument, got {nargs}.");
    var arg = _args[0];
    var ret = MK.unbox<String>(arg).Length;
    return MK.create(ret);
  }
  public static DObj bind_strip(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling str.strip; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<String>(_args[0]);
    var _arg1 = MK.cast(THelper<Char[]>.val, MK.unbox<String>(_args[1]));
    {
      var _return = _arg0.Trim(_arg1);
      return MK.create(_return);
    }
    throw new D_TypeError($"call str.strip; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind_rstrip(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs < 1)
      throw new D_TypeError($"calling str.rstrip; needs at least  (1,2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<String>(_args[0]);
    if (nargs == 1)
    {
      var _return = _arg0.TrimEnd();
      return MK.create(_return);
    }
    var _arg1 = MK.cast(THelper<Char[]>.val, MK.unbox<String>(_args[1]));
    {
      var _return = _arg0.TrimEnd(_arg1);
      return MK.create(_return);
    }
    throw new D_TypeError($"call str.rstrip; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind_lstrip(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs < 1)
      throw new D_TypeError($"calling str.lstrip; needs at least  (1,2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<String>(_args[0]);
    if (nargs == 1)
    {
      var _return = _arg0.TrimStart();
      return MK.create(_return);
    }
    var _arg1 = MK.cast(THelper<Char[]>.val, MK.unbox<String>(_args[1]));
    {
      var _return = _arg0.TrimStart(_arg1);
      return MK.create(_return);
    }
    throw new D_TypeError($"call str.lstrip; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind_lower(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"calling str.lower; needs at least  (1) arguments, got {nargs}.");
    var _arg0 = MK.unbox<String>(_args[0]);
    {
      var _return = _arg0.ToLowerInvariant();
      return MK.create(_return);
    }
    throw new D_TypeError($"call str.lower; needs at most (1) arguments, got {nargs}.");
  }
  public static DObj bind_upper(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"calling str.upper; needs at least  (1) arguments, got {nargs}.");
    var _arg0 = MK.unbox<String>(_args[0]);
    {
      var _return = _arg0.ToUpperInvariant();
      return MK.create(_return);
    }
    throw new D_TypeError($"call str.upper; needs at most (1) arguments, got {nargs}.");
  }
  public static DObj bind___contains__(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 2)
      throw new D_TypeError($"calling str.__contains__; needs at least  (2) arguments, got {nargs}.");
    var _arg0 = MK.unbox<String>(_args[0]);
    var _arg1 = MK.unbox<String>(_args[1]);
    {
      var _return = _arg0.Contains(_arg1);
      return MK.create(_return);
    }
    throw new D_TypeError($"call str.__contains__; needs at most (2) arguments, got {nargs}.");
  }
  public static DObj bind_format(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs < 1)
      throw new D_TypeError($"calling str.format; needs at least >= (1) arguments, got {nargs}.");
    var _arg0 = MK.unbox<String>(_args[0]);
    if (nargs == 1)
    {
      var _return = String.Format(_arg0);
      return MK.create(_return);
    }
    var _arg1 = new Object[nargs - 1 - 1];
    for(var _i = 1; _i < nargs; _i++)
      _arg1[_i - 1] = _args[_i];
    {
      var _return = String.Format(_arg0,_arg1);
      return MK.create(_return);
    }
  }
  public static DObj bind_substr(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs < 2)
      throw new D_TypeError($"calling str.substr; needs at least  (2,3) arguments, got {nargs}.");
    var _arg0 = MK.unbox<String>(_args[0]);
    var _arg1 = MK.unbox<Int32>(_args[1]);
    if (nargs == 2)
    {
      var _return = _arg0.Substring(_arg1);
      return MK.create(_return);
    }
    var _arg2 = MK.unbox<Int32>(_args[2]);
    {
      var _return = _arg0.Substring(_arg1,_arg2);
      return MK.create(_return);
    }
    throw new D_TypeError($"call str.substr; needs at most (3) arguments, got {nargs}.");
  }
  public static DObj bind_insert(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 3)
      throw new D_TypeError($"calling str.insert; needs at least  (3) arguments, got {nargs}.");
    var _arg0 = MK.unbox<String>(_args[0]);
    var _arg1 = MK.unbox<Int32>(_args[1]);
    var _arg2 = MK.unbox<String>(_args[2]);
    {
      var _return = _arg0.Insert(_arg1,_arg2);
      return MK.create(_return);
    }
    throw new D_TypeError($"call str.insert; needs at most (3) arguments, got {nargs}.");
  }
  public static DObj bind_remove(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs < 2)
      throw new D_TypeError($"calling str.remove; needs at least  (2,3) arguments, got {nargs}.");
    var _arg0 = MK.unbox<String>(_args[0]);
    var _arg1 = MK.unbox<Int32>(_args[1]);
    if (nargs == 2)
    {
      var _return = _arg0.Remove(_arg1);
      return MK.create(_return);
    }
    var _arg2 = MK.unbox<Int32>(_args[2]);
    {
      var _return = _arg0.Remove(_arg1,_arg2);
      return MK.create(_return);
    }
    throw new D_TypeError($"call str.remove; needs at most (3) arguments, got {nargs}.");
  }
  public static DObj bind_index(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs < 2)
      throw new D_TypeError($"calling str.index; needs at least  (2,3,4) arguments, got {nargs}.");
    var _arg0 = MK.unbox<String>(_args[0]);
    var _arg1 = MK.unbox<String>(_args[1]);
    if (nargs == 2)
    {
      var _return = _arg0.IndexOf(_arg1);
      return MK.create(_return);
    }
    var _arg2 = MK.unbox<Int32>(_args[2]);
    if (nargs == 3)
    {
      var _return = _arg0.IndexOf(_arg1,_arg2);
      return MK.create(_return);
    }
    var _arg3 = MK.unbox<Int32>(_args[3]);
    {
      var _return = _arg0.IndexOf(_arg1,_arg2,_arg3);
      return MK.create(_return);
    }
    throw new D_TypeError($"call str.index; needs at most (4) arguments, got {nargs}.");
  }
  public partial class Cls : DClsObj  {
  public string name => "str";
  public static Cls unique = new Cls();
  public Type NativeType => typeof(DStr);
  public System.Collections.Generic.Dictionary<string, (bool, DObj)> Getters {get; set;}
  public Cls()
  {
    DWrap.RegisterTypeMap(NativeType, this);
    Getters = new System.Collections.Generic.Dictionary<string, (bool, DObj)>
    {
      { "join", (false, MK.CreateFunc(bind_join)) },
      { "concat", (false, MK.CreateFunc(bind_concat)) },
      { "endswith", (false, MK.CreateFunc(bind_endswith)) },
      { "startswith", (false, MK.CreateFunc(bind_startswith)) },
      { "__len__", (false, MK.CreateFunc(bind___len__)) },
      { "strip", (false, MK.CreateFunc(bind_strip)) },
      { "rstrip", (false, MK.CreateFunc(bind_rstrip)) },
      { "lstrip", (false, MK.CreateFunc(bind_lstrip)) },
      { "lower", (false, MK.CreateFunc(bind_lower)) },
      { "upper", (false, MK.CreateFunc(bind_upper)) },
      { "__contains__", (false, MK.CreateFunc(bind___contains__)) },
      { "format", (false, MK.CreateFunc(bind_format)) },
      { "substr", (false, MK.CreateFunc(bind_substr)) },
      { "insert", (false, MK.CreateFunc(bind_insert)) },
      { "remove", (false, MK.CreateFunc(bind_remove)) },
      { "index", (false, MK.CreateFunc(bind_index)) },
    };
  }
  }
}
}
