using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Rewriting.API.Authorization;
using Rewriting.Common.Security;
using Rewriting.Context;
using Rewriting.Context.Entities;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Rewriting.API.Configuration;

public static class AuthConfiguration
{
    public static IServiceCollection AddAppAuth(this IServiceCollection services, IdentitySettings identitySettings)
    {
        IdentityModelEventSource.ShowPII = true;

        services.AddIdentity<UserIdentity, IdentityRole<Guid>>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;

            options.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddUserManager<UserManager<UserIdentity>>()
            .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
            .AddDefaultTokenProviders()
            ;

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = identitySettings.URL.StartsWith("https://");
                options.Authority = identitySettings.URL;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.Audience = "api";
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(AppScopes.OrdersRead, policy => policy.RequireClaim("scope", AppScopes.OrdersRead));
            options.AddPolicy(AppScopes.OrdersWrite, policy => policy.RequireClaim("scope", AppScopes.OrdersWrite));
            options.AddPolicy(AppScopes.OrdersEdit, policy => policy.Requirements.Add(new OwnerUidRequirement()));
            options.AddPolicy(AppScopes.OrdersDelete, policy => policy.RequireClaim(ClaimTypes.Role, AppRoles.Admin)
                                                                      .RequireClaim("scope", AppScopes.OrdersDelete));

            options.AddPolicy(AppScopes.OffersRead, policy => policy.RequireClaim("scope", AppScopes.OffersRead));
            options.AddPolicy(AppScopes.OffersWrite, policy => policy.RequireClaim("scope", AppScopes.OffersWrite));
            options.AddPolicy(AppScopes.OffersEdit, policy => policy.Requirements.Add(new OwnerUidRequirement()));
            options.AddPolicy(AppScopes.OffersDelete, policy => policy.RequireClaim(ClaimTypes.Role, AppRoles.Admin)
                                                                      .RequireClaim("scope", AppScopes.OffersDelete));

            options.AddPolicy(AppScopes.ContractsRead, policy => policy.RequireClaim("scope", AppScopes.ContractsRead));
            options.AddPolicy(AppScopes.ContractsWrite, policy => policy.RequireClaim("scope", AppScopes.ContractsWrite));
            options.AddPolicy(AppScopes.ContractsEdit, policy => policy.Requirements.Add(new OwnerUidRequirement()));
            options.AddPolicy(AppScopes.ContractsDelete, policy => policy.RequireClaim(ClaimTypes.Role, AppRoles.Admin)
                                                                         .RequireClaim("scope", AppScopes.ContractsDelete));
        });

        services.AddSingleton<IAuthorizationHandler, OrdersAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, OffersAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, ContractorAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, ClientAuthorizationHandler>();

        return services;
    }

    public static IApplicationBuilder UseAppAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}
