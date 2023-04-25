
namespace Rewriting.WebApp;

public class UserOrdersService : AbstractListService<OrderModel>
{
    public UserOrdersService(IApiService apiService) : base(apiService)
    {
    }

    protected override string GetEndpoint()
    {
        return "v1/Orders/GetOrdersByUser";
    }
}
