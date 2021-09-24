using System;
using System.Collections.Generic;


namespace DianaScript
{
    using Args = List<DObj>;

    public interface DObj
    {
        public DClsObj GetCls { get; }
        public object Native { get; }
        public DObj Call(Args args) => throw new D_TypeError($"{this.GetCls.Repr} instances are not callable");

        public DObj Get(string attr);
        public void Set(string attr, DObj o);
        IEnumerable<DObj> Iter => throw new D_TypeError($"{this.GetCls.Repr} instances are not iterable");
        bool Test => true;
        bool Not => !Test;
        public string Repr { get; }
        public string ToStr => Repr;
        public DObj BinOp(string op, DObj other) => throw new D_TypeError($"{this.GetCls.Repr} does not support {op} operator.");


    }


    // public class daccessor : DClsObj
    // {
    //     public static daccessor cls = new daccessor();
    //     public Dictionary<string, DAccessor> accessors => null;

    //     public string name => nameof(daccessor);
    // }
    // public class DAccessor : DInstObj
    // {
    //     public DObj Getter, Setter, Method;
    //     DClsObj sourceclass;
    //     public string attr;
    //     public bool bound;

    //     public DClsObj GetCls => daccessor.cls;

    //     public object Native => this;

    //     public string Repr => $"<{sourceclass.Repr}.{attr}>";

    //     public DObj defaultSetter(Args args)
    //     {
    //         throw new D_AttributeError(
    //             $"cannot set attribute {attr} for {sourceclass.Repr} instance.");
    //     }

    //     public DObj defaultGetter(Args args)
    //     {
    //         throw new D_AttributeError(
    //             $"cannot get attribute {attr} for {sourceclass.Repr} instance.");
    //     }

    //     public DObj defaultMethod(Args args)
    //     {
    //         throw new D_TypeError(
    //             $"no method {attr} for {sourceclass.Repr} instance.");
    //     }
    //     public DAccessor(DClsObj sourceclass, string attr, DObj getter=null,  DObj setter=null, DObj method=null, bool bound = true)
    //     {
    //         this.sourceclass = sourceclass;
    //         this.Getter = getter??MK.FuncN(defaultGetter);
    //         this.Setter = setter??MK.FuncN(defaultSetter);
    //         this.Method = method??MK.FuncN(defaultMethod);
    //         this.bound = bound;
    //     }

    // }
    public interface DClsObj : DObj
    {

        public DObj obj => this;
        public Dictionary<string, DObj> getters => null;
        public Dictionary<string, DObj> setters => null;
        public string name { get; }

        object DObj.Native => this;

        DClsObj DObj.GetCls => meta.cls;
        DObj DObj.Get(string attr)
        {
            DObj o;
            if (getters != null && getters.TryGetValue(attr, out o)) return o;
            throw new D_AttributeError(this, MK.String(attr));
        }
        void DObj.Set(string s, DObj v)
        {
            getters[s] = v;
        }
        string DObj.Repr => $"<{this.name}>";


        // __dict__ returns None if dict
    }


    public class meta : DClsObj
    {
        public static meta cls = new meta();

        public Dictionary<string, DObj> getters { set; get; }
        public Dictionary<string, DObj> setters { set; get; }

        public string name => nameof(meta);

        public DClsObj GetCls => this;



    }

    public interface DInstObj : DObj
    {

        DObj DObj.Get(string attr)
        {
            DObj o;
            var getters = this.GetCls.getters;
            if (getters != null && getters.TryGetValue(attr, out o))
            {
                return o;
            }

            throw new D_AttributeError(this.GetCls, MK.String(attr));

        }

        void DObj.Set(string attr, DObj value)
        {
            DObj o;
            var setters = this.GetCls.setters;
            if (setters != null && setters.TryGetValue(attr, out o))
            {
                o.Call(new Args { this, value });
            }

            throw new D_AttributeError(this.GetCls, MK.String(attr));
        }
    }

    public class dcode : DClsObj
    {

        public static dcode cls = new dcode();
        public string name => nameof(dcode);

    }
    public class DCode : DInstObj
    {
        public DClsObj GetCls => dcode.cls;

        public object Native => this;
        public string Repr => "<codeobj>";

        public int[] bc;
        public DObj[] consts;
        public int nfree;
        public int narg;
        public int nlocal;
        public bool varg;
        public string filename;
        public string name;
        public int[] locs;


    }

    public class dfunc : DClsObj
    {
        public static dfunc cls = new dfunc();

        public string name => nameof(dfunc);
    }

    public class DFunc : DInstObj
    {
        public DClsObj GetCls => dfunc.cls;

        public object Native => this;

        public string Repr => $"<function {code.name}>";

        public DCode code;

        public DObj[] freevals;
        public DObj[] defaults;
    }

    public class dframe : DClsObj
    {
        public static dframe cls = new dframe();

        public string name => nameof(dframe);
    }
    public class DFrame : DInstObj
    {
        public DClsObj GetCls { get; set; }
        public int offset;
        public List<DObj> vstack;
        public List<(int, int)> estack;
        public DObj[] localvals;
        public DFunc func;

        public DCode code => func.code;

        public DObj[] freevals => func.freevals;

        public object Native => this;

        public string Repr => $"<frame {func.code.name}>";
    }

    public class dint : DClsObj
    {
        public static dint cls = new dint();
        public Dictionary<string, DObj> getters;

        DObj TryParse(DObj i)
        {
            var s = i as DStr;
            int v;
            if (int.TryParse(s.value, out v))
                return MK.Int(v);
            return MK.Nil();
        }
        public dint()
        {
            getters = new Dictionary<string, DObj>{
                {"TryParse", MK.Func1(TryParse)},
                {"Parse",MK.Func1(x => MK.Int(int.Parse((x as DStr).value)))},
                {"MaxValue", MK.Func0(() => MK.Int(int.MaxValue))},
                {"MinValue", MK.Func0(() => MK.Int(int.MinValue))}
            };
        }


        public DObj Call(Args args)
        {
            if (args.Count == 0) return MK.Int(0);
            if (args.Count != 1) throw new D_TypeError($"object {(this as DObj).Repr} is not callable.");
            var x = args[1];
            switch (x)
            {
                case DInt _:
                    return x;
                case DBool b:
                    return MK.Int(b.value ? 1 : 0);
                case DFloat f:
                    return MK.Int((int)f.value);
                case DStr s:
                    return MK.Int(int.Parse(s.value));
                default:
                    throw new D_TypeError($"cannot cast {x.GetCls.Repr} to {(this as DObj).Repr}");
            }
            throw new NotImplementedException();
        }

        public string name => "int";
    }
    public class DInt : DInstObj
    {
        public DClsObj GetCls => dint.cls;
        public int value;

        public bool Test => value != 0;

        object DObj.Native => value;

        string DObj.Repr => value.ToString();

        public static implicit operator DInt(int d) => MK.Int(d);
        public static implicit operator int(DInt d) => d.value;
    }

    public class dbool : DClsObj
    {
        public static dbool cls = new dbool();
        public Dictionary<string, DObj> getters;


        public DObj TryParse(DObj i)
        {
            var s = i as DStr;
            bool v;
            if (bool.TryParse(s.value, out v))
                return MK.Bool(v);
            return MK.Nil();
        }
        public dbool()
        {
            getters = new Dictionary<string, DObj>{
                {"TryParse", MK.Func1(TryParse)},
                {"Parse",MK.Func1(x => MK.Bool(bool.Parse((x as DStr).value)))},
                {"FalseString", MK.Func0(() => MK.String(bool.FalseString))},
                {"TrueString", MK.Func0(() => MK.String(bool.TrueString))}
            };
        }

        public DObj Call(Args args)
        {
            if (args.Count == 0) return MK.Bool(false);
            if (args.Count != 1) throw new D_TypeError($"object {(this as DObj).Repr} is not callable.");
            var x = args[1];
            switch (x)
            {
                case DBool _:
                    return x;
                case DInt i:
                    return MK.Bool(i.value != 0);
                case DFloat f:
                    return MK.Bool(f.value != 0.0f);
                case DStr s:
                    return MK.Bool(bool.Parse(s.value));
                default:
                    throw new D_TypeError($"cannot cast {x.GetCls.Repr} to {(this as DObj).Repr}");
            }
            throw new NotImplementedException();
        }

        public string name => "bool";


    }

    public class DBool : DInstObj
    {
        public DClsObj GetCls => dbool.cls;
        public bool value;
        public object Native => value;

        public string Repr => value.ToString();
        public bool Test => value;
        public bool Not => !value;

        public DObj BinOp(string op, DObj other)
        {
            switch (op)
            {
                case "and":
                    return MK.Bool(this.value && other.Test);
                case "or":
                    return MK.Bool(this.value || other.Test);

                default:
                    throw new D_TypeError($"bool objects do not support {op}.");

            }
        }
        public static implicit operator DBool(bool d) => MK.Bool(d);
        public static implicit operator bool(DBool d) => d.value;
    }

    public class dfloat : DClsObj
    {
        public static dfloat cls = new dfloat();
        public Dictionary<string, DObj> getters;

        DObj TryParse(DObj i)
        {
            var s = i as DStr;
            float f;
            if (float.TryParse(s.value, out f))
                return MK.Float(f);
            return MK.Nil();
        }
        public dfloat()
        {
            getters = new Dictionary<string, DObj>{
                {"TryParse",MK.Func1(TryParse)},
                {"Parse",MK.Func1(x => MK.Float(float.Parse((x as DStr).value)))},
                {"Epsilon", MK.Float(float.Epsilon)}
            };
        }
        public DObj Call(Args args)
        {
            if (args.Count == 0) return MK.Bool(false);
            if (args.Count != 1) throw new D_TypeError($"object {(this as DObj).Repr} is not callable.");
            var x = args[1];
            switch (x)
            {
                case DBool _:
                    return x;
                case DInt i:
                    return MK.Bool(i.value != 0);
                case DFloat f:
                    return MK.Bool(f.value != 0.0f);
                case DStr s:
                    return MK.Bool(bool.Parse(s.value));
                default:
                    throw new D_TypeError($"cannot cast {x.GetCls.Repr} to {(this as DObj).Repr}");
            }
            throw new NotImplementedException();
        }

        public string name => "float";
    }

    public class DFloat : DInstObj
    {
        public DClsObj GetCls => dfloat.cls;
        public float value;

        public bool Test => value != 0.0;

        object DObj.Native => value;

        string DObj.Repr => value.ToString();

        public static implicit operator DFloat(float d) => MK.Float(d);
        public static implicit operator float(DFloat d) => d.value;
    }

    public class dstring : DClsObj
    {
        public static dstring cls = new dstring();
        public Dictionary<string, DObj> getters;

        public DObj Replace(DObj self, DObj a, DObj b)
        {

            return (self as DStr).value.Replace((a as DStr).value, (b as DStr).value).Then(MK.String);
        }

        public DObj Trim(DObj self, DObj a)
        {
            return (self as DStr).value.Trim((a as DStr).value.ToCharArray()).Then(MK.String);
        }

        public DObj GetItem(DObj self, DObj a)
        {

            string s = new string(new[] { (self as DStr).value[(a as DInt).value] });

            return MK.String(s);
        }

        public DObj Length(DObj self) => MK.Int((self as DStr).value.Length);
        public dstring()
        {
            var x = "1";

            
            getters = new Dictionary<string, DObj>{
                {"concat",MK.Func1(xs => xs.Iter.Map(x => x.ToStr).Then(string.Concat).Then(MK.String))},
                {"empty", MK.String(string.Empty)},
                {"join", MK.Func2((sep, xs) => String.Join(sep.ToStr, xs.Iter.Map(x => x.ToStr)).Then(MK.String))},
                {"replace", MK.Func3(Replace)},
                {"trim", MK.Func2(Trim)},
                {"length", MK.Func1(Length)},
                {"getitem", MK.Func2(GetItem)},
                {"endswith", }
            };
        }
        public DObj Call(Args args)
        {
            if (args.Count == 0) return MK.Bool(false);
            if (args.Count != 1) throw new D_TypeError($"object {(this as DObj).Repr} is not callable.");
            var x = args[1];
            switch (x)
            {
                case DBool _:
                    return x;
                case DInt i:
                    return MK.Bool(i.value != 0);
                case DFloat f:
                    return MK.Bool(f.value != 0.0f);
                case DStr s:
                    return MK.Bool(bool.Parse(s.value));
                default:
                    throw new D_TypeError($"cannot cast {x.GetCls.Repr} to {(this as DObj).Repr}");
            }
            throw new NotImplementedException();
        }

        public string name => "float";
    }

    public class DStr : DObj
    {
        public DClsObj GetCls { get; set; }
        public string value;
        public object Native => this.value;

        public static implicit operator DStr(string d) => MK.String(d);
        public static implicit operator string(DStr d) => d.value;
    }

    public class DTuple : DObj
    {
        public DClsObj GetCls { get; set; }
        public DObj[] elts;

    }


    public class DNil : DObj
    {
        public DClsObj GetCls { get; set; }
        public object Native => this;
    }
    public class DNativeWrap : DObj
    {
        public DClsObj GetCls { get; set; }
        public object value;
        public object Native => value;
    }

    // builtin functions
    public class BFunc0 : DObj
    {
        public DClsObj GetCls { get; set; }
        public Func<DObj> value;
        public object Native => value;
    }
    public class BFunc1 : DObj
    {
        public DClsObj GetCls { get; set; }
        public Func<DObj, DObj> value;
        public object Native => value;
    }

    public class BFunc2 : DObj
    {
        public DClsObj GetCls { get; set; }
        public Func<DObj, DObj, DObj> value;
        public object Native => value;
    }

    public class BFunc3 : DObj
    {
        public DClsObj GetCls { get; set; }
        public Func<DObj, DObj, DObj, DObj> value;
        public object Native => value;
    }

    public class DGenerator : DObj
    {
        public DClsObj GetCls { get; set; }
        public DFrame frame;
        public DObj yieldvalue;
        public bool running => frame.offset <= frame.code.bc.Length;
    }

}