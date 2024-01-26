namespace lapis.Asm.Ptr
{
    public class Gen
    {
        public static uint curr = 0;

        public static string Generate(byte size)
        {
            curr += size;
            return $"{PtrSize.ToString(size)} PTR [rbp-{curr}]";
        }
    }
}
