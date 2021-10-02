from __future__ import annotations
from dianascript.code_cons import *

def varslot_interp(i):
    chk = i & 0b11
    if chk == 0b00:
        return "local " + str(i >> 2)
    elif chk == 0b11:
        return "global " +  DFlatGraphCode.internstrings[i >> 2]
    else:
        raise NotImplementedError

def interpret(x):
    if isinstance(x, Diana_LoadVar):
        return x.__class__.__name__ + " " + varslot_interp(x.i)
    if isinstance(x, Diana_StoreVar):
        return x.__class__.__name__ + " " + varslot_interp(x.i)
    if isinstance(x, Diana_Const):
        return x.__class__.__name__ + " " + repr(DFlatGraphCode.dobjs[x.p_const].o)
    return x
def dis():
    for i, block in enumerate(DFlatGraphCode.blocks):
        print("block", i, ":")
        locs = block.location_data
        j = len(block.codes) if locs else locs[0][0] - 1
        for i, ptr in enumerate(block.codes):
            if i > j:
                j = i
                line, col = locs[i]
                print(f"(line {line} column {col})")
            t, builder = DFlatGraphCode.inspect[ptr.kind]
            if builder is None:
                print("    ", t.__name__, sep='')
                continue
            
            inst = builder[ptr.ind]
            print("    ", interpret(inst), sep='')


