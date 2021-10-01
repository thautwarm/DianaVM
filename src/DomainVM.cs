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
        public static Ops Make(Dictionary<InternString, DObjEx> d){
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
    }

    public static class DObjExtension
    {
        
        private static InvalidOperationException invalid_op(this DObjEx self, string op)
        {
            return new InvalidOperationException($"unsupported operator '{op}' for {self.Class.name}");
        }
        public static void __iter__(this DObjEx self)
        {
            var meth = self.Class.ops.__iter__;
            if (meth == null)
                throw new InvalidOperationException($"{self.Class.name} objects are not iterable.");
            meth(self);
        }
        public static DObjEx __call__(this DObjEx self, Args args)
        {
            var meth = self.Class.ops.__call__;
            if (meth == null)
                throw new InvalidOperationException($"{self.Class.name} objects are not callable.");
            return meth(self, args);
        }
        public static DObjEx __add__(this DObjEx self, DObjEx a)
        {
            var meth = self.Class.ops.__add__;
            if (meth == null)
                throw self.invalid_op("+");
            return meth(self, a);
        }
        public static DObjEx __sub__(this DObjEx self, DObjEx a)
        {
            var meth = self.Class.ops.__sub__;
            if (meth == null)
                throw self.invalid_op("-");
            return meth(self, a);
        }
        public static DObjEx __mul__(this DObjEx self, DObjEx a)
        {
            var meth = self.Class.ops.__mul__;
            if (meth == null)
                throw self.invalid_op("*");
            return meth(self, a);
        }
        public static DObjEx __pow__(this DObjEx self, DObjEx a)
        {
            var meth = self.Class.ops.__pow__;
            if (meth == null)
                throw self.invalid_op("**");
            return meth(self, a);
        }
        public static DObjEx __mod__(this DObjEx self, DObjEx a)
        {
            var meth = self.Class.ops.__mod__;
            if (meth == null)
                throw self.invalid_op("%");
            return meth(self, a);
        }
        public static DObjEx __lshift__(this DObjEx self, DObjEx a)
        {
            var meth = self.Class.ops.__lshift__;
            if (meth == null)
                throw self.invalid_op("<<");
            return meth(self, a);
        }

        public static DObjEx __rshift__(this DObjEx self,  DObjEx a)
        {
            var meth = self.Class.ops.__rshift__;
            if (meth == null)
                throw self.invalid_op(">>");
            return meth(self, a);
        }
        public static DObjEx __bitand__(this DObjEx self, DObjEx a)
        {
            var meth = self.Class.ops.__rshift__;
            if (meth == null)
                throw self.invalid_op("&");
            return meth(self, a);
        }
        public static DObjEx __bitor__(this DObjEx self, DObjEx a)
        {
            var meth = self.Class.ops.__bitor__;
            if (meth == null)
                throw self.invalid_op("|");
            return meth(self, a);
        }
        public static DObjEx __bitxor__(this DObjEx self, DObjEx a)
        {
            var meth = self.Class.ops.__bitxor__;
            if (meth == null)
                throw self.invalid_op("^");
            return meth(self, a);
        }
        public static DObjEx __getitem__(this DObjEx self, DObjEx a)
        {
            var meth = self.Class.ops.__getitem__;
            if (meth == null)
                throw new InvalidOperationException($"'{self.Class.name}' object is not subscriptable.");
            return meth(self, a);
        }
        public static void __setitem__(this DObjEx self, DObjEx a, DObjEx b)
        {
            var meth = self.Class.ops.__setitem__;
            if (meth == null)
                throw new InvalidOperationException($"'{self.Class.name}' object does not support item assignment.");
            meth(self, a, b);
        }

        public static void __delitem__(this DObjEx self, DObjEx a)
        {
            var meth = self.Class.ops.__delitem__;
            if (meth == null)
                throw new InvalidOperationException($"'{self.Class.name}' object does not support item deletion.");
            meth(self, a);
        }

        public static void __getattr__(this DObjEx self, InternString attr)
        {
            var meth = self.Class.ops.__getattr__;
            if (meth == null)
                throw self.invalid_op($"o.{attr}");
            meth(self, attr);
        }
        public static void __setattr__(this DObjEx self, InternString attr, DObjEx value)
        {
            var meth = self.Class.ops.__setattr__;
            if (meth == null)
                throw self.invalid_op($"o.{attr}=v");
            meth(self, attr, value);
        }
        public static int __len__(this DObjEx self)
        {
            var meth = self.Class.ops.__len__;
            if (meth == null)
                throw self.invalid_op($"len");
            return meth(self);
        }
        public static bool __bool__(this DObjEx self)
        {
            var meth = self.Class.ops.__bool__;
            if (meth == null)
                throw self.invalid_op("cast to bool");
            return meth(self);
        }

        public static  bool __not__(this DObjEx self)
        {
            var meth = self.Class.ops.__not__;
            if (meth != null)
                return meth(self);
            return !self.__bool__();
        }

        public static int __hash__(this DObjEx self)
        {
            var meth = self.Class.ops.__hash__;
            if (meth == null)
                throw self.invalid_op("hash");
            return meth(self);
        }
        public static bool __subclasscheck__(this DObjEx self, DObjEx cls)
        {
            var meth = self.Class.ops.__subclasscheck__;
            if (meth == null)
                throw self.invalid_op("typecheck");
            return meth(self, cls);
        }

        public static bool __contains__(this DObjEx self, DObjEx o)
        {
            var meth = self.Class.ops.__contains__;
            if (meth == null)
                throw self.invalid_op("contains");
            return meth(self, o);
        }

        public static bool __gt__(this DObjEx self, DObjEx o)
        {
            var meth = self.Class.ops.__gt__;
            if (meth == null)
                throw self.invalid_op(">");
            return meth(self, o);
        }
        public static bool __ge__(this DObjEx self, DObjEx o)
        {
            var meth = self.Class.ops.__ge__;
            if (meth == null)
                throw self.invalid_op(">=");
            return meth(self, o);
        }
        public static bool __lt__(this DObjEx self, DObjEx o)
        {
            var meth = self.Class.ops.__lt__;
            if (meth == null)
                throw self.invalid_op("<");
            return meth(self, o);
        }
        public static bool __le__(this DObjEx self, DObjEx o)
        {
            var meth = self.Class.ops.__le__;
            if (meth == null)
                throw self.invalid_op("<=");
            return meth(self, o);
        }
        public static bool __eq__(this DObjEx self, DObjEx o)
        {
            var meth = self.Class.ops.__eq__;
            if (meth == null)
                throw self.invalid_op("=");
            return meth(self, o);
        }
        public static  bool __ne__(this DObjEx self, DObjEx o)
        {
            var meth = self.Class.ops.__ne__;
            if (meth == null)
                throw self.invalid_op("!=");
            return meth(self, o);
        }
        public static DObjEx __enter__(this DObjEx self)
        {
            var meth = self.Class.ops.__enter__;
            if (meth == null)
                throw self.invalid_op("RAII");
            return meth(self);
        }

        public static void __exit__(this DObjEx self, DObjEx errtype, DObjEx err, DObjEx frames)
        {
            var meth = self.Class.ops.__exit__;
            if (meth == null)
                throw self.invalid_op("RAII");
            meth(self, errtype, err, frames);
        }

        
    }

    public struct MethodStruct
    {

    }
    public struct DomainTypes
    {



    }

}