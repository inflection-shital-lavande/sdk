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

        // Initialize SDK with the base URL
        var sdk = new SDK(baseUrl);
        try
        {
            // Authenticate the user and set the access token
            Console.WriteLine("Authenticating...");
            string accessToken = await sdk.Client.AuthenticateUser("admin", "uHqLYqjh");
            sdk.Client.SetAccessToken(accessToken);
            Console.WriteLine("Authentication successful. Access token set.");

            // Execute operations
            await ExecuteOperations(sdk);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
    private static async Task ExecuteOperations(SDK sdk)
    {
        try
        {
            Console.WriteLine("Executing operations...");
            await sdk.User.ExecuteUserOperations();
            await sdk.ActionPlan.ExecuteActionPlanOperations();
            await sdk.Animation.ExecuteAnimationOperations();
            await sdk.Appointment.ExecuteAppointmentOperations();
            Console.WriteLine("Operations executed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while executing operations: {ex.Message}");
        }
    }
}
