using System.Net.Http.Headers;
using System.Text;
using Common;
using Newtonsoft.Json;
using sdk.demo.src.api.action_plan.ActionPlanService;
using sdk.demo.src.api.animation.AnimationService;
using sdk.demo.src.api.appointment.AppointmentService;
using sdk.demo.src.api.user.UserService;
public class APIClient
{
    private readonly string _baseUrl;
    private static readonly HttpClient _httpClient = new HttpClient();
    public User Users { get; }
    public ActionPlan ActionPlans { get; }
    public Animation Animations { get; }
    public Appointment Appointments { get; }
    public APIClient(string baseUrl)
    {
        _baseUrl = baseUrl;

        _accessToken = CacheUtils.Get("accessToken");
        _refreshToken = CacheUtils.Get("refreshToken");

        Users = new User(this);
        ActionPlans = new ActionPlan(this);
        Animations = new Animation(this);
        Appointments = new Appointment(this);
    }

    private string? _accessToken;
    private string? _refreshToken;

    public void SetAccessToken(string accessToken)
    {
        _accessToken = accessToken;
        CacheUtils.SaveCache("accessToken", accessToken);
    }

    public void SetRefreshToken(string refreshToken)
    {
        _refreshToken = refreshToken;
        CacheUtils.SaveCache("refreshToken", refreshToken);
    }

    public async Task<string> AuthenticateUser(string username, string password)
    {
        var authService = new AuthService(_baseUrl);
        return await authService.AuthenticateUser(username, password);
    }

    public async Task<string> RefreshAccessToken()
    {
        if (_refreshToken == null)
        {
            throw new Exception("Refresh token is not available");
        }

        var authService = new AuthService(_baseUrl);
        return await authService.RefreshAccessToken(_refreshToken);
    }

    protected internal async Task<string> Request(string endpoint, HttpMethod method, object? data = null)
    {
        try
        {
            var url = $"{_baseUrl}{endpoint}";
            var requestMessage = new HttpRequestMessage(method, url);


            if (_accessToken != null)
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            }

            if (data != null)
            {
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseData);

                if (jsonResponse?.message == "Token expired")
                {
                    await RefreshAccessToken();
                    return await Request(endpoint, method, data);
                }
            }

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }

        catch (HttpRequestException ex)
        {
            throw new Exception("Request failed: " + ex.Message);
        }
    }
    public void ClearCachedTokens()
    {
        CacheUtils.ClearCache();
        Console.WriteLine("Cached tokens cleared.");
    }
}


