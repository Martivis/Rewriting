namespace Rewriting.API.Configuration;
using Serilog;

public static class LoggerConfiguration
{
    public static void AddAppLogger(this WebApplicationBuilder builder)
    {
        var logger = new Serilog.LoggerConfiguration()
            .Enrich.WithCorrelationId()
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        builder.Host.UseSerilog(logger, dispose: true);
    }
}
