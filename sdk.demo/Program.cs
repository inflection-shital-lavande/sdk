using Microsoft.Extensions.Configuration;
using sdk.demo.SDK;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Build the configuration from appsettings.json and user secrets.
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets<Program>();

        var configuration = builder.Build();

        // Fetch API configuration
        var apiConfig = configuration.GetSection("APIConfig");
        var baseUrl = apiConfig["BaseUrl"];
        var apiKey = apiConfig["ApiKey"];


        if (string.IsNullOrEmpty(baseUrl))
        {
            Console.WriteLine("BASE_URL not found in the configuration file.");
            return;
        }

        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("API Key is required but missing in the configuration.");
        }

        Console.WriteLine("Initializing SDK...");
        var sdk = new SDK(baseUrl);
    }
}
