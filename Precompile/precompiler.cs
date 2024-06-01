using lapis.Constants;

namespace velt.Precompiler
{
    public class Precompiler
    {
        public static void Compile(string b)
        {
            string[] lines = b.Split(';');

            foreach (string line in lines)
            {
                if (line.StartsWith(Consts.Token_define))
                {
                    string[] ops = line[Consts.Token_define.Length..].Split(' ');
                    b = b.Replace($"@{ops[0]}", ops[1]);
                }
            }
        }
    }
}
