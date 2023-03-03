using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Rewriting.API;
using Rewriting.Common.Security;
using Rewriting.Settings;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Rewriting.API.Configuration;

public static class SwaggerConfiguration
{
    /// <summary>
    /// Add swagger services with custom configuration
    /// </summary>
    /// <param name="services"></param>
    /// <param name="identitySettings">Identity server data</param>
    /// <returns></returns>
    public static IServiceCollection AddAppSwagger(this IServiceCollection services, IdentitySettings identitySettings)
    {
        var swaggerSettings = Settings.SettingsLoader.Load<SwaggerSettings>("Swagger");
        services.AddSingleton(swaggerSettings);

        if (!swaggerSettings.Enabled)
            return services;

        services.AddSwaggerGen(options =>
        {
            string xmlFileName = "api.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
            options.IncludeXmlComments(xmlPath);

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Name = "Bearer",
                Type = SecuritySchemeType.OAuth2,
                Scheme = "oauth2",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri($"{identitySettings.URL}/{identitySettings.TokenAddress}"),
                        Scopes = new Dictionary<string, string>
                            {
                                { AppScopes.OrdersRead, "OrdersRead" },
                                { AppScopes.OrdersWrite, "OrdersWrite" },
                                { AppScopes.OrdersDelete, "OrdersDelete" },
                                { AppScopes.OffersRead, "OffersRead" },
                                { AppScopes.OffersWrite, "OffersWrite" },
                            }
                    }
                }
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    new List<string>()
                }
            });
        });

        return services;
    }

    /// <summary>
    /// Register required swagger middlewares
    /// </summary>
    /// <param name="app"></param>
    /// <exception cref="ServiceNotFoundException">SwaggerSettings was not found in DI container</exception>
    public static void UseAppSwagger(this WebApplication app)
    {
        SwaggerSettings settings = app.Services.GetService<SwaggerSettings>() ??
            throw new InvalidOperationException("SwaggerSettings service was not found");

        if (!settings.Enabled)
            return;

        app.UseSwagger(options =>
        {
            options.RouteTemplate = "api/{documentname}/api.yaml";
        });
        app.UseSwaggerUI(options =>
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            provider.ApiVersionDescriptions.ToList().ForEach(description =>
            {
                options.SwaggerEndpoint($"/api/{description.GroupName}/api.yaml", description.GroupName.ToUpperInvariant());
            });
        });
    }
}
