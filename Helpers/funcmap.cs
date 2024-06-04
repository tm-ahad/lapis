namespace lapis.helpers
{
    public class FuncMap
    {
        private readonly Dictionary<string, FuncSign> map = [];

        public List<Tuple<string, byte>> GetFuncParams(string var_name)
        {
            if (map.TryGetValue(var_name, out FuncSign? value))
            {
                return value.arguments;
            }
            else
            {
                throw new Exception($"Error: var {var_name} not found");
            }
        }

        public byte GetFuncRetType(string var_name)
        {
            if (map.TryGetValue(var_name, out FuncSign? value))
            {
                return value.retType;
            }
            else
            {
                throw new Exception($"Error: var {var_name} not found");
            }
        }
        public FuncSign GetFunc(string var_name)
        {
            if (map.TryGetValue(var_name, out FuncSign? value))
            {
                return value;
            }
            else
            {
                throw new Exception($"Error: var {var_name} not found");
            }
        }

        public void SetFunc(string var_name, FuncSign func)
        {
            map[var_name] = func;
        }

        public Dictionary<string, FuncSign> Map() 
        {
            return map;
        }

        public bool Contains(KeyValuePair<string, FuncSign> sign) 
        {
            return map.Contains(sign);
        }
    }
}
