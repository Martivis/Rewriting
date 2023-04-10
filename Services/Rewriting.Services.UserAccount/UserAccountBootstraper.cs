using Microsoft.Extensions.DependencyInjection;

namespace Rewriting.Services.UserAccount;

public static class UserAccountBootstraper
{
    public static IServiceCollection AddAppUserAccountService(this IServiceCollection services)
    {
        services.AddScoped<IUserAccountService, UserAccountService>();
        services.AddScoped<IUserDataService, UserDataService>();
        return services;
    }
}
