using lapis.Asm.Ptr;

namespace lapis.Helpers
{
    public class Var
    {
        public string Ptr = string.Empty;
        public string Head;
        public byte Type;

        public Var(string head, byte type)
        {
            Head = head;
            Type = type;
            Ptr = $"{PtrSize.ToString(Types.Type.Size(type))} PTR [rbp-{head}]";
        }
    }
}
