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

### AIR Parser
```C#

var parser = new DianaScript.Parser("xxx.ran");
var code = parser.ReadCode()
```

### VM

```C#
var code = ... // maybe parsed from file using  DianaScript.Parser;
var vm = new DianaScript.VM();
vm.Run(code);
```

An exception will get raised to .NET side if it is not handled in DianaScript/DVM.
The exception type is compatible to both runtime.
