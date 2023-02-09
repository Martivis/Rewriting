namespace Rewriting.API.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;


public class SelfHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var assembly = Assembly.Load("Rewriting.API");
        var version = assembly.GetName().Version;

        return Task.FromResult(HealthCheckResult.Healthy(description: $"Build {version}"));
    }
}

