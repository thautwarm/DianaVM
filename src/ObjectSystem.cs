using System;
using System.Collections.Generic;
namespace DianaScript
{

    using DObject_Call_t = Func<DObj, Args, DObj>;

    public interface DObj
    {
        public DClsObj GetCls { get; set; }
        public object native => this;
        public string repr => GetCls.reprfunc(this);

        public string ToString() => GetCls.strfunc(this);

    }

    public class DClsObj : DObj
    {
        public DClsObj GetCls { get; set; }
        public DStr name;
        public Func<DObj, string, DObj> get;
        public Func<DObj, string, DObj> set;

        public DObject_Call_t call;
        public Func<DObj, string> reprfunc = o => o.GetCls.strfunc(o);
        public Func<DObj, string> strfunc = o => o.native.ToString();
        public Type native_type => ((DObj) this).native.GetType();


    }
    
    public class DError : DObj {

        public DClsObj GetCls { get; set; }
        public DObj[] args;
        public int error_id;
    }
    public class DCode : DObj
    {
        public DClsObj GetCls { get; set; }
        public int[] bc;
        public DObj[] consts;
        public int nfree;
        public int narg;
        public int nlocal;
        public bool varg;
        public string filename;
        public int[] locs;
    }

    public class DFunc : DObj
    {
        public DClsObj GetCls { get; set; }
        public DCode code;

        public DObj[] freevals;
        public DObj[] defaults;
    }
    
    public class DFrame: DObj
    {
        public DClsObj GetCls { get; set; }
        public int offset;
        public List<DObj> vstack;
        public List<(int, int)> estack;
        public DObj[] localvals;
        public DFunc func;

        public DCode code => func.code;

        public DObj[] freevals => func.freevals;
    }

    public class DInt: DObj{
        public DClsObj GetCls { get; set; }
        public int value;

        public object native => this.value;
    }


    public class DBool: DObj{
        public DClsObj GetCls { get; set; }
        public bool value;

        public object native => this.value;
    }


    public class DTuple: DObj{
        public DClsObj GetCls { get; set; }
        public DObj[] elts;

    }

    public class DStr: DObj{
        public DClsObj GetCls { get; set; }
        public string value;
        public object native => this.value;
    }

    public class DFloat: DObj{
        public DClsObj GetCls { get; set; }
        public float value;
        public object native => this.value;
    }
    public class DNil: DObj{
        public DClsObj GetCls { get; set; }
        public object native => this;
    }
    public class DNativeWrap: DObj{
        public DClsObj GetCls { get; set; }
        public object value;
        public object native => value;
    }

    // builtin functions
    public class BFunc0: DObj{
        public DClsObj GetCls { get; set; }
        public Func<DObj> value;
        public object native => value;
    }
    public class BFunc1: DObj{
        public DClsObj GetCls { get; set; }
        public Func<DObj, DObj> value;
        public object native => value;
    }
    
    public class BFunc2: DObj{
        public DClsObj GetCls { get; set; }
        public Func<DObj, DObj, DObj> value;
        public object native => value;
    }

    public class BFunc3: DObj{
        public DClsObj GetCls { get; set; }
        public Func<DObj, DObj, DObj, DObj> value;
        public object native => value;
    }
    

}