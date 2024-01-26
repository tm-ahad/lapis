namespace lapis.Asm.Ptr
{
    public class PtrSize
    {
        public const byte 
        BYTE = 0, 
        WORD = 1,
        DWORD = 2,
        QWORD = 3;

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
