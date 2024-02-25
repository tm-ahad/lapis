using lapis.Asm.Inst;
using lapis.Asm.Ptr;
using lapis.Constants;
using lapis.Helpers;
using velt.Helpers;

namespace lapis.parser
{
    public class Parser : HelperParsers
    {
        private readonly StructMap structMap = new();
        private new readonly FuncMap funcMap = new();
        private string loop_name = string.Empty;
        private ECmpOperations loop_comp = 0;
        private readonly int token_len = 3;
        private string? op1 = null;
        private string? op2 = null;

        public Parser() : base(new VarMap(), new FuncMap()) { }

        private List<Instruction> ParseSetDecr(string line)
        {
            var split = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string a = split[1];
            string b = split[2];

            string[] splA = a.Split(":", StringSplitOptions.RemoveEmptyEntries);

            if (splA.Length == 1)
            {
                Var varA = varMap.GetVar(a);
                byte type = varA.Type;
                byte size = Types.Type.Size(type);
                string ptr = varA.Pointer();

                var (extra, name, _) = ParseRawValue(type, a, b);
                byte nameSize = Types.Type.Size(varMap.GetVarType(name, type));

                List<Instruction> insts = extra;

                if (name != ptr)
                {
                    insts.Insert(0, new Instruction.Copy(ptr, size, name, nameSize));
                }

                return insts;
            }
            else 
            {
                a = splA[0];
                string index = varMap.GetVarPtr(splA[1]);
                string varAHead = varMap.GetVarHead(a);

                byte aItemType = (byte)(varMap.GetVarType(a, null) - Types.Type.Array);
                byte aItemSize = Types.Type.Size(aItemType);


                string destHead = $"{varAHead} - {index}*{aItemSize}";

                string destPtr = Gen.Make(aItemType, destHead);
                varMap.SetVar(Consts.Default_element, new Var(destHead, aItemType));

                var (extra, name, _) = ParseRawValue(aItemType, Consts.Default_element, b);
                byte nameSize = Types.Type.Size(varMap.GetVarType(name, aItemSize));

                List<Instruction> insts = extra;

                if (name != destPtr)
                {
                    insts.Insert(0, new Instruction.Copy(destPtr, aItemSize, name, nameSize));
                }

                return insts;
            }
        }

        private List<Instruction> ParseVarDecr(string line)
        {
            var insts = new List<Instruction>();
            string[] split = line.Split(' ');
            string type = split[1];
            string name = split[2];

            if (type.StartsWith('@'))
            {
                type = type.Substring(1);

                IEnumerable<Struct> structs_t = structMap.map
                    .Where(struc => struc.Name == type);
                Struct struct_t;

                if (structs_t.Count() == 0)
                {
                    throw new Exception($"Error: struct {type} not found");
                }
                else
                {
                    struct_t = structs_t.First();
                }

                foreach (var pair in struct_t.Props) 
                {
                    string prop_name = pair.Key;
                    byte prop_type = pair.Value;
                    byte prop_size = Types.Type.Size(prop_type);

                    Var prop_var = new Var(Gen.Generate(prop_size), prop_type);

                    insts.Add(new Instruction.Mov(
                        prop_var.Pointer(), 
                        Consts.Default_struct_property_value
                    ));
                    varMap.SetVar(prop_name, prop_var);
                }

                return insts;
            }

            string raw_val = split[3];
            byte size = Types.Type.Size(Types.Type.FromString(type));

            string ptr = Gen.Generate(size);
            Var var = new Var(ptr, Types.Type.FromString(type));
            ptr = var.Pointer(); 

            varMap.SetVar(name, var);

            var (extra, val, valType) = ParseRawValue
            (
                Types.Type.FromString(type),
                name,
                raw_val
            );

            if (valType == Consts.Token_type)
            {
                return [];
            }
            else if (size == PtrSize.UNKNOWN)
            {
                throw new Exception("Error: expected value type 'type'");
            }

            byte _size = Types.Type.Size(var.Type);
            byte valSize = Types.Type.Size(varMap.GetVarType(val, var.Type));

            if (valType == Consts.Token_value)
            {
                insts.Add(new Instruction.Mov(ptr, val));
            }
            else 
            {
                insts.Add(new Instruction.Copy(ptr, _size, val, valSize));
            }

            insts.AddRange(extra);

            return insts;
        }

        private List<Instruction> ParseRet(string _) 
        {
            List<Instruction> insts =
            [
                new Instruction.Ret()
            ];

            if (loop_name != string.Empty) 
            {
                insts.Insert(0, new Instruction.CmpOp(loop_comp, loop_name));
            }

            return insts;
        }
        
        private List<Instruction> ParseCallFunc(string line)
        {
            uint init_ptr_offset = Gen.GetCurr();

            string[] split = line.Split(" ");
            string func_name = split[1];
            string[] parms = split[2].Trim().Split(",");

            List<Instruction> insts = new List<Instruction>();
            var func_args = funcMap.GetFuncParams(func_name);

            for (int i = 0; i < func_args.Count; i++)
            {
                if (parms.Length <= i)
                {
                    throw new Exception($"Error: argument {i} not found (calling function {func_name})");
                }

                var arg = func_args[i];
                string arg_name = arg.Item1;
                byte arg_type = arg.Item2;

                Var _arg = varMap.GetVar(arg_name);
                string arg_ptr = _arg.Pointer();
                byte arg_size = Types.Type.Size(_arg.Type);

                string param = parms[i];

                var (ext, val, _) = ParseRawValue(arg_type, arg_name, param);
                byte valSize = Types.Type.Size(varMap.GetVarType(val, _arg.Type));

                insts.Add(new Instruction.Copy(arg_ptr, arg_size, val, valSize));
                insts.AddRange(ext);
            }
            insts.Add(new Instruction.Call(func_name));

            Gen.SetCurr(init_ptr_offset);
            return insts;
        }

        private List<Instruction> ParseFuncDecr(string line)
        {
            uint init_ptr_offset = Gen.GetCurr();
            string[] split = line.Split(' ');
            string name = split[2].Trim();
            string parms = string.Join(' ', split.Skip(3).ToArray());

            byte ret_type = Types.Type.FromString(split[1]);

            Var res = varMap.GetVar(Consts.Default_func_res);
            res.Type = ret_type;
            varMap.SetVar(Consts.Default_index, res);

            List<Tuple<string, byte>> args = new List<Tuple<string, byte>>();

            foreach (string arg in parms.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                string[] spl = arg.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                string arg_name = spl[1];
                byte arg_type = Types.Type.FromString(spl[0]);

                Var arg_var = new Var(
                    Gen.Generate(Types.Type.Size(arg_type)), 
                    arg_type
                );

                varMap.SetVar(arg_name, arg_var);
                args.Add(Tuple.Create(arg_name, arg_type));
            }

            Func func = new Func(ret_type, args);
            funcMap.SetFunc(name, func);

            List<Instruction> insts = new List<Instruction> 
            {
                new Instruction.Func(name)
            };

            Gen.SetCurr(init_ptr_offset);
            return insts;
        }

        private Instruction ParseAsmDecr(string line)
        {
            string asmLine = line.Substring(token_len).Trim();
            return ParseAsmTemplate(asmLine);
        }

        private List<Instruction> ParseControlFlow(string line)
        {
            string[] split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string keyword = split[0];
            var op_map = CmpOperations.Map();

            var op1_ptr = varMap.GetVarPtr(Consts.Default_operand1);
            var op2_ptr = varMap.GetVarPtr(Consts.Default_operand2);

            var (ext1, operand1, _) = ParseRawValue
            (
                Types.Type.Byte,
                Consts.Default_operand1,
                split[1]
            );

            var (ext2, operand2, _) = ParseRawValue
            (
                Types.Type.Byte,
                Consts.Default_operand2,
                split[3]
            );

            var op1Size = PtrSize.ExtractSizeFromPtr(operand1);
            var op2Size = PtrSize.ExtractSizeFromPtr(operand2);

            if (op1Size != op2Size) 
            {
                throw new Exception("Error: operands are not the same size");
            }

            var REGISTER = PtrSize.CopyRegisterName(op2Size, 0);

            string op = split[2];

            ECmpOperations? op_val_nullable = op_map[op];
            ECmpOperations op_val = ECmpOperations.Je;

            if (op_val_nullable == null) 
            {
                throw new Exception($"Unknown cmp operation {op}");
            }

            op_val = op_val_nullable ?? ECmpOperations.Je;
            string label_name = IdGen.Gen(7);

            List<Instruction> insts = 
            [
                new Instruction.CmpOp(op_val, label_name),
                new Instruction.Func(label_name),
            ];

            if (op1 != split[1] || op2 != split[3])
            {
                insts = 
                [
                    .. ext1,
                    .. ext2,
                    new Instruction.Mov(REGISTER, operand2),
                    new Instruction.Cmp(operand1, REGISTER), 
                    .. insts
                ];

                op1 = split[1];
                op2 = split[3];
            }

            if (keyword == Consts.Token_unt) 
            {
                loop_name = label_name;
                loop_comp = op_val;
            }

            return insts;
        }

        private List<Instruction> ParseInclude(string line)
        {
            string[] split = line.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            string include = split[1];
            string[] inc_split = include.Split('@', 2, StringSplitOptions.RemoveEmptyEntries);

            string include_type = inc_split[0];
            string include_path = inc_split[1];

            switch (include_type) 
            {
                case Consts.Token_self:
                    return Include.Self(include_path);
                case Consts.Token_std:
                    return Include.Std(include_path);
                default:
                    throw new Exception($"Error: invalid include type '{include_type}'");
            }
        }

        private void ParseStructDecr(string code)
        {
            var lines = code.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var decrLine = lines.First();
            var propDecrLines = lines.Skip(1);

            var split = decrLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var structName = split[1].Trim();

            Dictionary<string, byte> props = new Dictionary<string, byte>();

            foreach (string line in propDecrLines)
            {
                var spl = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                byte propType = Types.Type.FromString(spl.First());
                string propName = spl[1].Trim();

                props.Add(propName, propType);
            }

            structMap.map.Add(new Struct(structName, props));
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
                    case Consts.Token_fun:
                        insts.AddRange(ParseFuncDecr(line));
                        break;
                    case Consts.Token_cal:
                        insts.AddRange(ParseCallFunc(line));
                        break;
                    case Consts.Token_asm:
                        insts.Add(ParseAsmDecr(line));
                        break;
                    case Consts.Token_ret:
                        insts.AddRange(ParseRet(line));
                        break;
                    case Consts.Token_unt:
                    case Consts.Token_if:
                        insts.AddRange(ParseControlFlow(line));
                        break;
                    case Consts.Token_include:
                        insts.AddRange(ParseInclude(line));
                        break;
                    case Consts.Token_struct:
                        ParseStructDecr(line);
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
