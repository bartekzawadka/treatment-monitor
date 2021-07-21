using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Serilog;
using Treatment.Monitor.BusinessLogic.Notifier;
using Treatment.Monitor.Configuration;
using Treatment.Monitor.Configuration.Extensions;
using Treatment.Monitor.DataLayer;
using Treatment.Monitor.Notifier.Configuration;
using Treatment.Monitor.Notifier.JobActivation;

namespace Treatment.Monitor.Notifier
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder
                        .SetBasePath(context.HostingEnvironment.ContentRootPath)
                        .AddEnvironmentVariables()
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true);
                })
                .ConfigureServices((context, services) =>
                {
                    IConfiguration configuration = context.Configuration;
                    services.AddSingleton(configuration);

                    services.AddScoped<INotificationHandler, NotificationHandler>();

                    var emailConfiguration = configuration.GetObjectFromConfigurationSection<EmailConfiguration, EmailConfiguration>();
                    UpdateConfigBasedOnEnvVars(emailConfiguration);
                    services.AddSingleton(emailConfiguration);

                    services.AddSingletonDbContext(configuration, s => new TreatmentMonitorContext(s));
                    var mongoClient = new MongoClient(EnvironmentConfiguration.GetDbConnectionString(configuration));
                    var jobsDatabase = $"{Consts.DatabaseName}-jobs";

                    var serviceProvider = services.BuildServiceProvider();

                    services.AddHangfire(config => config
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseColouredConsoleLogProvider()
                        .UseActivator(new ContainerJobActivator(serviceProvider.GetRequiredService<IServiceScopeFactory>()))
                        .UseSerilogLogProvider()
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseMongoStorage(mongoClient, jobsDatabase, new MongoStorageOptions
                        {
                            MigrationOptions = new MongoMigrationOptions
                            {
                                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                                BackupStrategy = new CollectionMongoBackupStrategy()
                            },
                            Prefix = "hangfire",
                            CheckConnection = true
                        }));

                    services.AddHangfireServer(options => options.ServerName = "Treatment.Monitor.Notifier server");
                    services.AddHostedService<JobWorker>();
                });

        private static void UpdateConfigBasedOnEnvVars(EmailConfiguration emailConfiguration)
        {
            EnvironmentConfiguration.SetValueFromEnvVar<EmailConfiguration, string>(emailConfiguration, configuration => configuration.Password, "NOTIFIER_EMAIL_PASSWORD");
            EnvironmentConfiguration.SetValueFromEnvVar<EmailConfiguration, int>(emailConfiguration, configuration => configuration.Port, "NOTIFIER_EMAIL_PORT");
            EnvironmentConfiguration.SetValueFromEnvVar<EmailConfiguration, string>(emailConfiguration, configuration => configuration.Server, "NOTIFIER_EMAIL_SERVER");
            EnvironmentConfiguration.SetValueFromEnvVar<EmailConfiguration, string>(emailConfiguration, configuration => configuration.To, "NOTIFIER_EMAIL_TO");
            EnvironmentConfiguration.SetValueFromEnvVar<EmailConfiguration, string>(emailConfiguration, configuration => configuration.Username, "NOTIFIER_EMAIL_USERNAME");
            EnvironmentConfiguration.SetValueFromEnvVar<EmailConfiguration, string>(emailConfiguration, configuration => configuration.AppEndpoint, "NOTIFIER_EMAIL_APP_ENDPOINT");
            EnvironmentConfiguration.SetValueFromEnvVar<EmailConfiguration, string>(emailConfiguration, configuration => configuration.FromAddress, "NOTIFIER_EMAIL_FROM_ADDRESS");
            EnvironmentConfiguration.SetValueFromEnvVar<EmailConfiguration, string>(emailConfiguration, configuration => configuration.FromName, "NOTIFIER_EMAIL_FROM_NAME");
            EnvironmentConfiguration.SetValueFromEnvVar<EmailConfiguration, bool>(emailConfiguration, configuration => configuration.UseSsl, "NOTIFIER_EMAIL_USE_SSL");
        }
    }
}