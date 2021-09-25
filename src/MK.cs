using System;
using System.Collections.Generic;
namespace DianaScript{


public struct THelper<A>{
    public static THelper<A> val;
}

public static class MK{

    
    public static A Inst<A>() where A: new() {
        return new A();
    }
    public static B unbox<B>(DObj a) => unbox(THelper<B>.val, a);
    public static THelper<A> Hint<A>() => THelper<A>.val;
    public static T unbox<T>(THelper<T> _, T o) => o;
    public static B unbox<A, B>(THelper<B> _, A o) where B : A => (B) o;
    public static B unbox<B>(THelper<B> _, DObj o) => (B) o.Native;
    public static DObj cast(THelper<DObj> _, DObj o) => o;
    public static B cast<A, B>(THelper<B> _, A o) where A : B => o;
    public static A cast<A, B>(THelper<A> _, B o) where A : B => (A) o;
    public static DObj cast<A>(THelper<DObj> _, A o) => create(o);
    public static Char[] cast(THelper<Char[]> _, String s) => s.ToCharArray();
    public static String cast(THelper<String> _, Char[] s) => new String(s);
    public static String cast(THelper<Char> _, Char s) => new String(new[]{s});
    public static String cast(THelper<String[]> _, DObj s) {
        throw new NotImplementedException();
    }

    public static T[] cast<T>(THelper<T[]> _, DObj o) {
        var d = ((DArray) o);
        if (typeof(T) == d.eltype.NativeType){
            return d.src as T[];
        }
        else{
            throw new D_TypeError(nameof(T), o.Native.GetType().Name);
        }
    }

    public static float cast(THelper<float> _,  int s) => s;
    public static int cast(THelper<int> _,  float s) => (int) s;
    public static IEnumerable<DObj> cast(THelper<IEnumerable<DObj>> _,  DObj s) => s.__iter__;
    public static bool cast(THelper<bool> _,  DObj s) => s.__bool__;
    public static A cast<A>(THelper<A> _, A s) => s;

    
    public static DObj create(string s) => String(s);
    
    // public static B cast<A, B>(THelper<B> _, A s) where B : A => (B) s;
    public static DStr String(string s) => DStr.Make(s);
    public static DInt Int(int s) => DInt.Make(s);

    public static DObj create(int s) => Int(s);
    public static DBool Bool(bool b) => DBool.Make(b);

    public static DObj create(bool s) => Bool(s);
    public static DFloat Float(float b) => DFloat.Make(b);

    public static DObj create(float s) => Float(s);
    public static DNil Nil() => DNil.unique;

    public static DObj create() => Nil();
    public static DDict Dict(Dictionary<DObj, DObj> d) => throw new NotImplementedException();

    public static DObj create(Dictionary<DObj, DObj> d) => Dict(d);

    public static DTuple Tuple(DObj[] d) => throw new NotImplementedException();

    public static DObj create(DObj[] d) => Tuple(d);

    public static DList List(List<DObj> d) => throw new NotImplementedException();
    
    // public static T create<T>(T o) where T : DObj => o;
    public static DObj create(List<DObj> d) => List(d);
    
    public static DSet Set(HashSet<DObj> d) => throw new NotImplementedException();

    public static DObj create(HashSet<DObj> d) => Set(d);
    
    public static DArray Array<T>(T[] d) => throw new NotImplementedException();

    public static DObj create<T>(T[] d) => Array(d);


    public static DWrap Wrap(object o) => new DWrap(o);
    public static DObj create<T>(T a) => Wrap(a);

    // public static A unbox<A>(DObj o){
    //     if (o is A){
            
    //     }
    // }

    public static DBuiltinFunc CreateFunc(Func<Args, DObj> f) => 
    DBuiltinFunc.Make(f);
    public static DBuiltinFunc create(Func<Args, DObj> d) => CreateFunc(d);
}
}