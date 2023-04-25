namespace Rewriting.WebApp;

public class OrderService
{
    private readonly IApiService _apiService;

    public OrderService(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<OrderDetailsModel> GetOrderDetailsAsync(Guid uid)
    {
        return await _apiService.GetDataAsync<OrderDetailsModel>($"v1/Orders/GetOrderDetails?orderUid={uid}");
    }

    public async Task AddNewOrder(AddOrderModel model)
    {
        await _apiService.PostDataAsync("v1/Orders/AddOrder", model);
    }

    public async Task CancelOrder(Guid orderUid)
    {
        await _apiService.PatchDataAsync("v1/Orders/CancelOrder", orderUid);
    }
}
