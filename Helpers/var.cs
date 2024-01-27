namespace lapis.Helpers
{
    public class Var
    {
        public string Ptr = string.Empty;
        public byte Type;

        public Var(string ptr, byte type)
        {
            Ptr = ptr;
            Type = type;
        }
    }
}
