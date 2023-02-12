using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Context;

public static class AppDbBootstraper
{
    public static IServiceCollection AddAppDbContextFactory(this IServiceCollection services)
    {
        var settings = Settings.SettingsLoader.Load<AppDbSettings>("Database");
        services.AddSingleton(settings);

        services.AddDbContextFactory<AppDbContext>(AppDbOptionsFactory.Configure(settings.ConnectionString));
        return services;
    }
}
