using System.Net.Http.Headers;
using System.Text.Json;

namespace Rewriting.WebApp;

public abstract class AbstractApiGetService<TData>
{
    private HttpClient _httpClient;
    private WebAppSettings _settings;
    private IAuthService _authService;

    public AbstractApiGetService(HttpClient httpClient, WebAppSettings settings, IAuthService authService)
    {
        _httpClient = httpClient;
        _settings = settings;
        _authService = authService;
    }

    protected async Task<TData> GetDataAsync(string urn)
    {
        await SetAuthHeader();

        string url = $"{_settings.ApiUri}/{urn}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(content);

        var data = JsonSerializer.Deserialize<TData>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) 
            ?? throw new HttpRequestException("Unable to get data");

        return data;
    }

    private async Task SetAuthHeader()
    {
        var accessToken = await _authService.GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
    }
}
