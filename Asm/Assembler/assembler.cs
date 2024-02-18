using lapis.Constants;
using lapis.Helpers;

namespace lapis.Asm.Assembler
{
    public class Assembler
    {
        public Fetcher fetcher;

        public Assembler()
        {
            fetcher = new Fetcher();
        }

        public void Assemble(string name) 
        {
            string asmFile = $"{name}.asm";
            string assembleCommand = $"{Consts.assembler} {asmFile}";

            fetcher.ExecuteCommand(assembleCommand);
        }
    }
}
