namespace lapis.Helpers
{
    public enum ECmpOperations 
    {
        Je,
        Jb,
        Jg,
        Jge,
        Jbe
    }

    public class CmpOperations
    {
        public static string ToString(ECmpOperations op)
        {
            switch (op)
            {
                case ECmpOperations.Je: return "je";
                case ECmpOperations.Jb: return "jb";
                case ECmpOperations.Jg: return "jg";
                case ECmpOperations.Jge: return "jge";
                case ECmpOperations.Jbe: return "jbe";
                default:
                    throw new Exception("SUIIIIIIIIIIIIIIII");
            }
        }

        public static Dictionary<string, ECmpOperations> Map()
        {
            return new Dictionary<string, ECmpOperations> 
            {
                { "equ", ECmpOperations.Je },
                { "bel", ECmpOperations.Jb },
                { "gre", ECmpOperations.Jg },
                { "egr", ECmpOperations.Jge },
                { "ebl", ECmpOperations.Jbe }
            };
        }
    }
}
