using System.Text.Json;
using System.Net.Http.Headers;

namespace Rewriting.WebApp;

public abstract class AbstractListService<TData> : AbstractApiGetService<IEnumerable<TData>>
{

    protected AbstractListService(HttpClient httpClient, WebAppSettings settings, IAuthService authService)
        : base (httpClient, settings, authService)
    {     
    }

    public async Task<IEnumerable<TData>> GetItemsAsync(int page, int pageSize)
    {
        return await GetDataAsync(GetEndpoint() + GetParameters() + $"&page={page}&pageSize={pageSize}");
    }

    protected abstract string GetEndpoint();

    private string GetParameters()
    {
        var additionalParameters = GetAdditionalParameters();
        return "?" + additionalParameters;
    }

    protected virtual string GetAdditionalParameters() => "";
}
