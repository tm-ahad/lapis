using System.Text;
using lapis.constants;

namespace lapis.helpers
{
    public class FuncSign(byte ret_type, List<Tuple<string, byte>> _args)
    {
        public List<Tuple<string, byte>> arguments = _args;
        public byte retType = ret_type;

        public static Tuple<string, FuncSign> ParseFuncSign(string line)
        {
            string[] split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (split[0] != Consts.Token_fun)
            {
                throw new Exception("Invalid funtion sign!");
            }

            string retType = split[1];
            string name = split[2];
            string rawArgs = split[3];

            List<Tuple<string, byte>> args = [];
            string[] split2 = rawArgs.Split(",", StringSplitOptions.RemoveEmptyEntries);

            foreach (string arg in split2)
            {
                string[] split3 = arg.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                args.Add(Tuple.Create(split3[0], Types.Type.FromString(split3[1])));
            }

            FuncSign sign = new(Types.Type.FromString(retType), args);
            return Tuple.Create(name, sign);
        }

        public string ToString(string name) 
        {
            StringBuilder string_args = new();
            string_args.Append($"fun {Types.Type.ToString(retType)} {name}");


            foreach (Tuple<string, byte> pair in arguments) 
            {
                string_args.Append($"{pair.Item1} {Types.Type.ToString(pair.Item2)}");
                if (pair != arguments.Last()) 
                {
                    string_args.Append(',');
                }
            }

            return string_args.ToString();
        }
    }
}
