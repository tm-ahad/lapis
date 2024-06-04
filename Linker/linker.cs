using lapis.constants;
using lapis.helpers;

namespace lapis.link
{
    public class Linker
    {
        public Fetcher fetcher;

        public Linker()
        {
            fetcher = new Fetcher();
        }

        public void Link(string name) 
        {
            string objFile = $"{name}.{Consts.assemblerOutputFileExtension}";
            string linkCommand = $"{Consts.linker} {objFile} -o {name}";
            
            fetcher.ExecuteCommand(linkCommand);
        }
    }
}
