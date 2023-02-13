namespace Rewriting.Identity.Configuration;

public static class HealthCheckConfiguration
{
    public static IServiceCollection AddAppHealthCheck(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<SelfHealthCheck>("Rewriting.Identity");
        return services;
    }

    public static void UseAppHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health");
    }
}
