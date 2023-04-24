using System.Net.Http.Headers;
using System.Text.Json;

namespace Rewriting.WebApp;

public class OrderDetailsService
{
    private HttpClient _httpClient;
    private WebAppSettings _settings;
    private IAuthService _authService;

    public OrderDetailsService(HttpClient httpClient, WebAppSettings settings, IAuthService authService)
    {
        _httpClient = httpClient;
        _settings = settings;
        _authService = authService;
    }

    public async Task<OrderDetailsModel> GetOrderAsync(Guid uid)
    {
        await SetAuthHeader();

        string url = $"{_settings.ApiUri}/v1/Orders/GetOrderDetails?orderUid={uid}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(content);

        var data = JsonSerializer.Deserialize<OrderDetailsModel>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new OrderDetailsModel();

        return data;
    }

    private async Task SetAuthHeader()
    {
        var accessToken = await _authService.GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
    }
}
