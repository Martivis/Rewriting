namespace Rewriting.WebApp;

public class UserOrdersService : AbstractListService<OrderModel>
{
    public UserOrdersService(IApiGetService apiService) : base(apiService)
    {
    }

    protected override string GetEndpoint()
    {
        return "v1/Orders/GetOrdersByUser";
    }
}
