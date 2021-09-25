// using System;
// using DianaScript;
// using System.Collections.Generic;
// using System.Collections;

// public static class DevCode
// {
    
//     public class S
//     {
//         public int x;
//         public S(int x_)
//         {
//             x = x_;
//         }
//     }
 

//     public static int Main(String[] args)
//     {
//         var dvm = new VM();

//         var bytecode = new int[]{
//             (int)CODE.LOAD_CONST, 0, 
//             (int)CODE.LOAD_CONST, 1, 
//             (int)CODE.LOAD_CONST, 1,
//             (int)CODE.CALL, 2,
//             (int)CODE.RETURN
//         };
//         var consts = new DObj[]{
//           MK.CreateFunc( (args) => {
//             var x = args[0];
//             Console.WriteLine(x.__repr__);
//             throw new D_TypeError("require only 1 arg");
//           }),
//           MK.Int(1) 
//         };

//         var dcode = DCode.Make(bytecode,  consts: consts, filename: "a.ran", name: "__main__"); 
//         dvm.Run(dcode);
//         return 0;
//     }
// }