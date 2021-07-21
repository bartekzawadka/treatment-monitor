using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Treatment.Monitor.Configuration.Settings;

namespace Treatment.Monitor.Configuration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSingletonDbContext<T>(
            this IServiceCollection serviceCollection,
            IConfiguration configuration,
            Func<string, T> getContextFunc)
            where T : class
        {
            var connectionString = EnvironmentConfiguration.GetDbConnectionString(configuration);
            return serviceCollection.AddSingleton(getContextFunc(connectionString));
        }

        public static IServiceCollection AddSecuritySettings(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            IConfigurationSection securityConfigSection = configuration.GetSection(nameof(SecuritySettings));
            var securitySettings = securityConfigSection.Get<SecuritySettings>();
            return serviceCollection.AddSingleton<ISecuritySettings>(securitySettings);
        }
    }
}