using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
