using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
/*
primitive types:
int, float, str, bool, noneclass, meta,
ref: directref globalref
*/
namespace DianaScript
{

    using static CallDFuncExtensions;
    using NameSpace = Dictionary<InternString, DObj>;

    using BinaryOp = Func<DObj, DObj, DObj>;
    using CompareOp = Func<DObj, DObj, bool>;
    public struct Ops
    {
        public Dictionary<InternString, DObj> Dict;
        public Func<DObj, Args, DObj> __call__;
        public BinaryOp __add__;
        public BinaryOp __sub__;
        public BinaryOp __truediv__;
        public BinaryOp __floordiv__;
        public BinaryOp __mul__;
        public BinaryOp __pow__;
        public BinaryOp __mod__;
        public BinaryOp __lshift__;
        public BinaryOp __rshift__;
        public BinaryOp __bitand__;
        public BinaryOp __bitor__;
        public BinaryOp __bitxor__;
        public BinaryOp __getitem__;

        public CompareOp __subclasscheck__;
        public CompareOp __contains__;
        public CompareOp __gt__;
        public CompareOp __ge__;
        public CompareOp __lt__;
        public CompareOp __le__;
        public CompareOp __eq__;
        public CompareOp __ne__;
        public Action<DObj, DObj, DObj> __setitem__;
        public Action<DObj, DObj> __delitem__;
        public Func<DObj, InternString, DObj> __getattr__;
        public Action<DObj, InternString, DObj> __setattr__;

        public Func<DObj, DObj> __enter__;
        public Action<DObj, DObj, DObj, DObj> __exit__;
        public Func<DObj, IEnumerable<DObj>> __iter__;

        // unambiguous
        public Func<DObj, string> __repr__;

        // readable
        public Func<DObj, string> __str__;
        public Func<DObj, int> __len__;
        public Func<DObj, bool> __not__;

        public Func<DObj, DObj> __neg__;

        public Func<DObj, DObj> __invert__;

        public Func<DObj, bool> __bool__;
        public Func<DObj, int> __hash__;

        public static Ops defaultOps = new Ops
        {
            __hash__ = RuntimeHelpers.GetHashCode,
            __not__ = (x) => x.__bool__(),
            __bool__ = (x) => true,
            __repr__ = (x) => $"<{x.Class.name} at {RuntimeHelpers.GetHashCode(x)}>",
            __eq__ = (x, y) => x.Native == y.Native,
            __ne__ = (x, y) => !x.__eq__(y),

        };
        public Ops Make(Dictionary<InternString, DObj> d)
        {
            throw new NotImplementedException("");
        }
    }

    public interface DObj : IComparable<DObj>, IEquatable<DObj>
    {

        public DClsObj Class { get; }
        public object Native { get; }

        public IEnumerable<DObj> __iter__();
        public DObj __call__(Args args);
        public DObj __add__(DObj a);
        public DObj __sub__(DObj a);
        public DObj __mul__(DObj a);
        public DObj __floordiv__(DObj a);
        public DObj __truediv__(DObj a);
        public DObj __pow__(DObj a);
        public DObj __mod__(DObj a);
        public DObj __lshift__(DObj a);
        public DObj __rshift__(DObj a);
        public DObj __bitand__(DObj a);
        public DObj __bitor__(DObj a);
        public DObj __bitxor__(DObj a);
        public DObj __getitem__(DObj a);
        public void __setitem__(DObj a, DObj b);
        public void __delitem__(DObj a);
        public DObj __getattr__(InternString attr);
        public void __setattr__(InternString attr, DObj value);
        public int __len__();
        public bool __bool__();
        public bool __not__();
        public DObj __neg__();
        public DObj __invert__();
        public int __hash__();
        public string __repr__();
        public string __str__();
        public bool __subclasscheck__(DObj o);
        public bool __contains__(DObj o);
        public bool __gt__(DObj o);
        public bool __ge__(DObj o);
        public bool __lt__(DObj o);
        public bool __le__(DObj o);
        public bool __eq__(DObj o);
        public bool __ne__(DObj o);
        public DObj __enter__();
        public void __exit__(DObj errtype, DObj err, DObj frames);


        public static bool operator <(DObj operand1, DObj operand2)
        {
            return operand1.__lt__(operand1);
        }

        public static bool operator <=(DObj operand1, DObj operand2)
        {
            return operand1.__le__(operand1);
        }

        public static bool operator >(DObj operand1, DObj operand2)
        {
            return operand1.__gt__(operand1);
        }

        public static bool operator >=(DObj operand1, DObj operand2)
        {
            return operand1.__ge__(operand1);
        }


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
            get => (DObj)src.GetValue(i);
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
        public string __repr__() => $"Array({(this.eltype as DObj).__repr__()}, [{String.Join(", ", ((this as DObj).__iter__()))}])";
        public IEnumerable<DObj> __iter__()
        {
            int n = Count;
            for (var i = 0; i < n; i++)
            {
                yield return this[i];
            }

        }

    }

    public interface DClsObj : DObj
    {

        public Type NativeType { get; }

        public Ops ops { get; }
        public Dictionary<InternString, DObj> Dict { get; }

        // generate by stub
        public string name { get; }


    }


    public partial class meta : DClsObj
    {

        public static meta unique = new meta();

        public Ops ops => Ops.defaultOps;

        public Dictionary<InternString, DObj> Dict => null;

        public string name => nameof(meta);

        public DClsObj Class => this;

        public Type NativeType => typeof(Type);

        public object Native => NativeType;

        public DObj __call__(Args args)
        {
            var narg = args.NArgs;
            if (narg == 1)
            {
                return args[0].Class;
            }
            // create user class
            throw new NotImplementedException();
        }

    }

    public partial class DFunc : DObj
    {
        public object Native => this;

        public DRef[] freevals;
        public int[] nonargcells;
        public bool is_vararg;
        public int narg;
        public int nlocal;
        public int metadataInd;
        public int body;
        public Dictionary<InternString, DObj> nameSpace;
        public string __repr__()
        {
            var name = AWorld.funcmetas[metadataInd].name;
            return $"<func {name} at {(this as DObj).__hash__()}>";
        }

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
            public DObj __call__(Args args)
            {
                if (args.NArgs == 0) return MK.Int(0);
                if (args.NArgs != 1) throw new D_TypeError($"object {(this as DObj).__repr__()} is not callable.");
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
                        throw new D_TypeError($"cannot cast {x.Class.__repr__()} to {(this as DObj).__repr__()}");
                }
            }
        }

        public int value;

        public bool Test => value != 0;

        public object Native => value;

        public string __repr__() => value.ToString();

    }

    public interface Ref : DObj
    {
        public void set_contents(DObj value);
        public DObj get_contents();
    }


    public partial class DRef : Ref
    {
        public DObj cell_contents;

        public object Native => cell_contents;
        public DObj get_contents() => cell_contents;

        public void set_contents(DObj value) => cell_contents = value;

        public string __repr__() =>
            cell_contents == null ?
            "<DRef undefined>" : $"<DRef of {cell_contents.__repr__()}>";

        public DRef()
        {
            cell_contents = null;
        }

    }

    public partial class DRefGlobal : Ref
    {
        public NameSpace ns;
        public InternString s;

        public DObj get_contents() => ns[s];

        public void set_contents(DObj value) => ns[s] = value;

        public object Native => this;

        public string __repr__() => $"<DRefGlobal at {(this as DObj).__hash__()}>";

        public DRefGlobal(NameSpace ns, InternString s)
        {
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

        public string __repr__() => value.ToString();
        public bool __bool__() => value;
        public bool __not__() => !value;

        public partial class Cls
        {
            public DObj __call__(Args args)
            {
                if (args.Count == 0) return MK.Bool(false);
                if (args.Count != 1) throw new D_TypeError($"object {this.__repr__()} is not callable.");
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
                        throw new D_TypeError($"cannot cast {x.Class.__repr__()} to {(this as DObj).__repr__()}");
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
            public DObj __call__(Args args)
            {
                if (args.Count == 0) return MK.Float(0.0f);
                if (args.Count != 1) throw new D_TypeError($"object {(this as DObj).__repr__()} is not callable.");
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
                        throw new D_TypeError($"cannot cast {x.Class.__repr__()} to {this.__repr__()}");
                }
            }
        }
        public float value;

        public bool Test => value != 0.0;

        public object Native => value;

        public string __repr__() => value.ToString();

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
                if (args.Count != 1) throw new D_TypeError($"object {this.__repr__()} is not callable.");
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
                        throw new D_TypeError($"cannot cast {x.Class.__repr__()} to {this.__repr__()}");
                }
            }
        }
        public string value;
        public object Native => this.value;

        public string __repr__() => value.Replace("\"", "\\\"");


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

        public string __repr__() => "None";

        public static readonly DNil unique = new DNil();
        public object Native => this;
    }

    public partial class DWrap
    {
        public partial class Cls : DClsObj
        {

            public string name => $"wrap({NativeType.FullName})";
            public Type NativeType { get; set; }
            public Dictionary<InternString, DObj> dict { get; set; }
            public Dictionary<InternString, DObj> Dict => dict;
            public Cls()
            {
                this.dict = new Dictionary<InternString, DObj>
                {
                };
            }
        }
    }
    public partial class DWrap : DObj
    {
        public DClsObj _cls;
        public DClsObj Class => _cls;
        internal static Dictionary<Type, DClsObj> typemaps = new Dictionary<Type, DClsObj>();
        public object value;

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
        public string __repr__() => $@"[{String.Join(", ", src.Select(x => x.__repr__()))}]";
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

    public partial class DUserObj : DObj
    {
        private DClsObj _cls;
        public DClsObj Class => _cls;
        public Dictionary<InternString, DObj> table;

        public object Native => this;
        public DUserObj(DUserObj.Cls cls, Dictionary<InternString, DObj> table)
        {
            _cls = cls;
            this.table = table;

        }
        public partial class Cls
        {
            public string _name;
            public Ops _ops;
            public Dictionary<InternString, DObj> _methods;
            public Type NativeType => typeof(DUserObj.Cls);

            public Ops ops => _ops;
            public string name => _name;
            public Dictionary<InternString, DObj> Dict => _methods;

            public object Native => this;

            public Cls(string name, Ops ops, Dictionary<InternString, DObj> methods)
            {
                _name = name;
                _ops = ops;
                _methods = methods;
            }

        }
    }

}