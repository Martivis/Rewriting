using Microsoft.Extensions.DependencyInjection;

namespace Rewriting.Services.Orders;

public static class OrdersServiceBootstraper
{
    public static IServiceCollection AddOrderService(this IServiceCollection services)
    {
        services.AddSingleton<IOrderService, OrderService>();
        return services;
    }
}
