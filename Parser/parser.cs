﻿using lapis.constants;
using lapis.asm.inst;
using lapis.asm.ptr;
using lapis.helpers;
using lapis.context;

namespace lapis.parser
{
    public class Parser(Context ctx) : HelperParsers(new VarMap(), new FuncMap())
    {
        private readonly Include inc = new(ctx);
        private readonly Context ctx = ctx;

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
                    insts.Insert(0, new Instruction.Copy(false, ptr, size, name, nameSize));
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
                    insts.Insert(0, new Instruction.Copy(false, destPtr, aItemSize, name, nameSize));
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
                type = type[1..];

                IEnumerable<Struct> structs_t = ctx.structMap.map
                    .Where(struc => struc.Name == type);
                Struct struct_t;

                if (!structs_t.Any())
                {
                    throw new Exception($"Error: struct {type} not found");
                }
                else
                {
                    struct_t = structs_t.First();
                }

                foreach (var pair in struct_t.Props) 
                {
                    string prop_name = $"{name}{Consts.Token_prop}{pair.Key}";
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

            int arrayElements = Types.Type.arrElements(type);
            string ptr;

            var (extra, val, valType) = ParseRawValue
            (
                Types.Type.FromString(type),
                name,
                raw_val
            );

            if (arrayElements != -1)
            {
                ptr = Gen.Generate((uint)arrayElements*size);
                string[] vals = raw_val.Split('@')[1].Split(',');

                for (int i = 0; i < vals.Count(); i++) {
                    uint i_ptr = uint.Parse(ptr) + (uint)i*size;
                    Console.WriteLine(i_ptr);
                    insts.Add(new Instruction.Mov(Gen.Make(size, i_ptr.ToString()), vals[i]));
                }
            } 
            else
            {
                ptr = Gen.Generate(size);
            }

            Var var = new Var(ptr, Types.Type.FromString(type));
            ptr = var.Pointer(); 

            varMap.SetVar(name, var);

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
            else if (valType == Consts.Token_deref)
            {
                insts.Add(new Instruction.DerefPtr(val, ptr, _size));
            }
            else
            {
                bool isLea = valType == Consts.Token_ptr;
                insts.Add(new Instruction.Copy(isLea, ptr, _size, val, valSize));
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

            if (ctx.loopName != string.Empty) 
            {
                insts.Insert(0, new Instruction.CmpOp(ctx.loopComp, ctx.loopName));
            }

            ctx.inSe = false;

            return insts;
        }
        
        private List<Instruction> ParseCallFunc(string line)
        {
            uint init_ptr_offset = Gen.GetCurr();

            string[] split = line.Split(" ");
            string func_name = split[1];
            string[] parms = split[2].Trim().Split(",");

            var func_args = funcMap.GetFuncParams(func_name);
            List<Instruction> insts = new();

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

                insts.Add(new Instruction.Copy(false, arg_ptr, arg_size, val, valSize));
                insts.AddRange(ext);
            }

            insts.Add(new Instruction.Call(func_name));
            Gen.SetCurr(init_ptr_offset);

            return insts;
        }

        private List<Instruction> ParseFuncDecr(string line)
        {
            Tuple<string, FuncSign> fun = FuncSign.ParseFuncSign(line);

            uint init_ptr_offset = Gen.GetCurr();
            string[] split = line.Split(' ');
            string name = split[2].Trim();
            string parms = string.Join(' ', split.Skip(3).ToArray());

            byte ret_type = fun.Item2.retType;

            Var res = varMap.GetVar(Consts.Default_func_res);
            res.Type = ret_type;
            varMap.SetVar(Consts.Default_index, res);

            List<Tuple<string, byte>> args = [];

            foreach (Tuple<string, byte> arg in fun.Item2.arguments)
            {
                string argName = arg.Item1;
                byte argType = arg.Item2;

                Var argVar = new(
                    Gen.Generate(Types.Type.Size(argType)),
                    argType
                );

                varMap.SetVar(argName, argVar);
                args.Add(Tuple.Create(argName, argType));
            }

            FuncSign func = new(ret_type, args);
            funcMap.SetFunc(name, func);

            List<Instruction> insts =
            [
                new Instruction.Func(name)
            ];

            Gen.SetCurr(init_ptr_offset);
            return insts;
        }

        private Instruction.Asm ParseAsmDecr(string line)
        {
            string asmLine = line.Substring(Consts.TokenLen).Trim();
            return ParseAsmTemplate(asmLine);
        }

        private List<Instruction> ParseControlFlow(string line)
        {
            string[] split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string keyword = split[0];
            var opMap = CmpOperations.Map();

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

            ECmpOperations? op_val_nullable = opMap[op];

            if (op_val_nullable == null) 
            {
                throw new Exception($"Unknown cmp operation {op}");
            }
        
            string label_name = IdGen.Gen(10);

            List<Instruction> insts = 
            [
                new Instruction.CmpOp(ECmpOperations.Je, label_name),
                new Instruction.Func(label_name),
            ];

            if (ctx.op1 != split[1] || ctx.op2 != split[3])
            {
                insts = 
                [
                    .. ext1,
                    .. ext2,
                    new Instruction.Mov(REGISTER, operand2),
                    new Instruction.Cmp(operand1, REGISTER), 
                    .. insts
                ];

                ctx.op1 = split[1];
                ctx.op2 = split[3];
            }

            if (keyword == Consts.Token_unt) 
            {
                ctx.loopName = label_name;
                ctx.loopComp = ECmpOperations.Je;
            }

            return insts;
        }

        private List<Instruction> ParseInclude(string line)
        {
            string[] split = line.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            string include = split[1];
            string[] incSplit = include.Split('@', 2, StringSplitOptions.RemoveEmptyEntries);

            string includeType = incSplit[0];
            string includePath = incSplit[1];

            return includeType switch
            {
                Consts.Token_self => inc.Self(includePath),
                Consts.Token_std => inc.Std(includePath),
                _ => throw new Exception($"Error: invalid include type '{includeType}'"),
            };
        }

        private void ParseStructDecr(string code)
        {
            var lines = code.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var decrLine = lines.First();
            var propDecrLines = lines.Skip(1);

            var split = decrLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var structName = split[1].Trim();

            Dictionary<string, byte> props = [];

            foreach (string line in propDecrLines)
            {
                var spl = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                byte propType = Types.Type.FromString(spl.First());
                string propName = spl[1].Trim();

                props.Add(propName, propType);
            }

            ctx.structMap.map.Add(new Struct(structName, props));
        }

        public List<Instruction> Parse(string code)
        {
            List<Instruction> insts = [];

            foreach (string lin in code.Split(";", StringSplitOptions.RemoveEmptyEntries))
            {
                string line = lin.Trim();
                string tok = line[..Consts.TokenLen];

                if (ctx.isLib) 
                {
                    if (!ctx.inSe) 
                    {
                        continue;
                    }
                    else if (tok == Consts.Token_fun) 
                    {
                        ctx.inSe = false;
                    } 
                    else if (tok == Consts.Token_ret) 
                    {
                        ctx.inSe = true;
                    }
                    else if (tok != Consts.Token_struct) 
                    {
                        continue;
                    }
                }

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
                    case Consts.Token_funcheck:
                        Tuple<string, FuncSign> fun = FuncSign.ParseFuncSign(line);
                        ctx.funcheckMap.SetFunc(fun.Item1, fun.Item2);
                        break;
                    default:
                        if (tok.StartsWith(Consts.Token_comment)) 
                        {
                            continue;
                        }

                        throw new Exception($"Error: Line starts with invalid token '{tok}'");
                }
            }

            foreach (KeyValuePair<string, FuncSign> pair in ctx.funcheckMap.Map()) 
            {
                if (!ctx.funcMap.Contains(pair)) 
                {
                    throw new Exception($"Error: Function {pair.Key} with signature {pair.Value.ToString(pair.Key)} must be declared");
                }
            }

            return insts;
        }
    }
}
