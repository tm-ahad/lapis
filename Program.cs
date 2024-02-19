using lapis.Asm.Assembler;
using lapis.Asm.Gen;
using lapis.Asm.Inst;
using lapis.Link;
using lapis.parser; 

namespace lapis
{
    class Lapis
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: lapis <input_file> <output_file>");
                return;
            }

            string inputFile = args[0];
            string outputFile = args[1];

            Parser parser = new Parser();
            Assembler asm = new Assembler();
            Linker linker = new Linker();

            try
            {
                string fileContents = File.ReadAllText(inputFile);
                List<Instruction> insts = parser.Parse(fileContents);
                string asm_out = Gen.Generate(insts);

                File.WriteAllText($"{outputFile}.asm", asm_out);

                asm.Assemble(outputFile);
                linker.Link(outputFile);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found: " + inputFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}
