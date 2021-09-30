using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DianaScript
{

    using NameSpace =  Dictionary<InternString, DObj>;
    public interface DObj
    {

        public DClsObj GetCls { get; }
        public object Native { get; }

        public DObj Get(InternString attr)
        {
            (bool, DObj) o;
            var getters = this.GetCls.Getters;
            if (getters != null && getters.TryGetValue(attr, out o))
            {
                if (o.Item1) return o.Item2.__call__(new Args { this });
                return o.Item2;
            }
            throw new D_AttributeError(this.GetCls, MK.String(attr.ToString()));
        }

        

        public void Set(InternString attr, DObj value)
        {
            DObj o;
            var setters = this.GetCls.Setters;
            if (setters != null && setters.TryGetValue(attr, out o))
            {
                o.__call__(new Args { this, value });
            }

            throw new D_AttributeError(this.GetCls, MK.String(attr.ToString()));
        }

        public DObj __enter__() => throw new D_TypeError($"{this.GetCls.__repr__} does not support __enter__.");

        public void __exit__(DObj exctype, DObj exc, DObj frame) => throw new D_TypeError($"{this.GetCls.__repr__} does not support __enter__.");

        public DObj __call__(Args args) => throw new D_TypeError($"{this.GetCls.__repr__} instances are not callable");

        IEnumerable<DObj> __iter__ => throw new D_TypeError($"{this.GetCls.__repr__} instances are not iterable");
        public string __repr__ => $"{Native.ToString()}";
        public string __str__ => __repr__;

        public DObj __add__(DObj other) => throw new D_TypeError($"{this.GetCls.__repr__} does not support + operator.");

        public DObj __sub__(DObj other) => throw new D_TypeError($"{this.GetCls.__repr__} does not support - operator.");

        public DObj __truediv__(DObj other) => throw new D_TypeError($"{this.GetCls.__repr__} does not support / operator.");

        public DObj __floordiv__(DObj other) => throw new D_TypeError($"{this.GetCls.__repr__} does not support // operator.");
        public DObj __mul__(DObj other) => throw new D_TypeError($"{this.GetCls.__repr__} does not support * operator.");

        public DObj __pow__(DObj other) => throw new D_TypeError($"{this.GetCls.__repr__} does not support ** operator.");

        public DObj __mod__(DObj other) => throw new D_TypeError($"{this.GetCls.__repr__} does not support % operator.");

        public DObj __lshift__(DObj other) => throw new D_TypeError($"{this.GetCls.__repr__} does not support << operator.");

        public DObj __rshift__(DObj other) => throw new D_TypeError($"{this.GetCls.__repr__} does not support >> operator.");

        public DObj __bitand__(DObj other) => throw new D_TypeError($"{this.GetCls.__repr__} does not support & operator.");

        public DObj __bitor__(DObj other) => throw new D_TypeError($"{this.GetCls.__repr__} does not support | operator.");

        public DObj __bitxor__(DObj other) => throw new D_TypeError($"{this.GetCls.__repr__} does not support ^ operator.");


        public DObj __invert__ => throw new D_TypeError($"{this.GetCls.__repr__} does not support ~ operator.");

        public DObj __getitem__(DObj ind) => throw new D_TypeError($"{this.GetCls.__repr__} does not support item get.");

        public void __setitem__(DObj ind, DObj other) => throw new D_TypeError($"{this.GetCls.__repr__} does not support item set.");

        public void __delitem__(DObj ind) => throw new D_TypeError($"{this.GetCls.__repr__} does not support item delete.");

        int __len__ => throw new D_TypeError($"{this.GetCls.__repr__} does not support length.");
        bool __bool__ => true;
        bool __not__ => !__bool__;

        public bool __eq__(DObj other) => this.Native == other.Native;

        public bool __ne__(DObj other) => !(this.__eq__(other));

        public int __hash__ => Native.GetHashCode();

        public bool __contains__(DObj a) => throw new D_TypeError($"{this.GetCls.__repr__} does not support contains operation.");
        
        public bool __subclasscheck__(DObj a) => ((DClsObj) this) == ((DClsObj) a);

    }

    public partial class DArray : DObj
    {
        public DClsObj eltype;
        public Array src;

        public object Native => src;
        public static DArray Make(DClsObj eltype, int n)
        {
            return new DArray
            {
                eltype = eltype,
                src = System.Array.CreateInstance(eltype.NativeType, n)
            };
        }
        public static DArray Make<T>(T[] array)
        {
            var t = typeof(T);
            var eltype = DWrap.TypeMapOrCache(t);

            return new DArray { eltype = eltype, src = array as Array };
        }

        public DObj this[int i]
        {
            get =>
(DObj)src.GetValue(i);
            set
            {
                var native = value.Native.GetType();
                if (native == eltype.NativeType)
                {
                    src.SetValue(value, i);
                }
                else
                {
                    throw new D_TypeError(eltype.NativeType.Name, value.Native.GetType().Name);
                }
            }
        }
        public int Count => src.GetLength(0);
        public string __repr__ => $"Array({this.eltype.__repr__}, [{String.Join(", ", ((this as DObj).__iter__))}])";
        IEnumerable<DObj> DObj.__iter__
        {
            get
            {
                int n = Count;
                for (var i = 0; i < n; i++)
                {
                    yield return this[i];
                }
            }
        }
    }

    public interface DClsObj : DObj
    {

        public DObj AsObj => this;
        public Type NativeType { get; }

        public Dictionary<InternString, (bool, DObj)> Getters => null;
        public Dictionary<InternString, DObj> Setters => null;

        // generate by stub
        public string name { get; }

        // change by hand
        object DObj.Native => this;
        DClsObj DObj.GetCls => meta.cls;
        DObj DObj.Get(InternString attr)
        {
            (bool, DObj) o;
            if (Getters != null && Getters.TryGetValue(attr, out o))
                if (o.Item1)
                    return o.Item2.__call__(new Args { this });
                else
                    return o.Item2;
            throw new D_AttributeError(this, MK.String(attr.ToString()));
        }
        void DObj.Set(InternString attr, DObj v)
        {
            Getters[attr] = (false, v);
        }
        string DObj.__repr__ => $"<{this.name}>";
        // __dict__ returns None if dict
    }


    public class meta : DClsObj
    {

        public static meta cls = new meta();

        public string name => nameof(meta);

        public DClsObj GetCls => this;

        public Type NativeType => typeof(Type);

        DObj DObj.__call__(Args args)
        {
            var narg = args.NArgs;
            if (narg == 1)
            {
                return args[0].GetCls;
            }
            // create user class
            throw new NotImplementedException();
        }

    }

    public partial class DFunc : DObj
    {
        public object Native => this;
        public string Repr => $"<function>";
        public DRef[] freevals;
        public int[] nonargcells;
        public bool is_vararg;
        public int narg;
        public int nlocal;
        public int metadataInd;
        public int body;
        public Dictionary<InternString, DObj> nameSpace;
        public string __repr__ => $"<code at {(this as DObj).__hash__}>";

        public static DFunc Make(
            int body, int narg, int nlocal, int metadataInd, 
            NameSpace nameSpace = null, bool is_vararg = false,
            DRef[] freevals = null, int[] nonargcells = null
        )
        {
            return new DFunc
            {
                body = body,
                metadataInd = metadataInd,
                freevals = freevals ?? new DRef[0],
                nonargcells = nonargcells,
                nameSpace = nameSpace ?? new Dictionary<InternString, DObj>()
            };
        }
    }

    
    public partial class DInt : DObj
    {
        public static DInt Make(int i) => new DInt { value = i };
        public partial class Cls
        {
            DObj DObj.__call__(Args args)
            {
                if (args.NArgs == 0) return MK.Int(0);
                if (args.NArgs != 1) throw new D_TypeError($"object {(this as DObj).__repr__} is not callable.");
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
                        throw new D_TypeError($"cannot cast {x.GetCls.__repr__} to {(this as DObj).__repr__}");
                }
            }
        }

        public int value;

        public bool Test => value != 0;

        object DObj.Native => value;

        string DObj.__repr__ => value.ToString();

    }

    public interface Ref : DObj
    {
        public void set_contents(DObj value);
        public DObj get_contents();
    }


    public partial class DRef: Ref
    {
        public DObj cell_contents;

        public DObj get_contents() => cell_contents;

        public void set_contents(DObj value) => cell_contents = value;

        public object Native => this;

        public string __repr__ => $"<DRef at {(this as DObj).__hash__}>";

        public DRef(){
            cell_contents = null;
        }
    }

    public partial class DRefGlobal: Ref
    {
        public NameSpace ns;
        public InternString s;

        public DObj get_contents() => ns[s];

        public void set_contents(DObj value) => ns[s] = value;

        public object Native => this;

        public string __repr__ => $"<DRef at {(this as DObj).__hash__}>";

        public DRefGlobal(NameSpace ns, InternString s){
            this.ns = ns;
            this.s = s;
        }
    }


    public partial class DBool : DObj
    {
        public static DBool True = new DBool { value = true }, False = new DBool { value = false };
        public static DBool Make(bool i) => i ? True : False;
        public bool value;
        public object Native => value;

        public string __repr__ => value.ToString();
        bool DObj.__bool__ => value;
        bool DObj.__not__ => !value;

        public partial class Cls
        {
            DObj DObj.__call__(Args args)
            {
                if (args.Count == 0) return MK.Bool(false);
                if (args.Count != 1) throw new D_TypeError($"object {(this as DObj).__repr__} is not callable.");
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
                        throw new D_TypeError($"cannot cast {x.GetCls.__repr__} to {(this as DObj).__repr__}");
                }
            }
        }


        // public static implicit operator DBool(bool d) => MK.Bool(d);
        // public static implicit operator bool(DBool d) => d.value;
    }

    public partial class DFloat : DObj
    {
        public static DFloat Make(float i) => new DFloat { value = i };
        public partial class Cls
        {
            DObj DObj.__call__(Args args)
            {
                if (args.Count == 0) return MK.Float(0.0f);
                if (args.Count != 1) throw new D_TypeError($"object {(this as DObj).__repr__} is not callable.");
                var x = args[1];
                switch (x)
                {
                    case DBool b:
                        return MK.Float(b.value ? 1.0f : 0.0f);
                    case DInt i:
                        return MK.Float((float)i.value);
                    case DFloat _:
                        return x;
                    case DStr s:
                        return MK.Float(float.Parse(s.value));
                    default:
                        throw new D_TypeError($"cannot cast {x.GetCls.__repr__} to {(this as DObj).__repr__}");
                }
            }
        }
        public float value;

        public bool Test => value != 0.0;

        object DObj.Native => value;

        string DObj.__repr__ => value.ToString();

        // public static implicit operator DFloat(float d) => MK.Float(d);
        // public static implicit operator float(DFloat d) => d.value;
    }

    public partial class DStr : DObj
    {

        public static DStr Make(string i) => new DStr { value = i };
        public partial class Cls
        {
            public DObj __call__(Args args)
            {
                if (args.Count == 0) return MK.String("");
                if (args.Count != 1) throw new D_TypeError($"object {(this as DObj).__repr__} is not callable.");
                var x = args[1];
                switch (x)
                {
                    case DBool b:
                        return MK.String(b.value ? "True" : "False");
                    case DInt i:
                        return MK.String(i.value.ToString());
                    case DFloat f:
                        return MK.String(f.value.ToString());
                    case DStr _:
                        return x;
                    default:
                        throw new D_TypeError($"cannot cast {x.GetCls.__repr__} to {(this as DObj).__repr__}");
                }
            }
        }
        public string value;
        public object Native => this.value;

        public string __repr__ => value.Replace("\"", "\\\"");


    }

    public partial class DTuple : DObj
    {

        public static DTuple Make(DObj[] v) => new DTuple { elts = v };
        public object Native => elts;

        public int Length => elts.Length;

        public DObj[] elts;

        public DObj __add__(DObj other)
        {
            var o = (DTuple)other;
            var new_elts = new DObj[elts.Length + o.elts.Length];
            elts.CopyTo(new_elts, 0);
            o.elts.CopyTo(new_elts, elts.Length);
            return MK.Tuple(elts);
        }

        public bool __contains__(DObj o)
        {
            foreach (var a in elts)
            {
                if (o.__eq__(a))
                    return true;
            }
            return false;
        }

        public bool __eq__(DObj o)
        {
            if (o is DTuple another_tuple && another_tuple.elts.Length == elts.Length)
            {
                for (var i = 0; i < elts.Length; i++)
                {
                    if (elts[i].__ne__(another_tuple.elts[i]))
                        return false;
                }
                return true;

            }
            return false;
        }
    }


    public partial class DNil : DObj
    {
        public static DNil Make() => unique;
        public static readonly DNil unique = new DNil();
        public object Native => this;
    }

    public partial class DWrap
    {
        public partial class Cls : DClsObj
        {

            public string name => $"wrap({NativeType.FullName})";
            public static Cls unique = new Cls();
            public Type NativeType { get; set; }
            public System.Collections.Generic.Dictionary<string, (bool, DObj)> Getters { get; set; }
            public Cls()
            {
                Getters = new System.Collections.Generic.Dictionary<string, (bool, DObj)>
                {
                };
            }
        }
    }
    public partial class DWrap : DObj
    {
        public DClsObj _cls;
        public DClsObj GetCls => _cls;
        protected static Dictionary<Type, DClsObj> typemaps = new Dictionary<Type, DClsObj>();

        public static DClsObj TypeMapOrCache(Type t)
        {
            if (typemaps.TryGetValue(t, out var cls))
            {
                return cls;
            }
            else
            {
                var cls_ = new DWrap.Cls();
                cls_.NativeType = t;
                typemaps[t] = cls_;
                return cls_;
            }
        }

        public static void RegisterTypeMap(Type t, DClsObj cls)
        {
            typemaps[t] = cls;
        }

        public DWrap(object d)
        {
            value = d;
            _cls = TypeMapOrCache(d.GetType());

        }
        public object value;
        public object Native => value;
    }

    public partial class DDict : DObj
    {

        public static DDict Make(Dictionary<DObj, DObj> v) => new DDict { src = v };
        public Dictionary<DObj, DObj> src;
        public object Native => src;

        public DObj __getitem__(DObj k) => src[k];
        public void __setitem__(DObj k, DObj v) => src[k] = v;
        public void __delitem__(DObj k) => src.Remove(k);
    }

    public partial class DList : DObj
    {
        public static DList Make(List<DObj> v) => new DList { src = v };
        public List<DObj> src;
        public object Native => src;

    }

    public partial class DSet : DObj
    {
        public static DSet Make(HashSet<DObj> v) => new DSet { src = v };
        public HashSet<DObj> src;
        public object Native => src;

    }
    public partial class DBuiltinFunc : DObj
    {

        public Func<Args, DObj> func;
        public static DBuiltinFunc Make(Func<Args, DObj> f)
        {
            return new DBuiltinFunc { func = f };
        }
        public object Native => func;

        public DObj __call__(Args args) => func(args);
    }

}