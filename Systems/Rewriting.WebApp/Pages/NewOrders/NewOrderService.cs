using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Rewriting.WebApp;

public class NewOrderService : AbstractListService<OrderModel>
{
    public NewOrderService(HttpClient httpClient, WebAppSettings settings, IAuthService authService) 
        : base(httpClient, settings, authService)
    {
    }

    protected override string GetEndpointUrn()
    {
        return "v1/Orders/GetNewOrders";
    }
}
