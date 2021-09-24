using System;
using System.Collections.Generic;
using System.Linq;

namespace DianaScript
{

    public partial class DianaVM : VM
    {
        public VM vm => this;
        public Dictionary<Type, DClsObj> typemaps;
        public DClsObj BFunc0Cls, BFunc1Cls, BFunc2Cls,
                       BFunc3Cls, TupleCls, MetaCls, StrCls,
                       NilCls, FloatCls, ErrorCls, CodeCls;

        List<DStr> err_kinds = new List<DStr> { };
        public int NewErrId(DStr name)
        {
            var r = err_kinds.Count;
            err_kinds.Add(name);
            return r;

        }

        public int ErrId_InvalidJump, ErrId_TypeError, ErrId_ValueError, ErrId_AttributeError, ErrId_CallError;
        public DianaVM()
        {

            typemaps = new();
            Frames = new();
            ErrorFrames = new();

            MetaCls = new DClsObj
            {
                GetCls = null,
                name = null,
                get = defaultGet,
                set = defaultSet,
                call = defaultCall,
                reprfunc = o => ((DClsObj)o).name.value
            };
            MetaCls.GetCls = MetaCls;
            StrCls = _CreateCls();
            StrCls.name = CreateStr("string");
            MetaCls.name = CreateStr("Meta");

            ErrId_InvalidJump = NewErrId(CreateStr("InvalidJump"));
            ErrId_TypeError = NewErrId(CreateStr("TypeError"));
            ErrId_ValueError = NewErrId(CreateStr("ValueError"));
            ErrId_AttributeError = NewErrId(CreateStr("AttributeError"));
            ErrId_CallError = NewErrId(CreateStr("CallError"));

            IntCls = CreateCls("int");
            FloatCls = CreateCls("float");
            BoolCls = CreateCls("bool");
            NilCls = CreateCls("NoneClass");
            ErrorCls = CreateCls("Error");
            FrameCls = CreateCls("Frame");
            CodeCls = CreateCls("Code");
            TupleCls = CreateCls("Tuple");
            FuncCls = CreateCls("Function"); // user function
            BFunc0Cls = CreateCls("Func0");
            BFunc1Cls = CreateCls("Func1");
            BFunc2Cls = CreateCls("Func2");
            BFunc3Cls = CreateCls("Func3");
            GtorCls = CreateCls("Generator");

            StrCls.reprfunc = o => $"\"{((DStr)o).value.Replace("\"", "\\\"")}\"";
            StrCls.strfunc = o => ((DStr)o).value;
            BoolCls.reprfunc = o => ((DBool)o).value ? "true" : "false";
            
            TupleCls.reprfunc = o =>
            {
                var s = seq_repr(((DTuple)o).elts);
                if (s == null) return null;
                if (((DTuple)o).elts.Length == 1)
                {
                    s += ",";
                }
                return $"({s},)";
            };
            NilCls.reprfunc = o => "None";

            Nil = new DNil
            {
                GetCls = NilCls
            };

            MetaCls.call = call_metaclass;
            StrCls.call = call_strclass;
            IntCls.call = call_intclass;
            FloatCls.call = call_floatclass;
            BFunc0Cls.call = call_bfunc0;
            BFunc1Cls.call = call_bfunc1;
            BFunc2Cls.call = call_bfunc2;
            BFunc3Cls.call = call_bfunc3;

            ErrorCls.reprfunc = o => ReprErr(o as DError);
        }
        
        public string seq_repr(IEnumerable<DObj> seq){
            List<string> parts = new List<string>{};
            foreach(var a in seq){
                var s = a.repr;
                if (s == null) return null;
                parts.Add(s);
            }
            return String.Join(", ", parts);
        }
        private string ReprErr(DError err){
            string errname = err_kinds[err.error_id].value;
            var s = seq_repr(err.args);
            if (s == null) return null;
            return $@"{errname}({s})";
        }
        private DObj call_bfunc0(DObj bfunc, Args args)
        {
            int narg = args.NArgs;
            if (narg == 0)
            {
                var bfunc_ = (BFunc0)bfunc;
                return bfunc_.value();
            }
            CurrentError = Err_ArgMismatch(bfunc, args.NArgs, 0);
            return null;
        }

        private DObj call_bfunc1(DObj bfunc, Args args)
        {
            int narg = args.NArgs;
            int expect = 1;
            if (narg == expect)
            {
                var bfunc_ = (BFunc1)bfunc;
                return bfunc_.value(args[0]);
            }
            CurrentError = Err_ArgMismatch(bfunc, narg, expect);
            return null;
        }

        private DObj call_bfunc2(DObj bfunc, Args args)
        {
            int narg = args.NArgs;
            int expect = 2;
            if (narg == expect)
            {
                var bfunc_ = (BFunc2)bfunc;
                return bfunc_.value(args[0], args[1]);
            }
            CurrentError = Err_ArgMismatch(bfunc, narg, expect);
            return null;
        }

        private DObj call_bfunc3(DObj bfunc, Args args)
        {
            int narg = args.NArgs;
            int expect = 3;
            if (narg == expect)
            {
                var bfunc_ = (BFunc3)bfunc;
                return bfunc_.value(args[0], args[1], args[2]);
            }
            CurrentError = Err_ArgMismatch(bfunc, narg, expect);
            return null;
        }
        private DObj call_strclass(DObj cls, Args args)
        {
            if (args.NArgs == 1)
            {
                var o = args[0];
                return CreateStr(o.ToString());
            }
            CurrentError = Err_CallError(cls, args.NArgs, CreateStr($"Metaclass object cannot get called with {args.NArgs} arguments"));
            return null;
        }


        private DObj call_floatclass(DObj cls, Args args)
        {
            if (args.NArgs == 1)
            {
                var o = args[0];
                float r;
                switch (o)
                {
                    case DFloat a:
                        return a;
                    case DInt a:
                        return CreateFloat((float)a.value);
                    case DStr a:
                        {

                            if (float.TryParse(a.value, out r))
                            {
                                return CreateFloat(r);
                            }
                            CurrentError = Err_ValueError(
                                o,
                                CreateStr($"str {a.value} cannot get parsed into 32 bit float."));
                            return null;
                        }
                }


            }
            CurrentError = Err_CallError(cls, args.NArgs, CreateStr($"float class object cannot get called with {args.NArgs} arguments."));
            return null;
        }

        private DObj call_intclass(DObj cls, Args args)
        {
            if (args.NArgs == 1)
            {
                var o = args[0];
                int r;
                switch (o)
                {
                    case DInt a:
                        return a;
                    case DFloat a:
                        return CreateInt((int)a.value);
                    case DStr a:
                        {

                            if (int.TryParse(a.value, out r))
                            {
                                return CreateInt(r);
                            }
                            CurrentError = Err_ValueError(
                                o,
                                CreateStr($"str {a.value} cannot get parsed into 32 bit int."));
                            return null;
                        }
                }


            }
            CurrentError = Err_CallError(cls, args.NArgs, CreateStr($"int class object cannot get called with {args.NArgs} arguments."));
            return null;
        }

        private DError Err_ValueError(DObj o, DStr dStr)
        {
            return new DError
            {
                GetCls = ErrorCls,
                args = new[] { o, dStr },
                error_id = ErrId_ValueError
            };
        }

        private DObj call_metaclass(DObj cls, Args args)
        {
            if (args.NArgs == 1)
            {
                return args[0].GetCls;
            }
            CurrentError = Err_CallError(cls, args.NArgs, CreateStr($"Metaclass object cannot get called with {args.NArgs} arguments"));
            return null;
        }
    
        public DObj defaultGet(DObj o, string s)
        {
            CurrentError = Err_AttributeError(CreateStr(s), o);
            return null;
        }

        public DObj defaultSet(DObj o, string s)
        {
            CurrentError = Err_AttributeError(CreateStr(s), o);
            return null;
        }

        public DObj defaultCall(DObj func, Args args)
        {
            string s = func.repr;
            if (s == null) return null;
            CurrentError = Err_CallError(func, args.NArgs, CreateStr($"invalid call for {s}"));
            return null;
        }

        private DError Err_CallError(DObj func, int nArgs, DStr dStr)
        {
            return new DError
            {
                GetCls = ErrorCls,
                args = new[] { func, CreateInt(nArgs), dStr },
                error_id = ErrId_CallError
            };
        }

        public DClsObj _CreateCls() => new DClsObj
        {
            GetCls = MetaCls,
            name = null,
            get = defaultGet,
            set = defaultSet,
            call = defaultCall
        };

        public DClsObj CreateCls(string name) => new DClsObj
        {
            GetCls = MetaCls,
            name = CreateStr(name),
            get = defaultGet,
            set = defaultSet,
            call = defaultCall
        };
        public DClsObj FromNativeType(Type t)
        {
            DClsObj ret;
            if (typemaps.TryGetValue(t, out ret))
            {
                return ret;
            }
            ret = _CreateCls();
            ret.name = CreateStr(t.FullName);
            return ret;
        }

        public DStr CreateStr(string s) => new DStr
        {
            value = s,
            GetCls = StrCls
        };

        public DInt CreateInt(int i) => new DInt
        {
            value = i,
            GetCls = IntCls
        };
        public DBool CreateBool(bool b) => new DBool
        {
            value = b,
            GetCls = IntCls
        };
        public DFloat CreateFloat(float f) => new DFloat
        {
            value = f,
            GetCls = IntCls
        };

        public DTuple CreateTuple(DObj[] elts) => new DTuple
        {
            elts = elts,
            GetCls = TupleCls
        };

        public DNil CreateNil() => Nil;

        public DCode CreateCode(
            int[] bc,
            int[] locs,
            DObj[] consts,
            string filename,
            int nfree = 0,
            int narg = 0,
            int nlocal = 0,
            bool varg = false
        ) => new DCode
        {
            GetCls = CodeCls,
            bc = bc,
            locs = locs,
            consts = consts,
            filename = filename,
            nfree = nfree,
            narg = narg,
            nlocal = nlocal,
            varg = varg
        };

        public DNativeWrap CreateFromNative(object o) => new DNativeWrap
        {
            GetCls = FromNativeType(o.GetType()),
            value = o
        };

        public BFunc0 CreateBFunc0(Func<DObj> f) => new BFunc0
        {
            GetCls = BFunc0Cls,
            value = f
        };
        public BFunc1 CreateBFunc1(Func<DObj, DObj> f) => new BFunc1
        {
            GetCls = BFunc1Cls,
            value = f
        };

        public BFunc2 CreateBFunc2(Func<DObj, DObj, DObj> f) => new BFunc2
        {
            GetCls = BFunc2Cls,
            value = f
        };
        public BFunc3 CreateBFunc3(Func<DObj, DObj, DObj, DObj> f) => new BFunc3
        {
            GetCls = BFunc3Cls,
            value = f
        };
        public DClsObj IntCls { set; get; }

        public DClsObj BoolCls { get; set; }

        public DClsObj FrameCls { get; set; }

        public DNil Nil { get; set; }

        public List<DFrame> Frames { get; set; }

        public List<DFrame> ErrorFrames { get; set; }

        public DError CurrentError { get; set; }
        public DObj CurrentReturn { get; set; }

        public DClsObj FuncCls { get; set; }

        public DClsObj GtorCls { get; set; }

        public DError Err_InvalidJump(DObj d)
        {
            return new DError
            {
                GetCls = ErrorCls,
                args = new[] { d },
                error_id = ErrId_InvalidJump
            };
        }

        public DError Err_AttributeError(DStr s, DObj d)
        {
            return new DError
            {
                GetCls = ErrorCls,
                args = new[] { s, d },
                error_id = ErrId_AttributeError
            };

        }
        public DError Err_TypeError(DClsObj expected_type, DObj value)
        {
            return new DError
            {
                GetCls = ErrorCls,
                args = new[] { expected_type, value },
                error_id = ErrId_TypeError
            };
        }
        public DError Err_NotBool(DObj d)
        {
            return Err_TypeError(BoolCls, d);
        }

        public DError Err_ArgMismatch(DObj f, int n, int expect)
        {
            string f_repr = ((DObj)f).repr;
            if (f_repr == null) return null;
            return Err_CallError(f, n, CreateStr($"{f_repr} expects {expect} arguments, got {n}."));
        }
        public DError Err_ArgMismatchForUserFunc(DFunc f, int n)
        {
            string f_repr = ((DObj)f).repr;
            if (f_repr == null) return null;
            var expect = (f.code.varg ? ">=" : "") + $"{f.code.narg}";
            return Err_CallError(f, n, CreateStr($"{f_repr} takes expect {expect} arguments, got {n}."));
        }

        public DGenerator CreateGenerator(DFrame frame, DObj val)
        {
            return new DGenerator {
                frame = frame,
                GetCls = GtorCls,
                yieldvalue = val
            };
        }
    }

};
