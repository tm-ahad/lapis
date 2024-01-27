using lapis.Asm.Inst;
using lapis.Constants;
using System.Text.RegularExpressions;

namespace lapis.Helpers
{
    public class HelperParsers
    {
        protected VarMap varMap;
        public HelperParsers(VarMap varMap)

        {
            this.varMap = varMap;
        }

        protected Tuple<List<Instruction>, string> ParseRawValue(byte type, string name, string raw_val)
        {
            string ptr = varMap.GetVarPtr(name);

            string val = string.Empty;
            List<Instruction> inst = new List<Instruction>();
            string[] spl = raw_val.Split("@");

            string value_type = spl[0];
            string value = spl[1];

            switch (value_type)
            {
                case Consts.Token_var:
                    val = varMap.GetVarPtr(value);
                    break;
                case Consts.Token_value:
                    val = Types.Type.Value(value, type);
                    break;
                case Consts.Token_expr:
                    var (ext, b) = ParseExpr(ptr, value);
                    val = varMap.GetVarPtr(b);
                    inst = ext;
                    break;
                default:
                    throw new Exception($"Error: invalid value type '{value_type}'");
            }

            return new Tuple<List<Instruction>, string>(inst, val);
        }

        protected Tuple<List<Instruction>, string> ParseExpr(string name, string expr)
        {
            List<Instruction> insts = new List<Instruction>();
            string first = string.Empty;
            bool skipFirst = true;
            string pattern = @"(?=[*+-/])";
            string[] split = Regex.Split(expr, pattern);

            foreach (string element in split)
            {
                if (skipFirst)
                {
                    skipFirst = false;
                    first = element;
                    continue;
                }

                string num = element.Substring(1);

                switch (element[0])
                {
                    case '+':
                        insts.Add(new Instruction.Add(name, num));
                        break;
                    case '-':
                        insts.Add(new Instruction.Sub(name, num));
                        break;
                    case '*':
                        insts.Add(new Instruction.Mul(name, num));
                        break;
                    case '/':
                        insts.Add(new Instruction.Div(name, num));
                        break;
                    default:
                        throw new Exception("Error: Invalid operator");
                }
            }

            return new Tuple<List<Instruction>, string>(insts, first);
        }
    }
}
