using sdk.demo.SDK;
using sdk.demo.src.api.action_plan.ActionPlanService;
using sdk.demo.src.api.animation.AnimationService;
using sdk.demo.src.api.appointment.AppointmentService;
using sdk.demo.src.api.user.UserService;

public class Program
{
    public static async Task Main(string[] args)
    {
        const string base_url = "http://localhost:3456/api/v1";

        // Initialize SDK with the base URL
        var sdk = new SDK(base_url);

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
            await User.ExecuteUserOperations(sdk.Client);
            await ActionPlan.ExecuteActionPlanOperations(sdk.Client);
            await Animation.ExecuteAnimationOperations(sdk.Client);
            await Appointment.ExecuteAppointmentOperations(sdk.Client);
            Console.WriteLine("Operations executed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while executing operations: {ex.Message}");
        }
    }
}
