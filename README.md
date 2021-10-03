# DianaScript

<p align="center">
<img width="250px" src="https://raw.githubusercontent.com/thautwarm/DianaScript/master/static/diana.png"/>
</p>

## Features

DianaScript is a minimal programming language that aims at interfacing with .NET runtime, especially for game developement in Unity.

1. No `System.Reflection`.
3. Limited APIs provided by the developers. Safe to game users.
4. C#-friendly interops.
5. Python-like object system

## Usage

You should firstly compile the source code of Ch-lang(创创语言) to Diana bytecode using the compiler written in Python:

```
python -m dianascript runtests/b.ch --out runtests/b.ran
```

Then you load the bytecode using Diana VM:

```C#
public static int Main(String[] args)
{
    Console.WriteLine("path: " + args[0]);
    var dvm = new DVM();
    var loader = new AWorld.CodeLoder(args[0]);
    var metaInd = loader.LoadCode();    
    var global = GlobalNamespace.GetGlonal();
    dvm.exec_block(metaInd, global);    
    return 0;
    
}
```

An exception will get raised to .NET side if it is not handled in DianaScript/DVM.
The exception type is compatible to both runtime.

## Contributions

You can help us in many aspects:

1. Implementation for the whole object system.

   For instance, if you want to add `+` operator support for specific builtin types(`bool`, `int`, `float`, `list`, `dict`, `None`, `tuple`...), just like [`__add__` for diana integers](https://github.com/thautwarm/DianaScript/blob/3214bc67f7b9591956f25437fd1e6df02109155e/src/NumberMethods.cs#L259); you should

   1. add `"add"` to `ExceptionList1["DInt"]`

   2. implement [the interface](https://github.com/thautwarm/DianaScript/blob/3214bc67f7b9591956f25437fd1e6df02109155e/src/ObjectSystem.cs#L93) at a proper place:

       ```C#
        public DObj __add__(DObj a)
       ``` 

2. Writing tests for the language, just like what I did(of course not enough) at [test.ch](https://github.com/thautwarm/DianaScript/blob/3214bc67f7b9591956f25437fd1e6df02109155e/runtests/test.ch#L1).

3. Language design: should we use arbitrary-precision integers? Or, should we use dicts as sets, etc.. 

4. 绝赞摆烂中