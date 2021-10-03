using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace DianaScript
{

    public struct Bytecode
    {
        private int[] value;

        private Bytecode(int[] value)
        {
            this.value = value;
        }

        public static implicit operator int[](Bytecode t)
        {
            return t.value;
        }

        public static implicit operator Bytecode(int[] value)
        {
            return new Bytecode(value);
        }
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
        public (int, int)[] linenos;
        public string[] freenames;
        public string[] localnames;
        public Bytecode bytecode;
    }

    public partial class AWorld
    {

    // data definition
        private static List<string> _strings = new List<string>(200);
        public static List<string> strings => _strings;
        private static int Num_strings = 0;
        private static List<InternString> _internstrings = new List<InternString>(200);
        public static List<InternString> internstrings => _internstrings;
        private static int Num_internstrings = 0;
        private static List<DObj> _dobjs = new List<DObj>(200);
        public static List<DObj> dobjs => _dobjs;
        private static int Num_dobjs = 0;
        private static List<FuncMeta> _funcmetas = new List<FuncMeta>(200);
        public static List<FuncMeta> funcmetas => _funcmetas;
        private static int Num_funcmetas = 0;


    public partial class CodeLoder
    {


        private string Read(THint<string> _) => Readstring();



        private void Load_strings()
        {
#if A_DBG
            Console.WriteLine($"start loading data storage(strings).");
#endif
            var n = ReadInt();
            for (var i = 0; i < n; i++)
            {
#if A_DBG
                Console.WriteLine($"loading data strings[{i}].");
#endif
                AWorld.strings.Add(Readstring());
            }
        }

        private InternString Read(THint<InternString> _) => ReadInternString();



        private void Load_internstrings()
        {
#if A_DBG
            Console.WriteLine($"start loading data storage(internstrings).");
#endif
            var n = ReadInt();
            for (var i = 0; i < n; i++)
            {
#if A_DBG
                Console.WriteLine($"loading data internstrings[{i}].");
#endif
                AWorld.internstrings.Add(ReadInternString());
            }
        }

        private DObj Read(THint<DObj> _) => ReadDObj();



        private void Load_dobjs()
        {
#if A_DBG
            Console.WriteLine($"start loading data storage(dobjs).");
#endif
            var n = ReadInt();
            for (var i = 0; i < n; i++)
            {
#if A_DBG
                Console.WriteLine($"loading data dobjs[{i}].");
#endif
                AWorld.dobjs.Add(ReadDObj());
            }
        }

        private FuncMeta Read(THint<FuncMeta> _) => ReadFuncMeta();

        private FuncMeta ReadFuncMeta() => new FuncMeta
        {

        };
        private void Load_funcmetas()
        {
#if A_DBG
            Console.WriteLine($"start loading data storage(funcmetas).");
#endif

            var n = ReadInt();
            for (var i = 0; i < n; i++)
            {
#if A_DBG
                Console.WriteLine($"loading data funcmetas[{i}].");
#endif
                AWorld.funcmetas.Add(ReadFuncMeta());
            }
        }


        private Bytecode Read(THint<Bytecode> _) => ReadBytecode();
        private int ToIndex_int(int x) => x;
        private int ToIndex_InternString(InternString x) => x.identity;
        private Bytecode ReadBytecode()
        {
            var codes = new int[ReadInt()];
            var offset = 0;

            while (offset < codes.Length)
            {
                var code = (int) ReadInt();
                codes[offset++] = code;
                switch ((CODETAG)  code)
                {
                    case CODETAG.Diana_FunctionDef:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_LoadGlobalRef:
                    {
                        codes[offset++] = ToIndex_InternString(Read(THint<InternString>.val));
                        break;
                    }
                    case CODETAG.Diana_DelVar:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_LoadVar:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_StoreVar:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_Action:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_Return:
                    {
                        break;
                    }
                    case CODETAG.Diana_Break:
                    {
                        break;
                    }
                    case CODETAG.Diana_Continue:
                    {
                        break;
                    }
                    case CODETAG.Diana_JumpIfNot:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_JumpIf:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_Jump:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_TryCatch:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_TryFinally:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_TryCatchFinally:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_Loop:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_For:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_With:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_GetAttr:
                    {
                        codes[offset++] = ToIndex_InternString(Read(THint<InternString>.val));
                        break;
                    }
                    case CODETAG.Diana_SetAttr:
                    {
                        codes[offset++] = ToIndex_InternString(Read(THint<InternString>.val));
                        break;
                    }
                    case CODETAG.Diana_SetAttr_Iadd:
                    {
                        codes[offset++] = ToIndex_InternString(Read(THint<InternString>.val));
                        break;
                    }
                    case CODETAG.Diana_SetAttr_Isub:
                    {
                        codes[offset++] = ToIndex_InternString(Read(THint<InternString>.val));
                        break;
                    }
                    case CODETAG.Diana_SetAttr_Imul:
                    {
                        codes[offset++] = ToIndex_InternString(Read(THint<InternString>.val));
                        break;
                    }
                    case CODETAG.Diana_SetAttr_Itruediv:
                    {
                        codes[offset++] = ToIndex_InternString(Read(THint<InternString>.val));
                        break;
                    }
                    case CODETAG.Diana_SetAttr_Ifloordiv:
                    {
                        codes[offset++] = ToIndex_InternString(Read(THint<InternString>.val));
                        break;
                    }
                    case CODETAG.Diana_SetAttr_Imod:
                    {
                        codes[offset++] = ToIndex_InternString(Read(THint<InternString>.val));
                        break;
                    }
                    case CODETAG.Diana_SetAttr_Ipow:
                    {
                        codes[offset++] = ToIndex_InternString(Read(THint<InternString>.val));
                        break;
                    }
                    case CODETAG.Diana_SetAttr_Ilshift:
                    {
                        codes[offset++] = ToIndex_InternString(Read(THint<InternString>.val));
                        break;
                    }
                    case CODETAG.Diana_SetAttr_Irshift:
                    {
                        codes[offset++] = ToIndex_InternString(Read(THint<InternString>.val));
                        break;
                    }
                    case CODETAG.Diana_SetAttr_Ibitor:
                    {
                        codes[offset++] = ToIndex_InternString(Read(THint<InternString>.val));
                        break;
                    }
                    case CODETAG.Diana_SetAttr_Ibitand:
                    {
                        codes[offset++] = ToIndex_InternString(Read(THint<InternString>.val));
                        break;
                    }
                    case CODETAG.Diana_SetAttr_Ibitxor:
                    {
                        codes[offset++] = ToIndex_InternString(Read(THint<InternString>.val));
                        break;
                    }
                    case CODETAG.Diana_DelItem:
                    {
                        break;
                    }
                    case CODETAG.Diana_GetItem:
                    {
                        break;
                    }
                    case CODETAG.Diana_SetItem:
                    {
                        break;
                    }
                    case CODETAG.Diana_SetItem_Iadd:
                    {
                        break;
                    }
                    case CODETAG.Diana_SetItem_Isub:
                    {
                        break;
                    }
                    case CODETAG.Diana_SetItem_Imul:
                    {
                        break;
                    }
                    case CODETAG.Diana_SetItem_Itruediv:
                    {
                        break;
                    }
                    case CODETAG.Diana_SetItem_Ifloordiv:
                    {
                        break;
                    }
                    case CODETAG.Diana_SetItem_Imod:
                    {
                        break;
                    }
                    case CODETAG.Diana_SetItem_Ipow:
                    {
                        break;
                    }
                    case CODETAG.Diana_SetItem_Ilshift:
                    {
                        break;
                    }
                    case CODETAG.Diana_SetItem_Irshift:
                    {
                        break;
                    }
                    case CODETAG.Diana_SetItem_Ibitor:
                    {
                        break;
                    }
                    case CODETAG.Diana_SetItem_Ibitand:
                    {
                        break;
                    }
                    case CODETAG.Diana_SetItem_Ibitxor:
                    {
                        break;
                    }
                    case CODETAG.Diana_add:
                    {
                        break;
                    }
                    case CODETAG.Diana_sub:
                    {
                        break;
                    }
                    case CODETAG.Diana_mul:
                    {
                        break;
                    }
                    case CODETAG.Diana_truediv:
                    {
                        break;
                    }
                    case CODETAG.Diana_floordiv:
                    {
                        break;
                    }
                    case CODETAG.Diana_mod:
                    {
                        break;
                    }
                    case CODETAG.Diana_pow:
                    {
                        break;
                    }
                    case CODETAG.Diana_lshift:
                    {
                        break;
                    }
                    case CODETAG.Diana_rshift:
                    {
                        break;
                    }
                    case CODETAG.Diana_bitor:
                    {
                        break;
                    }
                    case CODETAG.Diana_bitand:
                    {
                        break;
                    }
                    case CODETAG.Diana_bitxor:
                    {
                        break;
                    }
                    case CODETAG.Diana_gt:
                    {
                        break;
                    }
                    case CODETAG.Diana_lt:
                    {
                        break;
                    }
                    case CODETAG.Diana_ge:
                    {
                        break;
                    }
                    case CODETAG.Diana_le:
                    {
                        break;
                    }
                    case CODETAG.Diana_eq:
                    {
                        break;
                    }
                    case CODETAG.Diana_ne:
                    {
                        break;
                    }
                    case CODETAG.Diana_in:
                    {
                        break;
                    }
                    case CODETAG.Diana_notin:
                    {
                        break;
                    }
                    case CODETAG.Diana_UnaryOp_invert:
                    {
                        break;
                    }
                    case CODETAG.Diana_UnaryOp_not:
                    {
                        break;
                    }
                    case CODETAG.Diana_UnaryOp_neg:
                    {
                        break;
                    }
                    case CODETAG.Diana_MKDict:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_MKSet:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_MKList:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_Call:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_Format:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_Const:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_MKTuple:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_Pack:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_Replicate:
                    {
                        codes[offset++] = ToIndex_int(Read(THint<int>.val));
                        break;
                    }
                    case CODETAG.Diana_Pop:
                    {
                        break;
                    }
                    default:
                        throw new InvalidDataException($"invalid code {code}");
                }
            }
            return codes;
        }

        private static readonly object _loaderSync = new object();

        public int LoadCode()
        {
            var metadataIndForEntryPoint = ReadInt();
            lock(_loaderSync)
            {
                    Load_strings();
                    Load_internstrings();
                    Load_dobjs();
                    Load_funcmetas();
                    Num_strings = strings.Count;
                    Num_internstrings = internstrings.Count;
                    Num_dobjs = dobjs.Count;
                    Num_funcmetas = funcmetas.Count;
            }
            return metadataIndForEntryPoint;
        }


    } // loader class
    } // aworld
}