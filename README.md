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
public static void Main(string[] args)
{
    var dvm = new DVM();
    var loader = AWorld.GetLoaderFrom(args[0]);
    var (metaInd, blockId) = loader.LoadCode();

    var g = GlobalNamespace.GetGlonal();
    dvm.exec_block(metaInd, blockId, g);
}
```

An exception will get raised to .NET side if it is not handled in DianaScript/DVM.
The exception type is compatible to both runtime.
