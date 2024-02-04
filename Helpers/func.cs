namespace lapis.Helpers
{
    public class Func
    {
        public List<Tuple<string, byte>> Params = new List<Tuple<string, byte>>();
        public byte Ret_type;

       public Func(byte ret_type, List<Tuple<string, byte>> args)
       {
            Ret_type = ret_type;
            Params = args;  
       }
    }
}
