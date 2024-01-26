using lapis.Asm.Ptr;
namespace lapis.Types
{
    public abstract class Type
    {
        public const byte Int = 0, Uint = 1, String = 2, Array = 3;
        private const string Arr_def = "[]";

        public static byte Size(byte type)
        {
            if (type == Int || type is Uint)
            {
                return PtrSize.DWORD;
            }

            throw new Exception($"Typerror: type {type} not found");
        }

        private static string ToString(byte type)
        {
            switch (type)
            {
                case Int: return "int";
                case Uint: return "uint";
                case String: return "string";
            }

            if (type > Array)
            {
                byte arr_element_type = (byte)(type - Array);
                string arr_element_type_str = ToString(arr_element_type);

                return $"[]{arr_element_type_str}";
            }

            throw new Exception("5846162495");
        }

        public static byte FromString(string s)
        {
            switch (s)
            {
                case "int": return Int;
                case "uint": return Uint;
                case "string": return String;
            }

            if (s.StartsWith(Arr_def))
            {
                string raw_type = s.Substring(Arr_def.Length);
                byte type = FromString(raw_type);

                return (byte)(Array + type);
            }

            throw new Exception($"Error: type {s} not found");
        }

        public static string Value(string rawVal, byte type)
        {
            if (type == Int)
            {
                return int.Parse(rawVal).ToString();
            }
            else if (type == Uint)
            {
                return uint.Parse(rawVal).ToString();
            }
            else if (type >= Array) 
            {
                byte arr_element_type = (byte)(type - Array);
                return arr_element_type.ToString();
            }

            throw new Exception($"Error: type {ToString(type)} not found");
        }
    }
}
