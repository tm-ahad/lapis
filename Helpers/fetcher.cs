using System.Text;
using System.Text.Json;

namespace lapis.Helpers
{
    public class Fetcher
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<string> FetchAsync(string url, CancellationToken cancellationToken = default)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url, cancellationToken);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching content from {url}: {ex.Message}");
                throw;
            }
        }

        public static async Task<string> FetchPrefixes(CancellationToken cancellationToken = default)
        {
            string jsonUrl = "https://raw.githubusercontent.com/tm-ahad/lapis/master/Asm_prefix/prefixes.json";
            string jsonContent = await FetchAsync(jsonUrl, cancellationToken);

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
                        string fileUrl = $"https://raw.githubusercontent.com/tm-ahad/lapis/master/Asm_prefix/{fileName}";

                        string fileContent = await FetchAsync(fileUrl, cancellationToken);
                        concatenatedContents.AppendLine($"{fileContent}\n");
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
    }
}
