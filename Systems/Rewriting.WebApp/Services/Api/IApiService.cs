namespace Rewriting.WebApp;

public interface IApiService
{
    Task<TData> GetDataAsync<TData>(string urn);
    Task PostDataAsync<TData>(string urn, TData data);
    Task PatchDataAsync<TData>(string urn, TData data);

}