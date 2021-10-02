using System;

namespace DianaScript
{
public partial class DRefGlobal
{
  public DClsObj Class => Cls.unique;
  public partial class Cls : DClsObj  {
  public string name => "dictcell";
  public static Cls unique = new Cls();
  public Type NativeType => typeof(DRefGlobal);
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
