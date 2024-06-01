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
            return op switch
            {
                ECmpOperations.Je => "je",
                ECmpOperations.Jb => "jb",
                ECmpOperations.Jg => "jg",
                ECmpOperations.Jge => "jge",
                ECmpOperations.Jbe => "jbe",
                _ => throw new Exception("SUIIIIIIIIIIIIIIII"),
            };
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
