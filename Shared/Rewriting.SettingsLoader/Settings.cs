using Microsoft.Extensions.Configuration;

namespace Rewriting.SettingsLoader
{
    public abstract class Settings
    {
        /// <summary>
        /// Loads configuration from appsettings.json and appsettings.Development.json on specified key
        /// </summary>
        /// <typeparam name="T">Settings container</typeparam>
        /// <param name="key">Settings key</param>
        /// <returns>Filled settings conteiner</returns>
        public static T Load<T>(string key) where T : new()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var settings = new T();
            
            configuration.GetSection(key).Bind(settings);
            return settings;
        }
    }
}