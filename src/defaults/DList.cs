using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
namespace DianaScript
{
public partial class DList
{


        private InvalidOperationException invalid_op(string op)
        {
            return new InvalidOperationException($"unsupported operator '{op}' for {this.Class.name}");
        }


        public IEnumerable<DObj> __iter__()
        {
            var meth = this.Class.ops.__iter__;
            if (meth == null)
                throw new InvalidOperationException($"{this.Class.name} objects are not iterable.");
            return meth(this);
        }


        public DObj __call__(Args args)
        {
            var meth = this.Class.ops.__call__;
            if (meth == null)
                throw new InvalidOperationException($"{this.Class.name} objects are not callable.");
            return meth(this, args);
        }


        public DObj __add__(DObj a)
        {
            var meth = this.Class.ops.__add__;
            if (meth == null)
                throw this.invalid_op("+");
            return meth(this, a);
        }


        public DObj __sub__(DObj a)
        {
            var meth = this.Class.ops.__sub__;
            if (meth == null)
                throw this.invalid_op("-");
            return meth(this, a);
        }

        public DObj __mul__(DObj a)
        {
            var meth = this.Class.ops.__mul__;
            if (meth == null)
                throw this.invalid_op("*");
            return meth(this, a);
        }


        public DObj __floordiv__(DObj a)
        {
            var meth = this.Class.ops.__floordiv__;
            if (meth == null)
                throw this.invalid_op("floor division");
            return meth(this, a);
        }

        public DObj __truediv__(DObj a)
        {
            var meth = this.Class.ops.__truediv__;
            if (meth == null)
                throw this.invalid_op("division");
            return meth(this, a);
        }

        public DObj __pow__(DObj a)
        {
            var meth = this.Class.ops.__pow__;
            if (meth == null)
                throw this.invalid_op("**");
            return meth(this, a);
        }

        public DObj __mod__(DObj a)
        {
            var meth = this.Class.ops.__mod__;
            if (meth == null)
                throw this.invalid_op("%");
            return meth(this, a);
        }

        public DObj __lshift__(DObj a)
        {
            var meth = this.Class.ops.__lshift__;
            if (meth == null)
                throw this.invalid_op("<<");
            return meth(this, a);
        }


        public DObj __rshift__(DObj a)
        {
            var meth = this.Class.ops.__rshift__;
            if (meth == null)
                throw this.invalid_op(">>");
            return meth(this, a);
        }


        public DObj __bitand__(DObj a)
        {
            var meth = this.Class.ops.__rshift__;
            if (meth == null)
                throw this.invalid_op("&");
            return meth(this, a);
        }

        public DObj __bitor__(DObj a)
        {
            var meth = this.Class.ops.__bitor__;
            if (meth == null)
                throw this.invalid_op("|");
            return meth(this, a);
        }

        public DObj __bitxor__(DObj a)
        {
            var meth = this.Class.ops.__bitxor__;
            if (meth == null)
                throw this.invalid_op("^");
            return meth(this, a);
        }


        public DObj __getitem__(DObj a)
        {
            var meth = this.Class.ops.__getitem__;
            if (meth == null)
                throw new InvalidOperationException($"'{this.Class.name}' object is not subscriptable.");
            return meth(this, a);
        }


        public void __setitem__(DObj a, DObj b)
        {
            var meth = this.Class.ops.__setitem__;
            if (meth == null)
                throw new InvalidOperationException($"'{this.Class.name}' object does not support item assignment.");
            meth(this, a, b);
        }

        public void __delitem__(DObj a)
        {
            var meth = this.Class.ops.__delitem__;
            if (meth == null)
                throw new InvalidOperationException($"'{this.Class.name}' object does not support item deletion.");
            meth(this, a);
        }

        public DObj __getattr__(InternString attr)
        {
            var meth = this.Class.ops.__getattr__;
            if (meth == null)
                throw this.invalid_op($"o.{attr}");
            return meth(this, attr);
        }

        public void __setattr__(InternString attr, DObj value)
        {
            var meth = this.Class.ops.__setattr__;
            if (meth == null)
                throw this.invalid_op($"o.{attr}=v");
            meth(this, attr, value);
        }

        public int __len__()
        {
            var meth = this.Class.ops.__len__;
            if (meth == null)
                throw new InvalidOperationException($"'{this.Class.name}' object has no concept of length.");
            return meth(this);
        }

        public bool __bool__()
        {
            var meth = this.Class.ops.__bool__;
            if (meth == null)
                throw new InvalidOperationException($"'{this.Class.name}' object cannot be casted to boolean.");
            return meth(this);
        }

        public DObj __neg__()
        {
            throw this.invalid_op($"negation(-)");
        }

        public DObj __invert__()
        {
            throw this.invalid_op($"invert(~)");
        }

        public bool __not__()
        {
            var meth = this.Class.ops.__bool__;
            if (meth == null)
                this.invalid_op("not");
            return meth(this);
        }

        public int __hash__()
        {
            return RuntimeHelpers.GetHashCode(this.Native);
        }


        public bool __subclasscheck__(DObj o)
        {
            var meth = this.Class.ops.__subclasscheck__;
            if (meth == null)
                throw new InvalidOperationException($"'{this.Class.name}' object cannot be used for runtime type check.");
            return meth(this, o);
        }

        public bool __contains__(DObj o)
        {
            var meth = this.Class.ops.__contains__;
            if (meth == null)
                throw this.invalid_op("contains");
            return meth(this, o);
        }

        public bool __gt__(DObj o)
        {
            var meth = this.Class.ops.__gt__;
            if (meth == null)
                throw this.invalid_op(">");
            return meth(this, o);
        }

        public bool __ge__(DObj o)
        {
            var meth = this.Class.ops.__ge__;
            if (meth == null)
                throw this.invalid_op(">=");
            return meth(this, o);
        }


        public bool __lt__(DObj o)
        {
            var meth = this.Class.ops.__lt__;
            if (meth == null)
                throw this.invalid_op("<");
            return meth(this, o);
        }


        public bool __le__(DObj o)
        {
            var meth = this.Class.ops.__le__;
            if (meth == null)
                throw this.invalid_op("<=");
            return meth(this, o);
        }


        public bool __eq__(DObj o)
        {
           var meth = this.Class.ops.__eq__;
            if (meth == null)
                throw this.invalid_op("=");
            return meth(this, o);
        }


        public bool __ne__(DObj o)
        {
            var meth = this.Class.ops.__ne__;
            if (meth == null)
                throw this.invalid_op("!=");
            return meth(this, o);
        }


        public DObj __enter__()
        {
            var meth = this.Class.ops.__enter__;
            if (meth == null)
                throw this.invalid_op("with-enter");
            return meth(this);
        }

        public void __exit__(DObj errtype, DObj err, DObj frames)
        {
            var meth = this.Class.ops.__exit__;
            if (meth == null)
                throw this.invalid_op("with-exit");
            meth(this, errtype, err, frames);
        }

        public string __str__()
        {
            var meth = this.Class.ops.__repr__;
            if (meth == null)
                return this.__repr__();
            return meth(this);
        }


        public int CompareTo(DObj other)
        {
            if (this.__eq__(other))
                return 0;
            else if (this.__gt__(other))
                return 1;
            else if (this.__lt__(other))
                return -1;
            else
                throw new D_InvalidComparison(this.Class);
        }


        public bool Equals(DObj other)
        {
                return this.__eq__(other);
        }

        public override bool Equals(Object other)
        {
                return this.__eq__(MK.create(other));
        }

        public override int GetHashCode() => __hash__();
    public partial class Cls : DClsObj
    {

        public DClsObj Class => meta.unique;

        public Ops ops => Ops.defaultOps;

        public object Native => NativeType;



        private InvalidOperationException invalid_op(string op)
        {
            return new InvalidOperationException($"unsupported operator '{op}' for class object {this.name}");
        }


        public IEnumerable<DObj> __iter__()
        {
            
            throw new InvalidOperationException($"class object '{this.name}' is not iterable.");
            
        }


        public DObj __call__(Args args)
        {
            throw new InvalidOperationException($"class object '{this.name}' is not callable.");
        }


        public DObj __add__(DObj a)
        {
            throw this.invalid_op("+");
        }


        public DObj __sub__(DObj a)
        {
            
            throw this.invalid_op("-");
            
        }

        public DObj __mul__(DObj a)
        {
            throw this.invalid_op("*");
        }

        public DObj __floordiv__(DObj a)
        {
            var meth = this.Class.ops.__floordiv__;
            if (meth == null)
                throw this.invalid_op("floor division");
            return meth(this, a);
        }

        public DObj __truediv__(DObj a)
        {
            var meth = this.Class.ops.__truediv__;
            if (meth == null)
                throw this.invalid_op("division");
            return meth(this, a);
        }

        public DObj __pow__(DObj a)
        {
            throw this.invalid_op("**");
        }

        public DObj __mod__(DObj a)
        {
            throw this.invalid_op("%");
        }

        public DObj __lshift__(DObj a)
        {
            throw this.invalid_op("<<");
        }


        public DObj __rshift__(DObj a)
        {
            throw this.invalid_op(">>");
        }


        public DObj __bitand__(DObj a)
        {
            
            throw this.invalid_op("&");
        }

        public DObj __bitor__(DObj a)
        {
            throw this.invalid_op("|");    
        }

        public DObj __bitxor__(DObj a)
        {

            throw this.invalid_op("^");

        }


        public DObj __getitem__(DObj a)
        {

            throw new InvalidOperationException($"class object'{this.name}' is not subscriptable.");

        }


        public void __setitem__(DObj a, DObj b)
        {

            throw new InvalidOperationException($"class object '{this.name}' does not support item assignment.");
        }

        public void __delitem__(DObj a)
        {
            throw new InvalidOperationException($"class object '{this.name}' does not support item deletion.");
        }

        public DObj __getattr__(InternString attr)
        {
            if (Dict == null || !Dict.TryGetValue(attr, out var meth))
                throw this.invalid_op($"o.{attr}");
            return meth;
        }

        public void __setattr__(InternString attr, DObj value)
        {
            throw this.invalid_op($"o.{attr}=v");
        }

        public int __len__()
        {
            throw this.invalid_op($"len");

        }

        public bool __bool__()
        {
            return true;
        }

        public DObj __neg__()
        {
            throw this.invalid_op($"negation(-)");
        }

        public DObj __invert__()
        {
            throw this.invalid_op($"invert(~)");
        }

        public bool __not__()
        {
            return !this.__bool__();
        }

        public int __hash__()
        {
            return RuntimeHelpers.GetHashCode(this);
        }


        public bool __subclasscheck__(DObj o)
        {
            return (o.Class == this);
        }

        public bool __contains__(DObj o)
        {
            throw this.invalid_op("contains");
        }

        public bool __gt__(DObj o)
        {
            throw this.invalid_op(">");
        }

        public bool __ge__(DObj o)
        {
            throw this.invalid_op(">=");
        }


        public bool __lt__(DObj o)
        {
            throw this.invalid_op("<");
        }


        public bool __le__(DObj o)
        {
            throw this.invalid_op("<=");
        }


        public bool __eq__(DObj o)
        {
           return this == o;
        }


        public bool __ne__(DObj o)
        {
            return !(__eq__(o));
        }


        public DObj __enter__()
        {
            throw this.invalid_op("with-enter");
        }

        public void __exit__(DObj errtype, DObj err, DObj frames)
        {

            throw this.invalid_op("with-exit");
        }

        public string __repr__()
        {
            return this.name;
        }


        public string __str__()
        {
            return __repr__();
        }



        public int CompareTo(DObj other)
        {
            if (this.__eq__(other))
                return 0;
            else if (this.__gt__(other))
                return 1;
            else if (this.__lt__(other))
                return -1;
            else
                throw new D_InvalidComparison(this.Class);
        }


        public bool Equals(DObj other)
        {
                return this.__eq__(other);
        }

        public override bool Equals(Object other)
        {
                return this.__eq__(MK.create(other));
        }

        public override int GetHashCode() => __hash__();




    }
}
}
