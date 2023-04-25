namespace Rewriting.WebApp
{
    public interface IApiGetService
    {
        Task<TData> GetDataAsync<TData>(string urn);
    }
}