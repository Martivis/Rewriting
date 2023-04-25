using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Rewriting.WebApp;

public class ApiService : IApiService
{
    private HttpClient _httpClient;
    private WebAppSettings _settings;
    private IAuthService _authService;

    public ApiService(HttpClient httpClient, WebAppSettings settings, IAuthService authService)
    {
        _httpClient = httpClient;
        _settings = settings;
        _authService = authService;
    }

    public async Task<TData> GetDataAsync<TData>(string urn)
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

    public async Task PostDataAsync<TData>(string urn, TData data)
    {
        await SetAuthHeader();
        string url = $"{_settings.ApiUri}/{urn}";

        var body = JsonSerializer.Serialize(data);
        var request = new StringContent(body, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, request);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(content);
    }

    public async Task PatchDataAsync<TData>(string urn, TData data)
    {
        await SetAuthHeader();
        string url = $"{_settings.ApiUri}/{urn}";

        var body = JsonSerializer.Serialize(data);
        var request = new StringContent(body, Encoding.UTF8, "application/json");

        var response = await _httpClient.PatchAsync(url, request);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(content);
    }

    private async Task SetAuthHeader()
    {
        var accessToken = await _authService.GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
    }
}
