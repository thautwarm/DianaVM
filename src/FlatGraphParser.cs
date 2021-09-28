using System;
using System.IO;
using System.Collections.Generic;
namespace DianaScript
{
public partial class AIRParser
{
    public CODE ReadCODE()
    {
        fileStream.Read(cache_4byte, 0, 1);
        return (CODE)cache_4byte[0];
    }

    public Ptr Read(THint<Ptr> _) => new Ptr(ReadCODE(), ReadInt());
    public Catch Read(THint<Catch> _) => new Catch
    {
        exc_target = Read(THint<int>.val),
        exc_type = Read(THint<int>.val),
        body = Read(THint<int>.val),
    };

    public FuncMeta Read(THint<FuncMeta> _) => new FuncMeta
    {
        is_vararg = Read(THint<bool>.val),
        freeslots = Read(THint<int[]>.val),
        narg = Read(THint<int>.val),
        nlocal = Read(THint<int>.val),
        name = Read(THint<InternString>.val),
        modname = Read(THint<InternString>.val),
        filename = Read(THint<string>.val),
        lineno = Read(THint<int>.val),
        freenames = Read(THint<string[]>.val),
        localnames = Read(THint<string[]>.val),
    };

    public Loc Read(THint<Loc> _) => new Loc
    {
        location_data = Read(THint<(int,int)[]>.val),
    };

    public Block Read(THint<Block> _) => new Block
    {
        codes = Read(THint<Ptr[]>.val),
        location_data = Read(THint<(int,int)[]>.val),
        filename = Read(THint<string>.val),
    };

    public Diana_FunctionDef Read(THint<Diana_FunctionDef> _) => new Diana_FunctionDef
    {
        target = Read(THint<int>.val),
        metadataInd = Read(THint<int>.val),
        code = Read(THint<int>.val),
    };

    public Diana_Return Read(THint<Diana_Return> _) => new Diana_Return
    {
        reg = Read(THint<int>.val),
    };

    public Diana_DelVar Read(THint<Diana_DelVar> _) => new Diana_DelVar
    {
        target = Read(THint<int>.val),
    };

    public Diana_LoadVar Read(THint<Diana_LoadVar> _) => new Diana_LoadVar
    {
        target = Read(THint<int>.val),
        p_val = Read(THint<int>.val),
    };

    public Diana_JumpIf Read(THint<Diana_JumpIf> _) => new Diana_JumpIf
    {
        p_val = Read(THint<int>.val),
        offset = Read(THint<int>.val),
    };

    public Diana_Jump Read(THint<Diana_Jump> _) => new Diana_Jump
    {
        offset = Read(THint<int>.val),
    };

    public Diana_Raise Read(THint<Diana_Raise> _) => new Diana_Raise
    {
        p_exc = Read(THint<int>.val),
    };

    public Diana_Assert Read(THint<Diana_Assert> _) => new Diana_Assert
    {
        value = Read(THint<int>.val),
        p_msg = Read(THint<int>.val),
    };

    public Diana_Control Read(THint<Diana_Control> _) => new Diana_Control
    {
        token = Read(THint<int>.val),
    };

    public Diana_Try Read(THint<Diana_Try> _) => new Diana_Try
    {
        body = Read(THint<int>.val),
        except_handlers = Read(THint<Catch[]>.val),
        final_body = Read(THint<int>.val),
    };

    public Diana_For Read(THint<Diana_For> _) => new Diana_For
    {
        target = Read(THint<int>.val),
        p_iter = Read(THint<int>.val),
        body = Read(THint<int>.val),
    };

    public Diana_With Read(THint<Diana_With> _) => new Diana_With
    {
        p_resource = Read(THint<int>.val),
        p_as = Read(THint<int>.val),
        body = Read(THint<int>.val),
    };

    public Diana_DelItem Read(THint<Diana_DelItem> _) => new Diana_DelItem
    {
        p_value = Read(THint<int>.val),
        p_item = Read(THint<int>.val),
    };

    public Diana_GetItem Read(THint<Diana_GetItem> _) => new Diana_GetItem
    {
        target = Read(THint<int>.val),
        p_value = Read(THint<int>.val),
        p_item = Read(THint<int>.val),
    };

    public Diana_BinaryOp_add Read(THint<Diana_BinaryOp_add> _) => new Diana_BinaryOp_add
    {
        target = Read(THint<int>.val),
        left = Read(THint<int>.val),
        right = Read(THint<int>.val),
    };

    public Diana_BinaryOp_sub Read(THint<Diana_BinaryOp_sub> _) => new Diana_BinaryOp_sub
    {
        target = Read(THint<int>.val),
        left = Read(THint<int>.val),
        right = Read(THint<int>.val),
    };

    public Diana_BinaryOp_mul Read(THint<Diana_BinaryOp_mul> _) => new Diana_BinaryOp_mul
    {
        target = Read(THint<int>.val),
        left = Read(THint<int>.val),
        right = Read(THint<int>.val),
    };

    public Diana_BinaryOp_truediv Read(THint<Diana_BinaryOp_truediv> _) => new Diana_BinaryOp_truediv
    {
        target = Read(THint<int>.val),
        left = Read(THint<int>.val),
        right = Read(THint<int>.val),
    };

    public Diana_BinaryOp_floordiv Read(THint<Diana_BinaryOp_floordiv> _) => new Diana_BinaryOp_floordiv
    {
        target = Read(THint<int>.val),
        left = Read(THint<int>.val),
        right = Read(THint<int>.val),
    };

    public Diana_BinaryOp_mod Read(THint<Diana_BinaryOp_mod> _) => new Diana_BinaryOp_mod
    {
        target = Read(THint<int>.val),
        left = Read(THint<int>.val),
        right = Read(THint<int>.val),
    };

    public Diana_BinaryOp_pow Read(THint<Diana_BinaryOp_pow> _) => new Diana_BinaryOp_pow
    {
        target = Read(THint<int>.val),
        left = Read(THint<int>.val),
        right = Read(THint<int>.val),
    };

    public Diana_BinaryOp_lshift Read(THint<Diana_BinaryOp_lshift> _) => new Diana_BinaryOp_lshift
    {
        target = Read(THint<int>.val),
        left = Read(THint<int>.val),
        right = Read(THint<int>.val),
    };

    public Diana_BinaryOp_rshift Read(THint<Diana_BinaryOp_rshift> _) => new Diana_BinaryOp_rshift
    {
        target = Read(THint<int>.val),
        left = Read(THint<int>.val),
        right = Read(THint<int>.val),
    };

    public Diana_BinaryOp_bitor Read(THint<Diana_BinaryOp_bitor> _) => new Diana_BinaryOp_bitor
    {
        target = Read(THint<int>.val),
        left = Read(THint<int>.val),
        right = Read(THint<int>.val),
    };

    public Diana_BinaryOp_bitand Read(THint<Diana_BinaryOp_bitand> _) => new Diana_BinaryOp_bitand
    {
        target = Read(THint<int>.val),
        left = Read(THint<int>.val),
        right = Read(THint<int>.val),
    };

    public Diana_BinaryOp_bitxor Read(THint<Diana_BinaryOp_bitxor> _) => new Diana_BinaryOp_bitxor
    {
        target = Read(THint<int>.val),
        left = Read(THint<int>.val),
        right = Read(THint<int>.val),
    };

    public Diana_UnaryOp_invert Read(THint<Diana_UnaryOp_invert> _) => new Diana_UnaryOp_invert
    {
        target = Read(THint<int>.val),
        p_value = Read(THint<int>.val),
    };

    public Diana_UnaryOp_not Read(THint<Diana_UnaryOp_not> _) => new Diana_UnaryOp_not
    {
        target = Read(THint<int>.val),
        p_value = Read(THint<int>.val),
    };

    public Diana_Dict Read(THint<Diana_Dict> _) => new Diana_Dict
    {
        target = Read(THint<int>.val),
        p_kvs = Read(THint<(int, int)[]>.val),
    };

    public Diana_Set Read(THint<Diana_Set> _) => new Diana_Set
    {
        target = Read(THint<int>.val),
        p_elts = Read(THint<int[]>.val),
    };

    public Diana_List Read(THint<Diana_List> _) => new Diana_List
    {
        target = Read(THint<int>.val),
        p_elts = Read(THint<int[]>.val),
    };

    public Diana_Call Read(THint<Diana_Call> _) => new Diana_Call
    {
        target = Read(THint<int>.val),
        p_f = Read(THint<int>.val),
        p_args = Read(THint<int[]>.val),
    };

    public Diana_Format Read(THint<Diana_Format> _) => new Diana_Format
    {
        target = Read(THint<int>.val),
        format = Read(THint<int>.val),
        args = Read(THint<int[]>.val),
    };

    public Diana_Const Read(THint<Diana_Const> _) => new Diana_Const
    {
        target = Read(THint<int>.val),
        p_const = Read(THint<int>.val),
    };

    public Diana_GetAttr Read(THint<Diana_GetAttr> _) => new Diana_GetAttr
    {
        target = Read(THint<int>.val),
        p_value = Read(THint<int>.val),
        p_attr = Read(THint<int>.val),
    };

    public Diana_MoveVar Read(THint<Diana_MoveVar> _) => new Diana_MoveVar
    {
        target = Read(THint<int>.val),
        slot = Read(THint<int>.val),
    };

    public Diana_Tuple Read(THint<Diana_Tuple> _) => new Diana_Tuple
    {
        target = Read(THint<int>.val),
        p_elts = Read(THint<int[]>.val),
    };

    public Diana_PackTuple Read(THint<Diana_PackTuple> _) => new Diana_PackTuple
    {
        targets = Read(THint<int[]>.val),
        p_value = Read(THint<int>.val),
    };

    public DFlatGraphCode Read(THint<DFlatGraphCode> _) => new DFlatGraphCode
    {
        strings = Read(THint<string[]>.val),
        dobjs = Read(THint<DObj[]>.val),
        internstrings = Read(THint<InternString[]>.val),
        catchs = Read(THint<Catch[]>.val),
        funcmetas = Read(THint<FuncMeta[]>.val),
        locs = Read(THint<Loc[]>.val),
        blocks = Read(THint<Block[]>.val),
        diana_functiondefs = Read(THint<Diana_FunctionDef[]>.val),
        diana_returns = Read(THint<Diana_Return[]>.val),
        diana_delvars = Read(THint<Diana_DelVar[]>.val),
        diana_loadvars = Read(THint<Diana_LoadVar[]>.val),
        diana_jumpifs = Read(THint<Diana_JumpIf[]>.val),
        diana_jumps = Read(THint<Diana_Jump[]>.val),
        diana_raises = Read(THint<Diana_Raise[]>.val),
        diana_asserts = Read(THint<Diana_Assert[]>.val),
        diana_controls = Read(THint<Diana_Control[]>.val),
        diana_trys = Read(THint<Diana_Try[]>.val),
        diana_fors = Read(THint<Diana_For[]>.val),
        diana_withs = Read(THint<Diana_With[]>.val),
        diana_delitems = Read(THint<Diana_DelItem[]>.val),
        diana_getitems = Read(THint<Diana_GetItem[]>.val),
        diana_binaryop_adds = Read(THint<Diana_BinaryOp_add[]>.val),
        diana_binaryop_subs = Read(THint<Diana_BinaryOp_sub[]>.val),
        diana_binaryop_muls = Read(THint<Diana_BinaryOp_mul[]>.val),
        diana_binaryop_truedivs = Read(THint<Diana_BinaryOp_truediv[]>.val),
        diana_binaryop_floordivs = Read(THint<Diana_BinaryOp_floordiv[]>.val),
        diana_binaryop_mods = Read(THint<Diana_BinaryOp_mod[]>.val),
        diana_binaryop_pows = Read(THint<Diana_BinaryOp_pow[]>.val),
        diana_binaryop_lshifts = Read(THint<Diana_BinaryOp_lshift[]>.val),
        diana_binaryop_rshifts = Read(THint<Diana_BinaryOp_rshift[]>.val),
        diana_binaryop_bitors = Read(THint<Diana_BinaryOp_bitor[]>.val),
        diana_binaryop_bitands = Read(THint<Diana_BinaryOp_bitand[]>.val),
        diana_binaryop_bitxors = Read(THint<Diana_BinaryOp_bitxor[]>.val),
        diana_unaryop_inverts = Read(THint<Diana_UnaryOp_invert[]>.val),
        diana_unaryop_nots = Read(THint<Diana_UnaryOp_not[]>.val),
        diana_dicts = Read(THint<Diana_Dict[]>.val),
        diana_sets = Read(THint<Diana_Set[]>.val),
        diana_lists = Read(THint<Diana_List[]>.val),
        diana_calls = Read(THint<Diana_Call[]>.val),
        diana_formats = Read(THint<Diana_Format[]>.val),
        diana_consts = Read(THint<Diana_Const[]>.val),
        diana_getattrs = Read(THint<Diana_GetAttr[]>.val),
        diana_movevars = Read(THint<Diana_MoveVar[]>.val),
        diana_tuples = Read(THint<Diana_Tuple[]>.val),
        diana_packtuples = Read(THint<Diana_PackTuple[]>.val),
    };

    public static readonly THint<int> int_hint = THint<int>.val;
    public int[] Read(THint<int[]> _)
    {
        int[] src = new int[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(int_hint);
        }
        return src;
    }
    public static readonly THint<(int, int)> _____int____int___hint = THint<(int, int)>.val;
    public (int, int)[] Read(THint<(int, int)[]> _)
    {
        (int, int)[] src = new (int, int)[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(_____int____int___hint);
        }
        return src;
    }
    public static readonly THint<float> float_hint = THint<float>.val;
    public float[] Read(THint<float[]> _)
    {
        float[] src = new float[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(float_hint);
        }
        return src;
    }
    public static readonly THint<bool> bool_hint = THint<bool>.val;
    public bool[] Read(THint<bool[]> _)
    {
        bool[] src = new bool[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(bool_hint);
        }
        return src;
    }
    public static readonly THint<string> string_hint = THint<string>.val;
    public string[] Read(THint<string[]> _)
    {
        string[] src = new string[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(string_hint);
        }
        return src;
    }
    public static readonly THint<DObj> DObj_hint = THint<DObj>.val;
    public DObj[] Read(THint<DObj[]> _)
    {
        DObj[] src = new DObj[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(DObj_hint);
        }
        return src;
    }
    public static readonly THint<InternString> InternString_hint = THint<InternString>.val;
    public InternString[] Read(THint<InternString[]> _)
    {
        InternString[] src = new InternString[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(InternString_hint);
        }
        return src;
    }
    public static readonly THint<Catch> Catch_hint = THint<Catch>.val;
    public Catch[] Read(THint<Catch[]> _)
    {
        Catch[] src = new Catch[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Catch_hint);
        }
        return src;
    }
    public static readonly THint<FuncMeta> FuncMeta_hint = THint<FuncMeta>.val;
    public FuncMeta[] Read(THint<FuncMeta[]> _)
    {
        FuncMeta[] src = new FuncMeta[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(FuncMeta_hint);
        }
        return src;
    }
    public static readonly THint<Loc> Loc_hint = THint<Loc>.val;
    public Loc[] Read(THint<Loc[]> _)
    {
        Loc[] src = new Loc[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Loc_hint);
        }
        return src;
    }
    public static readonly THint<Block> Block_hint = THint<Block>.val;
    public Block[] Read(THint<Block[]> _)
    {
        Block[] src = new Block[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Block_hint);
        }
        return src;
    }
    public static readonly THint<Diana_FunctionDef> Diana_FunctionDef_hint = THint<Diana_FunctionDef>.val;
    public Diana_FunctionDef[] Read(THint<Diana_FunctionDef[]> _)
    {
        Diana_FunctionDef[] src = new Diana_FunctionDef[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_FunctionDef_hint);
        }
        return src;
    }
    public static readonly THint<Diana_Return> Diana_Return_hint = THint<Diana_Return>.val;
    public Diana_Return[] Read(THint<Diana_Return[]> _)
    {
        Diana_Return[] src = new Diana_Return[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_Return_hint);
        }
        return src;
    }
    public static readonly THint<Diana_DelVar> Diana_DelVar_hint = THint<Diana_DelVar>.val;
    public Diana_DelVar[] Read(THint<Diana_DelVar[]> _)
    {
        Diana_DelVar[] src = new Diana_DelVar[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_DelVar_hint);
        }
        return src;
    }
    public static readonly THint<Diana_LoadVar> Diana_LoadVar_hint = THint<Diana_LoadVar>.val;
    public Diana_LoadVar[] Read(THint<Diana_LoadVar[]> _)
    {
        Diana_LoadVar[] src = new Diana_LoadVar[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_LoadVar_hint);
        }
        return src;
    }
    public static readonly THint<Diana_JumpIf> Diana_JumpIf_hint = THint<Diana_JumpIf>.val;
    public Diana_JumpIf[] Read(THint<Diana_JumpIf[]> _)
    {
        Diana_JumpIf[] src = new Diana_JumpIf[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_JumpIf_hint);
        }
        return src;
    }
    public static readonly THint<Diana_Jump> Diana_Jump_hint = THint<Diana_Jump>.val;
    public Diana_Jump[] Read(THint<Diana_Jump[]> _)
    {
        Diana_Jump[] src = new Diana_Jump[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_Jump_hint);
        }
        return src;
    }
    public static readonly THint<Diana_Raise> Diana_Raise_hint = THint<Diana_Raise>.val;
    public Diana_Raise[] Read(THint<Diana_Raise[]> _)
    {
        Diana_Raise[] src = new Diana_Raise[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_Raise_hint);
        }
        return src;
    }
    public static readonly THint<Diana_Assert> Diana_Assert_hint = THint<Diana_Assert>.val;
    public Diana_Assert[] Read(THint<Diana_Assert[]> _)
    {
        Diana_Assert[] src = new Diana_Assert[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_Assert_hint);
        }
        return src;
    }
    public static readonly THint<Diana_Control> Diana_Control_hint = THint<Diana_Control>.val;
    public Diana_Control[] Read(THint<Diana_Control[]> _)
    {
        Diana_Control[] src = new Diana_Control[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_Control_hint);
        }
        return src;
    }
    public static readonly THint<Diana_Try> Diana_Try_hint = THint<Diana_Try>.val;
    public Diana_Try[] Read(THint<Diana_Try[]> _)
    {
        Diana_Try[] src = new Diana_Try[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_Try_hint);
        }
        return src;
    }
    public static readonly THint<Diana_For> Diana_For_hint = THint<Diana_For>.val;
    public Diana_For[] Read(THint<Diana_For[]> _)
    {
        Diana_For[] src = new Diana_For[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_For_hint);
        }
        return src;
    }
    public static readonly THint<Diana_With> Diana_With_hint = THint<Diana_With>.val;
    public Diana_With[] Read(THint<Diana_With[]> _)
    {
        Diana_With[] src = new Diana_With[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_With_hint);
        }
        return src;
    }
    public static readonly THint<Diana_DelItem> Diana_DelItem_hint = THint<Diana_DelItem>.val;
    public Diana_DelItem[] Read(THint<Diana_DelItem[]> _)
    {
        Diana_DelItem[] src = new Diana_DelItem[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_DelItem_hint);
        }
        return src;
    }
    public static readonly THint<Diana_GetItem> Diana_GetItem_hint = THint<Diana_GetItem>.val;
    public Diana_GetItem[] Read(THint<Diana_GetItem[]> _)
    {
        Diana_GetItem[] src = new Diana_GetItem[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_GetItem_hint);
        }
        return src;
    }
    public static readonly THint<Diana_BinaryOp_add> Diana_BinaryOp_add_hint = THint<Diana_BinaryOp_add>.val;
    public Diana_BinaryOp_add[] Read(THint<Diana_BinaryOp_add[]> _)
    {
        Diana_BinaryOp_add[] src = new Diana_BinaryOp_add[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_BinaryOp_add_hint);
        }
        return src;
    }
    public static readonly THint<Diana_BinaryOp_sub> Diana_BinaryOp_sub_hint = THint<Diana_BinaryOp_sub>.val;
    public Diana_BinaryOp_sub[] Read(THint<Diana_BinaryOp_sub[]> _)
    {
        Diana_BinaryOp_sub[] src = new Diana_BinaryOp_sub[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_BinaryOp_sub_hint);
        }
        return src;
    }
    public static readonly THint<Diana_BinaryOp_mul> Diana_BinaryOp_mul_hint = THint<Diana_BinaryOp_mul>.val;
    public Diana_BinaryOp_mul[] Read(THint<Diana_BinaryOp_mul[]> _)
    {
        Diana_BinaryOp_mul[] src = new Diana_BinaryOp_mul[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_BinaryOp_mul_hint);
        }
        return src;
    }
    public static readonly THint<Diana_BinaryOp_truediv> Diana_BinaryOp_truediv_hint = THint<Diana_BinaryOp_truediv>.val;
    public Diana_BinaryOp_truediv[] Read(THint<Diana_BinaryOp_truediv[]> _)
    {
        Diana_BinaryOp_truediv[] src = new Diana_BinaryOp_truediv[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_BinaryOp_truediv_hint);
        }
        return src;
    }
    public static readonly THint<Diana_BinaryOp_floordiv> Diana_BinaryOp_floordiv_hint = THint<Diana_BinaryOp_floordiv>.val;
    public Diana_BinaryOp_floordiv[] Read(THint<Diana_BinaryOp_floordiv[]> _)
    {
        Diana_BinaryOp_floordiv[] src = new Diana_BinaryOp_floordiv[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_BinaryOp_floordiv_hint);
        }
        return src;
    }
    public static readonly THint<Diana_BinaryOp_mod> Diana_BinaryOp_mod_hint = THint<Diana_BinaryOp_mod>.val;
    public Diana_BinaryOp_mod[] Read(THint<Diana_BinaryOp_mod[]> _)
    {
        Diana_BinaryOp_mod[] src = new Diana_BinaryOp_mod[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_BinaryOp_mod_hint);
        }
        return src;
    }
    public static readonly THint<Diana_BinaryOp_pow> Diana_BinaryOp_pow_hint = THint<Diana_BinaryOp_pow>.val;
    public Diana_BinaryOp_pow[] Read(THint<Diana_BinaryOp_pow[]> _)
    {
        Diana_BinaryOp_pow[] src = new Diana_BinaryOp_pow[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_BinaryOp_pow_hint);
        }
        return src;
    }
    public static readonly THint<Diana_BinaryOp_lshift> Diana_BinaryOp_lshift_hint = THint<Diana_BinaryOp_lshift>.val;
    public Diana_BinaryOp_lshift[] Read(THint<Diana_BinaryOp_lshift[]> _)
    {
        Diana_BinaryOp_lshift[] src = new Diana_BinaryOp_lshift[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_BinaryOp_lshift_hint);
        }
        return src;
    }
    public static readonly THint<Diana_BinaryOp_rshift> Diana_BinaryOp_rshift_hint = THint<Diana_BinaryOp_rshift>.val;
    public Diana_BinaryOp_rshift[] Read(THint<Diana_BinaryOp_rshift[]> _)
    {
        Diana_BinaryOp_rshift[] src = new Diana_BinaryOp_rshift[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_BinaryOp_rshift_hint);
        }
        return src;
    }
    public static readonly THint<Diana_BinaryOp_bitor> Diana_BinaryOp_bitor_hint = THint<Diana_BinaryOp_bitor>.val;
    public Diana_BinaryOp_bitor[] Read(THint<Diana_BinaryOp_bitor[]> _)
    {
        Diana_BinaryOp_bitor[] src = new Diana_BinaryOp_bitor[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_BinaryOp_bitor_hint);
        }
        return src;
    }
    public static readonly THint<Diana_BinaryOp_bitand> Diana_BinaryOp_bitand_hint = THint<Diana_BinaryOp_bitand>.val;
    public Diana_BinaryOp_bitand[] Read(THint<Diana_BinaryOp_bitand[]> _)
    {
        Diana_BinaryOp_bitand[] src = new Diana_BinaryOp_bitand[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_BinaryOp_bitand_hint);
        }
        return src;
    }
    public static readonly THint<Diana_BinaryOp_bitxor> Diana_BinaryOp_bitxor_hint = THint<Diana_BinaryOp_bitxor>.val;
    public Diana_BinaryOp_bitxor[] Read(THint<Diana_BinaryOp_bitxor[]> _)
    {
        Diana_BinaryOp_bitxor[] src = new Diana_BinaryOp_bitxor[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_BinaryOp_bitxor_hint);
        }
        return src;
    }
    public static readonly THint<Diana_UnaryOp_invert> Diana_UnaryOp_invert_hint = THint<Diana_UnaryOp_invert>.val;
    public Diana_UnaryOp_invert[] Read(THint<Diana_UnaryOp_invert[]> _)
    {
        Diana_UnaryOp_invert[] src = new Diana_UnaryOp_invert[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_UnaryOp_invert_hint);
        }
        return src;
    }
    public static readonly THint<Diana_UnaryOp_not> Diana_UnaryOp_not_hint = THint<Diana_UnaryOp_not>.val;
    public Diana_UnaryOp_not[] Read(THint<Diana_UnaryOp_not[]> _)
    {
        Diana_UnaryOp_not[] src = new Diana_UnaryOp_not[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_UnaryOp_not_hint);
        }
        return src;
    }
    public static readonly THint<Diana_Dict> Diana_Dict_hint = THint<Diana_Dict>.val;
    public Diana_Dict[] Read(THint<Diana_Dict[]> _)
    {
        Diana_Dict[] src = new Diana_Dict[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_Dict_hint);
        }
        return src;
    }
    public static readonly THint<Diana_Set> Diana_Set_hint = THint<Diana_Set>.val;
    public Diana_Set[] Read(THint<Diana_Set[]> _)
    {
        Diana_Set[] src = new Diana_Set[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_Set_hint);
        }
        return src;
    }
    public static readonly THint<Diana_List> Diana_List_hint = THint<Diana_List>.val;
    public Diana_List[] Read(THint<Diana_List[]> _)
    {
        Diana_List[] src = new Diana_List[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_List_hint);
        }
        return src;
    }
    public static readonly THint<Diana_Call> Diana_Call_hint = THint<Diana_Call>.val;
    public Diana_Call[] Read(THint<Diana_Call[]> _)
    {
        Diana_Call[] src = new Diana_Call[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_Call_hint);
        }
        return src;
    }
    public static readonly THint<Diana_Format> Diana_Format_hint = THint<Diana_Format>.val;
    public Diana_Format[] Read(THint<Diana_Format[]> _)
    {
        Diana_Format[] src = new Diana_Format[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_Format_hint);
        }
        return src;
    }
    public static readonly THint<Diana_Const> Diana_Const_hint = THint<Diana_Const>.val;
    public Diana_Const[] Read(THint<Diana_Const[]> _)
    {
        Diana_Const[] src = new Diana_Const[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_Const_hint);
        }
        return src;
    }
    public static readonly THint<Diana_GetAttr> Diana_GetAttr_hint = THint<Diana_GetAttr>.val;
    public Diana_GetAttr[] Read(THint<Diana_GetAttr[]> _)
    {
        Diana_GetAttr[] src = new Diana_GetAttr[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_GetAttr_hint);
        }
        return src;
    }
    public static readonly THint<Diana_MoveVar> Diana_MoveVar_hint = THint<Diana_MoveVar>.val;
    public Diana_MoveVar[] Read(THint<Diana_MoveVar[]> _)
    {
        Diana_MoveVar[] src = new Diana_MoveVar[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_MoveVar_hint);
        }
        return src;
    }
    public static readonly THint<Diana_Tuple> Diana_Tuple_hint = THint<Diana_Tuple>.val;
    public Diana_Tuple[] Read(THint<Diana_Tuple[]> _)
    {
        Diana_Tuple[] src = new Diana_Tuple[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_Tuple_hint);
        }
        return src;
    }
    public static readonly THint<Diana_PackTuple> Diana_PackTuple_hint = THint<Diana_PackTuple>.val;
    public Diana_PackTuple[] Read(THint<Diana_PackTuple[]> _)
    {
        Diana_PackTuple[] src = new Diana_PackTuple[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_PackTuple_hint);
        }
        return src;
    }
    public static readonly THint<Ptr> Ptr_hint = THint<Ptr>.val;
    public Ptr[] Read(THint<Ptr[]> _)
    {
        Ptr[] src = new Ptr[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Ptr_hint);
        }
        return src;
    }
}
}
