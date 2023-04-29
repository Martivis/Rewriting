using Microsoft.Extensions.DependencyInjection;
using Rewriting.Settings;

namespace Rewriting.Services.Cache;

public static class CacheBootstraper
{
    public static IServiceCollection AddAppCache(this IServiceCollection services)
    {
        var settings = SettingsLoader.Load<CacheSettings>("Cache");
        services.AddSingleton(settings);

        services.AddStackExchangeRedisCache(options =>
        {
            options.InstanceName = "cache";
            options.Configuration = settings.Url;
        });

        services.AddScoped<ICacheService, CacheService>();

        return services;
    }
}