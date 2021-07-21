using System;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Serilog;
using Treatment.Monitor.Configuration;
using Treatment.Monitor.Configuration.Extensions;
using Treatment.Monitor.DataLayer;

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

                    services.AddSingletonDbContext(configuration, s => new TreatmentMonitorContext(s));
                    var mongoClient = new MongoClient(EnvironmentConfiguration.GetDbConnectionString(configuration));
                    var jobsDatabase = $"{Consts.DatabaseName}-jobs";

                    services.AddHangfire(config => config
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseColouredConsoleLogProvider()
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
    }
}