using System;
using System.IO;
using System.Collections.Generic;

namespace DianaScript
{

    public static class ConstPoolTag
    {
        public const byte BoolTag = 0b10;
        public const byte BoolBit = 0b01;
        public const byte SpecialTag = 0b01 << 7;

        public const byte Int = 0;
        public const byte Float = 1;
        public const byte Str = 2;


        public const byte Dict = 3;
        public const byte Set = 4;
        public const byte List = 5;
        public const byte Tuple = 6;
    }



    public partial class AWorld
    {
        public const bool DEBUG = true;
        private BinaryReader binaryReader;
        private byte[] cache_4byte = new byte[4];
        private byte[] cache_32byte = new byte[32];


        private AWorld(FileStream fs)
        {
            binaryReader = new BinaryReader(fs);

        }
        private AWorld(string path)
        {
            var fs = File.Open(path, FileMode.Open);
            binaryReader = new BinaryReader(fs);
        }

        public static AWorld GetLoaderFrom(string path) => new AWorld(path);
        public static AWorld GetLoaderFrom(FileStream fs) => new AWorld(fs);

        public (int, int) Read(THint<(int, int)> _)
        {
            return (ReadInt(), ReadInt());
        }
        public InternString Read(THint<InternString> _)
        {
            var s = ReadStr().ToIStr();
#if A_DBG
            Console.WriteLine($"parsing intern string: {s}");
        
#endif
            return s;
        }
        public int Read(THint<int> _) => ReadInt();
        public int ReadInt()
        {
            var i = binaryReader.ReadInt32();
#if A_DBG
            Console.WriteLine($"parse integer: '{i}'");
#endif
            return i;
        }

        public float Read(THint<float> _) => ReadFloat();
        public float ReadFloat()
        {
            var f = binaryReader.ReadSingle();
#if A_DBG
            Console.WriteLine($"parse float: '{f}'");
#endif
            return f;
        }

        public string Read(THint<string> _) => ReadStr();
        public string ReadStr()
        {
            var s = binaryReader.ReadString();
#if A_DBG
            Console.WriteLine($"parse string: '{s}'");
#endif            
            return s;
        }

        public bool Read(THint<bool> _) => ReadBool();
        public bool ReadBool()
        {
            
            binaryReader.Read(cache_4byte, 0, 1);
            if ((cache_4byte[0] & ConstPoolTag.SpecialTag) != 0 && (cache_4byte[0] & ConstPoolTag.BoolTag) != 0)
            {
                var b = (cache_4byte[0] & ConstPoolTag.BoolBit) != 0;

#if A_DBG
                Console.WriteLine($"parse bool: '{b}'");
#endif            
                return b;
            }
            throw new InvalidDataException("invalid data format for boolean.");
        }


        public DObj Read(THint<DObj> _) => ReadObj();
        public DObj ReadObj()
        {
            binaryReader.Read(cache_4byte, 0, 1);
            var tag = cache_4byte[0];

            if ((ConstPoolTag.SpecialTag & tag) != 0)
            {
                if ((tag & ConstPoolTag.BoolTag) != 0)
                {
#if A_DBG
    Console.WriteLine($"parsing bool '{(tag & ConstPoolTag.BoolBit) != 0}'.");
#endif
                    return MK.Bool((tag & ConstPoolTag.BoolBit) != 0);
                }
#if A_DBG
    Console.WriteLine($"parsing 'nil'.");
#endif
                return MK.Nil();
            }
            int len;
            switch (tag)
            {
                case ConstPoolTag.Int:
#if A_DBG
    Console.WriteLine($"parsing int object tag.");
#endif    
                    return MK.Int(ReadInt());

                case ConstPoolTag.Float:
#if A_DBG
    Console.WriteLine($"parsing float object tag.");
#endif    
                    return MK.Float(ReadFloat());

                case ConstPoolTag.Str:
                    return MK.String(ReadStr());

                case ConstPoolTag.Dict:
                    {
                        len = binaryReader.ReadInt32();
                        if (len % 2 != 0)
                            throw new InvalidDataException("a dict is not pairwise encoded!");
                        var ret = new Dictionary<DObj, DObj>(len / 2);
                        for (var i = 0; i < len; i += 2)
                        {
                            var key = ReadObj();
                            var value = ReadObj();
                            ret[key] = value;
                        }
                        return MK.Dict(ret);
                    }
                case ConstPoolTag.List:
                    {
                        len = binaryReader.ReadInt32();
                        var ret = new List<DObj>(len);
                        for (var i = 0; i < len; i++)
                        {
                            ret.Add(ReadObj());
                        }
                        return MK.List(ret);
                    }
                case ConstPoolTag.Set:
                    {
                        len = binaryReader.ReadInt32();
                        var ret = new HashSet<DObj>(len);
                        for (var i = 0; i < len; i++)
                        {
                            ret.Add(ReadObj());
                        }
                        return MK.Set(ret);
                    }
                case ConstPoolTag.Tuple:
                    {
                        len = binaryReader.ReadInt32();
                        var ret = new DObj[len];
                        for (var i = 0; i < len; i++)
                        {
                            ret[i] = ReadObj();
                        }
                        return MK.Tuple(ret);
                    }

                default:
                    throw new NotImplementedException($"unknown data tag {tag}.");
            }

        }

    }
}