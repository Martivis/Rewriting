using Microsoft.Extensions.DependencyInjection;

namespace Rewriting.Services.Offers;

public static class OfferServiceBootstraper
{
    public static IServiceCollection AddOfferService(this IServiceCollection services)
    {
        services.AddScoped<IOfferService, OfferService>();
        services.AddScoped<IOfferObservable>(provider => provider.GetService<IOfferService>() as OfferService);
        return services;
    }
}
