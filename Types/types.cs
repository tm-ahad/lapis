using lapis.Asm.Ptr;
using lapis.Constants;

namespace lapis.Types
{
    public class Type
    {
        public const byte
        Int = 0,
        Uint = 1,
        Byte = 2,
        I8 = 3,
        I16 = 4,
        U16 = 5,
        I64 = 6,
        U64 = 7,
        Array = 8,
        Struct = 9,
        Unknown = 255;

        public static byte Size(byte type)
        {
            if (type == Int || type == Uint)
            {
                return PtrSize.DWORD;
            }
            else if (type == I8 || type == Byte) 
            {
                return PtrSize.BYTE;
            }
            else if (type == I16 || type == U16)
            {
                return PtrSize.WORD;
            }
            else if (type == I64 || type == U64)
            {
                return PtrSize.QWORD;
            }
            else if (type >= Array) 
            {
                return Size((byte)(type - Array));
            }

            throw new Exception($"Typerror: type {type} not found");
        }

        public static string ToString(byte type)
        {
            switch (type)
            {
                case Int: return "int";
                case Uint: return "uint";
                case I8: return "int8";
                case Byte: return "byte";
                case I16: return "int16";
                case U16: return "uint16";
                case I64: return "int64";
                case U64: return "uint64";
                case Unknown: return "unknown";
            }

            if (type >= Array)
            {
                byte arr_element_type = (byte)(type - Array);
                string arr_element_type_str = ToString(arr_element_type);

                return $"{arr_element_type_str}[<size>]";
            }


            throw new Exception("5846162495");
        }

        public static bool isNumber(byte type)
        {
            return type == Int ||
                type == Uint ||
                type == I8 ||
                type == Byte ||
                type == I16 ||
                type == U16 ||
                type == I64 ||
                type == U64;
        }

        public static byte FromString(string s)
        {
            switch (s)
            {
                case "int": return Int;
                case "uint": return Uint;
                case "int8": return I8;
                case "byte": return Byte;
                case "int16": return I16;
                case "uint16": return U16;
                case "int64": return I64;
                case "uint64": return U64;
            }

            if (s.EndsWith(']') && s.Contains('['))
            {
                string raw_type = s.Substring(0, s.IndexOf('['));
                byte type = FromString(raw_type);
                return (byte)(Array + type);
            }

            throw new Exception($"Error: type {s} not found");
        }

        public static string Value(string rawVal, byte type)
        {
            switch (type)
            {
                case Byte: return byte.Parse(rawVal).ToString();
                case I8: return sbyte.Parse(rawVal).ToString();
                case I16: return Int16.Parse(rawVal).ToString();
                case U16: return UInt16.Parse(rawVal).ToString();
                case Int: return int.Parse(rawVal).ToString();
                case Uint: return uint.Parse(rawVal).ToString();
                case I64: return Int64.Parse(rawVal).ToString();
                case U64: return UInt64.Parse(rawVal).ToString();
                default: 
                    if (type >= Array)
                    {
                        byte arr_element_type = (byte)(type - Array);
                        return arr_element_type.ToString();
                    }

                    throw new Exception($"Error: type {ToString(type)} not found");
            };
        }

        public static int arrElements(string s)
        {
            int size = -1;

            if (s.EndsWith(']') && s.Contains('['))
            {
                int start = s.IndexOf('[') + 1;
                string ss = s[start..s.IndexOf("]")];
                _ = int.TryParse(ss, out size);
            }

            return size;
        }
    }
}
