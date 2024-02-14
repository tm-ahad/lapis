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
                case BYTE: return "byte";
                case WORD: return "word";
                case DWORD: return "dword";
                case QWORD: return "qword";
                case UNKNOWN: return "byte";
            }

            throw new Exception($"Error: invalid size {size}");
        }
    }
}
