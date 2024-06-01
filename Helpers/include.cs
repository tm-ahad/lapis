using lapis.Asm.Inst;
using lapis.parser;
using velt.Context;

namespace lapis.Helpers
{
    public class Include
    {
        public static List<Instruction> Self(string path) 
        {
            string content = File.ReadAllText(path);
           
            Context ctx = new Context();
            Parser parser = new Parser(ctx);

            return parser.Parse(content);
        }

        public static List<Instruction> Std(string path)
        {
            Context ctx = new Context();
            Parser parser = new Parser(ctx);
            Fetcher fetcher = new();

            string content = fetcher.GetStdLib(path);
            return parser.Parse(content);
        }
    }
}
