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
    
    public struct Ops
    {
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
    }


    public class DClsEx: DObjEx
    {
        public Ops ops;
        public string name;
        public DClsEx Class => throw new NotImplementedException();

    }


    public interface DObjEx
    {
        public DClsEx Class { get; }

        private InvalidOperationException invalid_op(string op)
        {
            return new InvalidOperationException($"unsupported operator '{op}' for {this.Class.name}");
        }

        public void __iter__()
        {
            var meth = Class.ops.__iter__;
            if (meth == null)
                throw new InvalidOperationException($"{this.Class.name} objects are not iterable.");
            meth(this);
        }
        public DObjEx __call__(Args args)
        {
            var meth = Class.ops.__call__;
            if (meth == null)
                throw new InvalidOperationException($"{this.Class.name} objects are not callable.");
            return meth(this, args);
        }
        public DObjEx __add__(DObjEx a)
        {
            var meth = Class.ops.__add__;
            if (meth == null)
                throw invalid_op("+");
            return meth(this, a);
        }
        public DObjEx __sub__(DObjEx a)
        {
            var meth = Class.ops.__sub__;
            if (meth == null)
                throw invalid_op("-");
            return meth(this, a);
        }
        public DObjEx __mul__(DObjEx a)
        {
            var meth = Class.ops.__mul__;
            if (meth == null)
                throw invalid_op("*");
            return meth(this, a);
        }
        public DObjEx __pow__(DObjEx a)
        {
            var meth = Class.ops.__pow__;
            if (meth == null)
                throw invalid_op("**");
            return meth(this, a);
        }
        public DObjEx __mod__(DObjEx a)
        {
            var meth = Class.ops.__mod__;
            if (meth == null)
                throw invalid_op("%");
            return meth(this, a);
        }
        public DObjEx __lshift__(DObjEx a)
        {
            var meth = Class.ops.__lshift__;
            if (meth == null)
                throw invalid_op("<<");
            return meth(this, a);
        }

        public DObjEx __rshift__(DObjEx a)
        {
            var meth = Class.ops.__rshift__;
            if (meth == null)
                throw invalid_op(">>");
            return meth(this, a);
        }
        public DObjEx __bitand__(DObjEx a)
        {
            var meth = Class.ops.__rshift__;
            if (meth == null)
                throw invalid_op("&");
            return meth(this, a);
        }
        public DObjEx __bitor__(DObjEx a)
        {
            var meth = Class.ops.__bitor__;
            if (meth == null)
                throw invalid_op("|");
            return meth(this, a);
        }
        public DObjEx __bitxor__(DObjEx a)
        {
            var meth = Class.ops.__bitxor__;
            if (meth == null)
                throw invalid_op("^");
            return meth(this, a);
        }
        public DObjEx __getitem__(DObjEx a)
        {
            var meth = Class.ops.__getitem__;
            if (meth == null)
                throw new InvalidOperationException($"'{this.Class.name}' object is not subscriptable.");
            return meth(this, a);
        }
        public void __setitem__(DObjEx a, DObjEx b)
        {
            var meth = Class.ops.__setitem__;
            if (meth == null)
                throw new InvalidOperationException($"'{this.Class.name}' object does not support item assignment.");
            meth(this, a, b);
        }

        public void __delitem__(DObjEx a)
        {
            var meth = Class.ops.__delitem__;
            if (meth == null)
                throw new InvalidOperationException($"'{this.Class.name}' object does not support item deletion.");
            meth(this, a);
        }

        public void __getattr__(InternString attr)
        {
            var meth = Class.ops.__getattr__;
            if (meth == null)
                throw invalid_op($"o.{attr}");
            meth(this, attr);
        }
        public void __setattr__(InternString attr, DObjEx value)
        {
            var meth = Class.ops.__setattr__;
            if (meth == null)
                throw invalid_op($"o.{attr}=v");
            meth(this, attr, value);
        }
        public int __len__()
        {
            var meth = Class.ops.__len__;
            if (meth == null)
                throw invalid_op($"len");
            return meth(this);
        }

        public bool __bool__()
        {
            var meth = Class.ops.__bool__;
            if (meth == null)
                throw invalid_op("cast to bool");
            return meth(this);
        }

        public bool __not__()
        {
            var meth = Class.ops.__not__;
            if (meth != null)
                return meth(this);
            return !__bool__();
        }

        public int __hash__()
        {
            var meth = Class.ops.__hash__;
            if (meth == null)
                throw invalid_op("hash");
            return meth(this);
        }
        public bool __subclasscheck__(DObjEx cls)
        {
            var meth = Class.ops.__subclasscheck__;
            if (meth == null)
                throw invalid_op("typecheck");
            return meth(this, cls);
        }

        public bool __contains__(DObjEx o)
        {
            var meth = Class.ops.__contains__;
            if (meth == null)
                throw invalid_op("contains");
            return meth(this, o);
        }

        public bool __gt__(DObjEx o)
        {
            var meth = Class.ops.__gt__;
            if (meth == null)
                throw invalid_op(">");
            return meth(this, o);
        }
        public bool __ge__(DObjEx o)
        {
            var meth = Class.ops.__ge__;
            if (meth == null)
                throw invalid_op(">=");
            return meth(this, o);
        }
        public bool __lt__(DObjEx o)
        {
            var meth = Class.ops.__lt__;
            if (meth == null)
                throw invalid_op("<");
            return meth(this, o);
        }
        public bool __le__(DObjEx o)
        {
            var meth = Class.ops.__le__;
            if (meth == null)
                throw invalid_op("<=");
            return meth(this, o);
        }
        public bool __eq__(DObjEx o)
        {
            var meth = Class.ops.__eq__;
            if (meth == null)
                throw invalid_op("=");
            return meth(this, o);
        }
        public bool __ne__(DObjEx o)
        {
            var meth = Class.ops.__ne__;
            if (meth == null)
                throw invalid_op("!=");
            return meth(this, o);
        }
        public DObjEx __enter__()
        {
            var meth = Class.ops.__enter__;
            if (meth == null)
                throw invalid_op("RAII");
            return meth(this);
        }

        public void __exit__(DObjEx errtype, DObjEx err, DObjEx frames)
        {
            var meth = Class.ops.__exit__;
            if (meth == null)
                throw invalid_op("RAII");
            meth(this, errtype, err, frames);
        }

        
    }

    public struct MethodStruct
    {

    }
    public struct DomainTypes
    {



    }

}