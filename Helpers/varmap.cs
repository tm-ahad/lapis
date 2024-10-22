﻿using lapis.asm.ptr;
using lapis.constants;

namespace lapis.helpers
{
    public class VarMap
    {
        private readonly Dictionary<string, Var> map = [];
        
        public VarMap() 
        {
            Var temp_index = new(Gen.Generate(4), Types.Type.U16);
            Var temp_op1 = new(Gen.Generate(2), Types.Type.Byte);
            Var temp_op2 = new(Gen.Generate(2), Types.Type.Byte);
            Var res = new(Gen.Generate(8), 255);

            map[Consts.Default_operand1] = temp_op1;
            map[Consts.Default_operand2] = temp_op2;
            map[Consts.Default_index] = temp_index;
            map[Consts.Default_func_res] = res;
        }

        public string GetVarPtr(string var_name)
        {
            if (map.ContainsKey(var_name)) 
            {
                return map[var_name].Pointer();
            }
            else
            {
                throw new Exception($"Error: var {var_name} not found");
            }
        }

        public string GetVarHead(string var_name)
        {
            if (map.TryGetValue(var_name, out Var? value))
            {
                return value.Head;
            }
            else
            {
                throw new Exception($"Error: var {var_name} not found");
            }
        }

        public byte GetVarType(string var_name, byte? def)
        {
            if (map.TryGetValue(var_name, out Var? value))
            {
                return value.Type;
            }
            else if (def == null)
            {
                throw new Exception($"Error: var {var_name} not found");
            }
            else 
            {
                return (byte)def;
            }
        }
        public Var GetVar(string var_name)
        {
            if (map.TryGetValue(var_name, out Var? value))
            {
                return value;
            }
            else
            {
                throw new Exception($"Error: var {var_name} not found");
            }
        }

        public void SetVar(string var_name, Var var)
        {
            map[var_name] = var;
        }
    }
}
