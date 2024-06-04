using lapis.asm.inst;
using lapis.constants;
using lapis.helpers;
using System.Text;

namespace lapis.asm.gen
{
    public class Gen
    {
        public static string Generate(List<Instruction> insts, bool isLib)
        {
            Fetcher fetcher = new();
            StringBuilder main = new();
            string prefixes = fetcher.FetchPrefixes();

            foreach (Instruction inst in insts)
            {
                main.AppendLine(inst.ToString());
            }

            if (isLib) 
            {
                return main.ToString();
            } 
            else 
            {
                return $"format {Consts.exeFormat} executable 3\n" +
                    $"segment readable executable\n"+
                    $"entry {Consts.entry}\n" + 
                    $"{prefixes}\n"+
                    $"{Consts.entry}:\n{main}"+
                    $"jmp {Consts.exitLabel}";
            }
        }
    }
}
