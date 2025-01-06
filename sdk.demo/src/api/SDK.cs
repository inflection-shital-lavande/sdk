namespace sdk.demo.SDK;
public class SDK
{
    public APIClient Client { get; set; }

    public SDK( string baseUrl)
    {
        Client = new APIClient(baseUrl);
    }
}



