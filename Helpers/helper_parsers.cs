using lapis.Asm.Inst;
using lapis.Asm.Ptr;
using lapis.Constants;
using System.Text.RegularExpressions;

namespace lapis.Helpers
{
    public class HelperParsers
    {
        protected VarMap varMap;
        protected FuncMap funcMap;

        public HelperParsers(VarMap varMap, FuncMap funcMap)
        {
            this.varMap = varMap;
            this.funcMap = funcMap;
        }
        protected Tuple<List<Instruction>, string> ParseRawValue(byte type, string name, string raw_val)
        {
            string ptr = varMap.GetVarPtr(name);

            string val = string.Empty;
            List<Instruction> inst = new List<Instruction>();
            string[] spl = raw_val.Split("@", 2);

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
                    val = b;
                    inst = ext;
                    break;
                default:
                    throw new Exception($"Error: invalid value type '{value_type}'");
            }

            return new Tuple<List<Instruction>, string>(inst, val);
        }

        protected Tuple<List<Instruction>, string> ParseExpr(string name, string expr)
        {
            string pattern = @"(?=[*+-/:])";

            List<Instruction> insts = new List<Instruction>();
            string[] split = Regex.Split(expr, pattern);

            string first = split[0];
            byte first_t = varMap.GetVarType(first);
            bool skipFirst = true;
            int curr_ind = 0;

            string prev = first;

            for (var i = 0; i < split.Length; i++)
            {
                string element = split[i];
                if (skipFirst)
                {
                    curr_ind += element.Length+1;
                    skipFirst = false;
                    continue;
                }

                string num = element.Substring(1);
                switch (element[0])
                {
                    case '+':
                        if (Types.Type.isNumber(first_t))
                        {
                            insts.Add(new Instruction.Add(name, num));
                            break;
                        }
                        throw new Exception("Error: cannot apply arithmetical operation on non-number type");
                    case '-':
                        if (Types.Type.isNumber(first_t))
                        {
                            insts.Add(new Instruction.Sub(name, num));
                            break;
                        }
                        throw new Exception("Error: cannot apply arithmetical operation on non-number type");
                    case '*':
                        if (Types.Type.isNumber(first_t))
                        {
                            insts.Add(new Instruction.Mul(name, num));
                            break;
                        }
                        throw new Exception("Error: cannot apply arithmetical operation on non-number type");
                    case '/':
                        if (Types.Type.isNumber(first_t))
                        {
                            insts.Add(new Instruction.Div(name, num));
                            break;
                        }
                        throw new Exception("Error: cannot apply arithmetical operation on non-number type");
                    case ':':
                        Var ind_var = new Var(
                            Gen.Generate(Types.Type.Size(Types.Type.U64)), 
                            Types.Type.U64
                        );
                        var ind_ptr = ind_var.Ptr;
                        varMap.SetVar(Consts.Default_index, ind_var);

                        Var arr = varMap.GetVar(prev);
                        string arr_head = arr.Head;
                        byte arr_type = arr.Type;

                        byte element_type = (byte)(arr_type - Types.Type.Array);
                        byte element_type_size = Types.Type.Size(element_type);

                        (List<Instruction> extra, string _name) = ParseRawValue(
                            Types.Type.U64,
                            Consts.Default_index, 
                            expr.Substring(curr_ind)
                        );

                        List<Instruction> ind_init_insts =
                        [
                            new Instruction.Mov(ind_ptr, _name),
                            .. extra,
                            new Instruction.Mov(
                                name,
                                Gen.Make(
                                    element_type, 
                                    $"{arr_head} - {ind_ptr}*{element_type_size}"
                                )
                            ),
                        ];

                        insts.AddRange(ind_init_insts);
                        break;
                    default:
                        throw new Exception("Error: Invalid operator");
                }

                prev = num;
                curr_ind += element.Length;
            }

            return new Tuple<List<Instruction>, string>
            (
                insts, 
                varMap.GetVarPtr(first)
            );
        }
    }
}
