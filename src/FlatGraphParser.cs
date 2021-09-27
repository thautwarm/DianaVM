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
    public Stmt_FunctionDef Read(THint<Stmt_FunctionDef> _) => new Stmt_FunctionDef
    {
        metadataInd = Read(THint<int>.val),
        code = Read(THint<Ptr>.val),
    };

    public Stmt_Return Read(THint<Stmt_Return> _) => new Stmt_Return
    {
        value = Read(THint<Ptr>.val),
    };

    public Stmt_DelGlobalName Read(THint<Stmt_DelGlobalName> _) => new Stmt_DelGlobalName
    {
        slot = Read(THint<int>.val),
    };

    public Stmt_DelLocalName Read(THint<Stmt_DelLocalName> _) => new Stmt_DelLocalName
    {
        slot = Read(THint<int>.val),
    };

    public Stmt_DelDerefName Read(THint<Stmt_DelDerefName> _) => new Stmt_DelDerefName
    {
        slot = Read(THint<int>.val),
    };

    public Stmt_DeleteItem Read(THint<Stmt_DeleteItem> _) => new Stmt_DeleteItem
    {
        value = Read(THint<Ptr>.val),
        item = Read(THint<Ptr>.val),
    };

    public Stmt_Assign Read(THint<Stmt_Assign> _) => new Stmt_Assign
    {
        targets = Read(THint<Ptr>.val),
        value = Read(THint<Ptr>.val),
    };

    public Stmt_AddAssign Read(THint<Stmt_AddAssign> _) => new Stmt_AddAssign
    {
        target = Read(THint<Ptr>.val),
        value = Read(THint<Ptr>.val),
    };

    public Stmt_SubAssign Read(THint<Stmt_SubAssign> _) => new Stmt_SubAssign
    {
        target = Read(THint<Ptr>.val),
        value = Read(THint<Ptr>.val),
    };

    public Stmt_MutAssign Read(THint<Stmt_MutAssign> _) => new Stmt_MutAssign
    {
        target = Read(THint<Ptr>.val),
        value = Read(THint<Ptr>.val),
    };

    public Stmt_TrueDivAssign Read(THint<Stmt_TrueDivAssign> _) => new Stmt_TrueDivAssign
    {
        target = Read(THint<Ptr>.val),
        value = Read(THint<Ptr>.val),
    };

    public Stmt_FloorDivAssign Read(THint<Stmt_FloorDivAssign> _) => new Stmt_FloorDivAssign
    {
        target = Read(THint<Ptr>.val),
        value = Read(THint<Ptr>.val),
    };

    public Stmt_ModAssign Read(THint<Stmt_ModAssign> _) => new Stmt_ModAssign
    {
        target = Read(THint<Ptr>.val),
        value = Read(THint<Ptr>.val),
    };

    public Stmt_PowAssign Read(THint<Stmt_PowAssign> _) => new Stmt_PowAssign
    {
        target = Read(THint<Ptr>.val),
        value = Read(THint<Ptr>.val),
    };

    public Stmt_LShiftAssign Read(THint<Stmt_LShiftAssign> _) => new Stmt_LShiftAssign
    {
        target = Read(THint<Ptr>.val),
        value = Read(THint<Ptr>.val),
    };

    public Stmt_RShiftAssign Read(THint<Stmt_RShiftAssign> _) => new Stmt_RShiftAssign
    {
        target = Read(THint<Ptr>.val),
        value = Read(THint<Ptr>.val),
    };

    public Stmt_BitOrAssign Read(THint<Stmt_BitOrAssign> _) => new Stmt_BitOrAssign
    {
        target = Read(THint<Ptr>.val),
        value = Read(THint<Ptr>.val),
    };

    public Stmt_BitAndAssign Read(THint<Stmt_BitAndAssign> _) => new Stmt_BitAndAssign
    {
        target = Read(THint<Ptr>.val),
        value = Read(THint<Ptr>.val),
    };

    public Stmt_BitXorAssign Read(THint<Stmt_BitXorAssign> _) => new Stmt_BitXorAssign
    {
        target = Read(THint<Ptr>.val),
        value = Read(THint<Ptr>.val),
    };

    public Stmt_For Read(THint<Stmt_For> _) => new Stmt_For
    {
        target = Read(THint<Ptr>.val),
        iter = Read(THint<Ptr>.val),
        body = Read(THint<Ptr>.val),
    };

    public Stmt_While Read(THint<Stmt_While> _) => new Stmt_While
    {
        cond = Read(THint<Ptr>.val),
        body = Read(THint<Ptr>.val),
    };

    public Stmt_If Read(THint<Stmt_If> _) => new Stmt_If
    {
        cond = Read(THint<Ptr>.val),
        then = Read(THint<Ptr>.val),
        orelse = Read(THint<Ptr>.val),
    };

    public Stmt_With Read(THint<Stmt_With> _) => new Stmt_With
    {
        expr = Read(THint<Ptr>.val),
        body = Read(THint<Ptr>.val),
    };

    public Stmt_Raise Read(THint<Stmt_Raise> _) => new Stmt_Raise
    {
        value = Read(THint<Ptr>.val),
    };

    public Stmt_RaiseFrom Read(THint<Stmt_RaiseFrom> _) => new Stmt_RaiseFrom
    {
        value = Read(THint<Ptr>.val),
        from_ = Read(THint<Ptr>.val),
    };

    public Stmt_Try Read(THint<Stmt_Try> _) => new Stmt_Try
    {
        body = Read(THint<Ptr>.val),
        err_slot = Read(THint<int>.val),
        except_handlers = Read(THint<Ptr>.val),
        final_body = Read(THint<Ptr>.val),
    };

    public Stmt_Assert Read(THint<Stmt_Assert> _) => new Stmt_Assert
    {
        value = Read(THint<Ptr>.val),
    };

    public Stmt_ExprStmt Read(THint<Stmt_ExprStmt> _) => new Stmt_ExprStmt
    {
        value = Read(THint<Ptr>.val),
    };

    public Stmt_Control Read(THint<Stmt_Control> _) => new Stmt_Control
    {
        kind = Read(THint<int>.val),
    };

    public Stmt_Block Read(THint<Stmt_Block> _) => new Stmt_Block
    {
        stmts = Read(THint<Ptr[]>.val),
    };

    public Expr_AddOp Read(THint<Expr_AddOp> _) => new Expr_AddOp
    {
        left = Read(THint<Ptr>.val),
        right = Read(THint<Ptr>.val),
    };

    public Expr_SubOp Read(THint<Expr_SubOp> _) => new Expr_SubOp
    {
        left = Read(THint<Ptr>.val),
        right = Read(THint<Ptr>.val),
    };

    public Expr_MutOp Read(THint<Expr_MutOp> _) => new Expr_MutOp
    {
        left = Read(THint<Ptr>.val),
        right = Read(THint<Ptr>.val),
    };

    public Expr_TrueDivOp Read(THint<Expr_TrueDivOp> _) => new Expr_TrueDivOp
    {
        left = Read(THint<Ptr>.val),
        right = Read(THint<Ptr>.val),
    };

    public Expr_FloorDivOp Read(THint<Expr_FloorDivOp> _) => new Expr_FloorDivOp
    {
        left = Read(THint<Ptr>.val),
        right = Read(THint<Ptr>.val),
    };

    public Expr_ModOp Read(THint<Expr_ModOp> _) => new Expr_ModOp
    {
        left = Read(THint<Ptr>.val),
        right = Read(THint<Ptr>.val),
    };

    public Expr_PowOp Read(THint<Expr_PowOp> _) => new Expr_PowOp
    {
        left = Read(THint<Ptr>.val),
        right = Read(THint<Ptr>.val),
    };

    public Expr_LShiftOp Read(THint<Expr_LShiftOp> _) => new Expr_LShiftOp
    {
        left = Read(THint<Ptr>.val),
        right = Read(THint<Ptr>.val),
    };

    public Expr_RShiftOp Read(THint<Expr_RShiftOp> _) => new Expr_RShiftOp
    {
        left = Read(THint<Ptr>.val),
        right = Read(THint<Ptr>.val),
    };

    public Expr_BitOrOp Read(THint<Expr_BitOrOp> _) => new Expr_BitOrOp
    {
        left = Read(THint<Ptr>.val),
        right = Read(THint<Ptr>.val),
    };

    public Expr_BitAndOp Read(THint<Expr_BitAndOp> _) => new Expr_BitAndOp
    {
        left = Read(THint<Ptr>.val),
        right = Read(THint<Ptr>.val),
    };

    public Expr_BitXorOp Read(THint<Expr_BitXorOp> _) => new Expr_BitXorOp
    {
        left = Read(THint<Ptr>.val),
        right = Read(THint<Ptr>.val),
    };

    public Expr_GlobalBinder Read(THint<Expr_GlobalBinder> _) => new Expr_GlobalBinder
    {
        slot = Read(THint<int>.val),
    };

    public Expr_LocalBinder Read(THint<Expr_LocalBinder> _) => new Expr_LocalBinder
    {
        slot = Read(THint<int>.val),
    };

    public Expr_DerefBinder Read(THint<Expr_DerefBinder> _) => new Expr_DerefBinder
    {
        slot = Read(THint<int>.val),
    };

    public Expr_AndOp Read(THint<Expr_AndOp> _) => new Expr_AndOp
    {
        left = Read(THint<Ptr>.val),
        right = Read(THint<Ptr>.val),
    };

    public Expr_OrOp Read(THint<Expr_OrOp> _) => new Expr_OrOp
    {
        left = Read(THint<Ptr>.val),
        right = Read(THint<Ptr>.val),
    };

    public Expr_InvertOp Read(THint<Expr_InvertOp> _) => new Expr_InvertOp
    {
        value = Read(THint<Ptr>.val),
    };

    public Expr_NotOp Read(THint<Expr_NotOp> _) => new Expr_NotOp
    {
        value = Read(THint<Ptr>.val),
    };

    public Expr_Lambda Read(THint<Expr_Lambda> _) => new Expr_Lambda
    {
        frees = Read(THint<int[]>.val),
        code = Read(THint<int>.val),
    };

    public Expr_IfExpr Read(THint<Expr_IfExpr> _) => new Expr_IfExpr
    {
        cond = Read(THint<Ptr>.val),
        then = Read(THint<Ptr>.val),
        orelse = Read(THint<Ptr>.val),
    };

    public Expr_Dict Read(THint<Expr_Dict> _) => new Expr_Dict
    {
        keys = Read(THint<Ptr[]>.val),
        values = Read(THint<Ptr[]>.val),
    };

    public Expr_Set Read(THint<Expr_Set> _) => new Expr_Set
    {
        elts = Read(THint<Ptr[]>.val),
    };

    public Expr_List Read(THint<Expr_List> _) => new Expr_List
    {
        elts = Read(THint<Ptr[]>.val),
    };

    public Expr_Generator Read(THint<Expr_Generator> _) => new Expr_Generator
    {
        target = Read(THint<Ptr>.val),
        iter = Read(THint<Ptr>.val),
        body = Read(THint<Ptr>.val),
    };

    public Expr_Comprehension Read(THint<Expr_Comprehension> _) => new Expr_Comprehension
    {
        adder = Read(THint<Ptr>.val),
        target = Read(THint<Ptr>.val),
        iter = Read(THint<Ptr>.val),
        body = Read(THint<Ptr>.val),
    };

    public Expr_Call Read(THint<Expr_Call> _) => new Expr_Call
    {
        f = Read(THint<Ptr>.val),
        args = Read(THint<Ptr>.val),
    };

    public Expr_Format Read(THint<Expr_Format> _) => new Expr_Format
    {
        format = Read(THint<int>.val),
        args = Read(THint<Ptr>.val),
    };

    public Expr_Const Read(THint<Expr_Const> _) => new Expr_Const
    {
        constInd = Read(THint<int>.val),
    };

    public Expr_Attr Read(THint<Expr_Attr> _) => new Expr_Attr
    {
        value = Read(THint<Ptr>.val),
        attr = Read(THint<int>.val),
    };

    public Expr_GlobalName Read(THint<Expr_GlobalName> _) => new Expr_GlobalName
    {
        slot = Read(THint<int>.val),
    };

    public Expr_LocalName Read(THint<Expr_LocalName> _) => new Expr_LocalName
    {
        slot = Read(THint<int>.val),
    };

    public Expr_DerefName Read(THint<Expr_DerefName> _) => new Expr_DerefName
    {
        slot = Read(THint<int>.val),
    };

    public Expr_Item Read(THint<Expr_Item> _) => new Expr_Item
    {
        value = Read(THint<Ptr>.val),
        item = Read(THint<Ptr>.val),
    };

    public Expr_Tuple Read(THint<Expr_Tuple> _) => new Expr_Tuple
    {
        elts = Read(THint<Ptr[]>.val),
    };

    public Arg_GlobalNameOut Read(THint<Arg_GlobalNameOut> _) => new Arg_GlobalNameOut
    {
        ind = Read(THint<int>.val),
    };

    public Arg_LocalNameOut Read(THint<Arg_LocalNameOut> _) => new Arg_LocalNameOut
    {
        ind = Read(THint<int>.val),
    };

    public Arg_DerefNameOut Read(THint<Arg_DerefNameOut> _) => new Arg_DerefNameOut
    {
        ind = Read(THint<int>.val),
    };

    public Arg_ItemOut Read(THint<Arg_ItemOut> _) => new Arg_ItemOut
    {
        value = Read(THint<Ptr>.val),
        item = Read(THint<Ptr>.val),
    };

    public Arg_AttrOut Read(THint<Arg_AttrOut> _) => new Arg_AttrOut
    {
        value = Read(THint<Ptr>.val),
        attr = Read(THint<int>.val),
    };

    public Arg_Val Read(THint<Arg_Val> _) => new Arg_Val
    {
        value = Read(THint<Ptr>.val),
    };

    public ExceptHandler_ArbitraryCatch Read(THint<ExceptHandler_ArbitraryCatch> _) => new ExceptHandler_ArbitraryCatch
    {
        assign_slot = Read(THint<int>.val),
        body = Read(THint<Ptr>.val),
    };

    public ExceptHandler_TypeCheckCatch Read(THint<ExceptHandler_TypeCheckCatch> _) => new ExceptHandler_TypeCheckCatch
    {
        type = Read(THint<Ptr>.val),
        assign_slot = Read(THint<int>.val),
        body = Read(THint<Ptr>.val),
    };

    public DFlatGraphCode Read(THint<DFlatGraphCode> _) => new DFlatGraphCode
    {
        stmt_functiondefs = Read(THint<Stmt_FunctionDef[]>.val),
        stmt_returns = Read(THint<Stmt_Return[]>.val),
        stmt_delglobalnames = Read(THint<Stmt_DelGlobalName[]>.val),
        stmt_dellocalnames = Read(THint<Stmt_DelLocalName[]>.val),
        stmt_delderefnames = Read(THint<Stmt_DelDerefName[]>.val),
        stmt_deleteitems = Read(THint<Stmt_DeleteItem[]>.val),
        stmt_assigns = Read(THint<Stmt_Assign[]>.val),
        stmt_addassigns = Read(THint<Stmt_AddAssign[]>.val),
        stmt_subassigns = Read(THint<Stmt_SubAssign[]>.val),
        stmt_mutassigns = Read(THint<Stmt_MutAssign[]>.val),
        stmt_truedivassigns = Read(THint<Stmt_TrueDivAssign[]>.val),
        stmt_floordivassigns = Read(THint<Stmt_FloorDivAssign[]>.val),
        stmt_modassigns = Read(THint<Stmt_ModAssign[]>.val),
        stmt_powassigns = Read(THint<Stmt_PowAssign[]>.val),
        stmt_lshiftassigns = Read(THint<Stmt_LShiftAssign[]>.val),
        stmt_rshiftassigns = Read(THint<Stmt_RShiftAssign[]>.val),
        stmt_bitorassigns = Read(THint<Stmt_BitOrAssign[]>.val),
        stmt_bitandassigns = Read(THint<Stmt_BitAndAssign[]>.val),
        stmt_bitxorassigns = Read(THint<Stmt_BitXorAssign[]>.val),
        stmt_fors = Read(THint<Stmt_For[]>.val),
        stmt_whiles = Read(THint<Stmt_While[]>.val),
        stmt_ifs = Read(THint<Stmt_If[]>.val),
        stmt_withs = Read(THint<Stmt_With[]>.val),
        stmt_raises = Read(THint<Stmt_Raise[]>.val),
        stmt_raisefroms = Read(THint<Stmt_RaiseFrom[]>.val),
        stmt_trys = Read(THint<Stmt_Try[]>.val),
        stmt_asserts = Read(THint<Stmt_Assert[]>.val),
        stmt_exprstmts = Read(THint<Stmt_ExprStmt[]>.val),
        stmt_controls = Read(THint<Stmt_Control[]>.val),
        stmt_blocks = Read(THint<Stmt_Block[]>.val),
        expr_addops = Read(THint<Expr_AddOp[]>.val),
        expr_subops = Read(THint<Expr_SubOp[]>.val),
        expr_mutops = Read(THint<Expr_MutOp[]>.val),
        expr_truedivops = Read(THint<Expr_TrueDivOp[]>.val),
        expr_floordivops = Read(THint<Expr_FloorDivOp[]>.val),
        expr_modops = Read(THint<Expr_ModOp[]>.val),
        expr_powops = Read(THint<Expr_PowOp[]>.val),
        expr_lshiftops = Read(THint<Expr_LShiftOp[]>.val),
        expr_rshiftops = Read(THint<Expr_RShiftOp[]>.val),
        expr_bitorops = Read(THint<Expr_BitOrOp[]>.val),
        expr_bitandops = Read(THint<Expr_BitAndOp[]>.val),
        expr_bitxorops = Read(THint<Expr_BitXorOp[]>.val),
        expr_globalbinders = Read(THint<Expr_GlobalBinder[]>.val),
        expr_localbinders = Read(THint<Expr_LocalBinder[]>.val),
        expr_derefbinders = Read(THint<Expr_DerefBinder[]>.val),
        expr_andops = Read(THint<Expr_AndOp[]>.val),
        expr_orops = Read(THint<Expr_OrOp[]>.val),
        expr_invertops = Read(THint<Expr_InvertOp[]>.val),
        expr_notops = Read(THint<Expr_NotOp[]>.val),
        expr_lambdas = Read(THint<Expr_Lambda[]>.val),
        expr_ifexprs = Read(THint<Expr_IfExpr[]>.val),
        expr_dicts = Read(THint<Expr_Dict[]>.val),
        expr_sets = Read(THint<Expr_Set[]>.val),
        expr_lists = Read(THint<Expr_List[]>.val),
        expr_generators = Read(THint<Expr_Generator[]>.val),
        expr_comprehensions = Read(THint<Expr_Comprehension[]>.val),
        expr_calls = Read(THint<Expr_Call[]>.val),
        expr_formats = Read(THint<Expr_Format[]>.val),
        expr_consts = Read(THint<Expr_Const[]>.val),
        expr_attrs = Read(THint<Expr_Attr[]>.val),
        expr_globalnames = Read(THint<Expr_GlobalName[]>.val),
        expr_localnames = Read(THint<Expr_LocalName[]>.val),
        expr_derefnames = Read(THint<Expr_DerefName[]>.val),
        expr_items = Read(THint<Expr_Item[]>.val),
        expr_tuples = Read(THint<Expr_Tuple[]>.val),
        arg_globalnameouts = Read(THint<Arg_GlobalNameOut[]>.val),
        arg_localnameouts = Read(THint<Arg_LocalNameOut[]>.val),
        arg_derefnameouts = Read(THint<Arg_DerefNameOut[]>.val),
        arg_itemouts = Read(THint<Arg_ItemOut[]>.val),
        arg_attrouts = Read(THint<Arg_AttrOut[]>.val),
        arg_vals = Read(THint<Arg_Val[]>.val),
        excepthandler_arbitrarycatchs = Read(THint<ExceptHandler_ArbitraryCatch[]>.val),
        excepthandler_typecheckcatchs = Read(THint<ExceptHandler_TypeCheckCatch[]>.val),
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
    public static readonly THint<DFlatGraphCode> DFlatGraphCode_hint = THint<DFlatGraphCode>.val;
    public DFlatGraphCode[] Read(THint<DFlatGraphCode[]> _)
    {
        DFlatGraphCode[] src = new DFlatGraphCode[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(DFlatGraphCode_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_FunctionDef> Stmt_FunctionDef_hint = THint<Stmt_FunctionDef>.val;
    public Stmt_FunctionDef[] Read(THint<Stmt_FunctionDef[]> _)
    {
        Stmt_FunctionDef[] src = new Stmt_FunctionDef[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_FunctionDef_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_Return> Stmt_Return_hint = THint<Stmt_Return>.val;
    public Stmt_Return[] Read(THint<Stmt_Return[]> _)
    {
        Stmt_Return[] src = new Stmt_Return[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_Return_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_DelGlobalName> Stmt_DelGlobalName_hint = THint<Stmt_DelGlobalName>.val;
    public Stmt_DelGlobalName[] Read(THint<Stmt_DelGlobalName[]> _)
    {
        Stmt_DelGlobalName[] src = new Stmt_DelGlobalName[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_DelGlobalName_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_DelLocalName> Stmt_DelLocalName_hint = THint<Stmt_DelLocalName>.val;
    public Stmt_DelLocalName[] Read(THint<Stmt_DelLocalName[]> _)
    {
        Stmt_DelLocalName[] src = new Stmt_DelLocalName[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_DelLocalName_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_DelDerefName> Stmt_DelDerefName_hint = THint<Stmt_DelDerefName>.val;
    public Stmt_DelDerefName[] Read(THint<Stmt_DelDerefName[]> _)
    {
        Stmt_DelDerefName[] src = new Stmt_DelDerefName[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_DelDerefName_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_DeleteItem> Stmt_DeleteItem_hint = THint<Stmt_DeleteItem>.val;
    public Stmt_DeleteItem[] Read(THint<Stmt_DeleteItem[]> _)
    {
        Stmt_DeleteItem[] src = new Stmt_DeleteItem[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_DeleteItem_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_Assign> Stmt_Assign_hint = THint<Stmt_Assign>.val;
    public Stmt_Assign[] Read(THint<Stmt_Assign[]> _)
    {
        Stmt_Assign[] src = new Stmt_Assign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_Assign_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_AddAssign> Stmt_AddAssign_hint = THint<Stmt_AddAssign>.val;
    public Stmt_AddAssign[] Read(THint<Stmt_AddAssign[]> _)
    {
        Stmt_AddAssign[] src = new Stmt_AddAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_AddAssign_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_SubAssign> Stmt_SubAssign_hint = THint<Stmt_SubAssign>.val;
    public Stmt_SubAssign[] Read(THint<Stmt_SubAssign[]> _)
    {
        Stmt_SubAssign[] src = new Stmt_SubAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_SubAssign_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_MutAssign> Stmt_MutAssign_hint = THint<Stmt_MutAssign>.val;
    public Stmt_MutAssign[] Read(THint<Stmt_MutAssign[]> _)
    {
        Stmt_MutAssign[] src = new Stmt_MutAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_MutAssign_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_TrueDivAssign> Stmt_TrueDivAssign_hint = THint<Stmt_TrueDivAssign>.val;
    public Stmt_TrueDivAssign[] Read(THint<Stmt_TrueDivAssign[]> _)
    {
        Stmt_TrueDivAssign[] src = new Stmt_TrueDivAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_TrueDivAssign_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_FloorDivAssign> Stmt_FloorDivAssign_hint = THint<Stmt_FloorDivAssign>.val;
    public Stmt_FloorDivAssign[] Read(THint<Stmt_FloorDivAssign[]> _)
    {
        Stmt_FloorDivAssign[] src = new Stmt_FloorDivAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_FloorDivAssign_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_ModAssign> Stmt_ModAssign_hint = THint<Stmt_ModAssign>.val;
    public Stmt_ModAssign[] Read(THint<Stmt_ModAssign[]> _)
    {
        Stmt_ModAssign[] src = new Stmt_ModAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_ModAssign_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_PowAssign> Stmt_PowAssign_hint = THint<Stmt_PowAssign>.val;
    public Stmt_PowAssign[] Read(THint<Stmt_PowAssign[]> _)
    {
        Stmt_PowAssign[] src = new Stmt_PowAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_PowAssign_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_LShiftAssign> Stmt_LShiftAssign_hint = THint<Stmt_LShiftAssign>.val;
    public Stmt_LShiftAssign[] Read(THint<Stmt_LShiftAssign[]> _)
    {
        Stmt_LShiftAssign[] src = new Stmt_LShiftAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_LShiftAssign_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_RShiftAssign> Stmt_RShiftAssign_hint = THint<Stmt_RShiftAssign>.val;
    public Stmt_RShiftAssign[] Read(THint<Stmt_RShiftAssign[]> _)
    {
        Stmt_RShiftAssign[] src = new Stmt_RShiftAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_RShiftAssign_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_BitOrAssign> Stmt_BitOrAssign_hint = THint<Stmt_BitOrAssign>.val;
    public Stmt_BitOrAssign[] Read(THint<Stmt_BitOrAssign[]> _)
    {
        Stmt_BitOrAssign[] src = new Stmt_BitOrAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_BitOrAssign_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_BitAndAssign> Stmt_BitAndAssign_hint = THint<Stmt_BitAndAssign>.val;
    public Stmt_BitAndAssign[] Read(THint<Stmt_BitAndAssign[]> _)
    {
        Stmt_BitAndAssign[] src = new Stmt_BitAndAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_BitAndAssign_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_BitXorAssign> Stmt_BitXorAssign_hint = THint<Stmt_BitXorAssign>.val;
    public Stmt_BitXorAssign[] Read(THint<Stmt_BitXorAssign[]> _)
    {
        Stmt_BitXorAssign[] src = new Stmt_BitXorAssign[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_BitXorAssign_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_For> Stmt_For_hint = THint<Stmt_For>.val;
    public Stmt_For[] Read(THint<Stmt_For[]> _)
    {
        Stmt_For[] src = new Stmt_For[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_For_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_While> Stmt_While_hint = THint<Stmt_While>.val;
    public Stmt_While[] Read(THint<Stmt_While[]> _)
    {
        Stmt_While[] src = new Stmt_While[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_While_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_If> Stmt_If_hint = THint<Stmt_If>.val;
    public Stmt_If[] Read(THint<Stmt_If[]> _)
    {
        Stmt_If[] src = new Stmt_If[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_If_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_With> Stmt_With_hint = THint<Stmt_With>.val;
    public Stmt_With[] Read(THint<Stmt_With[]> _)
    {
        Stmt_With[] src = new Stmt_With[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_With_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_Raise> Stmt_Raise_hint = THint<Stmt_Raise>.val;
    public Stmt_Raise[] Read(THint<Stmt_Raise[]> _)
    {
        Stmt_Raise[] src = new Stmt_Raise[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_Raise_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_RaiseFrom> Stmt_RaiseFrom_hint = THint<Stmt_RaiseFrom>.val;
    public Stmt_RaiseFrom[] Read(THint<Stmt_RaiseFrom[]> _)
    {
        Stmt_RaiseFrom[] src = new Stmt_RaiseFrom[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_RaiseFrom_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_Try> Stmt_Try_hint = THint<Stmt_Try>.val;
    public Stmt_Try[] Read(THint<Stmt_Try[]> _)
    {
        Stmt_Try[] src = new Stmt_Try[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_Try_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_Assert> Stmt_Assert_hint = THint<Stmt_Assert>.val;
    public Stmt_Assert[] Read(THint<Stmt_Assert[]> _)
    {
        Stmt_Assert[] src = new Stmt_Assert[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_Assert_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_ExprStmt> Stmt_ExprStmt_hint = THint<Stmt_ExprStmt>.val;
    public Stmt_ExprStmt[] Read(THint<Stmt_ExprStmt[]> _)
    {
        Stmt_ExprStmt[] src = new Stmt_ExprStmt[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_ExprStmt_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_Control> Stmt_Control_hint = THint<Stmt_Control>.val;
    public Stmt_Control[] Read(THint<Stmt_Control[]> _)
    {
        Stmt_Control[] src = new Stmt_Control[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_Control_hint);
        }
        return src;
    }
    public static readonly THint<Stmt_Block> Stmt_Block_hint = THint<Stmt_Block>.val;
    public Stmt_Block[] Read(THint<Stmt_Block[]> _)
    {
        Stmt_Block[] src = new Stmt_Block[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Stmt_Block_hint);
        }
        return src;
    }
    public static readonly THint<Expr_AddOp> Expr_AddOp_hint = THint<Expr_AddOp>.val;
    public Expr_AddOp[] Read(THint<Expr_AddOp[]> _)
    {
        Expr_AddOp[] src = new Expr_AddOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_AddOp_hint);
        }
        return src;
    }
    public static readonly THint<Expr_SubOp> Expr_SubOp_hint = THint<Expr_SubOp>.val;
    public Expr_SubOp[] Read(THint<Expr_SubOp[]> _)
    {
        Expr_SubOp[] src = new Expr_SubOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_SubOp_hint);
        }
        return src;
    }
    public static readonly THint<Expr_MutOp> Expr_MutOp_hint = THint<Expr_MutOp>.val;
    public Expr_MutOp[] Read(THint<Expr_MutOp[]> _)
    {
        Expr_MutOp[] src = new Expr_MutOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_MutOp_hint);
        }
        return src;
    }
    public static readonly THint<Expr_TrueDivOp> Expr_TrueDivOp_hint = THint<Expr_TrueDivOp>.val;
    public Expr_TrueDivOp[] Read(THint<Expr_TrueDivOp[]> _)
    {
        Expr_TrueDivOp[] src = new Expr_TrueDivOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_TrueDivOp_hint);
        }
        return src;
    }
    public static readonly THint<Expr_FloorDivOp> Expr_FloorDivOp_hint = THint<Expr_FloorDivOp>.val;
    public Expr_FloorDivOp[] Read(THint<Expr_FloorDivOp[]> _)
    {
        Expr_FloorDivOp[] src = new Expr_FloorDivOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_FloorDivOp_hint);
        }
        return src;
    }
    public static readonly THint<Expr_ModOp> Expr_ModOp_hint = THint<Expr_ModOp>.val;
    public Expr_ModOp[] Read(THint<Expr_ModOp[]> _)
    {
        Expr_ModOp[] src = new Expr_ModOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_ModOp_hint);
        }
        return src;
    }
    public static readonly THint<Expr_PowOp> Expr_PowOp_hint = THint<Expr_PowOp>.val;
    public Expr_PowOp[] Read(THint<Expr_PowOp[]> _)
    {
        Expr_PowOp[] src = new Expr_PowOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_PowOp_hint);
        }
        return src;
    }
    public static readonly THint<Expr_LShiftOp> Expr_LShiftOp_hint = THint<Expr_LShiftOp>.val;
    public Expr_LShiftOp[] Read(THint<Expr_LShiftOp[]> _)
    {
        Expr_LShiftOp[] src = new Expr_LShiftOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_LShiftOp_hint);
        }
        return src;
    }
    public static readonly THint<Expr_RShiftOp> Expr_RShiftOp_hint = THint<Expr_RShiftOp>.val;
    public Expr_RShiftOp[] Read(THint<Expr_RShiftOp[]> _)
    {
        Expr_RShiftOp[] src = new Expr_RShiftOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_RShiftOp_hint);
        }
        return src;
    }
    public static readonly THint<Expr_BitOrOp> Expr_BitOrOp_hint = THint<Expr_BitOrOp>.val;
    public Expr_BitOrOp[] Read(THint<Expr_BitOrOp[]> _)
    {
        Expr_BitOrOp[] src = new Expr_BitOrOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_BitOrOp_hint);
        }
        return src;
    }
    public static readonly THint<Expr_BitAndOp> Expr_BitAndOp_hint = THint<Expr_BitAndOp>.val;
    public Expr_BitAndOp[] Read(THint<Expr_BitAndOp[]> _)
    {
        Expr_BitAndOp[] src = new Expr_BitAndOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_BitAndOp_hint);
        }
        return src;
    }
    public static readonly THint<Expr_BitXorOp> Expr_BitXorOp_hint = THint<Expr_BitXorOp>.val;
    public Expr_BitXorOp[] Read(THint<Expr_BitXorOp[]> _)
    {
        Expr_BitXorOp[] src = new Expr_BitXorOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_BitXorOp_hint);
        }
        return src;
    }
    public static readonly THint<Expr_GlobalBinder> Expr_GlobalBinder_hint = THint<Expr_GlobalBinder>.val;
    public Expr_GlobalBinder[] Read(THint<Expr_GlobalBinder[]> _)
    {
        Expr_GlobalBinder[] src = new Expr_GlobalBinder[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_GlobalBinder_hint);
        }
        return src;
    }
    public static readonly THint<Expr_LocalBinder> Expr_LocalBinder_hint = THint<Expr_LocalBinder>.val;
    public Expr_LocalBinder[] Read(THint<Expr_LocalBinder[]> _)
    {
        Expr_LocalBinder[] src = new Expr_LocalBinder[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_LocalBinder_hint);
        }
        return src;
    }
    public static readonly THint<Expr_DerefBinder> Expr_DerefBinder_hint = THint<Expr_DerefBinder>.val;
    public Expr_DerefBinder[] Read(THint<Expr_DerefBinder[]> _)
    {
        Expr_DerefBinder[] src = new Expr_DerefBinder[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_DerefBinder_hint);
        }
        return src;
    }
    public static readonly THint<Expr_AndOp> Expr_AndOp_hint = THint<Expr_AndOp>.val;
    public Expr_AndOp[] Read(THint<Expr_AndOp[]> _)
    {
        Expr_AndOp[] src = new Expr_AndOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_AndOp_hint);
        }
        return src;
    }
    public static readonly THint<Expr_OrOp> Expr_OrOp_hint = THint<Expr_OrOp>.val;
    public Expr_OrOp[] Read(THint<Expr_OrOp[]> _)
    {
        Expr_OrOp[] src = new Expr_OrOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_OrOp_hint);
        }
        return src;
    }
    public static readonly THint<Expr_InvertOp> Expr_InvertOp_hint = THint<Expr_InvertOp>.val;
    public Expr_InvertOp[] Read(THint<Expr_InvertOp[]> _)
    {
        Expr_InvertOp[] src = new Expr_InvertOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_InvertOp_hint);
        }
        return src;
    }
    public static readonly THint<Expr_NotOp> Expr_NotOp_hint = THint<Expr_NotOp>.val;
    public Expr_NotOp[] Read(THint<Expr_NotOp[]> _)
    {
        Expr_NotOp[] src = new Expr_NotOp[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_NotOp_hint);
        }
        return src;
    }
    public static readonly THint<Expr_Lambda> Expr_Lambda_hint = THint<Expr_Lambda>.val;
    public Expr_Lambda[] Read(THint<Expr_Lambda[]> _)
    {
        Expr_Lambda[] src = new Expr_Lambda[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_Lambda_hint);
        }
        return src;
    }
    public static readonly THint<Expr_IfExpr> Expr_IfExpr_hint = THint<Expr_IfExpr>.val;
    public Expr_IfExpr[] Read(THint<Expr_IfExpr[]> _)
    {
        Expr_IfExpr[] src = new Expr_IfExpr[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_IfExpr_hint);
        }
        return src;
    }
    public static readonly THint<Expr_Dict> Expr_Dict_hint = THint<Expr_Dict>.val;
    public Expr_Dict[] Read(THint<Expr_Dict[]> _)
    {
        Expr_Dict[] src = new Expr_Dict[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_Dict_hint);
        }
        return src;
    }
    public static readonly THint<Expr_Set> Expr_Set_hint = THint<Expr_Set>.val;
    public Expr_Set[] Read(THint<Expr_Set[]> _)
    {
        Expr_Set[] src = new Expr_Set[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_Set_hint);
        }
        return src;
    }
    public static readonly THint<Expr_List> Expr_List_hint = THint<Expr_List>.val;
    public Expr_List[] Read(THint<Expr_List[]> _)
    {
        Expr_List[] src = new Expr_List[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_List_hint);
        }
        return src;
    }
    public static readonly THint<Expr_Generator> Expr_Generator_hint = THint<Expr_Generator>.val;
    public Expr_Generator[] Read(THint<Expr_Generator[]> _)
    {
        Expr_Generator[] src = new Expr_Generator[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_Generator_hint);
        }
        return src;
    }
    public static readonly THint<Expr_Comprehension> Expr_Comprehension_hint = THint<Expr_Comprehension>.val;
    public Expr_Comprehension[] Read(THint<Expr_Comprehension[]> _)
    {
        Expr_Comprehension[] src = new Expr_Comprehension[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_Comprehension_hint);
        }
        return src;
    }
    public static readonly THint<Expr_Call> Expr_Call_hint = THint<Expr_Call>.val;
    public Expr_Call[] Read(THint<Expr_Call[]> _)
    {
        Expr_Call[] src = new Expr_Call[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_Call_hint);
        }
        return src;
    }
    public static readonly THint<Expr_Format> Expr_Format_hint = THint<Expr_Format>.val;
    public Expr_Format[] Read(THint<Expr_Format[]> _)
    {
        Expr_Format[] src = new Expr_Format[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_Format_hint);
        }
        return src;
    }
    public static readonly THint<Expr_Const> Expr_Const_hint = THint<Expr_Const>.val;
    public Expr_Const[] Read(THint<Expr_Const[]> _)
    {
        Expr_Const[] src = new Expr_Const[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_Const_hint);
        }
        return src;
    }
    public static readonly THint<Expr_Attr> Expr_Attr_hint = THint<Expr_Attr>.val;
    public Expr_Attr[] Read(THint<Expr_Attr[]> _)
    {
        Expr_Attr[] src = new Expr_Attr[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_Attr_hint);
        }
        return src;
    }
    public static readonly THint<Expr_GlobalName> Expr_GlobalName_hint = THint<Expr_GlobalName>.val;
    public Expr_GlobalName[] Read(THint<Expr_GlobalName[]> _)
    {
        Expr_GlobalName[] src = new Expr_GlobalName[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_GlobalName_hint);
        }
        return src;
    }
    public static readonly THint<Expr_LocalName> Expr_LocalName_hint = THint<Expr_LocalName>.val;
    public Expr_LocalName[] Read(THint<Expr_LocalName[]> _)
    {
        Expr_LocalName[] src = new Expr_LocalName[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_LocalName_hint);
        }
        return src;
    }
    public static readonly THint<Expr_DerefName> Expr_DerefName_hint = THint<Expr_DerefName>.val;
    public Expr_DerefName[] Read(THint<Expr_DerefName[]> _)
    {
        Expr_DerefName[] src = new Expr_DerefName[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_DerefName_hint);
        }
        return src;
    }
    public static readonly THint<Expr_Item> Expr_Item_hint = THint<Expr_Item>.val;
    public Expr_Item[] Read(THint<Expr_Item[]> _)
    {
        Expr_Item[] src = new Expr_Item[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_Item_hint);
        }
        return src;
    }
    public static readonly THint<Expr_Tuple> Expr_Tuple_hint = THint<Expr_Tuple>.val;
    public Expr_Tuple[] Read(THint<Expr_Tuple[]> _)
    {
        Expr_Tuple[] src = new Expr_Tuple[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Expr_Tuple_hint);
        }
        return src;
    }
    public static readonly THint<Arg_GlobalNameOut> Arg_GlobalNameOut_hint = THint<Arg_GlobalNameOut>.val;
    public Arg_GlobalNameOut[] Read(THint<Arg_GlobalNameOut[]> _)
    {
        Arg_GlobalNameOut[] src = new Arg_GlobalNameOut[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Arg_GlobalNameOut_hint);
        }
        return src;
    }
    public static readonly THint<Arg_LocalNameOut> Arg_LocalNameOut_hint = THint<Arg_LocalNameOut>.val;
    public Arg_LocalNameOut[] Read(THint<Arg_LocalNameOut[]> _)
    {
        Arg_LocalNameOut[] src = new Arg_LocalNameOut[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Arg_LocalNameOut_hint);
        }
        return src;
    }
    public static readonly THint<Arg_DerefNameOut> Arg_DerefNameOut_hint = THint<Arg_DerefNameOut>.val;
    public Arg_DerefNameOut[] Read(THint<Arg_DerefNameOut[]> _)
    {
        Arg_DerefNameOut[] src = new Arg_DerefNameOut[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Arg_DerefNameOut_hint);
        }
        return src;
    }
    public static readonly THint<Arg_ItemOut> Arg_ItemOut_hint = THint<Arg_ItemOut>.val;
    public Arg_ItemOut[] Read(THint<Arg_ItemOut[]> _)
    {
        Arg_ItemOut[] src = new Arg_ItemOut[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Arg_ItemOut_hint);
        }
        return src;
    }
    public static readonly THint<Arg_AttrOut> Arg_AttrOut_hint = THint<Arg_AttrOut>.val;
    public Arg_AttrOut[] Read(THint<Arg_AttrOut[]> _)
    {
        Arg_AttrOut[] src = new Arg_AttrOut[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Arg_AttrOut_hint);
        }
        return src;
    }
    public static readonly THint<Arg_Val> Arg_Val_hint = THint<Arg_Val>.val;
    public Arg_Val[] Read(THint<Arg_Val[]> _)
    {
        Arg_Val[] src = new Arg_Val[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(Arg_Val_hint);
        }
        return src;
    }
    public static readonly THint<ExceptHandler_ArbitraryCatch> ExceptHandler_ArbitraryCatch_hint = THint<ExceptHandler_ArbitraryCatch>.val;
    public ExceptHandler_ArbitraryCatch[] Read(THint<ExceptHandler_ArbitraryCatch[]> _)
    {
        ExceptHandler_ArbitraryCatch[] src = new ExceptHandler_ArbitraryCatch[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(ExceptHandler_ArbitraryCatch_hint);
        }
        return src;
    }
    public static readonly THint<ExceptHandler_TypeCheckCatch> ExceptHandler_TypeCheckCatch_hint = THint<ExceptHandler_TypeCheckCatch>.val;
    public ExceptHandler_TypeCheckCatch[] Read(THint<ExceptHandler_TypeCheckCatch[]> _)
    {
        ExceptHandler_TypeCheckCatch[] src = new ExceptHandler_TypeCheckCatch[ReadInt()];
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Read(ExceptHandler_TypeCheckCatch_hint);
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
