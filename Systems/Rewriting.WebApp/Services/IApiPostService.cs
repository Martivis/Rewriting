namespace Rewriting.WebApp
{
    public interface IApiPostService
    {
        Task PostDataAsync<TData>(string urn, TData data);
    }
}