/*
primitive types:
int, float, str, bool, noneclass, meta,
ref: directref globalref

fields/properties are object related
methods are class related
a.
*/
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
namespace DianaScript
{

    using BinaryOp = Func<DObjEx, DObjEx, DObjEx>;
    using CompareOp = Func<DObjEx, DObjEx, bool>;
    using static DObjExt;
    public static class DObjExt
    {
        
        public static InvalidOperationException invalid_op(this DClsEx self, string op)
        {
            return new InvalidOperationException($"unsupported operator '{op}' for {self.Class.name}");
        }
    }
    public struct Ops
    {
        public Dictionary<InternString, DObjEx> Dict;
        public Func<DObjEx, Args, DObjEx> __call__;
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
        public Action<DObjEx, DObjEx, DObjEx> __setitem__;
        public Action<DObjEx, DObjEx> __delitem__;
        public Func<DObjEx, InternString, DObjEx> __getattr__;
        public Action<DObjEx, InternString, DObjEx> __setattr__;

        public Func<DObjEx, DObjEx> __enter__;
        public Action<DObjEx, DObjEx, DObjEx, DObjEx> __exit__;
        public Func<DObjEx, IEnumerable<DObjEx>> __iter__;

        // unambiguous
        public Func<DObjEx, string> __repr__;

        // readable
        public Func<DObjEx, string> __str__;
        public Func<DObjEx, int> __len__;
        public Func<DObjEx, bool> __not__;
        public Func<DObjEx, bool> __bool__;
        public Func<DObjEx, int> __hash__;

        public static Ops defaultOps = new Ops { 
            __hash__ = RuntimeHelpers.GetHashCode    
        };
        public Ops Make(Dictionary<InternString, DObjEx> d){
            throw new NotImplementedException("");
        }
    }


    public class DClsEx: DObjEx
    {
        public Ops ops;
        public string name;
        public Type NativeAsType;
        public object Native => NativeAsType;
        public DClsEx Class => throw new NotImplementedException();
        

    }
    
    public interface DObjEx
    {
        public DClsEx Class { get; }
        public object Native { get; }
        public void __iter__();
        public DObjEx __call__(Args args);
        public DObjEx __add__(DObjEx a);
        public DObjEx __sub__(DObjEx a);
        public DObjEx __mul__(DObjEx a);
        public DObjEx __pow__(DObjEx a);
        public DObjEx __mod__(DObjEx a);
        public DObjEx __lshift__(DObjEx a);
        public DObjEx __rshift__(DObjEx a);
        public DObjEx __bitand__(DObjEx a);
        public DObjEx __bitor__(DObjEx a);
        public DObjEx __bitxor__(DObjEx a);
        public DObjEx __getitem__(DObjEx a);
        public void __setitem__(DObjEx a, DObjEx b);
        public void __delitem__(DObjEx a);
        public void __getattr__(InternString attr);
        public void __setattr__(InternString attr, DObjEx value);
        public int __len__();
        public bool __bool__();
        public  bool __not__();
        public int __hash__();
        public bool __subclasscheck__(DObjEx o);
        public bool __contains__(DObjEx o);
        public bool __gt__(DObjEx o);
        public bool __ge__(DObjEx o);
        public bool __lt__(DObjEx o);
        public bool __le__(DObjEx o);
        public bool __eq__(DObjEx o);
        public  bool __ne__(DObjEx o);
        public DObjEx __enter__();
        public void __exit__(DObjEx errtype, DObjEx err, DObjEx frames);
    }


    public interface DDirectRef : DObjEx
    {
        public void set_contents(DObj value);
        public DObj get_contents();
    }

    public partial class DNilEx: DObjEx
    {
    }

    public partial class DWrapEx: DObjEx
    {
    }
    public partial class DMapEx: DObjEx
    {
    }
    public partial class DListEx: DObjEx
    {
    }
    public partial class DTupleEx: DObjEx
    {
    }
    public partial class DFloatEx: DObjEx
    {
        public DClsEx Class => throw new NotImplementedException();

        public object Native => throw new NotImplementedException();

        public DObjEx __add__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __bitand__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __bitor__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __bitxor__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public bool __bool__()
        {
            throw new NotImplementedException();
        }

        public DObjEx __call__(Args args)
        {
            throw new NotImplementedException();
        }

        public bool __contains__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public void __delitem__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __enter__()
        {
            throw new NotImplementedException();
        }

        public bool __eq__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public void __exit__(DObjEx errtype, DObjEx err, DObjEx frames)
        {
            throw new NotImplementedException();
        }

        public void __getattr__(InternString attr)
        {
            throw new NotImplementedException();
        }

        public DObjEx __getitem__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public bool __ge__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public bool __gt__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public int __hash__()
        {
            throw new NotImplementedException();
        }

        public void __iter__()
        {
            throw new NotImplementedException();
        }

        public int __len__()
        {
            throw new NotImplementedException();
        }

        public bool __le__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public DObjEx __lshift__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public bool __lt__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public DObjEx __mod__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __mul__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public bool __ne__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public bool __not__()
        {
            throw new NotImplementedException();
        }

        public DObjEx __pow__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __rshift__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public void __setattr__(InternString attr, DObjEx value)
        {
            throw new NotImplementedException();
        }

        public void __setitem__(DObjEx a, DObjEx b)
        {
            throw new NotImplementedException();
        }

        public bool __subclasscheck__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public DObjEx __sub__(DObjEx a)
        {
            throw new NotImplementedException();
        }
    }
    public partial class DStrEx: DObjEx
    {
        public DClsEx Class => throw new NotImplementedException();

        public DObjEx __add__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __bitand__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __bitor__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __bitxor__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public bool __bool__()
        {
            throw new NotImplementedException();
        }

        public DObjEx __call__(Args args)
        {
            throw new NotImplementedException();
        }

        public bool __contains__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public void __delitem__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __enter__()
        {
            throw new NotImplementedException();
        }

        public bool __eq__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public void __exit__(DObjEx errtype, DObjEx err, DObjEx frames)
        {
            throw new NotImplementedException();
        }

        public void __getattr__(InternString attr)
        {
            throw new NotImplementedException();
        }

        public DObjEx __getitem__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public bool __ge__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public bool __gt__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public int __hash__()
        {
            throw new NotImplementedException();
        }

        public void __iter__()
        {
            throw new NotImplementedException();
        }

        public int __len__()
        {
            throw new NotImplementedException();
        }

        public bool __le__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public DObjEx __lshift__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public bool __lt__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public DObjEx __mod__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __mul__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public bool __ne__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public bool __not__()
        {
            throw new NotImplementedException();
        }

        public DObjEx __pow__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __rshift__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public void __setattr__(InternString attr, DObjEx value)
        {
            throw new NotImplementedException();
        }

        public void __setitem__(DObjEx a, DObjEx b)
        {
            throw new NotImplementedException();
        }

        public bool __subclasscheck__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public DObjEx __sub__(DObjEx a)
        {
            throw new NotImplementedException();
        }
    }
    public partial class DBoolEx : DObjEx
    {
        public DClsEx Class => throw new NotImplementedException();

        public object Native => throw new NotImplementedException();

        public DObjEx __add__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __bitand__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __bitor__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __bitxor__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public bool __bool__()
        {
            throw new NotImplementedException();
        }

        public DObjEx __call__(Args args)
        {
            throw new NotImplementedException();
        }

        public bool __contains__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public void __delitem__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __enter__()
        {
            throw new NotImplementedException();
        }

        public bool __eq__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public void __exit__(DObjEx errtype, DObjEx err, DObjEx frames)
        {
            throw new NotImplementedException();
        }

        public void __getattr__(InternString attr)
        {
            throw new NotImplementedException();
        }

        public DObjEx __getitem__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public bool __ge__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public bool __gt__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public int __hash__()
        {
            throw new NotImplementedException();
        }

        public void __iter__()
        {
            throw new NotImplementedException();
        }

        public int __len__()
        {
            throw new NotImplementedException();
        }

        public bool __le__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public DObjEx __lshift__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public bool __lt__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public DObjEx __mod__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __mul__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public bool __ne__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public bool __not__()
        {
            throw new NotImplementedException();
        }

        public DObjEx __pow__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __rshift__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public void __setattr__(InternString attr, DObjEx value)
        {
            throw new NotImplementedException();
        }

        public void __setitem__(DObjEx a, DObjEx b)
        {
            throw new NotImplementedException();
        }

        public bool __subclasscheck__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public DObjEx __sub__(DObjEx a)
        {
            throw new NotImplementedException();
        }
    }
    public partial class DIntEx : DObjEx
    {
        public int value;
        public DClsEx Class => throw new NotImplementedException();

        public object Native => throw new NotImplementedException();

        public DObjEx __add__(DObjEx a)
        {
            
            throw new NotImplementedException();
        }

        public DObjEx __bitand__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __bitor__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __bitxor__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public bool __bool__()
        {
            throw new NotImplementedException();
        }

        public DObjEx __call__(Args args)
        {
            throw new NotImplementedException();
        }

        public bool __contains__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public void __delitem__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __enter__()
        {
            throw new NotImplementedException();
        }

        public bool __eq__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public void __exit__(DObjEx errtype, DObjEx err, DObjEx frames)
        {
            throw new NotImplementedException();
        }

        public void __getattr__(InternString attr)
        {
            throw new NotImplementedException();
        }

        public DObjEx __getitem__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public bool __ge__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public bool __gt__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public int __hash__()
        {
            throw new NotImplementedException();
        }

        public void __iter__()
        {
            throw new NotImplementedException();
        }

        public int __len__()
        {
            throw new NotImplementedException();
        }

        public bool __le__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public DObjEx __lshift__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public bool __lt__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public DObjEx __mod__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __mul__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public bool __ne__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public bool __not__()
        {
            throw new NotImplementedException();
        }

        public DObjEx __pow__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public DObjEx __rshift__(DObjEx a)
        {
            throw new NotImplementedException();
        }

        public void __setattr__(InternString attr, DObjEx value)
        {
            throw new NotImplementedException();
        }

        public void __setitem__(DObjEx a, DObjEx b)
        {
            throw new NotImplementedException();
        }

        public bool __subclasscheck__(DObjEx o)
        {
            throw new NotImplementedException();
        }

        public DObjEx __sub__(DObjEx a)
        {
            throw new NotImplementedException();
        }
    }

    // public class DObjExtension
    // {

    //     private static InvalidOperationException invalid_op(string op)
    //     {
    //         return new InvalidOperationException($"unsupported operator '{op}' for {this.Class.name}");
    //     }
    //     public void __iter__()
    //     {
    //         var meth = this.Class.ops.__iter__;
    //         if (meth == null)
    //             throw new InvalidOperationException($"{this.Class.name} objects are not iterable.");
    //         meth(this);
    //     }
    //     public DObjEx __call__(Args args)
    //     {
    //         var meth = this.Class.ops.__call__;
    //         if (meth == null)
    //             throw new InvalidOperationException($"{this.Class.name} objects are not callable.");
    //         return meth(this, args);
    //     }
    //     public DObjEx __add__(DObjEx a)
    //     {
    //         var meth = this.Class.ops.__add__;
    //         if (meth == null)
    //             throw this.invalid_op("+");
    //         return meth(this, a);
    //     }
    //     public DObjEx __sub__(DObjEx a)
    //     {
    //         var meth = this.Class.ops.__sub__;
    //         if (meth == null)
    //             throw this.invalid_op("-");
    //         return meth(this, a);
    //     }
    //     public DObjEx __mul__(DObjEx a)
    //     {
    //         var meth = this.Class.ops.__mul__;
    //         if (meth == null)
    //             throw this.invalid_op("*");
    //         return meth(this, a);
    //     }
    //     public DObjEx __pow__(DObjEx a)
    //     {
    //         var meth = this.Class.ops.__pow__;
    //         if (meth == null)
    //             throw this.invalid_op("**");
    //         return meth(this, a);
    //     }
    //     public DObjEx __mod__(DObjEx a)
    //     {
    //         var meth = this.Class.ops.__mod__;
    //         if (meth == null)
    //             throw this.invalid_op("%");
    //         return meth(this, a);
    //     }
    //     public DObjEx __lshift__(DObjEx a)
    //     {
    //         var meth = this.Class.ops.__lshift__;
    //         if (meth == null)
    //             throw this.invalid_op("<<");
    //         return meth(this, a);
    //     }

    //     public DObjEx __rshift__( DObjEx a)
    //     {
    //         var meth = this.Class.ops.__rshift__;
    //         if (meth == null)
    //             throw this.invalid_op(">>");
    //         return meth(this, a);
    //     }
    //     public DObjEx __bitand__(DObjEx a)
    //     {
    //         var meth = this.Class.ops.__rshift__;
    //         if (meth == null)
    //             throw this.invalid_op("&");
    //         return meth(this, a);
    //     }
    //     public DObjEx __bitor__(DObjEx a)
    //     {
    //         var meth = this.Class.ops.__bitor__;
    //         if (meth == null)
    //             throw this.invalid_op("|");
    //         return meth(this, a);
    //     }
    //     public DObjEx __bitxor__(DObjEx a)
    //     {
    //         var meth = this.Class.ops.__bitxor__;
    //         if (meth == null)
    //             throw this.invalid_op("^");
    //         return meth(this, a);
    //     }
    //     public DObjEx __getitem__(DObjEx a)
    //     {
    //         var meth = this.Class.ops.__getitem__;
    //         if (meth == null)
    //             throw new InvalidOperationException($"'{this.Class.name}' object is not subscriptable.");
    //         return meth(this, a);
    //     }
    //     public void __setitem__(DObjEx a, DObjEx b)
    //     {
    //         var meth = this.Class.ops.__setitem__;
    //         if (meth == null)
    //             throw new InvalidOperationException($"'{this.Class.name}' object does not support item assignment.");
    //         meth(this, a, b);
    //     }

    //     public void __delitem__(DObjEx a)
    //     {
    //         var meth = this.Class.ops.__delitem__;
    //         if (meth == null)
    //             throw new InvalidOperationException($"'{this.Class.name}' object does not support item deletion.");
    //         meth(this, a);
    //     }

    //     public void __getattr__(InternString attr)
    //     {
    //         var meth = this.Class.ops.__getattr__;
    //         if (meth == null)
    //             throw this.invalid_op($"o.{attr}");
    //         meth(this, attr);
    //     }
    //     public void __setattr__(InternString attr, DObjEx value)
    //     {
    //         var meth = this.Class.ops.__setattr__;
    //         if (meth == null)
    //             throw this.invalid_op($"o.{attr}=v");
    //         meth(this, attr, value);
    //     }
    //     public int __len__()
    //     {
    //         var meth = this.Class.ops.__len__;
    //         if (meth == null)
    //             throw this.invalid_op($"len");
    //         return meth(this);
    //     }
    //     public bool __bool__()
    //     {
    //         var meth = this.Class.ops.__bool__;
    //         if (meth == null)
    //             throw this.invalid_op("cast to bool");
    //         return meth(this);
    //     }

    //     public  bool __not__()
    //     {
    //         var meth = this.Class.ops.__not__;
    //         if (meth != null)
    //             return meth(this);
    //         return !this.__bool__();
    //     }

    //     public int __hash__()
    //     {
    //         var meth = this.Class.ops.__hash__;
    //         if (meth == null)
    //             throw this.invalid_op("hash");
    //         return meth(this);
    //     }
    //     public bool __subclasscheck__(DObjEx o)
    //     {
    //         var meth = this.Class.ops.__subclasscheck__;
    //         if (meth == null)
    //             throw this.invalid_op("typecheck");
    //         return meth(this, cls);
    //     }

    //     public bool __contains__(DObjEx o)
    //     {
    //         var meth = this.Class.ops.__contains__;
    //         if (meth == null)
    //             throw this.invalid_op("contains");
    //         return meth(this, o);
    //     }

    //     public bool __gt__(DObjEx o)
    //     {
    //         var meth = this.Class.ops.__gt__;
    //         if (meth == null)
    //             throw this.invalid_op(">");
    //         return meth(this, o);
    //     }
    //     public bool __ge__(DObjEx o)
    //     {
    //         var meth = this.Class.ops.__ge__;
    //         if (meth == null)
    //             throw this.invalid_op(">=");
    //         return meth(this, o);
    //     }
    //     public bool __lt__(DObjEx o)
    //     {
    //         var meth = this.Class.ops.__lt__;
    //         if (meth == null)
    //             throw this.invalid_op("<");
    //         return meth(this, o);
    //     }
    //     public bool __le__(DObjEx o)
    //     {
    //         var meth = this.Class.ops.__le__;
    //         if (meth == null)
    //             throw this.invalid_op("<=");
    //         return meth(this, o);
    //     }
    //     public bool __eq__(DObjEx o)
    //     {
    //         var meth = this.Class.ops.__eq__;
    //         if (meth == null)
    //             throw this.invalid_op("=");
    //         return meth(this, o);
    //     }
    //     public  bool __ne__(DObjEx o)
    //     {
    //         var meth = this.Class.ops.__ne__;
    //         if (meth == null)
    //             throw this.invalid_op("!=");
    //         return meth(this, o);
    //     }
    //     public DObjEx __enter__()
    //     {
    //         var meth = this.Class.ops.__enter__;
    //         if (meth == null)
    //             throw this.invalid_op("RAII");
    //         return meth(this);
    //     }

    //     public void __exit__(DObjEx errtype, DObjEx err, DObjEx frames)
    //     {
    //         var meth = this.Class.ops.__exit__;
    //         if (meth == null)
    //             throw this.invalid_op("RAII");
    //         meth(this, errtype, err, frames);
    //     }


}

    public struct MethodStruct
    {

    }
    public struct DomainTypes
    {



    }

}