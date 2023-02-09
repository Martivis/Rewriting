using Rewriting.API;
using Rewriting.Common.Exceptions;
using Rewriting.SettingsLoader;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Rewriting.API.Configuration;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddAppSwagger(this IServiceCollection services)
    {
        var swaggerSettings = Settings.Load<SwaggerSettings>("Swagger");
        services.AddSingleton(swaggerSettings);

        if (!swaggerSettings.Enabled)
            return services;

        services.AddSwaggerGen(options =>
        {
            string xmlFileName = "api.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
            options.IncludeXmlComments(xmlPath);
        });
        return services;
    }
    
    public static void UseAppSwagger(this WebApplication app)
    {
        SwaggerSettings settings = app.Services.GetService<SwaggerSettings>() ??
            throw new ServiceNotFoundException(typeof(SwaggerSettings));

        if (!settings.Enabled)
            return;

        app.UseSwagger();
        app.UseSwaggerUI();
    }
}
