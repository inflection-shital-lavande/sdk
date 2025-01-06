namespace Common;

public static class CacheUtils
{
    private static readonly Dictionary<string, string> _cache = new();

    public static void SetCache(string key, string value)
    {
        _cache[key] = value;
        Console.WriteLine($"Set cache for key: {key}");
    }

    public static string Get(string key)
    {
        var value = GetCache(key);
        return value;
    }

    public static string GetCache(string key)
    {
        return _cache.ContainsKey(key) ? _cache[key] : null;
    }

    public static Dictionary<string, string> LoadCache()
    {
        return new Dictionary<string, string>
            {
                { "api_key", GetCache("api_key") },
                { "access_token", GetCache("access_token") }
            };
    }

    public static void SaveCache(string apiKey, string accessToken)
    {
        SetCache("api_key", apiKey);
        SetCache("access_token", accessToken);
        Console.WriteLine("Saved API key and access token to cache.");
    }

    public static void ClearCache()
    {
        _cache.Clear();
        Console.WriteLine("In-memory cache cleared.");
    }
}
