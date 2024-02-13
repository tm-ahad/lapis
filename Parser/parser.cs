﻿using lapis.Asm.Inst;
using lapis.Asm.Ptr;
using lapis.Constants;
using lapis.Helpers;

namespace lapis.parser
{
    public class Parser : HelperParsers
    {
        private new readonly FuncMap funcMap = new();
        private readonly Fetcher fetcher = new();
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

            Var varA = varMap.GetVar(a);
            byte type = varA.Type;
            string ptr = varA.Ptr;

            var (extra, name) = ParseRawValue(type, a, b);

            List<Instruction> insts = extra;

            if (name != ptr)
            {
                insts.Insert(0, new Instruction.Mov(ptr, name));
            }

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
            ptr = var.Ptr;
            varMap.SetVar(name, var);

            if (size == PtrSize.UNKNOWN)
            {
                return [];
            }

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
                string arg_ptr = varMap.GetVarPtr(arg_name);

                string param = parms[i];

                var (ext, val) = ParseRawValue(arg_type, arg_name, param);
                insts.Add(new Instruction.Mov(arg_ptr, val));
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
            string code = line.Substring(token_len).Trim();
            return new Instruction.Asm(code);
        }

        private List<Instruction> ParseControlFlow(string line)
        {
            string[] split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string keyword = split[0];
            var op_map = CmpOperations.Map();

            var op1_ptr = varMap.GetVarPtr(Consts.Default_operand1);
            var op2_ptr = varMap.GetVarPtr(Consts.Default_operand2);

            var (ext1, operand1) = ParseRawValue
            (
                Types.Type.Byte,
                Consts.Default_operand1,
                split[1]
            );

            var (ext2, operand2) = ParseRawValue
            (
                Types.Type.Byte,
                Consts.Default_operand2,
                split[3]
            );
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
                    new Instruction.Cmp(operand1, operand2), 
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
