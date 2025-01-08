using System.Text;
using Newtonsoft.Json;
using Common;

public class AuthService
{
    private readonly string _baseUrl;

    private static readonly HttpClient _httpClient = new HttpClient();
    public AuthService(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    public async Task<string> AuthenticateUser(string username, string password)
    {
        var url = $"{_baseUrl}/users/login-password";
        var payload = new
        {
            UserName = username,
            Password = password
        };

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
        };

        try
        {
            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();

            dynamic data = JsonConvert.DeserializeObject(responseData);
            string accessToken = data.Data.AccessToken;
            string refreshToken = data.Data.RefreshToken;

            CacheUtils.SaveCache("accessToken", accessToken);
            CacheUtils.SaveCache("refreshToken", refreshToken);

            Console.WriteLine("Authenticated: " + JsonConvert.SerializeObject(data, Formatting.Indented));

            return accessToken;
        }
        catch (Exception ex)
        {
            throw new Exception("Authentication failed: " + ex.Message);
        }
    }

    public async Task<string> RefreshAccessToken(string refreshToken)
    {
        var url = $"{_baseUrl}/auth/refresh";
        var payload = new { RefreshToken = refreshToken };

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
        };

        try
        {
            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();

            dynamic data = JsonConvert.DeserializeObject(responseData);
            string accessToken = data.AccessToken;

            CacheUtils.SaveCache("accessToken", accessToken);
            return accessToken;
        }
        catch (Exception ex)
        {
            throw new Exception("Token refresh failed: " + ex.Message);
        }
    }
}
