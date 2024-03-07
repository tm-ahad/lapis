using lapis.Helpers;
using velt.Helpers;

namespace velt.Context
{
    public class Context
    {
        public readonly StructMap structMap = new();
        public readonly FuncMap funcMap = new();
        public string loop_name = string.Empty;
        public ECmpOperations loop_comp = 0;
        public string? op1 = null;
        public string? op2 = null;
    }
}
