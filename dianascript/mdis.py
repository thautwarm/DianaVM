from __future__ import annotations
from dianascript.code_cons import *

def varslot_interp(i):
    chk = i & 0b11
    if chk == 0b00:
        return "local " + str(i >> 2)
    elif chk == 0b11:
        return "global " +  Storage.internstrings[i >> 2]
    else:
        raise NotImplementedError

def interpret(x):
    if isinstance(x, Diana_LoadVar):
        return x.__class__.__name__ + " " + varslot_interp(x.i)
    if isinstance(x, Diana_StoreVar):
        return x.__class__.__name__ + " " + varslot_interp(x.i)
    if isinstance(x, Diana_Const):
        return x.__class__.__name__ + " " + repr(Storage.dobjs[x.p_const].o)
    return x


def dis():
    for i, funcmeta in enumerate(Storage.funcmetas):
        print(funcmeta.name, i, ":")
        locs = funcmeta.lineno
        offset = 0
        bytecode = funcmeta.bytecode
        while offset < len(bytecode):
            tag = bytecode[offset]
            assert isinstance(tag, int)
            t = TypeIndex[tag]
            anns = getattr(t, '__annotations__', {})
            kwargs = {}
            for (name, ann), suboff in zip(anns.items(), range(1, t.OFFSET)):
                operand = bytecode[offset + suboff]
                kwargs[name] = operand
            inst = t(**kwargs)
            print('  ', interpret(inst))
            offset += t.OFFSET
