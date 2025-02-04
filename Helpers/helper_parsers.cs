﻿using lapis.asm.inst;
using lapis.asm.ptr;
using lapis.constants;
using System.Text.RegularExpressions;

namespace lapis.helpers
{
    public class HelperParsers(VarMap varMap, FuncMap funcMap)
    {
        protected VarMap varMap = varMap;
        protected FuncMap funcMap = funcMap;

        protected Tuple<List<Instruction>, string, string> ParseRawValue(byte type, string name, string raw_val)
        {
            string[] spl = raw_val.Split("@", 2);
            List<Instruction> inst = [];

            string value_type = spl[0];
            string value = spl[1];

            Console.WriteLine(value_type);

            string val;
            switch (value_type)
            {
                case Consts.Token_ptr:
                case Consts.Token_deref:
                case Consts.Token_var:
                    val = varMap.GetVarPtr(value);
                    break;
                case Consts.Token_value:
                    val = Types.Type.Value(value, type);
                    break;
                case Consts.Token_type:
                    val = value;
                    break;
                case Consts.Token_expr:
                    string ptr = varMap.GetVarPtr(name);
                    var (ext, b) = ParseExpr(ptr, Types.Type.Size(type), value);
                    val = b;
                    inst = ext;
                    break;
                default:
                    throw new Exception($"Error: invalid value type '{value_type}'");
            }

            return new Tuple<List<Instruction>, string, string>(inst, val, value_type);
        }

        protected Instruction.Asm ParseAsmTemplate(string asm)
        {
            char tempChar = '$';
            int tempCharLen = 1;
            string pattern = $@"\{tempChar}.";

            Regex regex = new Regex(pattern);
            string newAsm = regex.Replace(asm, m =>
            {
                var val = m.Value.Substring(tempCharLen);
                var ptr = varMap.GetVarPtr(val);

                return ptr;
            });

            return new Instruction.Asm(newAsm);
        }

        protected Tuple<List<Instruction>, string> ParseExpr(string name, byte nameSize, string expr)
        {
            char indexingOperator = ':';
            string pattern =
            $@"(?=[{
                $"{Instruction.Xor.Operator}" +
                $"{Instruction.Mul.Operator}" +
                $"{Instruction.Add.Operator}" +
                $"{Instruction.Sub.Operator}" +
                $"{Instruction.Div.Operator}" +
                $"{Instruction.And.Operator}" +
                $"{Instruction.Or.Operator}" +
                $"{indexingOperator}"
            }])";

            string[] split = Regex.Split(expr, pattern);
            string resRegister = PtrSize.CopyRegisterName(nameSize, 0);

            List<Instruction> insts =
            [
                new Instruction.Mov(resRegister, name)
            ];

            string first = split[0].Trim();
            byte first_t = varMap.GetVarType(first, null);
            bool skipFirst = true;
            int curr_ind = 0;

            string prev = first;

            for (var i = 0; i < split.Length; i++)
            {
                string element = split[i];
                if (skipFirst)
                {
                    curr_ind += element.Length + 1;
                    skipFirst = false;
                    continue;
                }

                string num = element[1..].Trim();
                switch (element[0])
                {
                    case Instruction.Add.Operator:

                        if (Types.Type.isNumber(first_t))
                        {
                            string reg1 = PtrSize.CopyRegisterName(nameSize, 1);

                            insts.Add(new Instruction.Mov(reg1, num));
                            insts.Add(new Instruction.Add(nameSize));
                        }
                        throw new Exception("Error: cannot apply arithmetical operation on non-number type");

                    case Instruction.Sub.Operator:

                        if (Types.Type.isNumber(first_t))
                        {
                            string reg1 = PtrSize.CopyRegisterName(nameSize, 1);

                            insts.Add(new Instruction.Mov(reg1, num));
                            insts.Add(new Instruction.Sub(nameSize));
                            break;
                        }
                        throw new Exception("Error: cannot apply arithmetical operation on non-number type");

                    case Instruction.Mul.Operator:

                        if (Types.Type.isNumber(first_t))
                        {
                            string reg1 = PtrSize.CopyRegisterName(nameSize, 1);

                            insts.Add(new Instruction.Mov(reg1, num));
                            insts.Add(new Instruction.Mul(nameSize));
                            break;
                        }
                        throw new Exception("Error: cannot apply arithmetical operation on non-number type");

                    case Instruction.Div.Operator:

                        if (Types.Type.isNumber(first_t))
                        {
                            string reg1 = PtrSize.CopyRegisterName(nameSize, 1);

                            insts.Add(new Instruction.Mov(reg1, num));
                            insts.Add(new Instruction.Div(nameSize));
                        }
                        throw new Exception("Error: cannot apply arithmetical operation on non-number type");

                    case Instruction.And.Operator:

                        if (Types.Type.isNumber(first_t))
                        {
                            string reg1 = PtrSize.CopyRegisterName(nameSize, 1);

                            insts.Add(new Instruction.Mov(reg1, num));
                            insts.Add(new Instruction.And(nameSize));
                            break;
                        }
                        throw new Exception("Error: cannot apply arithmetical operation on non-number type");

                    case Instruction.Or.Operator:

                        if (Types.Type.isNumber(first_t))
                        {
                            string reg1 = PtrSize.CopyRegisterName(nameSize, 1);

                            insts.Add(new Instruction.Mov(reg1, num));
                            insts.Add(new Instruction.Or(nameSize));
                            break;
                        }
                        throw new Exception("Error: cannot apply arithmetical operation on non-number type");

                    case Instruction.Xor.Operator:

                        if (Types.Type.isNumber(first_t))
                        {
                            string reg1 = PtrSize.CopyRegisterName(nameSize, 1);

                            insts.Add(new Instruction.Mov(reg1, num));
                            insts.Add(new Instruction.Xor(nameSize));
                            break;
                        }
                        throw new Exception("Error: cannot apply arithmetical operation on non-number type");

                    case ':':

                        Var ind_var = new Var(
                            Gen.Generate(Types.Type.Size(Types.Type.U64)), 
                            Types.Type.U64
                        );
                        var ind_ptr = ind_var.Pointer();
                        varMap.SetVar(Consts.Default_index, ind_var);

                        Var arr = varMap.GetVar(prev);
                        string arr_head = arr.Head;
                        byte arr_type = arr.Type;

                        byte element_type = (byte)(arr_type - Types.Type.Array);
                        byte element_type_size = Types.Type.Size(element_type);

                        (List<Instruction> extra, string _name, string _) = ParseRawValue
                        (
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

            insts.Add(new Instruction.Mov(name, resRegister));
            return new Tuple<List<Instruction>, string>
            (
                insts, 
                varMap.GetVarPtr(first)
            );
        }
    }
}
