using System;

namespace DianaScript
{
public partial class DBuiltinFunc
{
  public DClsObj Class => Cls.unique;
  public partial class Cls : DClsObj  {
  public string name => "builtin_function";
  public static Cls unique = new Cls();
  public Type NativeType => typeof(DBuiltinFunc);
  public System.Collections.Generic.Dictionary<InternString, DObj> Dict {get; set;}
  public Cls()
  {
    DWrap.RegisterTypeMap(NativeType, this);
    Dict = new System.Collections.Generic.Dictionary<InternString, DObj>
    {
    };
  }
  }
}
}
