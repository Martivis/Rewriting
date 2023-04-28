using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Context;

public static class AppDbInitializer
{
    private const int MaxRetries = 5;
    private const int RetryDelayMs = 1000;

    public static void Execute(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetService<IServiceScopeFactory>()!.CreateScope();

        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();

        int retries = 0;
        while (retries < MaxRetries)
        {
            using var context = dbContextFactory.CreateDbContext();

            try
            {
                context.Database.Migrate();
                return;
            }
            catch
            {
                retries++;
                Task.Delay(RetryDelayMs);
            }
        }
    }
}
