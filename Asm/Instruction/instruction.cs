using lapis.Helpers;

namespace lapis.Asm.Inst
{
    public interface Instruction
    {
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

        public sealed class Add : Instruction
        {
            public string To;
            public string From;

            public Add(string To, string From)
            {
                this.To = To;
                this.From = From;
            }

            public override string ToString() => $"add {To},{From}";
        }

        public sealed class Sub : Instruction
        {
            public string To;
            public string From;

            public Sub(string To, string From)
            {
                this.To = To;
                this.From = From;
            }

            public override string ToString() => $"sub {To},{From}";
        }

        public sealed class Mul : Instruction
        {
            public string To;
            public string From;

            public Mul(string To, string From)
            {
                this.To = To;
                this.From = From;
            }

            public override string ToString() => $"imul {To},{From}";
        }

        public sealed class Div : Instruction
        {
            public string To;
            public string From;

            public Div(string To, string From)
            {
                this.To = To;
                this.From = From;
            }

            public override string ToString() => $"idiv {To},{From}";
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

        public sealed class Ret : Instruction
        {
            public Ret() { }
            public override string ToString() => "ret";
        }
    }
}
