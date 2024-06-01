using System.Diagnostics;
using System.Text;

namespace lapis.Helpers
{
    public class Fetcher
    {
        readonly PlatformID platformId;
        readonly string homePath;
        readonly string shell;

        public Fetcher()
        {
            PlatformID platform = Environment.OSVersion.Platform;
            platformId = platform;

            switch (platform)
            {
                case PlatformID.Win32NT:
                    homePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.lapis";
                    shell = "cmd.exe";
                    break;
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    homePath = $"{Environment.GetEnvironmentVariable("HOME")}/.lapis";
                    shell = "/bin/bash";
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
                    using StreamReader reader = new StreamReader(filePath);
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        res.AppendLine(line);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                throw new Exception($"Error: {filePath} not found.");
            }

            return res.ToString();
        }

        public void ExecuteCommand(string command) 
        {
            Process process = new Process();
            process.StartInfo.FileName = shell;

            if (platformId == PlatformID.Win32NT)
            {
                process.StartInfo.Arguments = $"/c \"{command}\"";
            }
            else 
            {
                process.StartInfo.Arguments = $"-c \"{command}\"";
            }

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;

            process.Start();
            process.WaitForExit();

            string error = process.StandardError.ReadToEnd();

            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine("Error:");
                Console.WriteLine(error);
            }
        }

        public string FetchPrefixes()
        {
            StringBuilder res = new StringBuilder();

            string dir = $"{homePath}/essentials";

            if (!Directory.Exists(dir))
            {
                throw new Exception("Velt is not installed on your machine.");
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
            string path = $"{homePath}/lib/{name}";
            return ReadFile(path);
        }
    }
}
