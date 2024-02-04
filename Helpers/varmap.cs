using lapis.Asm.Ptr;
using lapis.Constants;
using System;

namespace lapis.Helpers
{
    public class VarMap
    {
        private Dictionary<string, Var> map = new Dictionary<string, Var>();
        public VarMap() 
        {
            Var temp_index = new Var(Gen.Generate(4), Types.Type.U16);
            Var temp_op1 = new Var(Gen.Generate(1), Types.Type.Byte);
            Var temp_op2 = new Var(Gen.Generate(1), Types.Type.Byte);
            Var res = new Var(Gen.Generate(8), 255);

            map[Consts.Default_temp_operand1] = temp_op1;
            map[Consts.Default_temp_operand2] = temp_op2;
            map[Consts.Default_temp_index] = temp_index;
            map[Consts.Default_func_res] = res;
        }

        public string GetVarPtr(string var_name)
        {
            if (map.ContainsKey(var_name)) 
            {
                return map[var_name].Ptr;
            }
            else
            {
                throw new Exception($"Error: var {var_name} not found");
            }
        }

        public string GetVarHead(string var_name)
        {
            if (map.ContainsKey(var_name))
            {
                return map[var_name].Head;
            }
            else
            {
                throw new Exception($"Error: var {var_name} not found");
            }
        }

        public byte GetVarType(string var_name)
        {
            if (map.ContainsKey(var_name))
            {
                return map[var_name].Type;
            }
            else
            {
                throw new Exception($"Error: var {var_name} not found");
            }
        }
        public Var GetVar(string var_name)
        {
            if (map.ContainsKey(var_name))
            {
                return map[var_name];
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
