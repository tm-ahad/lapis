﻿using lapis.asm.ptr;

namespace lapis.constants
{
    public class Consts {
        // Tokens

        public const string Token_funcheck = "funcheck";
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
        public const string Token_deref = "deref";
        public const string Token_include = "inc";
        public const string Token_struct = "str";
        public const string Token_self = "self";
        public const string Token_type = "type";
        public const string Token_macro = "macro";
        public const string Token_define = "define";
        public const string Token_arr_def = "[]";
        public const string Token_ptr = "ptr";
        public const char Token_comment = '!';
        public const char Token_prop = ':';

        // Built-in variables

        public const string Default_operand1 = "__TEMP_OPERAND1";
        public const string Default_operand2 = "__TEMP_OPERAND2";
        public const string Default_index = "__TEMP_INDEX";

        public const string Default_element = "__TEMP_ELEMENT";
       
        //Compiler config

        public const string Default_func_res = "res";
        public const string Default_struct_property_value = "0";

        public const byte Ptr_size = PtrSize.DWORD;
        public const byte TokenLen = 3;

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

        //Other config
        public const string lib_dir = "/libs";
        public const string main_file = "main.velt";
    }
}
