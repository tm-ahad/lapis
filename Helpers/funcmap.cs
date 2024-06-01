namespace lapis.Helpers
{
    public class FuncMap
    {
        private readonly Dictionary<string, Func> map = [];

        public List<Tuple<string, byte>> GetFuncParams(string var_name)
        {
            if (map.ContainsKey(var_name))
            {
                return map[var_name].Params;
            }
            else
            {
                throw new Exception($"Error: var {var_name} not found");
            }
        }

        public byte GetFuncRetType(string var_name)
        {
            if (map.ContainsKey(var_name))
            {
                return map[var_name].Ret_type;
            }
            else
            {
                throw new Exception($"Error: var {var_name} not found");
            }
        }
        public Func GetFunc(string var_name)
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

        public void SetFunc(string var_name, Func func)
        {
            map[var_name] = func;
        }
    }
}
