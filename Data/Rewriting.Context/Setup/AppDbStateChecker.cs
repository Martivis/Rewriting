using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Context;

public static class AppDbStateChecker
{
    private const int MaxRetries = 5;
    private const int RetryDelayMs = 1000;
    public static void Check(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetService<IServiceScopeFactory>()!.CreateScope();

        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();

        int retries = 0;

        while (retries < MaxRetries)
        {
        	try
        	{
            	using var context = dbContextFactory.CreateDbContext();

            	if (!context.Database.CanConnect())
        			throw new InvalidOperationException("Can't connect to database");

            	if (context.Database.GetPendingMigrations().Any())
                	throw new InvalidOperationException("Some migrations were not applied");

            	var appliedMigrations = context.Database.GetAppliedMigrations();
                	var assemblyMigrations = context.Database.GetMigrations();

            	if (!appliedMigrations.SequenceEqual(assemblyMigrations))
                	throw new InvalidOperationException("Can't match applied migrations with assembly migrations");

            	return;
            }
            catch
            {
                retries++;
                Task.Delay(RetryDelayMs).Wait();
            }
        }

    }
}
