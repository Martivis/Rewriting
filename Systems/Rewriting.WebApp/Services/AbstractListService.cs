using System.Text.Json;
using System.Net.Http.Headers;

namespace Rewriting.WebApp;

public abstract class AbstractListService<TData>
{
    private readonly IApiGetService _apiService;

    protected AbstractListService(IApiGetService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IEnumerable<TData>> GetItemsAsync(int page, int pageSize)
    {
        return await _apiService.GetDataAsync<IEnumerable<TData>>(GetEndpoint() + GetParameters() + $"&page={page}&pageSize={pageSize}");
    }

    protected abstract string GetEndpoint();

    private string GetParameters()
    {
        var additionalParameters = GetAdditionalParameters();
        return "?" + additionalParameters;
    }

    protected virtual string GetAdditionalParameters() => "";
}
