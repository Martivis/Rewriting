namespace Rewriting.WebApp.Services
{
    public class UserOrderService : AbstractListService<OrderModel>
    {
        public UserOrderService(HttpClient httpClient, WebAppSettings settings) : base(httpClient, settings)
        {
        }

        protected override string GetEndpointUrn()
        {
            return "v1/Orders/GetOrdersByUser";
        }
    }
}
