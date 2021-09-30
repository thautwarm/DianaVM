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
    public int[] targets;

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
public struct Diana_JumpIfNot
{
    public int off;

}
public struct Diana_JumpIf
{
    public int off;

}
public struct Diana_Jump
{
    public int off;

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
public struct Diana_SetAttr_Iadd
{
    public InternString attr;

}
public struct Diana_SetAttr_Isub
{
    public InternString attr;

}
public struct Diana_SetAttr_Imul
{
    public InternString attr;

}
public struct Diana_SetAttr_Itruediv
{
    public InternString attr;

}
public struct Diana_SetAttr_Ifloordiv
{
    public InternString attr;

}
public struct Diana_SetAttr_Imod
{
    public InternString attr;

}
public struct Diana_SetAttr_Ipow
{
    public InternString attr;

}
public struct Diana_SetAttr_Ilshift
{
    public InternString attr;

}
public struct Diana_SetAttr_Irshift
{
    public InternString attr;

}
public struct Diana_SetAttr_Ibitor
{
    public InternString attr;

}
public struct Diana_SetAttr_Ibitand
{
    public InternString attr;

}
public struct Diana_SetAttr_Ibitxor
{
    public InternString attr;

}
public struct Diana_SetAttr_Igt
{
    public InternString attr;

}
public struct Diana_SetAttr_Ilt
{
    public InternString attr;

}
public struct Diana_SetAttr_Ige
{
    public InternString attr;

}
public struct Diana_SetAttr_Ile
{
    public InternString attr;

}
public struct Diana_SetAttr_Ieq
{
    public InternString attr;

}
public struct Diana_SetAttr_Ineq
{
    public InternString attr;

}
public struct Diana_SetAttr_Iin
{
    public InternString attr;

}
public struct Diana_SetAttr_Inotin
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
public struct Diana_Replicate
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
    Diana_JumpIfNot,
    Diana_JumpIf,
    Diana_Jump,
    Diana_Control,
    Diana_Try,
    Diana_Loop,
    Diana_For,
    Diana_With,
    Diana_GetAttr,
    Diana_SetAttr,
    Diana_SetAttr_Iadd,
    Diana_SetAttr_Isub,
    Diana_SetAttr_Imul,
    Diana_SetAttr_Itruediv,
    Diana_SetAttr_Ifloordiv,
    Diana_SetAttr_Imod,
    Diana_SetAttr_Ipow,
    Diana_SetAttr_Ilshift,
    Diana_SetAttr_Irshift,
    Diana_SetAttr_Ibitor,
    Diana_SetAttr_Ibitand,
    Diana_SetAttr_Ibitxor,
    Diana_SetAttr_Igt,
    Diana_SetAttr_Ilt,
    Diana_SetAttr_Ige,
    Diana_SetAttr_Ile,
    Diana_SetAttr_Ieq,
    Diana_SetAttr_Ineq,
    Diana_SetAttr_Iin,
    Diana_SetAttr_Inotin,
    Diana_DelItem,
    Diana_GetItem,
    Diana_SetItem,
    Diana_SetItem_Iadd,
    Diana_SetItem_Isub,
    Diana_SetItem_Imul,
    Diana_SetItem_Itruediv,
    Diana_SetItem_Ifloordiv,
    Diana_SetItem_Imod,
    Diana_SetItem_Ipow,
    Diana_SetItem_Ilshift,
    Diana_SetItem_Irshift,
    Diana_SetItem_Ibitor,
    Diana_SetItem_Ibitand,
    Diana_SetItem_Ibitxor,
    Diana_SetItem_Igt,
    Diana_SetItem_Ilt,
    Diana_SetItem_Ige,
    Diana_SetItem_Ile,
    Diana_SetItem_Ieq,
    Diana_SetItem_Ineq,
    Diana_SetItem_Iin,
    Diana_SetItem_Inotin,
    Diana_add,
    Diana_sub,
    Diana_mul,
    Diana_truediv,
    Diana_floordiv,
    Diana_mod,
    Diana_pow,
    Diana_lshift,
    Diana_rshift,
    Diana_bitor,
    Diana_bitand,
    Diana_bitxor,
    Diana_gt,
    Diana_lt,
    Diana_ge,
    Diana_le,
    Diana_eq,
    Diana_neq,
    Diana_in,
    Diana_notin,
    Diana_UnaryOp_invert,
    Diana_UnaryOp_not,
    Diana_UnaryOp_neg,
    Diana_MKDict,
    Diana_MKSet,
    Diana_MKList,
    Diana_Call,
    Diana_Format,
    Diana_Const,
    Diana_MKTuple,
    Diana_Pack,
    Diana_Replicate,
    Diana_Pop,
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

    public Diana_JumpIfNot[] diana_jumpifnots;

    public Diana_JumpIf[] diana_jumpifs;

    public Diana_Jump[] diana_jumps;

    public Diana_Control[] diana_controls;

    public Diana_Try[] diana_trys;

    public Diana_Loop[] diana_loops;

    public Diana_For[] diana_fors;

    public Diana_With[] diana_withs;

    public Diana_GetAttr[] diana_getattrs;

    public Diana_SetAttr[] diana_setattrs;

    public Diana_SetAttr_Iadd[] diana_setattr_iadds;

    public Diana_SetAttr_Isub[] diana_setattr_isubs;

    public Diana_SetAttr_Imul[] diana_setattr_imuls;

    public Diana_SetAttr_Itruediv[] diana_setattr_itruedivs;

    public Diana_SetAttr_Ifloordiv[] diana_setattr_ifloordivs;

    public Diana_SetAttr_Imod[] diana_setattr_imods;

    public Diana_SetAttr_Ipow[] diana_setattr_ipows;

    public Diana_SetAttr_Ilshift[] diana_setattr_ilshifts;

    public Diana_SetAttr_Irshift[] diana_setattr_irshifts;

    public Diana_SetAttr_Ibitor[] diana_setattr_ibitors;

    public Diana_SetAttr_Ibitand[] diana_setattr_ibitands;

    public Diana_SetAttr_Ibitxor[] diana_setattr_ibitxors;

    public Diana_SetAttr_Igt[] diana_setattr_igts;

    public Diana_SetAttr_Ilt[] diana_setattr_ilts;

    public Diana_SetAttr_Ige[] diana_setattr_iges;

    public Diana_SetAttr_Ile[] diana_setattr_iles;

    public Diana_SetAttr_Ieq[] diana_setattr_ieqs;

    public Diana_SetAttr_Ineq[] diana_setattr_ineqs;

    public Diana_SetAttr_Iin[] diana_setattr_iins;

    public Diana_SetAttr_Inotin[] diana_setattr_inotins;

    public Diana_MKDict[] diana_mkdicts;

    public Diana_MKSet[] diana_mksets;

    public Diana_MKList[] diana_mklists;

    public Diana_Call[] diana_calls;

    public Diana_Format[] diana_formats;

    public Diana_Const[] diana_consts;

    public Diana_MKTuple[] diana_mktuples;

    public Diana_Pack[] diana_packs;

    public Diana_Replicate[] diana_replicates;

}
}
