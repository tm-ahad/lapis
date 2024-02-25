using lapis.Asm.Ptr;
using lapis.Helpers;

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
            public string To;
            public string From;
            public byte ToSize;
            public byte FromSize;

            public Copy(string To, byte ToSize, string From, byte FromSize)
            {
                this.To = To;
                this.From = From;
                this.ToSize = ToSize;
                this.FromSize = FromSize;
            }

            public override string ToString() {
                string crn = PtrSize.CopyRegisterName(ToSize, 0);

                if (ToSize > FromSize)
                {
                    return $"movzx {crn},{From}\nmov {To}, {crn}";
                }
                else 
                {
                    return $"mov {crn},{From}\nmov {To}, {crn}";
                }
            }
        }

        public sealed class Add : OperatorInstruction
        {
            public const char Operator = '+';
            public byte size;

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
            public byte size;

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
            public byte size;

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
            public byte size;

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
            public string function;

            public Call(string function)
            {
                this.function = function;
            }

            public override string ToString() => $"call {function}";
        }

        public sealed class Func : Instruction
        {
            public string function;

            public Func(string function)
            {
                this.function = function;
            }

            public override string ToString() => $"{function}:";
        }

        public sealed class Asm : Instruction
        {
            public string code;

            public Asm(string asm) 
            {
                code = asm;
            }

            public override string ToString() => code;
        }

        public sealed class CmpOp : Instruction
        {
            public ECmpOperations op;
            public string label;

            public CmpOp(ECmpOperations op, string label)
            {
                this.op = op;
                this.label = label;
            }

            public override string ToString() => $"{CmpOperations.ToString(op)} {label}";
        }

        public sealed class Cmp : Instruction
        {
            public string a;
            public string b;

            public Cmp(string a, string b)
            {
                this.a = a;
                this.b = b;
            }

            public override string ToString() => $"cmp {a},{b}";
        }

        public sealed class Jmp : Instruction
        {
            public string label;

            public Jmp(string label)
            {
                this.label = label;
            }

            public override string ToString() => $"jmp {label}";
        }

        public sealed class And : OperatorInstruction
        {
            public const char Operator = '&';
            public byte size;

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
            public byte size;

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
            public byte size;

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

        public sealed class Ret : Instruction
        {
            public Ret() { }
            public override string ToString() => "ret";
        }
    }
}
