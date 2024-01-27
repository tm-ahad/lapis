using lapis.Asm.Inst;
using lapis.Asm.Ptr;
using lapis.Constants;
using lapis.Helpers;

namespace lapis.parser
{
    public class Parser : HelperParsers
    {
        private int token_len = 3;

        public Parser() : base(new VarMap()) { }

        private List<Instruction> ParseSetDecr(string line)
        {
            var split = line.Split(" ");
            string a = split[1];
            string b = split[2];

            Var varA = varMap.GetVar(a);
            byte type = varA.Type;
            string ptr = varA.Ptr;

            var (extra, name) = ParseRawValue(type, a, b);

            List<Instruction> insts = new List<Instruction>
            {
                new Instruction.Mov(ptr, name)
            };
            insts.AddRange(extra);

            return insts;
        }
        private List<Instruction> ParseVarDecr(string line)
        {
            string[] split = line.Split(' ');
            string type = split[1];
            string name = split[2];

            string raw_val = split[3];
            
            byte size = Types.Type.Size(Types.Type.FromString(type));
            string ptr = Gen.Generate(size);

            Var var = new Var(ptr, Types.Type.FromString(type));
            varMap.SetVar(name, var);

            var (extra, val) = ParseRawValue(
                Types.Type.FromString(type),
                name,
                raw_val
            );

            var insts = new List<Instruction>
            {
                new Instruction.Mov(ptr, val)
            };

            insts.AddRange(extra);

            return insts;
        }

        public List<Instruction> Parse(string code)
        {
            List<Instruction> insts = new List<Instruction>();

            foreach (string lin in code.Split(";"))
            {
                string line = lin.Trim();
                if (line.Length == 0) continue;

                string tok = line.Substring(0, token_len);

                switch (tok)
                {
                    case Consts.Token_var:
                        insts.AddRange(ParseVarDecr(line));
                        break;
                    case Consts.Token_set:
                        insts.AddRange(ParseSetDecr(line));
                        break;
                    case Consts.Token_comment:
                        break;
                    default:
                        throw new Exception($"Error: Line starts with invalid token '{tok}'");
                }
            }

            return insts;
        }
    }
}
