from pathlib import Path

directory = Path(__file__).parent


def read(filename):
    methods = []
    strs = []
    current = "?"
    for each in (directory / filename).open():
        if each.startswith(" ") or not each.strip():
            strs.append(each)
        else:
            methods.append((current, "".join(strs)))
            strs.clear()
            current = each.strip()
    if strs:
        methods.append((current, "".join(strs)))
        strs.clear()
    return methods


eqs = ["eq", "ne"]
cmps = ["lt", "gt", "le", "ge"]
bitops = ["bitand", "bitor", "bitxor", "lshift", "rshift"]
ariths = ["add", "sub", "mul", "pow", "floordiv", "truediv"]


# for obj
ExceptionList1 = {
    "DBuiltinFunc": ["call"],
    "DRef": ["repr"],
    "DRefGlobal": ["repr"],
    "DStr": ["repr"],
    "DInt": ["repr", "mod", *eqs, *cmps, *bitops, *ariths],
    "DFloat": ["repr"],
    "DBool": ["repr", "bool", "not"],
    "DNil": ["repr"],
    "DArray": ["iter", "repr"],
    "DList": ["repr", 'getitem', 'setitem', "delitem"],
    "DSet": [],
    "DDict": ["getitem", "setitem", "delitem"],
    "DTuple": ["add", "contains", "eq"],
    "DFunc": ["repr"],
    "DWrap": [],
    "meta": ["call"],
    "DUserObj": [],
    "GlobalNamespace": [],
}

# for cls
ExceptionList2 = {
    "DBuiltinFunc": ["native"],
    "DRef": ["native"],
    "DRefGlobal": ["native"],
    "DStr": ["native", "call"],
    "DInt": ["native", "call"],
    "DFloat": ["native", "call"],
    "DBool": ["native", "call"],
    "DNil": ["native"],
    "DArray": ["native"],
    "DList": ["native"],
    "DSet": ["native"],
    "DDict": ["native"],
    "DTuple": ["native"],
    "DFunc": ["native"],
    "DWrap": ["native"],
    "DUserObj": ["native", "nativetype_correspond", "ops"],
    "GlobalNamespace": ["native"],
}

gendir = directory / ".." / "src" / "defaults"
clsmethods = read("DefaultClassOps.txt")
objmethods = read("DefaultObjOps.txt")
for name, ban_list1 in ExceptionList1.items():

    with (gendir / f"{name}.cs").open("w", encoding="utf-8") as f:

        print("using System;", file=f)
        print("using System.Runtime.CompilerServices;", file=f)
        print("using System.Collections.Generic;", file=f)
        print("namespace DianaScript", file=f)
        print("{", file=f)
        print(f"public partial class {name}", file=f)
        print("{", file=f)
        for n, code in objmethods:
            if n not in ban_list1:
                print(code.replace("$$", name), file=f)

        if name not in ExceptionList2:
            pass
        else:
            print(f"    public partial class Cls : DClsObj", file=f)
            print("    {", file=f)
            ban_list2 = ExceptionList2[name]
            for n, code in clsmethods:
                if n not in ban_list2:
                    print(code.replace("$$", name), file=f)
            print("    }", file=f)
        print("}", file=f)
        print("}", file=f)
