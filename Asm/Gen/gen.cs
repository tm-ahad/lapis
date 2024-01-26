using lapis.Asm.Inst;

namespace lapis.Asm.Gen
{
    internal class Gen
    {
        public static string Generate(List<Instruction> insts) 
        {
            string main = string.Empty;

            foreach (Instruction inst in insts)
            {
                main.Concat(inst.ToString());
                main.Concat("\n");
            }

            return $"section .text\nglobal _start\n_start:{main}\ncall exit";
        }
    }
}
