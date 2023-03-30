using Rewriting.API.Configuration;

namespace Rewriting.API.Configuration
{
    public static class HealthCheckConfiguration
    {
        /// <summary>
        /// Add HealthChecks service
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<SelfHealthCheck>("Rewriting.API");
            return services;
        }

        /// <summary>
        /// Map health checks
        /// </summary>
        /// <param name="app"></param>
        public static void UseAppHealthChecks(this WebApplication app)
        {
            app.MapHealthChecks("/health");
        }
    }
}
