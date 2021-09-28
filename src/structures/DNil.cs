using System;

namespace DianaScript
{
public partial class DNil
{
  public DClsObj GetCls => Cls.unique;
  public partial class Cls : DClsObj  {
  public string name => "NoneType";
  public static Cls unique = new Cls();
  public Type NativeType => typeof(DNil);
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
