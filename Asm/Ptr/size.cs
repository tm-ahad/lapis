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

        public static string CopyRegisterName(byte size) 
        {
            switch (size)
            {
                case UNKNOWN:
                case BYTE: return Consts.copyRegister8;
                case WORD: return Consts.copyRegister16;
                case DWORD: return Consts.copyRegister32;
                case QWORD: return Consts.copyRegister64;
            }

            throw new Exception($"Error: invalid size {size}");
        }
    }
}
