namespace lapis.Helpers
{
    public class VarMap
    {
        private Dictionary<string, string> map = new Dictionary<string, string>();
        public VarMap() { }

        public string GetVar(string var_name)
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

        public void SetVar(string var_name, string ptr)
        {
            map[var_name] = ptr;
        }
    }
}
