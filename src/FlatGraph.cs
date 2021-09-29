using System;
using System.Collections.Generic;
namespace DianaScript
{
public struct Catch
{
    public int exc_target;
    public int exc_type;
    public int body;

    public static Catch Make(int exc_target, int exc_type, int body) => new Catch
    {
        exc_target = exc_target,

        exc_type = exc_type,

        body = body,

    };
}
public struct FuncMeta
{
    public bool is_vararg;
    public int[] freeslots;
    public int narg;
    public int nlocal;
    public InternString name;
    public string filename;
    public int lineno;
    public string[] freenames;
    public string[] localnames;

    public static FuncMeta Make(bool is_vararg, int[] freeslots, int narg, int nlocal, InternString name, string filename, int lineno, string[] freenames, string[] localnames) => new FuncMeta
    {
        is_vararg = is_vararg,

        freeslots = freeslots,

        narg = narg,

        nlocal = nlocal,

        name = name,

        filename = filename,

        lineno = lineno,

        freenames = freenames,

        localnames = localnames,

    };
}
public struct Loc
{
    public (int,int)[] location_data;

    public static Loc Make((int,int)[] location_data) => new Loc
    {
        location_data = location_data,

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
    public int target;
    public int metadataInd;
    public int code;

}
public struct Diana_Return
{
    public int reg;

}
public struct Diana_DelVar
{
    public int target;

}
public struct Diana_LoadAsCell
{
    public int target;

}
public struct Diana_LoadGlobalRef
{
    public int target;
    public int p_val;

}
public struct Diana_LoadVar
{
    public int target;
    public int p_val;

}
public struct Diana_Raise
{
    public int p_exc;

}
public struct Diana_Assert
{
    public int value;
    public int p_msg;

}
public struct Diana_Control
{
    public int token;

}
public struct Diana_Try
{
    public int body;
    public Catch[] except_handlers;
    public int final_body;

}
public struct Diana_While
{
    public int p_cond;
    public int body;

}
public struct Diana_For
{
    public int target;
    public int p_iter;
    public int body;

}
public struct Diana_With
{
    public int p_resource;
    public int p_as;
    public int body;

}
public struct Diana_DelItem
{
    public int p_value;
    public int p_item;

}
public struct Diana_GetItem
{
    public int target;
    public int p_value;
    public int p_item;

}
public struct Diana_BinaryOp_add
{
    public int target;
    public int left;
    public int right;

}
public struct Diana_BinaryOp_sub
{
    public int target;
    public int left;
    public int right;

}
public struct Diana_BinaryOp_mul
{
    public int target;
    public int left;
    public int right;

}
public struct Diana_BinaryOp_truediv
{
    public int target;
    public int left;
    public int right;

}
public struct Diana_BinaryOp_floordiv
{
    public int target;
    public int left;
    public int right;

}
public struct Diana_BinaryOp_mod
{
    public int target;
    public int left;
    public int right;

}
public struct Diana_BinaryOp_pow
{
    public int target;
    public int left;
    public int right;

}
public struct Diana_BinaryOp_lshift
{
    public int target;
    public int left;
    public int right;

}
public struct Diana_BinaryOp_rshift
{
    public int target;
    public int left;
    public int right;

}
public struct Diana_BinaryOp_bitor
{
    public int target;
    public int left;
    public int right;

}
public struct Diana_BinaryOp_bitand
{
    public int target;
    public int left;
    public int right;

}
public struct Diana_BinaryOp_bitxor
{
    public int target;
    public int left;
    public int right;

}
public struct Diana_UnaryOp_invert
{
    public int target;
    public int p_value;

}
public struct Diana_UnaryOp_not
{
    public int target;
    public int p_value;

}
public struct Diana_Dict
{
    public int target;
    public (int, int)[] p_kvs;

}
public struct Diana_Set
{
    public int target;
    public int[] p_elts;

}
public struct Diana_List
{
    public int target;
    public int[] p_elts;

}
public struct Diana_Call
{
    public int target;
    public int p_f;
    public int[] p_args;

}
public struct Diana_Format
{
    public int target;
    public int format;
    public int[] args;

}
public struct Diana_Const
{
    public int target;
    public int p_const;

}
public struct Diana_GetAttr
{
    public int target;
    public int p_value;
    public int p_attr;

}
public struct Diana_MoveVar
{
    public int target;
    public int slot;

}
public struct Diana_Tuple
{
    public int target;
    public int[] p_elts;

}
public struct Diana_PackTuple
{
    public int[] targets;
    public int p_value;

}
public enum CODE
{
    Diana_FunctionDef,
    Diana_Return,
    Diana_DelVar,
    Diana_LoadAsCell,
    Diana_LoadGlobalRef,
    Diana_LoadVar,
    Diana_Raise,
    Diana_Assert,
    Diana_Control,
    Diana_Try,
    Diana_While,
    Diana_For,
    Diana_With,
    Diana_DelItem,
    Diana_GetItem,
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
    Diana_Dict,
    Diana_Set,
    Diana_List,
    Diana_Call,
    Diana_Format,
    Diana_Const,
    Diana_GetAttr,
    Diana_MoveVar,
    Diana_Tuple,
    Diana_PackTuple,
}

public partial class DFlatGraphCode
{
    public string[] strings;

    public DObj[] dobjs;

    public InternString[] internstrings;

    public Catch[] catchs;

    public FuncMeta[] funcmetas;

    public Loc[] locs;

    public Block[] blocks;

    public Diana_FunctionDef[] diana_functiondefs;

    public Diana_Return[] diana_returns;

    public Diana_DelVar[] diana_delvars;

    public Diana_LoadAsCell[] diana_loadascells;

    public Diana_LoadGlobalRef[] diana_loadglobalrefs;

    public Diana_LoadVar[] diana_loadvars;

    public Diana_Raise[] diana_raises;

    public Diana_Assert[] diana_asserts;

    public Diana_Control[] diana_controls;

    public Diana_Try[] diana_trys;

    public Diana_While[] diana_whiles;

    public Diana_For[] diana_fors;

    public Diana_With[] diana_withs;

    public Diana_DelItem[] diana_delitems;

    public Diana_GetItem[] diana_getitems;

    public Diana_BinaryOp_add[] diana_binaryop_adds;

    public Diana_BinaryOp_sub[] diana_binaryop_subs;

    public Diana_BinaryOp_mul[] diana_binaryop_muls;

    public Diana_BinaryOp_truediv[] diana_binaryop_truedivs;

    public Diana_BinaryOp_floordiv[] diana_binaryop_floordivs;

    public Diana_BinaryOp_mod[] diana_binaryop_mods;

    public Diana_BinaryOp_pow[] diana_binaryop_pows;

    public Diana_BinaryOp_lshift[] diana_binaryop_lshifts;

    public Diana_BinaryOp_rshift[] diana_binaryop_rshifts;

    public Diana_BinaryOp_bitor[] diana_binaryop_bitors;

    public Diana_BinaryOp_bitand[] diana_binaryop_bitands;

    public Diana_BinaryOp_bitxor[] diana_binaryop_bitxors;

    public Diana_UnaryOp_invert[] diana_unaryop_inverts;

    public Diana_UnaryOp_not[] diana_unaryop_nots;

    public Diana_Dict[] diana_dicts;

    public Diana_Set[] diana_sets;

    public Diana_List[] diana_lists;

    public Diana_Call[] diana_calls;

    public Diana_Format[] diana_formats;

    public Diana_Const[] diana_consts;

    public Diana_GetAttr[] diana_getattrs;

    public Diana_MoveVar[] diana_movevars;

    public Diana_Tuple[] diana_tuples;

    public Diana_PackTuple[] diana_packtuples;

}
}
