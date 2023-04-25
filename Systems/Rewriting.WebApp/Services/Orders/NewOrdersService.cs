namespace Rewriting.WebApp;

public class NewOrdersService : AbstractListService<OrderModel>
{
    public NewOrdersService(IApiService apiService) : base(apiService)
    {
    }

    protected override string GetEndpoint()
    {
        return "v1/Orders/GetNewOrders";
    }
}
