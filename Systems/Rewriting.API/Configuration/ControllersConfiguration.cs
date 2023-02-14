

using Rewriting.Common.JsonConverters;

namespace Rewriting.API.Configuration;

public static class ControllersConfiguration
{
    public static IServiceCollection AddAppControllers(this IServiceCollection services)
    {
        services.AddControllers().AddValidator().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
        });
        return services;
    }
}
