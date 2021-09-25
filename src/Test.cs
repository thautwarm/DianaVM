using System;
using DianaScript;
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
 

    public static int Main(String[] args)
    {
        var dvm = new VM();

        var bytecode = new int[]{
            (int)CODE.LOAD_CONST, 0, 
            (int)CODE.LOAD_CONST, 1, 
            (int)CODE.LOAD_CONST, 0, 
            (int)CODE.LOAD_CONST, 1,
            (int)CODE.CALL, 1,
            (int)CODE.CALL, 2,
            (int)CODE.RETURN
        };
        var consts = new DObj[]{
          MK.CreateFunc( (args) => {
            var x = args[0];
             Console.WriteLine(x.__repr__);
             return dvm.Nil;

          }),
          MK.Int(1) 
        };

        var dcode = DCode.Make(bytecode,  consts: consts); 
        if (null == dvm.Run(dcode)){
            Console.WriteLine("unhandled error:" + ( MK.create(dvm.CurrentError)).__repr__);
        }
        Console.WriteLine("xxx");
        return 0;
    }
}