namespace Rewriting.WebApp;

public class OrderDetailsService : AbstractApiGetService<OrderDetailsModel>
{

    public OrderDetailsService(HttpClient httpClient, WebAppSettings settings, IAuthService authService)
        : base(httpClient, settings, authService)
    {
    }

    public async Task<OrderDetailsModel> GetOrderAsync(Guid uid)
    {
        return await GetDataAsync($"v1/Orders/GetOrderDetails?orderUid={uid}");
    }
}
