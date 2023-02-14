namespace Rewriting.API.Configuration;

public static class AutoMapperConfiguration
{
    public static IServiceCollection AddAppAutomapper(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName is not null);

        services.AddAutoMapper(assemblies);

        return services;
    }
}
