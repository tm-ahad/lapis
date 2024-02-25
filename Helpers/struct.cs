namespace velt.Helpers
{
    public class Struct
    {
        public Dictionary<string, byte> Props;
        public string Name;

        public Struct(string name, Dictionary<string, byte> props)
        {
            Name = name;
            Props = props;
        }
    }
}
