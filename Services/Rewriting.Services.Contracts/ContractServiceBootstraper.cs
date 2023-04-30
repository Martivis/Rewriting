﻿using Microsoft.Extensions.DependencyInjection;

namespace Rewriting.Services.Contracts;

public static class ContractServiceBootstraper
{
    public static IServiceCollection AddContractService(this IServiceCollection services)
    {
        services.AddScoped<IContractService, ContractService>();
        services.AddScoped<IContractRepository, ContractRepository>();
        services.AddScoped<IResultRepository, ResultRepository>();
        services.AddScoped<IContractObservable>(provider => provider.GetService<IContractService>() as ContractService);
        return services;
    }
}
