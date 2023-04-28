using Microsoft.Extensions.DependencyInjection;

namespace Rewriting.Services.Orders;

public static class OrdersServiceBootstraper
{
    public static IServiceCollection AddOrderService(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderObservable>(provider => provider.GetService<IOrderService>() as OrderService);
        return services;
    }

    public static IServiceCollection AddCachedOrderService(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, CachedOrderService>();
        services.AddScoped<OrderService, OrderService>();
        services.AddScoped<IOrderObservable>(provider => provider.GetService<OrderService>());
        return services;
    }
}
