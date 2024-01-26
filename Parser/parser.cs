using lapis.Asm.Inst;
using lapis.Asm.Ptr;
using lapis.Constants;
using lapis.Helpers;

namespace lapis.parser
{
    public class Parser
    {
        private VarMap varMap;
        private int token_len = 3;

        public Parser() 
        {
            varMap = new VarMap();
        }

        private List<Instruction> ParseVarDecr(string line) 
        {
            string[] split = line.Split(' ');
            string type = split[1];
            string value_type_and_name = split[2];

            string[] spl = value_type_and_name.Split("@");
            string value_type = spl[0];
            string name = spl[1];

            string raw_val = split[3];
            string val;

            switch (value_type)
            {
                case Consts.Token_var:
                    val = varMap.GetVar(raw_val);
                    break;
                case Consts.Token_value:
                    val = Types.Type.Value(raw_val, Types.Type.FromString(type));
                    break;
                default:
                    throw new Exception("Error: invalid value type '{value_type}'");
            }

            byte size = Types.Type.Size(Types.Type.FromString(type));
            string ptr = Gen.Generate(size);

            var insts = new List<Instruction>
            {
                new Instruction.Mov(ptr, val)
            };

            varMap.SetVar(name, ptr);
            return insts;
        }

        private List<Instruction> ParseSetDecr(string line)
        {
            var split = line.Split(" ");
            string a = split[1];
            string b = split[2];

            string a_ptr = varMap.GetVar(a);
            string b_ptr = varMap.GetVar(b);

            List<Instruction> insts = new List<Instruction>
            {
                new Instruction.Mov(a_ptr, b_ptr)
            };

            return insts;
        }

        public List<Instruction> Parse(string code)
        {
            List<Instruction> insts = new List<Instruction>();

            foreach (string line in code.Split(";"))
            {
                string tok = line.Substring(0, token_len);

                switch (tok)
                {
                    case Consts.Token_var: 
                        insts.Concat(ParseVarDecr(line));
                        break;
                    case Consts.Token_set: 
                        insts.Concat(ParseSetDecr(line));
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
