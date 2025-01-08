using Microsoft.Extensions.Configuration;
using sdk.demo.SDK;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets<Program>();

        var configuration = builder.Build();

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

        var userName = configuration["USER_NAME"];
        var password = configuration["PASSWORD"];

        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        {
            Console.WriteLine("User credentials are missing in the configuration.");
            return;
        }

        Console.WriteLine("Initializing SDK...");
        var sdk = new SDK(baseUrl);

        try
        {
            // Authenticate the user and set the access token
            string accessToken = await sdk.Client.AuthenticateUser(userName, "Inflection@123");
            sdk.Client.SetAccessToken(accessToken);
            Console.WriteLine("Access Token Set Successfully.");

            await ExecuteOperations(sdk);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Authentication failed: {ex.Message}");
        }
    }

    private static async Task ExecuteOperations(SDK sdk)
    {
        try
        {
            Console.WriteLine("Executing operations...");
            await sdk.User.ExecuteUserOperations();

            Console.WriteLine("Operations executed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while executing operations: {ex.Message}");
        }
    }
}


