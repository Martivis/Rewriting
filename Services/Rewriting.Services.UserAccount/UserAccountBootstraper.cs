using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.UserAccount;

public static class UserAccountBootstraper
{
    public static IServiceCollection AddAppUserAccountService(this IServiceCollection services)
    {
        services.AddScoped<IUserAccountService, UserAccountService>();
        return services;
    }
}
