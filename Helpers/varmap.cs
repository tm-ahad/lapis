using System;

namespace lapis.Helpers
{
    public class VarMap
    {
        private Dictionary<string, Var> map = new Dictionary<string, Var>();
        public VarMap() { }

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
