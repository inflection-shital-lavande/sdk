using System.Net.Http.Headers;
using System.Text;
using Common;
using Newtonsoft.Json;
public class APIClient
{
    private readonly string _baseUrl;
    private static readonly HttpClient _httpClient = new HttpClient();

    public APIClient(string baseUrl)
    {
        _baseUrl = baseUrl;

        _accessToken = CacheUtils.Get("accessToken");
        _refreshToken = CacheUtils.Get("refreshToken");
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

            SetAccessToken(accessToken);
            SetRefreshToken(refreshToken);

            Console.WriteLine("Authenticated: " + JsonConvert.SerializeObject(JsonConvert.DeserializeObject(responseData), Formatting.Indented));

            return accessToken;
        }
        catch (Exception ex)
        {
            throw new Exception("Authentication failed: " + ex.Message);
        }
    }
    public async Task<string> RefreshAccessToken()
    {
        if (_refreshToken == null)
        {
            throw new Exception("Refresh token is not available");
        }

        var url = $"{_baseUrl}/auth/refresh";
        var payload = new { RefreshToken = _refreshToken };

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

            SetAccessToken(accessToken);
            return accessToken;
        }
        catch (Exception ex)
        {
            throw new Exception("Token refresh failed: " + ex.Message);
        }
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


