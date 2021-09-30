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
        exc_type = Read(THint<int>.val),
        body = Read(THint<int>.val),
    };

    public FuncMeta Read(THint<FuncMeta> _) => new FuncMeta
    {
        is_vararg = Read(THint<bool>.val),
        freeslots = Read(THint<int[]>.val),
        nonargcells = Read(THint<int[]>.val),
        narg = Read(THint<int>.val),
        nlocal = Read(THint<int>.val),
        name = Read(THint<InternString>.val),
        filename = Read(THint<string>.val),
        lineno = Read(THint<int>.val),
        freenames = Read(THint<string[]>.val),
        localnames = Read(THint<string[]>.val),
    };

    public Block Read(THint<Block> _) => new Block
    {
        codes = Read(THint<Ptr[]>.val),
        location_data = Read(THint<(int,int)[]>.val),
        filename = Read(THint<string>.val),
    };

    public Diana_FunctionDef Read(THint<Diana_FunctionDef> _) => new Diana_FunctionDef
    {
        metadataInd = Read(THint<int>.val),
        code = Read(THint<int>.val),
    };

    public Diana_LoadGlobalRef Read(THint<Diana_LoadGlobalRef> _) => new Diana_LoadGlobalRef
    {
        istr = Read(THint<InternString>.val),
    };

    public Diana_DelVar Read(THint<Diana_DelVar> _) => new Diana_DelVar
    {
        target = Read(THint<int>.val),
    };

    public Diana_LoadVar Read(THint<Diana_LoadVar> _) => new Diana_LoadVar
    {
        i = Read(THint<int>.val),
    };

    public Diana_StoreVar Read(THint<Diana_StoreVar> _) => new Diana_StoreVar
    {
        i = Read(THint<int>.val),
    };

    public Diana_Action Read(THint<Diana_Action> _) => new Diana_Action
    {
        kind = Read(THint<int>.val),
    };

    public Diana_ControlIf Read(THint<Diana_ControlIf> _) => new Diana_ControlIf
    {
        arg = Read(THint<int>.val),
    };

    public Diana_Control Read(THint<Diana_Control> _) => new Diana_Control
    {
        arg = Read(THint<int>.val),
    };

    public Diana_Try Read(THint<Diana_Try> _) => new Diana_Try
    {
        body = Read(THint<int>.val),
        except_handlers = Read(THint<Catch[]>.val),
        final_body = Read(THint<int>.val),
    };

    public Diana_Loop Read(THint<Diana_Loop> _) => new Diana_Loop
    {
        body = Read(THint<int>.val),
    };

    public Diana_For Read(THint<Diana_For> _) => new Diana_For
    {
        body = Read(THint<int>.val),
    };

    public Diana_With Read(THint<Diana_With> _) => new Diana_With
    {
        body = Read(THint<int>.val),
    };

    public Diana_GetAttr Read(THint<Diana_GetAttr> _) => new Diana_GetAttr
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttr Read(THint<Diana_SetAttr> _) => new Diana_SetAttr
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttrOp_add Read(THint<Diana_SetAttrOp_add> _) => new Diana_SetAttrOp_add
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttrOp_sub Read(THint<Diana_SetAttrOp_sub> _) => new Diana_SetAttrOp_sub
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttrOp_mul Read(THint<Diana_SetAttrOp_mul> _) => new Diana_SetAttrOp_mul
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttrOp_truediv Read(THint<Diana_SetAttrOp_truediv> _) => new Diana_SetAttrOp_truediv
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttrOp_floordiv Read(THint<Diana_SetAttrOp_floordiv> _) => new Diana_SetAttrOp_floordiv
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttrOp_mod Read(THint<Diana_SetAttrOp_mod> _) => new Diana_SetAttrOp_mod
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttrOp_pow Read(THint<Diana_SetAttrOp_pow> _) => new Diana_SetAttrOp_pow
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttrOp_lshift Read(THint<Diana_SetAttrOp_lshift> _) => new Diana_SetAttrOp_lshift
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttrOp_rshift Read(THint<Diana_SetAttrOp_rshift> _) => new Diana_SetAttrOp_rshift
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttrOp_bitor Read(THint<Diana_SetAttrOp_bitor> _) => new Diana_SetAttrOp_bitor
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttrOp_bitand Read(THint<Diana_SetAttrOp_bitand> _) => new Diana_SetAttrOp_bitand
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttrOp_bitxor Read(THint<Diana_SetAttrOp_bitxor> _) => new Diana_SetAttrOp_bitxor
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_MKDict Read(THint<Diana_MKDict> _) => new Diana_MKDict
    {
        n = Read(THint<int>.val),
    };

    public Diana_MKSet Read(THint<Diana_MKSet> _) => new Diana_MKSet
    {
        n = Read(THint<int>.val),
    };

    public Diana_MKList Read(THint<Diana_MKList> _) => new Diana_MKList
    {
        n = Read(THint<int>.val),
    };

    public Diana_Call Read(THint<Diana_Call> _) => new Diana_Call
    {
        n = Read(THint<int>.val),
    };

    public Diana_Format Read(THint<Diana_Format> _) => new Diana_Format
    {
        format = Read(THint<int>.val),
        argn = Read(THint<int>.val),
    };

    public Diana_Const Read(THint<Diana_Const> _) => new Diana_Const
    {
        p_const = Read(THint<int>.val),
    };

    public Diana_MKTuple Read(THint<Diana_MKTuple> _) => new Diana_MKTuple
    {
        n = Read(THint<int>.val),
    };

    public Diana_Pack Read(THint<Diana_Pack> _) => new Diana_Pack
    {
        n = Read(THint<int>.val),
    };

    public DFlatGraphCode Read(THint<DFlatGraphCode> _) => new DFlatGraphCode
    {
        strings = Read(THint<string[]>.val),
        dobjs = Read(THint<DObj[]>.val),
        internstrings = Read(THint<InternString[]>.val),
        catchs = Read(THint<Catch[]>.val),
        funcmetas = Read(THint<FuncMeta[]>.val),
        blocks = Read(THint<Block[]>.val),
        diana_functiondefs = Read(THint<Diana_FunctionDef[]>.val),
        diana_loadglobalrefs = Read(THint<Diana_LoadGlobalRef[]>.val),
        diana_delvars = Read(THint<Diana_DelVar[]>.val),
        diana_loadvars = Read(THint<Diana_LoadVar[]>.val),
        diana_storevars = Read(THint<Diana_StoreVar[]>.val),
        diana_actions = Read(THint<Diana_Action[]>.val),
        diana_controlifs = Read(THint<Diana_ControlIf[]>.val),
        diana_controls = Read(THint<Diana_Control[]>.val),
        diana_trys = Read(THint<Diana_Try[]>.val),
        diana_loops = Read(THint<Diana_Loop[]>.val),
        diana_fors = Read(THint<Diana_For[]>.val),
        diana_withs = Read(THint<Diana_With[]>.val),
        diana_getattrs = Read(THint<Diana_GetAttr[]>.val),
        diana_setattrs = Read(THint<Diana_SetAttr[]>.val),
        diana_setattrop_adds = Read(THint<Diana_SetAttrOp_add[]>.val),
        diana_setattrop_subs = Read(THint<Diana_SetAttrOp_sub[]>.val),
        diana_setattrop_muls = Read(THint<Diana_SetAttrOp_mul[]>.val),
        diana_setattrop_truedivs = Read(THint<Diana_SetAttrOp_truediv[]>.val),
        diana_setattrop_floordivs = Read(THint<Diana_SetAttrOp_floordiv[]>.val),
        diana_setattrop_mods = Read(THint<Diana_SetAttrOp_mod[]>.val),
        diana_setattrop_pows = Read(THint<Diana_SetAttrOp_pow[]>.val),
        diana_setattrop_lshifts = Read(THint<Diana_SetAttrOp_lshift[]>.val),
        diana_setattrop_rshifts = Read(THint<Diana_SetAttrOp_rshift[]>.val),
        diana_setattrop_bitors = Read(THint<Diana_SetAttrOp_bitor[]>.val),
        diana_setattrop_bitands = Read(THint<Diana_SetAttrOp_bitand[]>.val),
        diana_setattrop_bitxors = Read(THint<Diana_SetAttrOp_bitxor[]>.val),
        diana_mkdicts = Read(THint<Diana_MKDict[]>.val),
        diana_mksets = Read(THint<Diana_MKSet[]>.val),
        diana_mklists = Read(THint<Diana_MKList[]>.val),
        diana_calls = Read(THint<Diana_Call[]>.val),
        diana_formats = Read(THint<Diana_Format[]>.val),
        diana_consts = Read(THint<Diana_Const[]>.val),
        diana_mktuples = Read(THint<Diana_MKTuple[]>.val),
        diana_packs = Read(THint<Diana_Pack[]>.val),
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
    public static readonly THint<Diana_LoadGlobalRef> Diana_LoadGlobalRef_hint = THint<Diana_LoadGlobalRef>.val;
    public Diana_LoadGlobalRef[] Read(THint<Diana_LoadGlobalRef[]> _)
    {
        Diana_LoadGlobalRef[] src = new Diana_LoadGlobalRef[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_LoadGlobalRef_hint);
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
    public static readonly THint<Diana_StoreVar> Diana_StoreVar_hint = THint<Diana_StoreVar>.val;
    public Diana_StoreVar[] Read(THint<Diana_StoreVar[]> _)
    {
        Diana_StoreVar[] src = new Diana_StoreVar[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_StoreVar_hint);
        }
        return src;
    }
    public static readonly THint<Diana_Action> Diana_Action_hint = THint<Diana_Action>.val;
    public Diana_Action[] Read(THint<Diana_Action[]> _)
    {
        Diana_Action[] src = new Diana_Action[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_Action_hint);
        }
        return src;
    }
    public static readonly THint<Diana_ControlIf> Diana_ControlIf_hint = THint<Diana_ControlIf>.val;
    public Diana_ControlIf[] Read(THint<Diana_ControlIf[]> _)
    {
        Diana_ControlIf[] src = new Diana_ControlIf[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_ControlIf_hint);
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
    public static readonly THint<Diana_Loop> Diana_Loop_hint = THint<Diana_Loop>.val;
    public Diana_Loop[] Read(THint<Diana_Loop[]> _)
    {
        Diana_Loop[] src = new Diana_Loop[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_Loop_hint);
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
    public static readonly THint<Diana_SetAttr> Diana_SetAttr_hint = THint<Diana_SetAttr>.val;
    public Diana_SetAttr[] Read(THint<Diana_SetAttr[]> _)
    {
        Diana_SetAttr[] src = new Diana_SetAttr[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttrOp_add> Diana_SetAttrOp_add_hint = THint<Diana_SetAttrOp_add>.val;
    public Diana_SetAttrOp_add[] Read(THint<Diana_SetAttrOp_add[]> _)
    {
        Diana_SetAttrOp_add[] src = new Diana_SetAttrOp_add[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttrOp_add_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttrOp_sub> Diana_SetAttrOp_sub_hint = THint<Diana_SetAttrOp_sub>.val;
    public Diana_SetAttrOp_sub[] Read(THint<Diana_SetAttrOp_sub[]> _)
    {
        Diana_SetAttrOp_sub[] src = new Diana_SetAttrOp_sub[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttrOp_sub_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttrOp_mul> Diana_SetAttrOp_mul_hint = THint<Diana_SetAttrOp_mul>.val;
    public Diana_SetAttrOp_mul[] Read(THint<Diana_SetAttrOp_mul[]> _)
    {
        Diana_SetAttrOp_mul[] src = new Diana_SetAttrOp_mul[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttrOp_mul_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttrOp_truediv> Diana_SetAttrOp_truediv_hint = THint<Diana_SetAttrOp_truediv>.val;
    public Diana_SetAttrOp_truediv[] Read(THint<Diana_SetAttrOp_truediv[]> _)
    {
        Diana_SetAttrOp_truediv[] src = new Diana_SetAttrOp_truediv[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttrOp_truediv_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttrOp_floordiv> Diana_SetAttrOp_floordiv_hint = THint<Diana_SetAttrOp_floordiv>.val;
    public Diana_SetAttrOp_floordiv[] Read(THint<Diana_SetAttrOp_floordiv[]> _)
    {
        Diana_SetAttrOp_floordiv[] src = new Diana_SetAttrOp_floordiv[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttrOp_floordiv_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttrOp_mod> Diana_SetAttrOp_mod_hint = THint<Diana_SetAttrOp_mod>.val;
    public Diana_SetAttrOp_mod[] Read(THint<Diana_SetAttrOp_mod[]> _)
    {
        Diana_SetAttrOp_mod[] src = new Diana_SetAttrOp_mod[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttrOp_mod_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttrOp_pow> Diana_SetAttrOp_pow_hint = THint<Diana_SetAttrOp_pow>.val;
    public Diana_SetAttrOp_pow[] Read(THint<Diana_SetAttrOp_pow[]> _)
    {
        Diana_SetAttrOp_pow[] src = new Diana_SetAttrOp_pow[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttrOp_pow_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttrOp_lshift> Diana_SetAttrOp_lshift_hint = THint<Diana_SetAttrOp_lshift>.val;
    public Diana_SetAttrOp_lshift[] Read(THint<Diana_SetAttrOp_lshift[]> _)
    {
        Diana_SetAttrOp_lshift[] src = new Diana_SetAttrOp_lshift[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttrOp_lshift_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttrOp_rshift> Diana_SetAttrOp_rshift_hint = THint<Diana_SetAttrOp_rshift>.val;
    public Diana_SetAttrOp_rshift[] Read(THint<Diana_SetAttrOp_rshift[]> _)
    {
        Diana_SetAttrOp_rshift[] src = new Diana_SetAttrOp_rshift[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttrOp_rshift_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttrOp_bitor> Diana_SetAttrOp_bitor_hint = THint<Diana_SetAttrOp_bitor>.val;
    public Diana_SetAttrOp_bitor[] Read(THint<Diana_SetAttrOp_bitor[]> _)
    {
        Diana_SetAttrOp_bitor[] src = new Diana_SetAttrOp_bitor[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttrOp_bitor_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttrOp_bitand> Diana_SetAttrOp_bitand_hint = THint<Diana_SetAttrOp_bitand>.val;
    public Diana_SetAttrOp_bitand[] Read(THint<Diana_SetAttrOp_bitand[]> _)
    {
        Diana_SetAttrOp_bitand[] src = new Diana_SetAttrOp_bitand[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttrOp_bitand_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttrOp_bitxor> Diana_SetAttrOp_bitxor_hint = THint<Diana_SetAttrOp_bitxor>.val;
    public Diana_SetAttrOp_bitxor[] Read(THint<Diana_SetAttrOp_bitxor[]> _)
    {
        Diana_SetAttrOp_bitxor[] src = new Diana_SetAttrOp_bitxor[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttrOp_bitxor_hint);
        }
        return src;
    }
    public static readonly THint<Diana_MKDict> Diana_MKDict_hint = THint<Diana_MKDict>.val;
    public Diana_MKDict[] Read(THint<Diana_MKDict[]> _)
    {
        Diana_MKDict[] src = new Diana_MKDict[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_MKDict_hint);
        }
        return src;
    }
    public static readonly THint<Diana_MKSet> Diana_MKSet_hint = THint<Diana_MKSet>.val;
    public Diana_MKSet[] Read(THint<Diana_MKSet[]> _)
    {
        Diana_MKSet[] src = new Diana_MKSet[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_MKSet_hint);
        }
        return src;
    }
    public static readonly THint<Diana_MKList> Diana_MKList_hint = THint<Diana_MKList>.val;
    public Diana_MKList[] Read(THint<Diana_MKList[]> _)
    {
        Diana_MKList[] src = new Diana_MKList[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_MKList_hint);
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
    public static readonly THint<Diana_MKTuple> Diana_MKTuple_hint = THint<Diana_MKTuple>.val;
    public Diana_MKTuple[] Read(THint<Diana_MKTuple[]> _)
    {
        Diana_MKTuple[] src = new Diana_MKTuple[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_MKTuple_hint);
        }
        return src;
    }
    public static readonly THint<Diana_Pack> Diana_Pack_hint = THint<Diana_Pack>.val;
    public Diana_Pack[] Read(THint<Diana_Pack[]> _)
    {
        Diana_Pack[] src = new Diana_Pack[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_Pack_hint);
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
