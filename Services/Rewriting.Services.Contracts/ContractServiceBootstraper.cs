using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Contracts;

public static class ContractServiceBootstraper
{
    public static IServiceCollection AddContractService(this IServiceCollection services)
    {
        services.AddSingleton<IContractService, ContractService>();
        return services;
    }
}
