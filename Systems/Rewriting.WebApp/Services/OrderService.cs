using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Rewriting.WebApp.Services;

public class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;

    public OrderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<OrderModel>> GetOrdersAsync(int page, int pageSize)
    {
        string url = $"{WebAppSettings.ApiUri}/v1/Orders/GetNewOrders?page={page}&pageSize={pageSize}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        var data = JsonSerializer.Deserialize<IEnumerable<OrderModel>>(content, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<OrderModel>();

        return data;
    }
}
