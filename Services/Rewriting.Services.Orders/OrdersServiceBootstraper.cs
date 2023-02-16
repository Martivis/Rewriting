using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Orders;

public static class OrdersServiceBootstraper
{
    public static IServiceCollection AddOrderService(this IServiceCollection services)
    {
        services.AddSingleton<IOrderService, OrderService>();
        return services;
    }
}
