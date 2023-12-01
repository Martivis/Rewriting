using Microsoft.Extensions.DependencyInjection;

namespace Rewriting.Services.TextComparer;

public static class Bootstraper
{
    public static IServiceCollection AddTextComparer(IServiceCollection services)
    {
        services.AddSingleton<ITextCanonizer, TextCanonizer>();
        services.AddSingleton<IShingleParser, ShingleParser>();
        services.AddSingleton<ITextComparer, TextComparer>();

        return services;
    }
}