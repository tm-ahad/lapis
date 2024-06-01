using lapis.Asm.Assembler;
using lapis.Asm.Gen;
using lapis.Asm.Inst;
using lapis.Link;
using lapis.parser;
using velt.Context;
using velt.Precompiler;

namespace lapis
{
    class Lapis
    {
        static void Main(string[] args)
        {

            if (args.Length < 1)
            {
                Console.WriteLine("Usage: velt <input_file> <output_file>");
                return;
            }
            else if (args[0] == "version")
            {
                Console.WriteLine("Version 0.1.0");
                return;
            }

            string inputFile = args[0];
            string outputFile = args[1];

            Context ctx = new();
            Parser parser = new(ctx);
            Assembler asm = new();
            Linker linker = new();

            try
            {
                string code = File.ReadAllText(inputFile);

                Precompiler.Compile(code);
                List<Instruction> insts = parser.Parse(code);
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
