using System;

namespace DianaScript
{
public partial class DRef
{
  public DClsObj GetCls => Cls.unique;
  public partial class Cls : DClsObj  {
  public string name => "cell";
  public static Cls unique = new Cls();
  public Type NativeType => typeof(DRef);
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
