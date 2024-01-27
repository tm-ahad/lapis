using lapis.Asm.Inst;
using lapis.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace lapis.Asm.Gen
{
    public class Gen
    {
        public static async Task<string> GenerateAsync(List<Instruction> insts)
        {
            StringBuilder main = new StringBuilder();
            string prefixes = await Fetcher.FetchPrefixes();

            foreach (Instruction inst in insts)
            {
                main.AppendLine(inst.ToString());
            }

            return $"section .text\nglobal _start\n{prefixes}\n_start:\n{main}call exit";
        }
    }
}
