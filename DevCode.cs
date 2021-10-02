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
        var loader = AWorld.GetLoaderFrom(args[0]);
        var (metaInd, blockId) = loader.LoadCode();
        
        var g = GlobalNamespace.GetGlonal();
        dvm.exec_block(metaInd, blockId, g);
        
        return 0;
        
    }
}