namespace lapis.Helpers
{
    public class Func(byte ret_type, List<Tuple<string, byte>> args)
    {
       public List<Tuple<string, byte>> Params = args;
       public byte Ret_type = ret_type;
    }
}
