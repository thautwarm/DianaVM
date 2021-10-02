using System;

namespace DianaScript
{
public partial class DFunc
{
  public DClsObj Class => Cls.unique;
  public partial class Cls : DClsObj  {
  public string name => "function";
  public static Cls unique = new Cls();
  public Type NativeType => typeof(DFunc);
  public System.Collections.Generic.Dictionary<InternString, (bool, DObj)> Getters {get; set;}
  public Cls()
  {
    DWrap.RegisterTypeMap(NativeType, this);
    Getters = new System.Collections.Generic.Dictionary<InternString, (bool, DObj)>
    {
    };
  }
  }
}
}
