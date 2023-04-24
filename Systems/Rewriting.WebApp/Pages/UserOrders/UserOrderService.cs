namespace Rewriting.WebApp;

public class UserOrderService : AbstractListService<OrderModel>
{
    public UserOrderService(HttpClient httpClient, WebAppSettings settings, IAuthService authService)
        : base(httpClient, settings, authService)
    {
    }

    protected override string GetEndpoint()
    {
        return "v1/Orders/GetOrdersByUser";
    }
}
