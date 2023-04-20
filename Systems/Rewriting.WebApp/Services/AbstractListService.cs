using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Rewriting.WebApp.Services;

public abstract class AbstractListService<TData>
{
    private readonly HttpClient _httpClient;
    private readonly WebAppSettings _settings;

    protected AbstractListService(HttpClient httpClient, WebAppSettings settings)
    {
        _httpClient = httpClient;
        _settings = settings;
    }

    public async Task<IEnumerable<TData>> GetOrdersAsync(int page, int pageSize)
    {
        string url = $"{_settings.ApiUri}/{GetEndpointUrn()}?page={page}&pageSize={pageSize}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        var data = JsonSerializer.Deserialize<IEnumerable<TData>>(content, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<TData>();

        return data;
    }

    protected abstract string GetEndpointUrn();
}
