using Microsoft.Extensions.DependencyInjection;

namespace Rewriting.Services.Orders;

public static class OrdersServiceBootstraper
{
    public static IServiceCollection AddOrderService(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderObservable>(provider => provider.GetService<IOrderService>() as OrderService);
        return services;
    }
}
