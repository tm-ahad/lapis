using lapis.Asm.Gen;
using lapis.Asm.Inst;
using lapis.parser; 

namespace lapis
{
    class Lapis
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: lapis <input_file> <output_file>");
                return;
            }

            string input_file = args[0];
            string output_file = args[1];

            Parser parser = new Parser();

            try
            {
                string fileContents = File.ReadAllText(input_file);
                List<Instruction> insts = parser.Parse(fileContents);
                string asm_out = await Gen.GenerateAsync(insts);

                File.WriteAllText(output_file, asm_out);

                Console.WriteLine($"Compiled {input_file} to {output_file}");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found: " + input_file);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}
