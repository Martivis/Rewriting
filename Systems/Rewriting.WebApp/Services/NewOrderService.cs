using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Rewriting.WebApp.Services
{
    public class NewOrderService : AbstractListService<OrderModel>
    {
        public NewOrderService(HttpClient httpClient, WebAppSettings settings) : base(httpClient, settings)
        {
        }

        protected override string GetEndpointUrn()
        {
            return "v1/Orders/GetNewOrders";
        }
    }
}
