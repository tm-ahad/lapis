using lapis.constants;
using lapis.helpers;

namespace lapis.asm.assembler
{
    public class Assembler
    {
        private Fetcher fetcher = new();

        public void Assemble(string name) 
        {
            string asmFile = $"{name}.asm";
            string assembleCommand = $"{Consts.assembler} {asmFile} {name}.{Consts.assemblerOutputFileExtension}";

            fetcher.ExecuteCommand(assembleCommand);
        }
    }
}
