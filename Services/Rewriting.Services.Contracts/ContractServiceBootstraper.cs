using Microsoft.Extensions.DependencyInjection;

namespace Rewriting.Services.Contracts;

public static class ContractServiceBootstraper
{
    public static IServiceCollection AddContractService(this IServiceCollection services)
    {
        services.AddSingleton<IContractService, ContractService>();
        return services;
    }
}
