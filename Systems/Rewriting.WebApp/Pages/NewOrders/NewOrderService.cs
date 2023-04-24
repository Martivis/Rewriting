namespace Rewriting.WebApp;

public class NewOrderService : AbstractListService<OrderModel>
{
    public NewOrderService(HttpClient httpClient, WebAppSettings settings, IAuthService authService) 
        : base(httpClient, settings, authService)
    {
    }

    protected override string GetEndpoint()
    {
        return "v1/Orders/GetNewOrders";
    }
}
