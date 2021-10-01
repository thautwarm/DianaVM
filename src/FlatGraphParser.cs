using System;
using System.IO;
using System.Collections.Generic;
namespace DianaScript
{
    public partial class AWorld
    {
        private CODE ReadCODE()
        {
            fileStream.Read(cache_4byte, 0, 1);
            return (CODE)cache_4byte[0];
        }

        private Ptr ReadFromCode(CODE code) => code switch
        {
            CODE.Diana_FunctionDef => new Ptr(code, Num_Diana_FunctionDef + ReadInt()),
            CODE.Diana_LoadGlobalRef => new Ptr(code, Num_Diana_LoadGlobalRef + ReadInt()),
            CODE.Diana_DelVar => new Ptr(code, Num_Diana_DelVar + ReadInt()),
            CODE.Diana_LoadVar => new Ptr(code, Num_Diana_LoadVar + ReadInt()),
            CODE.Diana_StoreVar => new Ptr(code, Num_Diana_StoreVar + ReadInt()),
            CODE.Diana_Action => new Ptr(code, Num_Diana_Action + ReadInt()),
            CODE.Diana_ControlIf => new Ptr(code, Num_Diana_ControlIf + ReadInt()),
            CODE.Diana_JumpIfNot => new Ptr(code, Num_Diana_JumpIfNot + ReadInt()),
            CODE.Diana_JumpIf => new Ptr(code, Num_Diana_JumpIf + ReadInt()),
            CODE.Diana_Jump => new Ptr(code, Num_Diana_Jump + ReadInt()),
            CODE.Diana_Control => new Ptr(code, Num_Diana_Control + ReadInt()),
            CODE.Diana_Try => new Ptr(code, Num_Diana_Try + ReadInt()),
            CODE.Diana_Loop => new Ptr(code, Num_Diana_Loop + ReadInt()),
            CODE.Diana_For => new Ptr(code, Num_Diana_For + ReadInt()),
            CODE.Diana_With => new Ptr(code, Num_Diana_With + ReadInt()),
            CODE.Diana_GetAttr => new Ptr(code, Num_Diana_GetAttr + ReadInt()),
            CODE.Diana_SetAttr => new Ptr(code, Num_Diana_SetAttr + ReadInt()),
            CODE.Diana_SetAttr_Iadd => new Ptr(code, Num_Diana_SetAttr_Iadd + ReadInt()),
            CODE.Diana_SetAttr_Isub => new Ptr(code, Num_Diana_SetAttr_Isub + ReadInt()),
            CODE.Diana_SetAttr_Imul => new Ptr(code, Num_Diana_SetAttr_Imul + ReadInt()),
            CODE.Diana_SetAttr_Itruediv => new Ptr(code, Num_Diana_SetAttr_Itruediv + ReadInt()),
            CODE.Diana_SetAttr_Ifloordiv => new Ptr(code, Num_Diana_SetAttr_Ifloordiv + ReadInt()),
            CODE.Diana_SetAttr_Imod => new Ptr(code, Num_Diana_SetAttr_Imod + ReadInt()),
            CODE.Diana_SetAttr_Ipow => new Ptr(code, Num_Diana_SetAttr_Ipow + ReadInt()),
            CODE.Diana_SetAttr_Ilshift => new Ptr(code, Num_Diana_SetAttr_Ilshift + ReadInt()),
            CODE.Diana_SetAttr_Irshift => new Ptr(code, Num_Diana_SetAttr_Irshift + ReadInt()),
            CODE.Diana_SetAttr_Ibitor => new Ptr(code, Num_Diana_SetAttr_Ibitor + ReadInt()),
            CODE.Diana_SetAttr_Ibitand => new Ptr(code, Num_Diana_SetAttr_Ibitand + ReadInt()),
            CODE.Diana_SetAttr_Ibitxor => new Ptr(code, Num_Diana_SetAttr_Ibitxor + ReadInt()),
            CODE.Diana_DelItem => new Ptr(code, 0),
            CODE.Diana_GetItem => new Ptr(code, 0),
            CODE.Diana_SetItem => new Ptr(code, 0),
            CODE.Diana_SetItem_Iadd => new Ptr(code, 0),
            CODE.Diana_SetItem_Isub => new Ptr(code, 0),
            CODE.Diana_SetItem_Imul => new Ptr(code, 0),
            CODE.Diana_SetItem_Itruediv => new Ptr(code, 0),
            CODE.Diana_SetItem_Ifloordiv => new Ptr(code, 0),
            CODE.Diana_SetItem_Imod => new Ptr(code, 0),
            CODE.Diana_SetItem_Ipow => new Ptr(code, 0),
            CODE.Diana_SetItem_Ilshift => new Ptr(code, 0),
            CODE.Diana_SetItem_Irshift => new Ptr(code, 0),
            CODE.Diana_SetItem_Ibitor => new Ptr(code, 0),
            CODE.Diana_SetItem_Ibitand => new Ptr(code, 0),
            CODE.Diana_SetItem_Ibitxor => new Ptr(code, 0),
            CODE.Diana_add => new Ptr(code, 0),
            CODE.Diana_sub => new Ptr(code, 0),
            CODE.Diana_mul => new Ptr(code, 0),
            CODE.Diana_truediv => new Ptr(code, 0),
            CODE.Diana_floordiv => new Ptr(code, 0),
            CODE.Diana_mod => new Ptr(code, 0),
            CODE.Diana_pow => new Ptr(code, 0),
            CODE.Diana_lshift => new Ptr(code, 0),
            CODE.Diana_rshift => new Ptr(code, 0),
            CODE.Diana_bitor => new Ptr(code, 0),
            CODE.Diana_bitand => new Ptr(code, 0),
            CODE.Diana_bitxor => new Ptr(code, 0),
            CODE.Diana_gt => new Ptr(code, 0),
            CODE.Diana_lt => new Ptr(code, 0),
            CODE.Diana_ge => new Ptr(code, 0),
            CODE.Diana_le => new Ptr(code, 0),
            CODE.Diana_eq => new Ptr(code, 0),
            CODE.Diana_ne => new Ptr(code, 0),
            CODE.Diana_in => new Ptr(code, 0),
            CODE.Diana_notin => new Ptr(code, 0),
            CODE.Diana_UnaryOp_invert => new Ptr(code, 0),
            CODE.Diana_UnaryOp_not => new Ptr(code, 0),
            CODE.Diana_UnaryOp_neg => new Ptr(code, 0),
            CODE.Diana_MKDict => new Ptr(code, Num_Diana_MKDict + ReadInt()),
            CODE.Diana_MKSet => new Ptr(code, Num_Diana_MKSet + ReadInt()),
            CODE.Diana_MKList => new Ptr(code, Num_Diana_MKList + ReadInt()),
            CODE.Diana_Call => new Ptr(code, Num_Diana_Call + ReadInt()),
            CODE.Diana_Format => new Ptr(code, Num_Diana_Format + ReadInt()),
            CODE.Diana_Const => new Ptr(code, Num_Diana_Const + ReadInt()),
            CODE.Diana_MKTuple => new Ptr(code, Num_Diana_MKTuple + ReadInt()),
            CODE.Diana_Pack => new Ptr(code, Num_Diana_Pack + ReadInt()),
            CODE.Diana_Replicate => new Ptr(code, Num_Diana_Replicate + ReadInt()),
            CODE.Diana_Pop => new Ptr(code, 0),
            _ => throw new ArgumentOutOfRangeException("unknown code {code}.")
        };
        private Ptr Read(THint<Ptr> _) => ReadFromCode(ReadCODE());
        private void Load_strings()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                strings.Add(Read(THint<string>.val));
        }
        private void Load_dobjs()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                dobjs.Add(Read(THint<DObj>.val));
        }
        private void Load_internstrings()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                internstrings.Add(Read(THint<InternString>.val));
        }
        private Catch ReadCatch() => new Catch
        {
            exc_type = Read(THint<int>.val),
            body = Read(THint<int>.val),
        };

        private Catch Read(THint<Catch> _) => ReadCatch();
        private void Load_catchs()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                catchs.Add(ReadCatch());
        }

        private FuncMeta ReadFuncMeta() => new FuncMeta
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

        private FuncMeta Read(THint<FuncMeta> _) => ReadFuncMeta();
        private void Load_funcmetas()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                funcmetas.Add(ReadFuncMeta());
        }

        private Block ReadBlock() => new Block
        {
            codes = Read(THint<Ptr[]>.val),
            location_data = Read(THint<(int, int)[]>.val),
            filename = Read(THint<string>.val),
        };

        private Block Read(THint<Block> _) => ReadBlock();
        private void Load_blocks()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                blocks.Add(ReadBlock());
        }

        private Diana_FunctionDef ReadDiana_FunctionDef() => new Diana_FunctionDef
        {
            metadataInd = Read(THint<int>.val),
            code = Read(THint<int>.val),
        };

        private void Load_diana_functiondefs()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_functiondefs.Add(ReadDiana_FunctionDef());
        }

        private Diana_LoadGlobalRef ReadDiana_LoadGlobalRef() => new Diana_LoadGlobalRef
        {
            istr = Read(THint<InternString>.val),
        };

        private void Load_diana_loadglobalrefs()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_loadglobalrefs.Add(ReadDiana_LoadGlobalRef());
        }

        private Diana_DelVar ReadDiana_DelVar() => new Diana_DelVar
        {
            targets = Read(THint<int[]>.val),
        };

        private void Load_diana_delvars()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_delvars.Add(ReadDiana_DelVar());
        }

        private Diana_LoadVar ReadDiana_LoadVar() => new Diana_LoadVar
        {
            i = Read(THint<int>.val),
        };

        private void Load_diana_loadvars()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_loadvars.Add(ReadDiana_LoadVar());
        }

        private Diana_StoreVar ReadDiana_StoreVar() => new Diana_StoreVar
        {
            i = Read(THint<int>.val),
        };

        private void Load_diana_storevars()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_storevars.Add(ReadDiana_StoreVar());
        }

        private Diana_Action ReadDiana_Action() => new Diana_Action
        {
            kind = Read(THint<int>.val),
        };

        private void Load_diana_actions()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_actions.Add(ReadDiana_Action());
        }

        private Diana_ControlIf ReadDiana_ControlIf() => new Diana_ControlIf
        {
            arg = Read(THint<int>.val),
        };

        private void Load_diana_controlifs()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_controlifs.Add(ReadDiana_ControlIf());
        }

        private Diana_JumpIfNot ReadDiana_JumpIfNot() => new Diana_JumpIfNot
        {
            off = Read(THint<int>.val),
        };

        private void Load_diana_jumpifnots()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_jumpifnots.Add(ReadDiana_JumpIfNot());
        }

        private Diana_JumpIf ReadDiana_JumpIf() => new Diana_JumpIf
        {
            off = Read(THint<int>.val),
        };

        private void Load_diana_jumpifs()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_jumpifs.Add(ReadDiana_JumpIf());
        }

        private Diana_Jump ReadDiana_Jump() => new Diana_Jump
        {
            off = Read(THint<int>.val),
        };

        private void Load_diana_jumps()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_jumps.Add(ReadDiana_Jump());
        }

        private Diana_Control ReadDiana_Control() => new Diana_Control
        {
            arg = Read(THint<int>.val),
        };

        private void Load_diana_controls()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_controls.Add(ReadDiana_Control());
        }

        private Diana_Try ReadDiana_Try() => new Diana_Try
        {
            body = Read(THint<int>.val),
            except_handlers = Read(THint<Catch[]>.val),
            final_body = Read(THint<int>.val),
        };

        private void Load_diana_trys()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_trys.Add(ReadDiana_Try());
        }

        private Diana_Loop ReadDiana_Loop() => new Diana_Loop
        {
            body = Read(THint<int>.val),
        };

        private void Load_diana_loops()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_loops.Add(ReadDiana_Loop());
        }

        private Diana_For ReadDiana_For() => new Diana_For
        {
            body = Read(THint<int>.val),
        };

        private void Load_diana_fors()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_fors.Add(ReadDiana_For());
        }

        private Diana_With ReadDiana_With() => new Diana_With
        {
            body = Read(THint<int>.val),
        };

        private void Load_diana_withs()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_withs.Add(ReadDiana_With());
        }

        private Diana_GetAttr ReadDiana_GetAttr() => new Diana_GetAttr
        {
            attr = Read(THint<InternString>.val),
        };

        private void Load_diana_getattrs()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_getattrs.Add(ReadDiana_GetAttr());
        }

        private Diana_SetAttr ReadDiana_SetAttr() => new Diana_SetAttr
        {
            attr = Read(THint<InternString>.val),
        };

        private void Load_diana_setattrs()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_setattrs.Add(ReadDiana_SetAttr());
        }

        private Diana_SetAttr_Iadd ReadDiana_SetAttr_Iadd() => new Diana_SetAttr_Iadd
        {
            attr = Read(THint<InternString>.val),
        };

        private void Load_diana_setattr_iadds()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_setattr_iadds.Add(ReadDiana_SetAttr_Iadd());
        }

        private Diana_SetAttr_Isub ReadDiana_SetAttr_Isub() => new Diana_SetAttr_Isub
        {
            attr = Read(THint<InternString>.val),
        };

        private void Load_diana_setattr_isubs()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_setattr_isubs.Add(ReadDiana_SetAttr_Isub());
        }

        private Diana_SetAttr_Imul ReadDiana_SetAttr_Imul() => new Diana_SetAttr_Imul
        {
            attr = Read(THint<InternString>.val),
        };

        private void Load_diana_setattr_imuls()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_setattr_imuls.Add(ReadDiana_SetAttr_Imul());
        }

        private Diana_SetAttr_Itruediv ReadDiana_SetAttr_Itruediv() => new Diana_SetAttr_Itruediv
        {
            attr = Read(THint<InternString>.val),
        };

        private void Load_diana_setattr_itruedivs()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_setattr_itruedivs.Add(ReadDiana_SetAttr_Itruediv());
        }

        private Diana_SetAttr_Ifloordiv ReadDiana_SetAttr_Ifloordiv() => new Diana_SetAttr_Ifloordiv
        {
            attr = Read(THint<InternString>.val),
        };

        private void Load_diana_setattr_ifloordivs()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_setattr_ifloordivs.Add(ReadDiana_SetAttr_Ifloordiv());
        }

        private Diana_SetAttr_Imod ReadDiana_SetAttr_Imod() => new Diana_SetAttr_Imod
        {
            attr = Read(THint<InternString>.val),
        };

        private void Load_diana_setattr_imods()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_setattr_imods.Add(ReadDiana_SetAttr_Imod());
        }

        private Diana_SetAttr_Ipow ReadDiana_SetAttr_Ipow() => new Diana_SetAttr_Ipow
        {
            attr = Read(THint<InternString>.val),
        };

        private void Load_diana_setattr_ipows()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_setattr_ipows.Add(ReadDiana_SetAttr_Ipow());
        }

        private Diana_SetAttr_Ilshift ReadDiana_SetAttr_Ilshift() => new Diana_SetAttr_Ilshift
        {
            attr = Read(THint<InternString>.val),
        };

        private void Load_diana_setattr_ilshifts()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_setattr_ilshifts.Add(ReadDiana_SetAttr_Ilshift());
        }

        private Diana_SetAttr_Irshift ReadDiana_SetAttr_Irshift() => new Diana_SetAttr_Irshift
        {
            attr = Read(THint<InternString>.val),
        };

        private void Load_diana_setattr_irshifts()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_setattr_irshifts.Add(ReadDiana_SetAttr_Irshift());
        }

        private Diana_SetAttr_Ibitor ReadDiana_SetAttr_Ibitor() => new Diana_SetAttr_Ibitor
        {
            attr = Read(THint<InternString>.val),
        };

        private void Load_diana_setattr_ibitors()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_setattr_ibitors.Add(ReadDiana_SetAttr_Ibitor());
        }

        private Diana_SetAttr_Ibitand ReadDiana_SetAttr_Ibitand() => new Diana_SetAttr_Ibitand
        {
            attr = Read(THint<InternString>.val),
        };

        private void Load_diana_setattr_ibitands()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_setattr_ibitands.Add(ReadDiana_SetAttr_Ibitand());
        }

        private Diana_SetAttr_Ibitxor ReadDiana_SetAttr_Ibitxor() => new Diana_SetAttr_Ibitxor
        {
            attr = Read(THint<InternString>.val),
        };

        private void Load_diana_setattr_ibitxors()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_setattr_ibitxors.Add(ReadDiana_SetAttr_Ibitxor());
        }

        private Diana_MKDict ReadDiana_MKDict() => new Diana_MKDict
        {
            n = Read(THint<int>.val),
        };

        private void Load_diana_mkdicts()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_mkdicts.Add(ReadDiana_MKDict());
        }

        private Diana_MKSet ReadDiana_MKSet() => new Diana_MKSet
        {
            n = Read(THint<int>.val),
        };

        private void Load_diana_mksets()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_mksets.Add(ReadDiana_MKSet());
        }

        private Diana_MKList ReadDiana_MKList() => new Diana_MKList
        {
            n = Read(THint<int>.val),
        };

        private void Load_diana_mklists()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_mklists.Add(ReadDiana_MKList());
        }

        private Diana_Call ReadDiana_Call() => new Diana_Call
        {
            n = Read(THint<int>.val),
        };

        private void Load_diana_calls()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_calls.Add(ReadDiana_Call());
        }

        private Diana_Format ReadDiana_Format() => new Diana_Format
        {
            format = Read(THint<int>.val),
            argn = Read(THint<int>.val),
        };

        private void Load_diana_formats()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_formats.Add(ReadDiana_Format());
        }

        private Diana_Const ReadDiana_Const() => new Diana_Const
        {
            p_const = Read(THint<int>.val),
        };

        private void Load_diana_consts()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_consts.Add(ReadDiana_Const());
        }

        private Diana_MKTuple ReadDiana_MKTuple() => new Diana_MKTuple
        {
            n = Read(THint<int>.val),
        };

        private void Load_diana_mktuples()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_mktuples.Add(ReadDiana_MKTuple());
        }

        private Diana_Pack ReadDiana_Pack() => new Diana_Pack
        {
            n = Read(THint<int>.val),
        };

        private void Load_diana_packs()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_packs.Add(ReadDiana_Pack());
        }

        private Diana_Replicate ReadDiana_Replicate() => new Diana_Replicate
        {
            n = Read(THint<int>.val),
        };

        private void Load_diana_replicates()
        {
            int len = ReadInt();
            for (var i = 0; i < len; i++)
                diana_replicates.Add(ReadDiana_Replicate());
        }

        public void LoadCode()
        {
            lock (_loaderSync)
            {
                Load_strings();
                Load_dobjs();
                Load_internstrings();
                Load_catchs();
                Load_funcmetas();
                Load_blocks();
                Load_diana_functiondefs();
                Load_diana_loadglobalrefs();
                Load_diana_delvars();
                Load_diana_loadvars();
                Load_diana_storevars();
                Load_diana_actions();
                Load_diana_controlifs();
                Load_diana_jumpifnots();
                Load_diana_jumpifs();
                Load_diana_jumps();
                Load_diana_controls();
                Load_diana_trys();
                Load_diana_loops();
                Load_diana_fors();
                Load_diana_withs();
                Load_diana_getattrs();
                Load_diana_setattrs();
                Load_diana_setattr_iadds();
                Load_diana_setattr_isubs();
                Load_diana_setattr_imuls();
                Load_diana_setattr_itruedivs();
                Load_diana_setattr_ifloordivs();
                Load_diana_setattr_imods();
                Load_diana_setattr_ipows();
                Load_diana_setattr_ilshifts();
                Load_diana_setattr_irshifts();
                Load_diana_setattr_ibitors();
                Load_diana_setattr_ibitands();
                Load_diana_setattr_ibitxors();
                Load_diana_mkdicts();
                Load_diana_mksets();
                Load_diana_mklists();
                Load_diana_calls();
                Load_diana_formats();
                Load_diana_consts();
                Load_diana_mktuples();
                Load_diana_packs();
                Load_diana_replicates();
            }
        }

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
    }
}
