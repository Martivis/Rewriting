using Microsoft.Extensions.DependencyInjection;
using Rewriting.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.RabbitMQ;

public static class RabbitMQBootstraper
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services)
    {
        var settings = SettingsLoader.Load<RabbitMQSettings>("RabbitMQ");
        services.AddSingleton(settings);
        services.AddSingleton<IRabbitMQ, RabbitMQ>();
        return services;
    }
}
