using lapis.Asm.Ptr;
using System.Reflection;

namespace lapis.Helpers
{
    public class Var
    {
        public string Head;
        public byte Type;

        public Var(string head, byte type)
        {
            Head = head;
            Type = type;
        }

        public string Pointer() 
        {
            return $"{PtrSize.ToString(Types.Type.Size(Type))} [rbp-{Head}]";
        }
    }
}
