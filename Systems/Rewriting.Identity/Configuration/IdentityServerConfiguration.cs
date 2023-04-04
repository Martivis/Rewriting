using Microsoft.AspNetCore.Identity;
using Rewriting.Context;
using Rewriting.Context.Entities;

namespace Rewriting.Identity.Configuration;

public static class IdentityServerConfiguration
{
    public static IServiceCollection AddAppIdentityServer(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;

            options.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddDefaultTokenProviders()
            ;

        services.AddIdentityServer()
            .AddAspNetIdentity<ApplicationUser>()
            .AddInMemoryApiResources(AppResources.Resourses)
            .AddInMemoryApiScopes(AppApiScopes.ApiScopes)
            .AddInMemoryClients(AppClients.Clients)
            .AddInMemoryIdentityResources(AppIdentityResources.Resources)
            .AddDeveloperSigningCredential();

        return services;
    }

    public static IApplicationBuilder UseAppIdentityServer(this IApplicationBuilder app)
    {
        app.UseIdentityServer();
        return app;
    }
}
