using Microsoft.Extensions.Options;

namespace Crawlers.Core.Shared;

public interface IIdGenService
{
    public Task<string> GetIdAsync();
    public Task<string> GetIdsAsync(int count = 1);
}

public class IdGenService : IIdGenService
{
    private readonly IOptions<IdGenConfiguration> _options;
    private readonly IBaseHttpClient _baseHttpClient;

    public IdGenService(IBaseHttpClient baseHttpClient, IOptions<IdGenConfiguration> options)
    {
        _baseHttpClient = baseHttpClient;
        _options = options;
    }
    
    public async Task<string> GetIdAsync()
    {
        return await _baseHttpClient.GetAsync(_options.Value.Url);
    }

    public async Task<string> GetIdsAsync(int count = 1)
    {
        return await _baseHttpClient.GetAsync($"{_options.Value.UrlCount}{count}");
    }
}
