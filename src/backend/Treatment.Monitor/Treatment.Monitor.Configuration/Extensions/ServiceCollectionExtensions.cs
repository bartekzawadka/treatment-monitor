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
            IConfiguration configuration) =>
            serviceCollection.AddSingletonFromConfiguration<SecuritySettings, ISecuritySettings>(configuration);

        public static IServiceCollection AddSingletonFromConfiguration<TSection, TInterface>(
            this IServiceCollection serviceCollection,
            IConfiguration configuration,
            string sectionName = null)
            where TSection : TInterface
            where TInterface : class
        {
            TInterface obj = configuration.GetObjectFromConfigurationSection<TSection, TInterface>(sectionName);
            return serviceCollection.AddSingleton(obj);
        }
    }
}