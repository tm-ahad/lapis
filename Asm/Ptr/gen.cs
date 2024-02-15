namespace lapis.Asm.Ptr
{
    public class Gen
    {
        private static uint Curr = 0;

        public static string Generate(byte size)
        {
            Curr += size;
            return $"{Curr}";
        }

        public static string Make(byte type, string head) 
        {
            return $"{PtrSize.ToString(Types.Type.Size(type))} [rbp-{head}]";
        }

        public static uint GetCurr() => Curr;
        public static void SetCurr(uint curr) 
        {
            Curr = curr;
        }
    }
}
