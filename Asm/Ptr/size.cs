namespace lapis.Asm.Ptr
{
    public class PtrSize
    {
        public const byte 
        BYTE = 1, 
        WORD = 2,
        DWORD = 4,
        QWORD = 8;

        public static string ToString(byte size)
        {
            switch (size)
            {
                case BYTE: return "BYTE";
                case WORD: return "WORD";
                case DWORD: return "DWORD";
                case QWORD: return "QWORD";
            }

            return string.Empty;
        }
    }
}
