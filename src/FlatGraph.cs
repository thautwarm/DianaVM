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
        public (int, int)[] location_data;
        public string filename;

        public static Block Make(Ptr[] codes, (int, int)[] location_data, string filename) => new Block
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
        Diana_FunctionDef = 0,
        Diana_LoadGlobalRef = 1,
        Diana_DelVar = 2,
        Diana_LoadVar = 3,
        Diana_StoreVar = 4,
        Diana_Action = 5,
        Diana_ControlIf = 6,
        Diana_JumpIfNot = 7,
        Diana_JumpIf = 8,
        Diana_Jump = 9,
        Diana_Control = 10,
        Diana_Try = 11,
        Diana_Loop = 12,
        Diana_For = 13,
        Diana_With = 14,
        Diana_GetAttr = 15,
        Diana_SetAttr = 16,
        Diana_SetAttr_Iadd = 17,
        Diana_SetAttr_Isub = 18,
        Diana_SetAttr_Imul = 19,
        Diana_SetAttr_Itruediv = 20,
        Diana_SetAttr_Ifloordiv = 21,
        Diana_SetAttr_Imod = 22,
        Diana_SetAttr_Ipow = 23,
        Diana_SetAttr_Ilshift = 24,
        Diana_SetAttr_Irshift = 25,
        Diana_SetAttr_Ibitor = 26,
        Diana_SetAttr_Ibitand = 27,
        Diana_SetAttr_Ibitxor = 28,
        Diana_DelItem = 29,
        Diana_GetItem = 30,
        Diana_SetItem = 31,
        Diana_SetItem_Iadd = 32,
        Diana_SetItem_Isub = 33,
        Diana_SetItem_Imul = 34,
        Diana_SetItem_Itruediv = 35,
        Diana_SetItem_Ifloordiv = 36,
        Diana_SetItem_Imod = 37,
        Diana_SetItem_Ipow = 38,
        Diana_SetItem_Ilshift = 39,
        Diana_SetItem_Irshift = 40,
        Diana_SetItem_Ibitor = 41,
        Diana_SetItem_Ibitand = 42,
        Diana_SetItem_Ibitxor = 43,
        Diana_add = 44,
        Diana_sub = 45,
        Diana_mul = 46,
        Diana_truediv = 47,
        Diana_floordiv = 48,
        Diana_mod = 49,
        Diana_pow = 50,
        Diana_lshift = 51,
        Diana_rshift = 52,
        Diana_bitor = 53,
        Diana_bitand = 54,
        Diana_bitxor = 55,
        Diana_gt = 56,
        Diana_lt = 57,
        Diana_ge = 58,
        Diana_le = 59,
        Diana_eq = 60,
        Diana_ne = 61,
        Diana_in = 62,
        Diana_notin = 63,
        Diana_UnaryOp_invert = 64,
        Diana_UnaryOp_not = 65,
        Diana_UnaryOp_neg = 66,
        Diana_MKDict = 67,
        Diana_MKSet = 68,
        Diana_MKList = 69,
        Diana_Call = 70,
        Diana_Format = 71,
        Diana_Const = 72,
        Diana_MKTuple = 73,
        Diana_Pack = 74,
        Diana_Replicate = 75,
        Diana_Pop = 76,
    }

    public partial class AWorld
    {
        private static readonly object _loaderSync = new object();
        public static List<string> strings = new List<string>(200);
        private static int Num_string = 0;

        public static List<DObj> dobjs = new List<DObj>(200);
        private static int Num_DObj = 0;

        public static List<InternString> internstrings = new List<InternString>(200);
        private static int Num_InternString = 0;

        public static List<Catch> catchs = new List<Catch>(200);
        private static int Num_Catch = 0;

        public static List<FuncMeta> funcmetas = new List<FuncMeta>(200);
        private static int Num_FuncMeta = 0;

        public static List<Block> blocks = new List<Block>(200);
        private static int Num_Block = 0;

        public static List<Diana_FunctionDef> diana_functiondefs = new List<Diana_FunctionDef>(200);
        private static int Num_Diana_FunctionDef = 0;

        public static List<Diana_LoadGlobalRef> diana_loadglobalrefs = new List<Diana_LoadGlobalRef>(200);
        private static int Num_Diana_LoadGlobalRef = 0;

        public static List<Diana_DelVar> diana_delvars = new List<Diana_DelVar>(200);
        private static int Num_Diana_DelVar = 0;

        public static List<Diana_LoadVar> diana_loadvars = new List<Diana_LoadVar>(200);
        private static int Num_Diana_LoadVar = 0;

        public static List<Diana_StoreVar> diana_storevars = new List<Diana_StoreVar>(200);
        private static int Num_Diana_StoreVar = 0;

        public static List<Diana_Action> diana_actions = new List<Diana_Action>(200);
        private static int Num_Diana_Action = 0;

        public static List<Diana_ControlIf> diana_controlifs = new List<Diana_ControlIf>(200);
        private static int Num_Diana_ControlIf = 0;

        public static List<Diana_JumpIfNot> diana_jumpifnots = new List<Diana_JumpIfNot>(200);
        private static int Num_Diana_JumpIfNot = 0;

        public static List<Diana_JumpIf> diana_jumpifs = new List<Diana_JumpIf>(200);
        private static int Num_Diana_JumpIf = 0;

        public static List<Diana_Jump> diana_jumps = new List<Diana_Jump>(200);
        private static int Num_Diana_Jump = 0;

        public static List<Diana_Control> diana_controls = new List<Diana_Control>(200);
        private static int Num_Diana_Control = 0;

        public static List<Diana_Try> diana_trys = new List<Diana_Try>(200);
        private static int Num_Diana_Try = 0;

        public static List<Diana_Loop> diana_loops = new List<Diana_Loop>(200);
        private static int Num_Diana_Loop = 0;

        public static List<Diana_For> diana_fors = new List<Diana_For>(200);
        private static int Num_Diana_For = 0;

        public static List<Diana_With> diana_withs = new List<Diana_With>(200);
        private static int Num_Diana_With = 0;

        public static List<Diana_GetAttr> diana_getattrs = new List<Diana_GetAttr>(200);
        private static int Num_Diana_GetAttr = 0;

        public static List<Diana_SetAttr> diana_setattrs = new List<Diana_SetAttr>(200);
        private static int Num_Diana_SetAttr = 0;

        public static List<Diana_SetAttr_Iadd> diana_setattr_iadds = new List<Diana_SetAttr_Iadd>(200);
        private static int Num_Diana_SetAttr_Iadd = 0;

        public static List<Diana_SetAttr_Isub> diana_setattr_isubs = new List<Diana_SetAttr_Isub>(200);
        private static int Num_Diana_SetAttr_Isub = 0;

        public static List<Diana_SetAttr_Imul> diana_setattr_imuls = new List<Diana_SetAttr_Imul>(200);
        private static int Num_Diana_SetAttr_Imul = 0;

        public static List<Diana_SetAttr_Itruediv> diana_setattr_itruedivs = new List<Diana_SetAttr_Itruediv>(200);
        private static int Num_Diana_SetAttr_Itruediv = 0;

        public static List<Diana_SetAttr_Ifloordiv> diana_setattr_ifloordivs = new List<Diana_SetAttr_Ifloordiv>(200);
        private static int Num_Diana_SetAttr_Ifloordiv = 0;

        public static List<Diana_SetAttr_Imod> diana_setattr_imods = new List<Diana_SetAttr_Imod>(200);
        private static int Num_Diana_SetAttr_Imod = 0;

        public static List<Diana_SetAttr_Ipow> diana_setattr_ipows = new List<Diana_SetAttr_Ipow>(200);
        private static int Num_Diana_SetAttr_Ipow = 0;

        public static List<Diana_SetAttr_Ilshift> diana_setattr_ilshifts = new List<Diana_SetAttr_Ilshift>(200);
        private static int Num_Diana_SetAttr_Ilshift = 0;

        public static List<Diana_SetAttr_Irshift> diana_setattr_irshifts = new List<Diana_SetAttr_Irshift>(200);
        private static int Num_Diana_SetAttr_Irshift = 0;

        public static List<Diana_SetAttr_Ibitor> diana_setattr_ibitors = new List<Diana_SetAttr_Ibitor>(200);
        private static int Num_Diana_SetAttr_Ibitor = 0;

        public static List<Diana_SetAttr_Ibitand> diana_setattr_ibitands = new List<Diana_SetAttr_Ibitand>(200);
        private static int Num_Diana_SetAttr_Ibitand = 0;

        public static List<Diana_SetAttr_Ibitxor> diana_setattr_ibitxors = new List<Diana_SetAttr_Ibitxor>(200);
        private static int Num_Diana_SetAttr_Ibitxor = 0;

        public static List<Diana_MKDict> diana_mkdicts = new List<Diana_MKDict>(200);
        private static int Num_Diana_MKDict = 0;

        public static List<Diana_MKSet> diana_mksets = new List<Diana_MKSet>(200);
        private static int Num_Diana_MKSet = 0;

        public static List<Diana_MKList> diana_mklists = new List<Diana_MKList>(200);
        private static int Num_Diana_MKList = 0;

        public static List<Diana_Call> diana_calls = new List<Diana_Call>(200);
        private static int Num_Diana_Call = 0;

        public static List<Diana_Format> diana_formats = new List<Diana_Format>(200);
        private static int Num_Diana_Format = 0;

        public static List<Diana_Const> diana_consts = new List<Diana_Const>(200);
        private static int Num_Diana_Const = 0;

        public static List<Diana_MKTuple> diana_mktuples = new List<Diana_MKTuple>(200);
        private static int Num_Diana_MKTuple = 0;

        public static List<Diana_Pack> diana_packs = new List<Diana_Pack>(200);
        private static int Num_Diana_Pack = 0;

        public static List<Diana_Replicate> diana_replicates = new List<Diana_Replicate>(200);
        private static int Num_Diana_Replicate = 0;

    }
}
