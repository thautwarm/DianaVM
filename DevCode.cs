using System;
using DianaScript;
using System.Collections.Generic;
using System.Collections;

public static class DevCode
{

    public static int Main(String[] args)
    {
        Console.WriteLine("path: " + args[0]);
        var dvm = new DVM();
        var loader = new AWorld.CodeLoder(args[0]);
        var metaInd = loader.LoadCode();
        
        var g = GlobalNamespace.GetGlonal();
        dvm.exec_block(metaInd, g);
        
        return 0;
        
    }
}