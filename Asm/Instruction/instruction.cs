using lapis.Asm.Ptr;
using lapis.Constants;
using lapis.Helpers;
using System.Runtime.CompilerServices;

namespace lapis.Asm.Inst
{
    public interface OperatorInstruction: Instruction
    {
        public static char Operator = char.MinValue;
    }

    public interface Instruction
    {
        public string ToString();

        public sealed class Mov : Instruction
        {
            public string To;
            public string From;

            public Mov(string To, string From)
            {
                this.To = To;
                this.From = From;
            }

            public override string ToString() => $"mov {To},{From}";
        }

        public sealed class Copy : Instruction
        {
            private string To;
            private string From;
            private byte ToSize;
            private byte FromSize;
            private bool isLea;

            public Copy(bool isLea, string To, byte ToSize, string From, byte FromSize)
            {
                this.To = To;
                this.From = From;
                this.ToSize = ToSize;
                this.FromSize = FromSize;
                this.isLea = isLea;
            }

            public override string ToString() {
                string crn = PtrSize.CopyRegisterName(ToSize, 0);
                string comm = isLea ? "lea" : ToSize > FromSize ? "movzx" : "mov";

                return $"{comm} {crn},{From}\nmov {To}, {crn}";
            }
        }

        public sealed class Add : OperatorInstruction
        {
            public const char Operator = '+';
            private byte size;

            public Add(byte size)
            {
                this.size = size;
            }

            public override string ToString()
            {
                string reg1 = PtrSize.CopyRegisterName(size, 0);
                string reg2 = PtrSize.CopyRegisterName(size, 1);
                return $"add {reg1},{reg2}";
            }
        }

        public sealed class Sub : OperatorInstruction
        {
            public const char Operator = '-';
            private byte size;

            public Sub(byte size)
            {
                this.size = size;
            }

            public override string ToString()
            {
                string reg1 = PtrSize.CopyRegisterName(size, 0);
                string reg2 = PtrSize.CopyRegisterName(size, 1);
                return $"sub {reg1},{reg2}";
            }
        }

        public sealed class Mul : OperatorInstruction
        {
            public const char Operator = '*';
            private byte size;

            public Mul(byte size)
            {
                this.size = size;
            }

            public override string ToString()
            {
                string reg1 = PtrSize.CopyRegisterName(size, 0);
                string reg2 = PtrSize.CopyRegisterName(size, 1);
                return $"imul {reg1},{reg2}";
            }
        }

        public sealed class Div : OperatorInstruction
        {
            public const char Operator = '/';
            private byte size;

            public Div(byte size)
            {
                this.size = size;
            }

            public override string ToString()
            {
                string reg1 = PtrSize.CopyRegisterName(size, 0);
                string reg2 = PtrSize.CopyRegisterName(size, 1);
                return $"idiv {reg1},{reg2}";
            }
        }

        public sealed class Call : Instruction
        {
            private string function;

            public Call(string function)
            {
                this.function = function;
            }

            public override string ToString() => $"call {function}";
        }

        public sealed class Func : Instruction
        {
            private string function;

            public Func(string function)
            {
                this.function = function;
            }

            public override string ToString() => $"{function}:";
        }

        public sealed class Asm : Instruction
        {
            private string code;

            public Asm(string asm) 
            {
                code = asm;
            }

            public override string ToString() => code;
        }

        public sealed class CmpOp : Instruction
        {
            private ECmpOperations op;
            private string label;

            public CmpOp(ECmpOperations op, string label)
            {
                this.op = op;
                this.label = label;
            }

            public override string ToString() => $"{CmpOperations.ToString(op)} {label}";
        }

        public sealed class Cmp : Instruction
        {
            private string a;
            private string b;

            public Cmp(string a, string b)
            {
                this.a = a;
                this.b = b;
            }

            public override string ToString() => $"cmp {a},{b}";
        }

        public sealed class Jmp : Instruction
        {
            private string label;

            public Jmp(string label)
            {
                this.label = label;
            }

            public override string ToString() => $"jmp {label}";
        }

        public sealed class And : OperatorInstruction
        {
            public const char Operator = '&';
            private byte size;

            public And(byte size)
            {
                this.size = size;
            }

            public override string ToString()
            {
                string reg1 = PtrSize.CopyRegisterName(size, 0);
                string reg2 = PtrSize.CopyRegisterName(size, 1);
                return $"and {reg1},{reg2}";
            }
        }

        public sealed class Or : OperatorInstruction
        {
            public const char Operator = '|';
            private byte size;

            public Or(byte size)
            {
                this.size = size;
            }

            public override string ToString()
            {
                string reg1 = PtrSize.CopyRegisterName(size, 0);
                string reg2 = PtrSize.CopyRegisterName(size, 1);
                return $"or {reg1},{reg2}";
            }
        }

        public sealed class Xor : OperatorInstruction
        {
            public const char Operator = '^';
            private byte size;

            public Xor(byte size)
            {
                this.size = size;
            }

            public override string ToString()
            {
                string reg1 = PtrSize.CopyRegisterName(size, 0);
                string reg2 = PtrSize.CopyRegisterName(size, 1);
                return $"xor {reg1},{reg2}";
            }
        }

        public sealed class DerefPtr : Instruction
        {
            private string DerefingPtr;
            private string ptr;
            private byte size;

            public DerefPtr(string Derefing_ptr, string ptr, byte size)
            {
                DerefingPtr = Derefing_ptr;
                this.ptr = ptr;
                this.size = size;
            }

            public override string ToString()
            {
                string reg1 = PtrSize.CopyRegisterName(Consts.Ptr_size, 0);
                string reg2 = PtrSize.CopyRegisterName(size, 1);

                return $"mov {reg1}, {DerefingPtr}\nmov {reg2},dword [{reg1}]\nmov {ptr}, {reg2}";
            }
        }

        public sealed class Ret : Instruction
        {
            public override string ToString() => "ret";
        }
    }
}
