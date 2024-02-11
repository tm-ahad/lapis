using System.Text;
using System.Text.Json;

namespace lapis.Helpers
{
    public class Fetcher
    {
        string home_path;

        public Fetcher()
        {
            PlatformID platform = Environment.OSVersion.Platform;

            switch (platform)
            {
                case PlatformID.Win32NT:
                    home_path = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.lapis";
                    break;
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    home_path = $"{Environment.GetEnvironmentVariable("HOME")}/.lapis";
                    break;
                default:
                    throw new Exception($"Unsupported platform {platform}");
            }
        }

        static string ReadFile(string filePath)
        {
            StringBuilder res = new StringBuilder();
            if (File.Exists(filePath))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            res.AppendLine(line);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                throw new Exception("Lapis is not installed on your machine.");
            }

            return res.ToString();
        }

        public string FetchPrefixes()
        {
            StringBuilder res = new StringBuilder();

            string dir = $"{home_path}/essentials";

            if (!Directory.Exists(dir))
            {
                throw new Exception("Lapis is not installed on your machine.");
            }

            string[] files = Directory.GetFiles(dir);

            foreach (string file in files)
            {
                string content = File.ReadAllText(file);
                res.AppendLine(content);
            }

            return res.ToString();
        }
        public string GetStdLib(string name)
        {
            string path = $"{home_path}/lib/{name}.lps";
            return ReadFile(path);
        }
    }
}
