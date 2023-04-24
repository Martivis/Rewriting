using System.Text.Json;
using System.Net.Http.Headers;

namespace Rewriting.WebApp;

public abstract class AbstractListService<TData>
{
    private readonly HttpClient _httpClient;
    private readonly WebAppSettings _settings;
    private readonly IAuthService _authService;

    protected AbstractListService(HttpClient httpClient, WebAppSettings settings, IAuthService authService)
    {
        _httpClient = httpClient;
        _settings = settings;
        _authService = authService;        
    }

    public async Task<IEnumerable<TData>> GetItemsAsync(int page, int pageSize)
    {
        var headerTask = SetAuthHeader();

        string url = $"{_settings.ApiUri}/{GetEndpointUrn()}?page={page}&pageSize={pageSize}{GetRequestParameters()}";

        await headerTask;
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(content);

        var data = JsonSerializer.Deserialize<IEnumerable<TData>>(content, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<TData>();

        return data;
    }

    private async Task SetAuthHeader()
    {
        var accessToken = await _authService.GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
    }

    protected abstract string GetEndpointUrn();

    private string GetRequestParameters()
    {
        var additionalParameters = GetAdditionalParameters();
        if (string.IsNullOrEmpty(additionalParameters))
            return "";
        return "&" + additionalParameters;
    }
    protected virtual string GetAdditionalParameters() => "";
}
