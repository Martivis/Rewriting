using Microsoft.Extensions.DependencyInjection;

namespace Rewriting.Services.Offers
{
    public static class OfferServiceBootstraper
    {
        public static IServiceCollection AddOfferService(this IServiceCollection services)
        {
            services.AddSingleton<IOfferService, OfferService>();
            return services;
        }
    }
}
