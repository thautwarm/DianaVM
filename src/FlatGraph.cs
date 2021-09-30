using System;
using System.Collections.Generic;
namespace DianaScript
{
public struct Catch
{
    public int exc_type;
    public int body;

    public static Catch Make(int exc_type, int body) => new Catch
    {
        exc_type = exc_type,

        body = body,

    };
}
public struct FuncMeta
{
    public bool is_vararg;
    public int[] freeslots;
    public int[] nonargcells;
    public int narg;
    public int nlocal;
    public InternString name;
    public string filename;
    public int lineno;
    public string[] freenames;
    public string[] localnames;

    public static FuncMeta Make(bool is_vararg, int[] freeslots, int[] nonargcells, int narg, int nlocal, InternString name, string filename, int lineno, string[] freenames, string[] localnames) => new FuncMeta
    {
        is_vararg = is_vararg,

        freeslots = freeslots,

        nonargcells = nonargcells,

        narg = narg,

        nlocal = nlocal,

        name = name,

        filename = filename,

        lineno = lineno,

        freenames = freenames,

        localnames = localnames,

    };
}
public struct Block
{
    public Ptr[] codes;
    public (int,int)[] location_data;
    public string filename;

    public static Block Make(Ptr[] codes, (int,int)[] location_data, string filename) => new Block
    {
        codes = codes,

        location_data = location_data,

        filename = filename,

    };
}
public struct Diana_FunctionDef
{
    public int metadataInd;
    public int code;

}
public struct Diana_LoadGlobalRef
{
    public InternString istr;

}
public struct Diana_DelVar
{
    public int target;

}
public struct Diana_LoadVar
{
    public int i;

}
public struct Diana_StoreVar
{
    public int i;

}
public struct Diana_Action
{
    public int kind;

}
public struct Diana_ControlIf
{
    public int arg;

}
public struct Diana_Control
{
    public int arg;

}
public struct Diana_Try
{
    public int body;
    public Catch[] except_handlers;
    public int final_body;

}
public struct Diana_Loop
{
    public int body;

}
public struct Diana_For
{
    public int body;

}
public struct Diana_With
{
    public int body;

}
public struct Diana_GetAttr
{
    public InternString attr;

}
public struct Diana_SetAttr
{
    public InternString attr;

}
public struct Diana_SetAttrOp_add
{
    public InternString attr;

}
public struct Diana_SetAttrOp_sub
{
    public InternString attr;

}
public struct Diana_SetAttrOp_mul
{
    public InternString attr;

}
public struct Diana_SetAttrOp_truediv
{
    public InternString attr;

}
public struct Diana_SetAttrOp_floordiv
{
    public InternString attr;

}
public struct Diana_SetAttrOp_mod
{
    public InternString attr;

}
public struct Diana_SetAttrOp_pow
{
    public InternString attr;

}
public struct Diana_SetAttrOp_lshift
{
    public InternString attr;

}
public struct Diana_SetAttrOp_rshift
{
    public InternString attr;

}
public struct Diana_SetAttrOp_bitor
{
    public InternString attr;

}
public struct Diana_SetAttrOp_bitand
{
    public InternString attr;

}
public struct Diana_SetAttrOp_bitxor
{
    public InternString attr;

}
public struct Diana_MKDict
{
    public int n;

}
public struct Diana_MKSet
{
    public int n;

}
public struct Diana_MKList
{
    public int n;

}
public struct Diana_Call
{
    public int n;

}
public struct Diana_Format
{
    public int format;
    public int argn;

}
public struct Diana_Const
{
    public int p_const;

}
public struct Diana_MKTuple
{
    public int n;

}
public struct Diana_Pack
{
    public int n;

}
public enum CODE
{
    Diana_FunctionDef,
    Diana_LoadGlobalRef,
    Diana_DelVar,
    Diana_LoadVar,
    Diana_StoreVar,
    Diana_Action,
    Diana_ControlIf,
    Diana_Control,
    Diana_Try,
    Diana_Loop,
    Diana_For,
    Diana_With,
    Diana_GetAttr,
    Diana_SetAttr,
    Diana_SetAttrOp_add,
    Diana_SetAttrOp_sub,
    Diana_SetAttrOp_mul,
    Diana_SetAttrOp_truediv,
    Diana_SetAttrOp_floordiv,
    Diana_SetAttrOp_mod,
    Diana_SetAttrOp_pow,
    Diana_SetAttrOp_lshift,
    Diana_SetAttrOp_rshift,
    Diana_SetAttrOp_bitor,
    Diana_SetAttrOp_bitand,
    Diana_SetAttrOp_bitxor,
    Diana_DelItem,
    Diana_GetItem,
    Diana_SetItem,
    Diana_SetItemOp_add,
    Diana_SetItemOp_sub,
    Diana_SetItemOp_mul,
    Diana_SetItemOp_truediv,
    Diana_SetItemOp_floordiv,
    Diana_SetItemOp_mod,
    Diana_SetItemOp_pow,
    Diana_SetItemOp_lshift,
    Diana_SetItemOp_rshift,
    Diana_SetItemOp_bitor,
    Diana_SetItemOp_bitand,
    Diana_SetItemOp_bitxor,
    Diana_BinaryOp_add,
    Diana_BinaryOp_sub,
    Diana_BinaryOp_mul,
    Diana_BinaryOp_truediv,
    Diana_BinaryOp_floordiv,
    Diana_BinaryOp_mod,
    Diana_BinaryOp_pow,
    Diana_BinaryOp_lshift,
    Diana_BinaryOp_rshift,
    Diana_BinaryOp_bitor,
    Diana_BinaryOp_bitand,
    Diana_BinaryOp_bitxor,
    Diana_UnaryOp_invert,
    Diana_UnaryOp_not,
    Diana_MKDict,
    Diana_MKSet,
    Diana_MKList,
    Diana_Call,
    Diana_Format,
    Diana_Const,
    Diana_MKTuple,
    Diana_Pack,
}

public partial class DFlatGraphCode
{
    public string[] strings;

    public DObj[] dobjs;

    public InternString[] internstrings;

    public Catch[] catchs;

    public FuncMeta[] funcmetas;

    public Block[] blocks;

    public Diana_FunctionDef[] diana_functiondefs;

    public Diana_LoadGlobalRef[] diana_loadglobalrefs;

    public Diana_DelVar[] diana_delvars;

    public Diana_LoadVar[] diana_loadvars;

    public Diana_StoreVar[] diana_storevars;

    public Diana_Action[] diana_actions;

    public Diana_ControlIf[] diana_controlifs;

    public Diana_Control[] diana_controls;

    public Diana_Try[] diana_trys;

    public Diana_Loop[] diana_loops;

    public Diana_For[] diana_fors;

    public Diana_With[] diana_withs;

    public Diana_GetAttr[] diana_getattrs;

    public Diana_SetAttr[] diana_setattrs;

    public Diana_SetAttrOp_add[] diana_setattrop_adds;

    public Diana_SetAttrOp_sub[] diana_setattrop_subs;

    public Diana_SetAttrOp_mul[] diana_setattrop_muls;

    public Diana_SetAttrOp_truediv[] diana_setattrop_truedivs;

    public Diana_SetAttrOp_floordiv[] diana_setattrop_floordivs;

    public Diana_SetAttrOp_mod[] diana_setattrop_mods;

    public Diana_SetAttrOp_pow[] diana_setattrop_pows;

    public Diana_SetAttrOp_lshift[] diana_setattrop_lshifts;

    public Diana_SetAttrOp_rshift[] diana_setattrop_rshifts;

    public Diana_SetAttrOp_bitor[] diana_setattrop_bitors;

    public Diana_SetAttrOp_bitand[] diana_setattrop_bitands;

    public Diana_SetAttrOp_bitxor[] diana_setattrop_bitxors;

    public Diana_MKDict[] diana_mkdicts;

    public Diana_MKSet[] diana_mksets;

    public Diana_MKList[] diana_mklists;

    public Diana_Call[] diana_calls;

    public Diana_Format[] diana_formats;

    public Diana_Const[] diana_consts;

    public Diana_MKTuple[] diana_mktuples;

    public Diana_Pack[] diana_packs;

}
}
