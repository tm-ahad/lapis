namespace lapis.Constants
{
    public class Consts {
        // Tokens

        public const string Token_var = "var";
        public const string Token_set = "set";
        public const string Token_add = "add";
        public const string Token_sub = "sub";
        public const string Token_mul = "mul";
        public const string Token_div = "div";
        public const string Token_fun = "fun";
        public const string Token_cal = "cal";
        public const string Token_asm = "asm";
        public const string Token_ret = "ret";
        public const string Token_unt = "unt";
        public const string Token_std = "std";
        public const string Token_if = "@if";
        public const string Token_expr = "expr";
        public const string Token_value = "lit";
        public const string Token_comment = "###";
        public const string Token_include = "inc";
        public const string Token_self = "self";

        // Built-in variables

        public const string Default_operand1 = "__TEMP_OPERAND1";
        public const string Default_operand2 = "__TEMP_OPERAND2";
        public const string Default_func_res = "res";
        public const string Default_index = "__TEMP_INDEX";

        //Assembler config

        public const string flags = "-felf64";

        //Assembly config

        public const string copyRegister8 = "bl";
        public const string copyRegister16 = "ax";
        public const string copyRegister32 = "ecx";
        public const string copyRegister64 = "rdx";

    }
}
