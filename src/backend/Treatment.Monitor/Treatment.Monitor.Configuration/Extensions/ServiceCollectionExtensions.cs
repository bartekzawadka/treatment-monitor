using System;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Treatment.Monitor.Configuration.JobActivation;
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

        public static IServiceCollection AddHangfireConfiguration(
            this IServiceCollection serviceCollection,
            IConfiguration configuration,
            bool addServer,
            string serverName)
        {
            var mongoClient = new MongoClient(EnvironmentConfiguration.GetDbConnectionString(configuration));
            var jobsDatabase = $"{Consts.DatabaseName}-jobs";

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var mongoStorageOptions = new MongoStorageOptions
            {
                MigrationOptions = new MongoMigrationOptions
                {
                    MigrationStrategy = new MigrateMongoMigrationStrategy(),
                    BackupStrategy = new CollectionMongoBackupStrategy()
                },
                Prefix = "notifier",
                CheckConnection = true
            };
            var mongoStorage = new MongoStorage(mongoClient, jobsDatabase, mongoStorageOptions);

            serviceCollection.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseColouredConsoleLogProvider()
                .UseActivator(new ContainerJobActivator(serviceProvider.GetRequiredService<IServiceScopeFactory>()))
                .UseSerilogLogProvider()
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMongoStorage(mongoClient, jobsDatabase, mongoStorageOptions));
            JobStorage.Current = mongoStorage;

            if (addServer)
            {
                serviceCollection.AddHangfireServer(options => options.ServerName = serverName);
            }

            return serviceCollection;
        }
    }
}