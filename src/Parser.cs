using System;
using System.IO;
using System.Collections.Generic;

namespace DianaScript
{

    public static class ConstPoolTag
    {
        public const byte BoolTag = 0b10;
        public const byte SpecialTag = 0b01 << 7;

        public const byte Int = 0;
        public const byte Float = 1;
        public const byte Str = 2;


        public const byte Dict = 3;
        public const byte Set = 4;
        public const byte List = 5;
        public const byte Tuple = 6;
    }
    

    
    public partial class AIRParser
    {
        FileStream fileStream;
        byte[] cache_4byte;
        
        byte[] cache_32byte;

        public void setup_cache()
        {
            cache_4byte = new byte[4];
            cache_32byte = new byte[4];
        }
        public AIRParser(FileStream fs)
        {
            fileStream = fs;
            setup_cache();
        }
        public AIRParser(string path)
        {
            var fs = File.Open(path, FileMode.Open);
            fileStream = fs;
            setup_cache();
        }
        
        public (int, int) Read(THint<(int, int)> _){
            return (ReadInt(), ReadInt());
        }
        public InternString Read(THint<InternString> _){
            return ReadStr().ToIStr();
        }
        public int Read(THint<int> _) => ReadInt();
        public int ReadInt()
        {
            fileStream.Read(cache_4byte, 0, 4);
            return BitConverter.ToInt32(cache_4byte, 0);
        }

        public float Read(THint<float> _) => ReadFloat();
        public float ReadFloat()
        {
            fileStream.Read(cache_4byte, 0, 4);
            return BitConverter.ToSingle(cache_4byte, 0);
        }

        public string Read(THint<string> _) => ReadStr();
        public string ReadStr()
        {

            fileStream.Read(cache_4byte, 0, 32);
            var strlen = BitConverter.ToInt32(cache_4byte, 0);
            if (strlen % 2 != 0)
                throw new InvalidDataException("a string is not utf-16 encoded!");

            void cache32_tobuff(System.Text.StringBuilder sbuff, int nbyte)
            {
                for (int j = 0; j < nbyte; j += 2)
                {
                    sbuff.Append(BitConverter.ToChar(cache_32byte, j));
                }
            }
            var sbuff = new System.Text.StringBuilder();

            for (int i = 0; i < strlen / 32; i++)
            {
                fileStream.Read(cache_32byte, 0, 32);
                cache32_tobuff(sbuff, 32);
            }

            var left = strlen % 32;
            fileStream.Read(cache_4byte, 0, left);
            cache32_tobuff(sbuff, left);
            return sbuff.ToString();

        }

        public bool Read(THint<bool> _) => ReadBool();
        public bool ReadBool()
        {
            fileStream.Read(cache_4byte, 0, 1);
            if ((cache_4byte[0] & ConstPoolTag.SpecialTag) == 1 && (cache_4byte[0] & ConstPoolTag.BoolTag) == 1)
            {
                return (cache_4byte[0] & 0b01) == 1;
            }
            throw new InvalidDataException("invalid data format for boolean.");
        }
        
        

        public DObj Read(THint<DObj> _) => ReadObj();
        public DObj ReadObj()
        {
            fileStream.Read(cache_4byte, 0, 1);
            var tag = cache_4byte[0];

            if ((ConstPoolTag.SpecialTag & tag) == 1)
            {
                if ((tag & ConstPoolTag.BoolTag) == 1)
                {
                    return MK.Bool((tag & 0b01) == 1);
                }
                return MK.Nil();
            }
            int len;
            switch (tag)
            {
                case ConstPoolTag.Int:
                    return MK.Int(ReadInt());

                case ConstPoolTag.Float:
                    return MK.Float(ReadFloat());

                case ConstPoolTag.Str:
                    return MK.String(ReadStr());

                case ConstPoolTag.Dict:
                    {
                        fileStream.Read(cache_4byte, 0, 4);
                        len = BitConverter.ToInt32(cache_4byte, 0);
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
                        fileStream.Read(cache_4byte, 0, 4);
                        len = BitConverter.ToInt32(cache_4byte, 0);
                        var ret = new List<DObj>(len);
                        for (var i = 0; i < len; i++)
                        {
                            ret.Add(ReadObj());
                        }
                        return MK.List(ret);
                    }
                case ConstPoolTag.Set:
                    {
                        fileStream.Read(cache_4byte, 0, 4);
                        len = BitConverter.ToInt32(cache_4byte, 0);
                        var ret = new HashSet<DObj>(len);
                        for (var i = 0; i < len; i++)
                        {
                            ret.Add(ReadObj());
                        }
                        return MK.Set(ret);
                    }
                case ConstPoolTag.Tuple:
                    {
                        fileStream.Read(cache_4byte, 0, 4);
                        len = BitConverter.ToInt32(cache_4byte, 0);
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