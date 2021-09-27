using System;
using System.Collections.Generic;
namespace DianaScript
{
public class Stmt_FunctionDef
{
    public int metadataInd;

    public Ptr code;

}

public class Stmt_Return
{
    public Ptr value;

}

public class Stmt_DelGlobalName
{
    public int slot;

}

public class Stmt_DelLocalName
{
    public int slot;

}

public class Stmt_DelDerefName
{
    public int slot;

}

public class Stmt_DeleteItem
{
    public Ptr value;

    public Ptr item;

}

public class Stmt_Assign
{
    public Ptr targets;

    public Ptr value;

}

public class Stmt_AddAssign
{
    public Ptr target;

    public Ptr value;

}

public class Stmt_SubAssign
{
    public Ptr target;

    public Ptr value;

}

public class Stmt_MutAssign
{
    public Ptr target;

    public Ptr value;

}

public class Stmt_TrueDivAssign
{
    public Ptr target;

    public Ptr value;

}

public class Stmt_FloorDivAssign
{
    public Ptr target;

    public Ptr value;

}

public class Stmt_ModAssign
{
    public Ptr target;

    public Ptr value;

}

public class Stmt_PowAssign
{
    public Ptr target;

    public Ptr value;

}

public class Stmt_LShiftAssign
{
    public Ptr target;

    public Ptr value;

}

public class Stmt_RShiftAssign
{
    public Ptr target;

    public Ptr value;

}

public class Stmt_BitOrAssign
{
    public Ptr target;

    public Ptr value;

}

public class Stmt_BitAndAssign
{
    public Ptr target;

    public Ptr value;

}

public class Stmt_BitXorAssign
{
    public Ptr target;

    public Ptr value;

}

public class Stmt_For
{
    public Ptr target;

    public Ptr iter;

    public Ptr body;

}

public class Stmt_While
{
    public Ptr cond;

    public Ptr body;

}

public class Stmt_If
{
    public Ptr cond;

    public Ptr then;

    public Ptr orelse;

}

public class Stmt_With
{
    public Ptr expr;

    public Ptr body;

}

public class Stmt_Raise
{
    public Ptr value;

}

public class Stmt_RaiseFrom
{
    public Ptr value;

    public Ptr from_;

}

public class Stmt_Try
{
    public Ptr body;

    public int err_slot;

    public Ptr except_handlers;

    public Ptr final_body;

}

public class Stmt_Assert
{
    public Ptr value;

}

public class Stmt_ExprStmt
{
    public Ptr value;

}

public class Stmt_Control
{
    public int kind;

}

public class Stmt_Block
{
    public Ptr[] stmts;

}

public class Expr_AddOp
{
    public Ptr left;

    public Ptr right;

}

public class Expr_SubOp
{
    public Ptr left;

    public Ptr right;

}

public class Expr_MutOp
{
    public Ptr left;

    public Ptr right;

}

public class Expr_TrueDivOp
{
    public Ptr left;

    public Ptr right;

}

public class Expr_FloorDivOp
{
    public Ptr left;

    public Ptr right;

}

public class Expr_ModOp
{
    public Ptr left;

    public Ptr right;

}

public class Expr_PowOp
{
    public Ptr left;

    public Ptr right;

}

public class Expr_LShiftOp
{
    public Ptr left;

    public Ptr right;

}

public class Expr_RShiftOp
{
    public Ptr left;

    public Ptr right;

}

public class Expr_BitOrOp
{
    public Ptr left;

    public Ptr right;

}

public class Expr_BitAndOp
{
    public Ptr left;

    public Ptr right;

}

public class Expr_BitXorOp
{
    public Ptr left;

    public Ptr right;

}

public class Expr_GlobalBinder
{
    public int slot;

}

public class Expr_LocalBinder
{
    public int slot;

}

public class Expr_DerefBinder
{
    public int slot;

}

public class Expr_AndOp
{
    public Ptr left;

    public Ptr right;

}

public class Expr_OrOp
{
    public Ptr left;

    public Ptr right;

}

public class Expr_InvertOp
{
    public Ptr value;

}

public class Expr_NotOp
{
    public Ptr value;

}

public class Expr_Lambda
{
    public int[] frees;

    public int code;

}

public class Expr_IfExpr
{
    public Ptr cond;

    public Ptr then;

    public Ptr orelse;

}

public class Expr_Dict
{
    public Ptr[] keys;

    public Ptr[] values;

}

public class Expr_Set
{
    public Ptr[] elts;

}

public class Expr_List
{
    public Ptr[] elts;

}

public class Expr_Generator
{
    public Ptr target;

    public Ptr iter;

    public Ptr body;

}

public class Expr_Comprehension
{
    public Ptr adder;

    public Ptr target;

    public Ptr iter;

    public Ptr body;

}

public class Expr_Call
{
    public Ptr f;

    public Ptr args;

}

public class Expr_Format
{
    public int format;

    public Ptr args;

}

public class Expr_Const
{
    public int constInd;

}

public class Expr_Attr
{
    public Ptr value;

    public int attr;

}

public class Expr_GlobalName
{
    public int slot;

}

public class Expr_LocalName
{
    public int slot;

}

public class Expr_DerefName
{
    public int slot;

}

public class Expr_Item
{
    public Ptr value;

    public Ptr item;

}

public class Expr_Tuple
{
    public Ptr[] elts;

}

public class Arg_GlobalNameOut
{
    public int ind;

}

public class Arg_LocalNameOut
{
    public int ind;

}

public class Arg_DerefNameOut
{
    public int ind;

}

public class Arg_ItemOut
{
    public Ptr value;

    public Ptr item;

}

public class Arg_AttrOut
{
    public Ptr value;

    public int attr;

}

public class Arg_Val
{
    public Ptr value;

}

public class ExceptHandler_ArbitraryCatch
{
    public int assign_slot;

    public Ptr body;

}

public class ExceptHandler_TypeCheckCatch
{
    public Ptr type;

    public int assign_slot;

    public Ptr body;

}

public enum CODE
{
    Stmt_FunctionDef,
    Stmt_Return,
    Stmt_DelGlobalName,
    Stmt_DelLocalName,
    Stmt_DelDerefName,
    Stmt_DeleteItem,
    Stmt_Assign,
    Stmt_AddAssign,
    Stmt_SubAssign,
    Stmt_MutAssign,
    Stmt_TrueDivAssign,
    Stmt_FloorDivAssign,
    Stmt_ModAssign,
    Stmt_PowAssign,
    Stmt_LShiftAssign,
    Stmt_RShiftAssign,
    Stmt_BitOrAssign,
    Stmt_BitAndAssign,
    Stmt_BitXorAssign,
    Stmt_For,
    Stmt_While,
    Stmt_If,
    Stmt_With,
    Stmt_Raise,
    Stmt_RaiseFrom,
    Stmt_Try,
    Stmt_Assert,
    Stmt_ExprStmt,
    Stmt_Control,
    Stmt_Block,
    Expr_AddOp,
    Expr_SubOp,
    Expr_MutOp,
    Expr_TrueDivOp,
    Expr_FloorDivOp,
    Expr_ModOp,
    Expr_PowOp,
    Expr_LShiftOp,
    Expr_RShiftOp,
    Expr_BitOrOp,
    Expr_BitAndOp,
    Expr_BitXorOp,
    Expr_GlobalBinder,
    Expr_LocalBinder,
    Expr_DerefBinder,
    Expr_AndOp,
    Expr_OrOp,
    Expr_InvertOp,
    Expr_NotOp,
    Expr_Lambda,
    Expr_IfExpr,
    Expr_Dict,
    Expr_Set,
    Expr_List,
    Expr_Generator,
    Expr_Comprehension,
    Expr_Call,
    Expr_Format,
    Expr_Const,
    Expr_Attr,
    Expr_GlobalName,
    Expr_LocalName,
    Expr_DerefName,
    Expr_Item,
    Expr_Tuple,
    Arg_GlobalNameOut,
    Arg_LocalNameOut,
    Arg_DerefNameOut,
    Arg_ItemOut,
    Arg_AttrOut,
    Arg_Val,
    ExceptHandler_ArbitraryCatch,
    ExceptHandler_TypeCheckCatch,
}

public class DFlatGraphCode
{
    public Stmt_FunctionDef[] stmt_functiondefs;

    public Stmt_Return[] stmt_returns;

    public Stmt_DelGlobalName[] stmt_delglobalnames;

    public Stmt_DelLocalName[] stmt_dellocalnames;

    public Stmt_DelDerefName[] stmt_delderefnames;

    public Stmt_DeleteItem[] stmt_deleteitems;

    public Stmt_Assign[] stmt_assigns;

    public Stmt_AddAssign[] stmt_addassigns;

    public Stmt_SubAssign[] stmt_subassigns;

    public Stmt_MutAssign[] stmt_mutassigns;

    public Stmt_TrueDivAssign[] stmt_truedivassigns;

    public Stmt_FloorDivAssign[] stmt_floordivassigns;

    public Stmt_ModAssign[] stmt_modassigns;

    public Stmt_PowAssign[] stmt_powassigns;

    public Stmt_LShiftAssign[] stmt_lshiftassigns;

    public Stmt_RShiftAssign[] stmt_rshiftassigns;

    public Stmt_BitOrAssign[] stmt_bitorassigns;

    public Stmt_BitAndAssign[] stmt_bitandassigns;

    public Stmt_BitXorAssign[] stmt_bitxorassigns;

    public Stmt_For[] stmt_fors;

    public Stmt_While[] stmt_whiles;

    public Stmt_If[] stmt_ifs;

    public Stmt_With[] stmt_withs;

    public Stmt_Raise[] stmt_raises;

    public Stmt_RaiseFrom[] stmt_raisefroms;

    public Stmt_Try[] stmt_trys;

    public Stmt_Assert[] stmt_asserts;

    public Stmt_ExprStmt[] stmt_exprstmts;

    public Stmt_Control[] stmt_controls;

    public Stmt_Block[] stmt_blocks;

    public Expr_AddOp[] expr_addops;

    public Expr_SubOp[] expr_subops;

    public Expr_MutOp[] expr_mutops;

    public Expr_TrueDivOp[] expr_truedivops;

    public Expr_FloorDivOp[] expr_floordivops;

    public Expr_ModOp[] expr_modops;

    public Expr_PowOp[] expr_powops;

    public Expr_LShiftOp[] expr_lshiftops;

    public Expr_RShiftOp[] expr_rshiftops;

    public Expr_BitOrOp[] expr_bitorops;

    public Expr_BitAndOp[] expr_bitandops;

    public Expr_BitXorOp[] expr_bitxorops;

    public Expr_GlobalBinder[] expr_globalbinders;

    public Expr_LocalBinder[] expr_localbinders;

    public Expr_DerefBinder[] expr_derefbinders;

    public Expr_AndOp[] expr_andops;

    public Expr_OrOp[] expr_orops;

    public Expr_InvertOp[] expr_invertops;

    public Expr_NotOp[] expr_notops;

    public Expr_Lambda[] expr_lambdas;

    public Expr_IfExpr[] expr_ifexprs;

    public Expr_Dict[] expr_dicts;

    public Expr_Set[] expr_sets;

    public Expr_List[] expr_lists;

    public Expr_Generator[] expr_generators;

    public Expr_Comprehension[] expr_comprehensions;

    public Expr_Call[] expr_calls;

    public Expr_Format[] expr_formats;

    public Expr_Const[] expr_consts;

    public Expr_Attr[] expr_attrs;

    public Expr_GlobalName[] expr_globalnames;

    public Expr_LocalName[] expr_localnames;

    public Expr_DerefName[] expr_derefnames;

    public Expr_Item[] expr_items;

    public Expr_Tuple[] expr_tuples;

    public Arg_GlobalNameOut[] arg_globalnameouts;

    public Arg_LocalNameOut[] arg_localnameouts;

    public Arg_DerefNameOut[] arg_derefnameouts;

    public Arg_ItemOut[] arg_itemouts;

    public Arg_AttrOut[] arg_attrouts;

    public Arg_Val[] arg_vals;

    public ExceptHandler_ArbitraryCatch[] excepthandler_arbitrarycatchs;

    public ExceptHandler_TypeCheckCatch[] excepthandler_typecheckcatchs;

}
}
