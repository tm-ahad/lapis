using lapis.asm.ptr;

namespace lapis.helpers
{
    public class Var(string head, byte type)
    {
        public string Head = head;
        public byte Type = type;

        public string Pointer() 
        {
            return $"{PtrSize.ToString(Types.Type.Size(Type))} [rbp-{Head}]";
        }
    }
}
