namespace Crawlers.Core.Shared;

public interface IBaseHttpClient
{
    public Task<string> GetAsync(string url);
}

public class BaseHttpClient : IBaseHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BaseHttpClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> GetAsync(string url)
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.37.3");
        
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}