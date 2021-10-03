using System;

namespace DianaScript
{
    using static MathF;
    public static class NumberMethods
    {

        static int s_intmod(int a, int b)
        {
            int r = a % b;
            return r < 0 ? r + b : r;
        }

        static float s_floatmod(float a, float b)
        {
            float r = a % b;
            return r < 0 ? r + b : r;
        }



        public static Exception unsupported_ops(DObj lhs, string op, DObj rhs) =>
             new InvalidOperationException($"'unsupported operation: '{lhs.Class.name}' {op} '{rhs.Class.name}'.");
        public static DObj int_add(DInt self, DObj other)
        {
            switch (other)
            {
                case DFloat v: return MK.Float(self.value + v.value);
                case DBool v: return MK.Int(self.value + (v.value ? 1 : 0));
                case DInt v: return MK.Int(self.value + v.value);
                default:
                    throw unsupported_ops(self, "+", other);
            }
        }
        public static DObj int_sub(DInt self, DObj other)
        {
            switch (other)
            {
                case DFloat v: return MK.Float(self.value - v.value);
                case DBool v: return MK.Int(self.value - (v.value ? 1 : 0));
                case DInt v: return MK.Int(self.value - v.value);
                default:
                    throw unsupported_ops(self, "-", other);
            }
        }
        public static DObj int_mul(DInt self, DObj other)
        {
            switch (other)
            {
                case DFloat v: return MK.Float(self.value * v.value);
                case DBool v: return MK.Int(self.value * (v.value ? 1 : 0));
                case DInt v: return MK.Int(self.value * v.value);
                default:
                    throw unsupported_ops(self, "*", other);
            }
        }

        public static DObj int_floordiv(DInt self, DObj other)
        {
            switch (other)
            {
                case DFloat v: return MK.Int((int)(self.value / v.value));
                case DBool v: return MK.Int(self.value / (v.value ? 1 : 0));
                case DInt v: return MK.Int(self.value / v.value);
                default:
                    throw unsupported_ops(self, "/", other);
            }
        }
        public static DObj int_truediv(DInt self, DObj other)
        {
            switch (other)
            {
                case DFloat v: return MK.Float(self.value / v.value);
                case DBool v: return MK.Float(self.value / (v.value ? 1 : 0));
                case DInt v: return MK.Float(((float)self.value) / v.value);
                default:
                    throw unsupported_ops(self, "/", other);
            }
        }


        public static DObj int_mod(DInt self, DObj other)
        {
            switch (other)
            {
                case DFloat v: return MK.Float(s_floatmod(((float)self.value), v.value));
                case DBool v: return MK.Int(s_intmod(self.value, v.value ? 1 : 0));
                case DInt v: return MK.Int(s_intmod(self.value, v.value));
                default:
                    throw unsupported_ops(self, "%", other);
            }
        }


        static int s_intpow(int b, int pow)
        {
            int result = 1;
            while (true)
            {
                if ((pow & 1) == 1)
                    result *= b;
                pow >>= 1;
                if (pow != 0)
                    break;
                b *= b;
            }

            return result;
        }
        public static DObj int_pow(DInt self, DObj other)
        {
            switch (other)
            {
                case DFloat v: return MK.Float(Pow(self.value, v.value));
                case DBool v: return MK.Int(v.value ? self.value : 1);
                case DInt v:
                    var i = v.value;
                    if (i < 0)
                        return MK.Float(MathF.Pow(self.value, v.value));

                    return MK.Int(s_intpow(self.value, v.value));
                default:
                    throw unsupported_ops(self, "**", other);
            }
        }

        public static DObj int_lshift(DInt self, DObj other)
        {
            switch (other)
            {
                case DFloat v: throw unsupported_ops(self, "<<", other);
                case DBool v: return MK.Int(v.value ? (self.value << 1) : self.value);
                case DInt v: return MK.Int(self.value << v.value);
                default:
                    throw unsupported_ops(self, "<<", other);
            }
        }

        public static DObj int_rshift(DInt self, DObj other)
        {
            switch (other)
            {
                case DFloat v: throw unsupported_ops(self, ">>", other);
                case DBool v: return MK.Int(v.value ? (self.value >> 1) : self.value);
                case DInt v: return MK.Int(self.value >> v.value);
                default:
                    throw unsupported_ops(self, ">>", other);
            }
        }

        public static DObj int_bitand(DInt self, DObj other)
        {
            switch (other)
            {
                case DFloat v: throw unsupported_ops(self, "&", other);
                case DBool v: return MK.Int(v.value ? (self.value & 1) : 0);
                case DInt v: return MK.Int(self.value & v.value);
                default:
                    throw unsupported_ops(self, "&", other);
            }
        }

        public static DObj int_bitor(DInt self, DObj other)
        {
            switch (other)
            {
                case DFloat v: throw unsupported_ops(self, "|", other);
                case DBool v: return MK.Int(v.value ? (self.value | 1) : 0);
                case DInt v: return MK.Int(self.value | v.value);
                default:
                    throw unsupported_ops(self, "|", other);
            }
        }

        public static DObj int_bitxor(DInt self, DObj other)
        {
            switch (other)
            {
                case DFloat v: throw unsupported_ops(self, "^", other);
                case DBool v: return MK.Int(v.value ? (self.value ^ 1) : 0);
                case DInt v: return MK.Int(self.value ^ v.value);
                default:
                    throw unsupported_ops(self, "^", other);
            }
        }

        static int dbool_to_int(DBool b) => b.value ? 1 : 0;
        public static bool int_lt(DInt self, DObj other)
        {
            switch (other)
            {
                case DFloat v: return (float)self.value < v.value;
                case DInt v: return self.value < v.value;
                default:
                    throw unsupported_ops(self, "<", other);
            }
        }

        public static bool int_le(DInt self, DObj other)
        {
            switch (other)
            {
                case DFloat v: return (float)self.value <= v.value;
                case DInt v: return self.value <= v.value;
                default:
                    throw unsupported_ops(self, "<=", other);
            }
        }

        public static bool int_gt(DInt self, DObj other)
        {
            switch (other)
            {
                case DFloat v: return (float)self.value > v.value;
                case DInt v: return self.value > v.value;
                default:
                    throw unsupported_ops(self, ">", other);
            }
        }

        public static bool int_ge(DInt self, DObj other)
        {
            switch (other)
            {
                case DFloat v: return (float)self.value >= v.value;
                case DInt v: return self.value >= v.value;
                default:
                    throw unsupported_ops(self, ">=", other);
            }
        }

        public static bool int_eq(DInt self, DObj other)
        {
            switch (other)
            {
                case DFloat v: return (float)self.value == v.value;
                case DInt v: return self.value == v.value;
                default:
                    return false;
            }
        }

        public static bool int_ne(DInt self, DObj other)
        {
            switch (other)
            {
                case DFloat v: return (float)self.value != v.value;
                case DInt v: return self.value != v.value;
                default:
                    return true;
            }
        }
    }

    public partial class DInt
    {

        public DObj __add__(DObj o) => NumberMethods.int_add(this, o);
        public DObj __sub__(DObj o) => NumberMethods.int_sub(this, o);

        public DObj __mul__(DObj o) => NumberMethods.int_mul(this, o);

        public DObj __floordiv__(DObj o) => NumberMethods.int_floordiv(this, o);
        public DObj __truediv__(DObj o) => NumberMethods.int_truediv(this, o);

        public DObj __mod__(DObj o) => NumberMethods.int_mod(this, o);

        public DObj __pow__(DObj o) => NumberMethods.int_pow(this, o);

        public DObj __lshift__(DObj o) => NumberMethods.int_lshift(this, o);

        public DObj __rshift__(DObj o) => NumberMethods.int_rshift(this, o);

        public bool __eq__(DObj o) => NumberMethods.int_eq(this, o);

        public bool __ne__(DObj o) => NumberMethods.int_ne(this, o);

        public bool __lt__(DObj o) => NumberMethods.int_lt(this, o);

        public bool __gt__(DObj o) => NumberMethods.int_gt(this, o);

        public bool __le__(DObj o) => NumberMethods.int_le(this, o);

        public bool __ge__(DObj o) => NumberMethods.int_ge(this, o);

        public DObj __bitand__(DObj o) => NumberMethods.int_bitand(this, o);
        public DObj __bitor__(DObj o) => NumberMethods.int_bitor(this, o);
        public DObj __bitxor__(DObj o) => NumberMethods.int_bitxor(this, o);


    }
}