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

            public override string ToString()
            {
                return $"mov {To}, {From}";
            }
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

            public override string ToString()
            {
                return $"add {To},{From}";
            }
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

            public override string ToString()
            {
                return $"sub {To},{From}";
            }
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

            public override string ToString()
            {
                return $"imul {To},{From}";
            }
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

            public override string ToString()
            {
                return $"idiv {To},{From}";
            }
        }

        public sealed class Call : Instruction
        {
            public string function;

            public Call(string function)
            {
                this.function = function;
            }

            public override string ToString()
            {
                return $"call {function}";
            }
        }
    }
}
