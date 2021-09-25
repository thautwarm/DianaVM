using System;
// using DianaScript;
using System.Collections.Generic;
using System.Collections;

public static class Test
{
    
    public class S
    {
        public int x;
        public S(int x_)
        {
            x = x_;
        }
    }
    public static void test()
    {
        
        Func<object, int> f = (x) =>
        {
            return ((List<S>)x)[2].x;
        };
        var tlist = typeof(List<>);
        var tslist = tlist.MakeGenericType(typeof(S));
        var xs = Activator.CreateInstance(tslist);
        
        var xs_ = (ArrayList) xs;
        xs_.Add(new S(1));
        xs_.Add(new S(2));
        xs_.Add(new S(3));
        Console.WriteLine(f(xs));
    }

    // public static int Main(String[] args)
    // {
    //     var dvm = new DianaVM();

    //     var bytecode = new int[]{
    //         (int)CODE.LOAD_CONST, 0, 
    //         (int)CODE.LOAD_CONST, 1, 
    //         (int)CODE.LOAD_CONST, 0, 
    //         (int)CODE.LOAD_CONST, 1,
    //         (int)CODE.CALL, 1,
    //         (int)CODE.CALL, 2,
    //         (int)CODE.RETURN
    //     };
    //     var consts = new DObj[]{
    //       dvm.CreateBFunc1( (DObj x) => {
    //          Console.WriteLine(x.repr);
    //          return dvm.Nil;

    //       }),
    //       dvm.CreateInt(1) 
    //     };

    //     var dcode = dvm.CreateCode(bytecode, new int[]{}, consts, ""); 
    //     if (null == dvm.vm.Run(dcode)){
    //         Console.WriteLine("unhandled error:" + ((DObj) dvm.vm.CurrentError).repr);
    //     }
    //     Console.WriteLine("xxx");
    //     return 0;
    // }
}