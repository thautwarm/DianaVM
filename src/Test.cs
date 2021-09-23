using System;
using DianaScript;


public static class Test
{
    
    public static int Main(String[] args)
    {
        var dvm = new DianaVM();

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
          dvm.CreateBFunc1( (DObj x) => {
             Console.WriteLine(x.repr);
             return dvm.Nil;
              
          }),
          dvm.CreateInt(1) 
        };

        var dcode = dvm.CreateCode(bytecode, new int[]{}, consts, ""); 
        if (null == dvm.vm.Run(dcode)){
            Console.WriteLine("unhandled error:" + ((DObj) dvm.vm.CurrentError).repr);
        }
        Console.WriteLine("xxx");
        return 0;
    }
}