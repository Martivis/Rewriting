using Microsoft.Extensions.DependencyInjection;

namespace Rewriting.Services.TextComparer;

public static class Bootstraper
{
    public static IServiceCollection AddTextComparer(IServiceCollection services)
    {
        services.AddTransient<ITextCanonizer, TextCanonizer>();
        services.AddTransient<IShingleParser, ShingleParser>();
        services.AddTransient<ITextComparer, TextComparer>();

        return services;
    }
}