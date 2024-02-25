namespace velt.Helpers
{
    public enum EBitwiseOperators
    {
        And,
        Or,
        Xor,
        Lsh,
        Rsh
    }

    public class BitWiseOperator
    {
        public static string ToString(EBitwiseOperators op)
        {
            switch (op) 
            {
                case EBitwiseOperators.And: return "and";
                case EBitwiseOperators.Or: return "or";
                case EBitwiseOperators.Xor: return "xor";
                default: return "SUIIIIIIIIII";
            }
        }
    }
}
