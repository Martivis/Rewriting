using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Rewriting.Services.Cache;

internal class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<TData?> Get<TData>(string key) where TData : class, new()
    {
        var dataString = await _cache.GetStringAsync(key);

        if (dataString is null)
            return null;

        return JsonSerializer.Deserialize<TData>(dataString) ?? new TData();
    }

    public async Task Set<TData>(string key, TData value)
    {
        await _cache.SetStringAsync(key, JsonSerializer.Serialize(value), new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(10)
        });
    }

    public async Task Remove(string key)
    {
        await _cache.RemoveAsync(key);
    }
}
