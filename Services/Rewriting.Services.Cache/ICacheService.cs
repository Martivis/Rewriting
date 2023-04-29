namespace Rewriting.Services.Cache;

public interface ICacheService
{
    Task<TData?> Get<TData>(string key) where TData : class, new();
    Task Remove(string key);
    Task Set<TData>(string key, TData value);
}