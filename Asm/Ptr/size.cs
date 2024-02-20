using lapis.Constants;

namespace lapis.Asm.Ptr
{
    public class PtrSize
    {
        public const byte 
        BYTE = 1, 
        WORD = 2,
        DWORD = 4,
        QWORD = 8,
        UNKNOWN = 0;

        public static string ToString(byte size)
        {
            switch (size)
            {
                case UNKNOWN:
                case BYTE: return "byte";
                case WORD: return "word";
                case DWORD: return "dword";
                case QWORD: return "qword";
            }

            throw new Exception($"Error: invalid size {size}");
        }

        public static byte FromString(string size)
        {
            switch (size)
            {
                case "unknown":
                case "byte": return BYTE;
                case "word": return WORD;
                case "dword": return DWORD;
                case "qword": return QWORD;
            }

            throw new Exception($"Error: invalid size {size}");
        }

        public static string CopyRegisterName(byte size, byte index) 
        {
            switch (size) 
            {
                case BYTE: return Consts.coupleRegister8[index];
                case WORD: return Consts.coupleRegister16[index];
                case DWORD: return Consts.coupleRegister32[index];
                case QWORD: return Consts.coupleRegister64[index];
            }

            return "B.A.L";
        }

        public static byte ExtractSizeFromPtr (string ptr)
        {
            string ss = ptr.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[0];
            return FromString(ss);
        }
    }
}
