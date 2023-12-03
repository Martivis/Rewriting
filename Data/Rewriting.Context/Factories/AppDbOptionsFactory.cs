using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rewriting.Context.Interceptors;

namespace Rewriting.Context;

public static class AppDbOptionsFactory
{
    private static readonly TaggedQueryCommandInterceptor _interceptor = new();
    
    public static DbContextOptions<AppDbContext> Create(string connectionString)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        Configure(connectionString).Invoke(builder);
        return builder.Options;
    }

    public static Action<DbContextOptionsBuilder> Configure(string connectionString)
    {
        return optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(connectionString, options =>
            {
                options.CommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds)
                    .MigrationsHistoryTable("_EFMigrationHistory", DbConstants.DatabaseScheme)
                    .MigrationsAssembly(DbConstants.MigrationsAssembly);
            })
                .EnableSensitiveDataLogging()
                .UseLazyLoadingProxies();
        };
    }
    
    public static Action<DbContextOptionsBuilder> ConfigureTagsSupport(string connectionString)
    {
        return optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(connectionString, options =>
                {
                    options.CommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds)
                        .MigrationsHistoryTable("_EFMigrationHistory", DbConstants.DatabaseScheme)
                        .MigrationsAssembly(DbConstants.MigrationsAssembly);
                })
                .AddInterceptors(_interceptor)
                .EnableSensitiveDataLogging()
                .UseLazyLoadingProxies();
        };
    }
}
