using Microsoft.Extensions.Configuration;

namespace Rewriting.SettingsLoader
{
    public abstract class Settings
    {
        public static T Load<T>(string key)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var settings = (T)Activator.CreateInstance(typeof(T))!;
            
            configuration.GetSection(key).Bind(settings);
            return settings;
        }
    }
}