namespace lapis.helpers
{
    public class Struct(string name, Dictionary<string, byte> props)
    {
        public Dictionary<string, byte> Props = props;
        public string Name = name;
    }
}
