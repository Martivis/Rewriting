using Microsoft.Extensions.DependencyInjection;

namespace Rewriting.Services.TextComparer;

public static class Bootstraper
{
    public static IServiceCollection AddTextComparer(this IServiceCollection services)
    {
        services.AddTransient<IHashCounter, Crc32HashCounter>();
        services.AddTransient<ITextCanonizer, TextCanonizer>();
        services.AddTransient<IShingleParser, ShingleParser>();
        services.AddTransient<ITextComparer, TextComparer>();

        return services;
    }
}