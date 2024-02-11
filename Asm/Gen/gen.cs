using lapis.Asm.Inst;
using lapis.Helpers;
using System.Text;

namespace lapis.Asm.Gen
{
    public class Gen
    {
        public static string Generate(List<Instruction> insts)
        {
            Fetcher fetcher = new Fetcher();
            StringBuilder main = new StringBuilder();
            string prefixes = fetcher.FetchPrefixes();

            foreach (Instruction inst in insts)
            {
                main.AppendLine(inst.ToString());
            }

            return $"section .text\nglobal _start\n{prefixes}_start:\n{main}jmp exit";
        }
    }
}
