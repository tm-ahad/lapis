using lapis.Constants;
using lapis.Helpers;

namespace lapis.Asm.Assembler
{
    public class Assembler
    {
        public Fetcher fetcher = new();

        public void Assemble(string name) 
        {
            string asmFile = $"{name}.asm";
            string assembleCommand = $"{Consts.assembler} {asmFile} {name}.{Consts.assemblerOutputFileExtension}";

            fetcher.ExecuteCommand(assembleCommand);
        }
    }
}
