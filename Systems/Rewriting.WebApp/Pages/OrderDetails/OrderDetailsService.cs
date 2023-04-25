namespace Rewriting.WebApp;

public class OrderDetailsService
{
    private readonly IApiGetService _apiService;

    public OrderDetailsService(IApiGetService apiService)
    {
        _apiService = apiService;
    }

    public async Task<OrderDetailsModel> GetOrderAsync(Guid uid)
    {
        return await _apiService.GetDataAsync<OrderDetailsModel>($"v1/Orders/GetOrderDetails?orderUid={uid}");
    }
}
