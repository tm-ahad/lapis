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
        public const string Token_type = "type";

        // Built-in variables

        public const string Default_operand1 = "__TEMP_OPERAND1";
        public const string Default_operand2 = "__TEMP_OPERAND2";
        public const string Default_index = "__TEMP_INDEX";

        public const string Default_element = "__TEMP_ELEMENT";
        public const string Default_func_res = "res";

        //Assembler config

        public const string exeFormat = "elf64";
        public const string assembler = "fasm";
        public const string assemblerOutputFileExtension = "obj";

        //Assembly config

        public static readonly string[] coupleRegister8 = ["bl", "dl"];
        public static readonly string[] coupleRegister16 = ["ax", "cx"];
        public static readonly string[] coupleRegister32 = ["eax", "edi"];
        public static readonly string[] coupleRegister64 = ["r8", "r9"];

        public const string entry = "main";
        public const string exitLabel = "exit";

        //Linker config

        public const string linker = "ld";
    }
}
