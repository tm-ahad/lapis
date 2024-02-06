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
                Console.WriteLine("The specified file does not exist.");
            }

            return res.ToString();
        }

        public string FetchPrefixes()
        {
            string jsonUrl = $"{home_path}/Asm_prefix/prefixes.json";
            string jsonContent = ReadFile(jsonUrl);

            StringBuilder concatenatedContents = new StringBuilder();

            try
            {
                JsonDocument jsonDocument = JsonDocument.Parse(jsonContent);
                JsonElement root = jsonDocument.RootElement;

                if (root.TryGetProperty("prefixes", out JsonElement prefixesArray))
                {
                    foreach (JsonElement prefixToken in prefixesArray.EnumerateArray())
                    {
                        string fileName = prefixToken.GetString();
                        string fileUrl = $"{home_path}/Asm_prefix/{fileName}";

                        string fileContent = ReadFile(fileUrl);
                        concatenatedContents.AppendLine(fileContent);
                    }
                }
                else
                {
                    Console.WriteLine("Property 'prefixes' not found in JSON.");
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
                throw;
            }

            return concatenatedContents.ToString();
        }
        public string GetStdLib(string name)
        {
            string path = $"{home_path}/lib/{name}.lps";
            return ReadFile(path);
        }
    }
}
