using lapis.asm.assembler;
using lapis.asm.gen;
using lapis.asm.inst;
using lapis.constants;
using lapis.precompiler;
using lapis.context;
using lapis.parser;
using lapis.link;
using LibGit2Sharp;

namespace lapis
{
    class Lapis
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: lapis build");
                return;
            }

            string inputFile = Consts.main_file;
            string outputFile = "main";

            Assembler asm = new();
            Linker linker = new();

            switch (args[0]) 
            {
                case "version":
                    Console.WriteLine("Version 0.1.0");
                    break;
                case "get":
                    string url = args[1];
                    string path = Repository.Clone(url, Consts.lib_dir);
                    string code = File.ReadAllText(Path.Combine(path, Consts.main_file));

                    Context context = new(path, true);
                    Parser _parser = new(context);
                    List<Instruction> insts = _parser.Parse(code);

                    string asm_out = Gen.Generate(insts, true);
                    File.WriteAllText($"{outputFile}.asm", asm_out);

                    asm.Assemble(outputFile);
                    linker.Link(outputFile);

                    break;
                default: 
                    break;
            }

            Context ctx = new("/", false);
            Parser parser = new(ctx);

            try
            {
                string code = File.ReadAllText(inputFile);

                Precompiler.Compile(code);
                List<Instruction> insts = parser.Parse(code);
                string asm_out = Gen.Generate(insts, false);

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
