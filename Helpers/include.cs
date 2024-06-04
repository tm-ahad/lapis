using lapis.constants;
using lapis.asm.inst;
using lapis.parser;
using lapis.context;

namespace lapis.helpers
{
    public class Include(Context con)
    {
        private readonly Context context = con;

        public List<Instruction> Self(string path) 
        {
            string content = File.ReadAllText(Path.Combine(context.working_dir, path));
            Parser parser = new(context);

            return parser.Parse(content);
        }

        public List<Instruction> Std(string path)
        {
            Parser parser = new(context);
            Fetcher fetcher = new();

            string content = fetcher.GetStdLib(path);
            return parser.Parse(content);
        }

        public static List<Instruction> External(string name) 
        {
            string asm = File.ReadAllText(Path.Combine(Consts.lib_dir, name, Consts.main_file));
            List<Instruction> instructions = [new Instruction.Asm(asm)];
            return instructions;
        }
    }
}
