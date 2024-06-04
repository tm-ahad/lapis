using lapis.helpers;

namespace lapis.context
{
    public class Context(string dir, bool lib)
    {
        public readonly StructMap structMap = new();
        public readonly FuncMap funcheckMap = new();
        public readonly FuncMap funcMap = new();
        public string loopName = string.Empty;
        public ECmpOperations loopComp = 0;
        public string working_dir = dir;
        public string? op1 = null;
        public string? op2 = null;
        public bool inSe = true;
        public bool isLib = lib;
    }
}
