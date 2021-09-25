using System;
using System.Collections.Generic;
namespace DianaScript
{
public partial class DArray
{
  public DClsObj GetCls => Cls.unique;
  public static DObj bind___len__(Args _args)
  {
    var nargs = _args.NArgs;
    if (nargs != 1)
      throw new D_TypeError($"accessing array.__len__; needs only 1 argument, got {nargs}.");
    var arg = _args[0];
    var ret = MK.unbox<DArray>(arg).Count;
    return MK.create(ret);
  }
  public partial class Cls : DClsObj  {
  public string name => "array";
  public static Cls unique = new Cls();
  public Type NativeType => typeof(DArray);
  public System.Collections.Generic.Dictionary<string, (bool, DObj)> Getters {get; set;}
  public Cls()
  {
    DWrap.RegisterTypeMap(NativeType, this);
    Getters = new System.Collections.Generic.Dictionary<string, (bool, DObj)>
    {
      { "__len__", (false, MK.CreateFunc(bind___len__)) },
    };
  }
  }
}
}
