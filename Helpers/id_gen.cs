namespace lapis.Helpers
{
    public class IdGen
    {
        public static string Gen(int len)
        {
            const string chars = "0123456789ABCDEF";

            Random random = new Random();

            string randomString = new string(Enumerable.Range(0, len)
                .Select(_ => chars[random.Next(chars.Length)])
                .ToArray());

            return $".{randomString}";
        }
    }
}
