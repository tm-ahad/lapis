using lapis.Asm.Inst;
using lapis.parser;

namespace lapis.Helpers
{
    public class Include
    {
        public static List<Instruction> Self(string path) 
        {
            string content = File.ReadAllText(path);
            Parser parser = new();
            return parser.Parse(content);
        }

        public static List<Instruction> Std(string path)
        {
            Parser parser = new();
            Fetcher fetcher = new();

            string content = fetcher.GetStdLib(path);

            return parser.Parse(content);
        }
    }
}
