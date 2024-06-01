using lapis.Asm.Inst;
using lapis.Constants;
using lapis.Helpers;
using System.Text;

namespace lapis.Asm.Gen
{
    public class Gen
    {
        public static string Generate(List<Instruction> insts)
        {
            Fetcher fetcher = new();
            StringBuilder main = new();
            string prefixes = fetcher.FetchPrefixes();

            foreach (Instruction inst in insts)
            {
                main.AppendLine(inst.ToString());
            }

            return $"format {Consts.exeFormat} executable 3\n" +
                $"segment readable executable\n"+
                $"entry {Consts.entry}\n" + 
                $"{prefixes}\n"+
                $"{Consts.entry}:\n{main}"+
                $"jmp {Consts.exitLabel}";
        }
    }
}
