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
        targets = Read(THint<int[]>.val),
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

    public Diana_JumpIfNot Read(THint<Diana_JumpIfNot> _) => new Diana_JumpIfNot
    {
        off = Read(THint<int>.val),
    };

    public Diana_JumpIf Read(THint<Diana_JumpIf> _) => new Diana_JumpIf
    {
        off = Read(THint<int>.val),
    };

    public Diana_Jump Read(THint<Diana_Jump> _) => new Diana_Jump
    {
        off = Read(THint<int>.val),
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

    public Diana_SetAttr_Iadd Read(THint<Diana_SetAttr_Iadd> _) => new Diana_SetAttr_Iadd
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttr_Isub Read(THint<Diana_SetAttr_Isub> _) => new Diana_SetAttr_Isub
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttr_Imul Read(THint<Diana_SetAttr_Imul> _) => new Diana_SetAttr_Imul
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttr_Itruediv Read(THint<Diana_SetAttr_Itruediv> _) => new Diana_SetAttr_Itruediv
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttr_Ifloordiv Read(THint<Diana_SetAttr_Ifloordiv> _) => new Diana_SetAttr_Ifloordiv
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttr_Imod Read(THint<Diana_SetAttr_Imod> _) => new Diana_SetAttr_Imod
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttr_Ipow Read(THint<Diana_SetAttr_Ipow> _) => new Diana_SetAttr_Ipow
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttr_Ilshift Read(THint<Diana_SetAttr_Ilshift> _) => new Diana_SetAttr_Ilshift
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttr_Irshift Read(THint<Diana_SetAttr_Irshift> _) => new Diana_SetAttr_Irshift
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttr_Ibitor Read(THint<Diana_SetAttr_Ibitor> _) => new Diana_SetAttr_Ibitor
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttr_Ibitand Read(THint<Diana_SetAttr_Ibitand> _) => new Diana_SetAttr_Ibitand
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttr_Ibitxor Read(THint<Diana_SetAttr_Ibitxor> _) => new Diana_SetAttr_Ibitxor
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttr_Igt Read(THint<Diana_SetAttr_Igt> _) => new Diana_SetAttr_Igt
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttr_Ilt Read(THint<Diana_SetAttr_Ilt> _) => new Diana_SetAttr_Ilt
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttr_Ige Read(THint<Diana_SetAttr_Ige> _) => new Diana_SetAttr_Ige
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttr_Ile Read(THint<Diana_SetAttr_Ile> _) => new Diana_SetAttr_Ile
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttr_Ieq Read(THint<Diana_SetAttr_Ieq> _) => new Diana_SetAttr_Ieq
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttr_Ineq Read(THint<Diana_SetAttr_Ineq> _) => new Diana_SetAttr_Ineq
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttr_Iin Read(THint<Diana_SetAttr_Iin> _) => new Diana_SetAttr_Iin
    {
        attr = Read(THint<InternString>.val),
    };

    public Diana_SetAttr_Inotin Read(THint<Diana_SetAttr_Inotin> _) => new Diana_SetAttr_Inotin
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

    public Diana_Replicate Read(THint<Diana_Replicate> _) => new Diana_Replicate
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
        diana_jumpifnots = Read(THint<Diana_JumpIfNot[]>.val),
        diana_jumpifs = Read(THint<Diana_JumpIf[]>.val),
        diana_jumps = Read(THint<Diana_Jump[]>.val),
        diana_controls = Read(THint<Diana_Control[]>.val),
        diana_trys = Read(THint<Diana_Try[]>.val),
        diana_loops = Read(THint<Diana_Loop[]>.val),
        diana_fors = Read(THint<Diana_For[]>.val),
        diana_withs = Read(THint<Diana_With[]>.val),
        diana_getattrs = Read(THint<Diana_GetAttr[]>.val),
        diana_setattrs = Read(THint<Diana_SetAttr[]>.val),
        diana_setattr_iadds = Read(THint<Diana_SetAttr_Iadd[]>.val),
        diana_setattr_isubs = Read(THint<Diana_SetAttr_Isub[]>.val),
        diana_setattr_imuls = Read(THint<Diana_SetAttr_Imul[]>.val),
        diana_setattr_itruedivs = Read(THint<Diana_SetAttr_Itruediv[]>.val),
        diana_setattr_ifloordivs = Read(THint<Diana_SetAttr_Ifloordiv[]>.val),
        diana_setattr_imods = Read(THint<Diana_SetAttr_Imod[]>.val),
        diana_setattr_ipows = Read(THint<Diana_SetAttr_Ipow[]>.val),
        diana_setattr_ilshifts = Read(THint<Diana_SetAttr_Ilshift[]>.val),
        diana_setattr_irshifts = Read(THint<Diana_SetAttr_Irshift[]>.val),
        diana_setattr_ibitors = Read(THint<Diana_SetAttr_Ibitor[]>.val),
        diana_setattr_ibitands = Read(THint<Diana_SetAttr_Ibitand[]>.val),
        diana_setattr_ibitxors = Read(THint<Diana_SetAttr_Ibitxor[]>.val),
        diana_setattr_igts = Read(THint<Diana_SetAttr_Igt[]>.val),
        diana_setattr_ilts = Read(THint<Diana_SetAttr_Ilt[]>.val),
        diana_setattr_iges = Read(THint<Diana_SetAttr_Ige[]>.val),
        diana_setattr_iles = Read(THint<Diana_SetAttr_Ile[]>.val),
        diana_setattr_ieqs = Read(THint<Diana_SetAttr_Ieq[]>.val),
        diana_setattr_ineqs = Read(THint<Diana_SetAttr_Ineq[]>.val),
        diana_setattr_iins = Read(THint<Diana_SetAttr_Iin[]>.val),
        diana_setattr_inotins = Read(THint<Diana_SetAttr_Inotin[]>.val),
        diana_mkdicts = Read(THint<Diana_MKDict[]>.val),
        diana_mksets = Read(THint<Diana_MKSet[]>.val),
        diana_mklists = Read(THint<Diana_MKList[]>.val),
        diana_calls = Read(THint<Diana_Call[]>.val),
        diana_formats = Read(THint<Diana_Format[]>.val),
        diana_consts = Read(THint<Diana_Const[]>.val),
        diana_mktuples = Read(THint<Diana_MKTuple[]>.val),
        diana_packs = Read(THint<Diana_Pack[]>.val),
        diana_replicates = Read(THint<Diana_Replicate[]>.val),
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
    public static readonly THint<Diana_JumpIfNot> Diana_JumpIfNot_hint = THint<Diana_JumpIfNot>.val;
    public Diana_JumpIfNot[] Read(THint<Diana_JumpIfNot[]> _)
    {
        Diana_JumpIfNot[] src = new Diana_JumpIfNot[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_JumpIfNot_hint);
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
    public static readonly THint<Diana_SetAttr_Iadd> Diana_SetAttr_Iadd_hint = THint<Diana_SetAttr_Iadd>.val;
    public Diana_SetAttr_Iadd[] Read(THint<Diana_SetAttr_Iadd[]> _)
    {
        Diana_SetAttr_Iadd[] src = new Diana_SetAttr_Iadd[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_Iadd_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttr_Isub> Diana_SetAttr_Isub_hint = THint<Diana_SetAttr_Isub>.val;
    public Diana_SetAttr_Isub[] Read(THint<Diana_SetAttr_Isub[]> _)
    {
        Diana_SetAttr_Isub[] src = new Diana_SetAttr_Isub[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_Isub_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttr_Imul> Diana_SetAttr_Imul_hint = THint<Diana_SetAttr_Imul>.val;
    public Diana_SetAttr_Imul[] Read(THint<Diana_SetAttr_Imul[]> _)
    {
        Diana_SetAttr_Imul[] src = new Diana_SetAttr_Imul[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_Imul_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttr_Itruediv> Diana_SetAttr_Itruediv_hint = THint<Diana_SetAttr_Itruediv>.val;
    public Diana_SetAttr_Itruediv[] Read(THint<Diana_SetAttr_Itruediv[]> _)
    {
        Diana_SetAttr_Itruediv[] src = new Diana_SetAttr_Itruediv[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_Itruediv_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttr_Ifloordiv> Diana_SetAttr_Ifloordiv_hint = THint<Diana_SetAttr_Ifloordiv>.val;
    public Diana_SetAttr_Ifloordiv[] Read(THint<Diana_SetAttr_Ifloordiv[]> _)
    {
        Diana_SetAttr_Ifloordiv[] src = new Diana_SetAttr_Ifloordiv[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_Ifloordiv_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttr_Imod> Diana_SetAttr_Imod_hint = THint<Diana_SetAttr_Imod>.val;
    public Diana_SetAttr_Imod[] Read(THint<Diana_SetAttr_Imod[]> _)
    {
        Diana_SetAttr_Imod[] src = new Diana_SetAttr_Imod[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_Imod_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttr_Ipow> Diana_SetAttr_Ipow_hint = THint<Diana_SetAttr_Ipow>.val;
    public Diana_SetAttr_Ipow[] Read(THint<Diana_SetAttr_Ipow[]> _)
    {
        Diana_SetAttr_Ipow[] src = new Diana_SetAttr_Ipow[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_Ipow_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttr_Ilshift> Diana_SetAttr_Ilshift_hint = THint<Diana_SetAttr_Ilshift>.val;
    public Diana_SetAttr_Ilshift[] Read(THint<Diana_SetAttr_Ilshift[]> _)
    {
        Diana_SetAttr_Ilshift[] src = new Diana_SetAttr_Ilshift[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_Ilshift_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttr_Irshift> Diana_SetAttr_Irshift_hint = THint<Diana_SetAttr_Irshift>.val;
    public Diana_SetAttr_Irshift[] Read(THint<Diana_SetAttr_Irshift[]> _)
    {
        Diana_SetAttr_Irshift[] src = new Diana_SetAttr_Irshift[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_Irshift_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttr_Ibitor> Diana_SetAttr_Ibitor_hint = THint<Diana_SetAttr_Ibitor>.val;
    public Diana_SetAttr_Ibitor[] Read(THint<Diana_SetAttr_Ibitor[]> _)
    {
        Diana_SetAttr_Ibitor[] src = new Diana_SetAttr_Ibitor[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_Ibitor_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttr_Ibitand> Diana_SetAttr_Ibitand_hint = THint<Diana_SetAttr_Ibitand>.val;
    public Diana_SetAttr_Ibitand[] Read(THint<Diana_SetAttr_Ibitand[]> _)
    {
        Diana_SetAttr_Ibitand[] src = new Diana_SetAttr_Ibitand[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_Ibitand_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttr_Ibitxor> Diana_SetAttr_Ibitxor_hint = THint<Diana_SetAttr_Ibitxor>.val;
    public Diana_SetAttr_Ibitxor[] Read(THint<Diana_SetAttr_Ibitxor[]> _)
    {
        Diana_SetAttr_Ibitxor[] src = new Diana_SetAttr_Ibitxor[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_Ibitxor_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttr_Igt> Diana_SetAttr_Igt_hint = THint<Diana_SetAttr_Igt>.val;
    public Diana_SetAttr_Igt[] Read(THint<Diana_SetAttr_Igt[]> _)
    {
        Diana_SetAttr_Igt[] src = new Diana_SetAttr_Igt[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_Igt_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttr_Ilt> Diana_SetAttr_Ilt_hint = THint<Diana_SetAttr_Ilt>.val;
    public Diana_SetAttr_Ilt[] Read(THint<Diana_SetAttr_Ilt[]> _)
    {
        Diana_SetAttr_Ilt[] src = new Diana_SetAttr_Ilt[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_Ilt_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttr_Ige> Diana_SetAttr_Ige_hint = THint<Diana_SetAttr_Ige>.val;
    public Diana_SetAttr_Ige[] Read(THint<Diana_SetAttr_Ige[]> _)
    {
        Diana_SetAttr_Ige[] src = new Diana_SetAttr_Ige[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_Ige_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttr_Ile> Diana_SetAttr_Ile_hint = THint<Diana_SetAttr_Ile>.val;
    public Diana_SetAttr_Ile[] Read(THint<Diana_SetAttr_Ile[]> _)
    {
        Diana_SetAttr_Ile[] src = new Diana_SetAttr_Ile[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_Ile_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttr_Ieq> Diana_SetAttr_Ieq_hint = THint<Diana_SetAttr_Ieq>.val;
    public Diana_SetAttr_Ieq[] Read(THint<Diana_SetAttr_Ieq[]> _)
    {
        Diana_SetAttr_Ieq[] src = new Diana_SetAttr_Ieq[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_Ieq_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttr_Ineq> Diana_SetAttr_Ineq_hint = THint<Diana_SetAttr_Ineq>.val;
    public Diana_SetAttr_Ineq[] Read(THint<Diana_SetAttr_Ineq[]> _)
    {
        Diana_SetAttr_Ineq[] src = new Diana_SetAttr_Ineq[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_Ineq_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttr_Iin> Diana_SetAttr_Iin_hint = THint<Diana_SetAttr_Iin>.val;
    public Diana_SetAttr_Iin[] Read(THint<Diana_SetAttr_Iin[]> _)
    {
        Diana_SetAttr_Iin[] src = new Diana_SetAttr_Iin[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_Iin_hint);
        }
        return src;
    }
    public static readonly THint<Diana_SetAttr_Inotin> Diana_SetAttr_Inotin_hint = THint<Diana_SetAttr_Inotin>.val;
    public Diana_SetAttr_Inotin[] Read(THint<Diana_SetAttr_Inotin[]> _)
    {
        Diana_SetAttr_Inotin[] src = new Diana_SetAttr_Inotin[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_SetAttr_Inotin_hint);
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
    public static readonly THint<Diana_Replicate> Diana_Replicate_hint = THint<Diana_Replicate>.val;
    public Diana_Replicate[] Read(THint<Diana_Replicate[]> _)
    {
        Diana_Replicate[] src = new Diana_Replicate[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Diana_Replicate_hint);
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
