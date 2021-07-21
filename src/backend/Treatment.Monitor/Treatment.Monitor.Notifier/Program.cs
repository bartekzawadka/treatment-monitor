using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Treatment.Monitor.BusinessLogic.Email;
using Treatment.Monitor.BusinessLogic.Notifier;
using Treatment.Monitor.Configuration;
using Treatment.Monitor.Configuration.Extensions;
using Treatment.Monitor.Configuration.Settings;
using Treatment.Monitor.DataLayer;
using Treatment.Monitor.DataLayer.Repositories;
using TreatmentModel = Treatment.Monitor.DataLayer.Models.Treatment;

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

                    services.AddSingleton<IGenericRepository<TreatmentModel>, GenericRepository<TreatmentModel>>();
                    services.AddScoped<INotificationHandler, NotificationHandler>();

                    var emailConfiguration = configuration.GetObjectFromConfigurationSection<EmailConfiguration, EmailConfiguration>();
                    UpdateConfigBasedOnEnvVars(emailConfiguration);
                    services.AddSingleton(emailConfiguration);
                    services.AddSingleton<IEmailSender, EmailSender>();

                    services.AddSingletonDbContext(configuration, s => new TreatmentMonitorContext(s));
                    services.AddHangfireConfiguration(configuration, true, "Treatment.Monitor.Notifier server");
                    services.AddHostedService<JobWorker>();
                });

        private static void UpdateConfigBasedOnEnvVars(EmailConfiguration emailConfiguration)
        {
            EnvironmentConfiguration.SetValueFromEnvVar<EmailConfiguration, string>(emailConfiguration, configuration => configuration.Password, "NOTIFIER_EMAIL_PASSWORD");
            EnvironmentConfiguration.SetValueFromEnvVar<EmailConfiguration, int>(emailConfiguration, configuration => configuration.Port, "NOTIFIER_EMAIL_PORT");
            EnvironmentConfiguration.SetValueFromEnvVar<EmailConfiguration, string>(emailConfiguration, configuration => configuration.Server, "NOTIFIER_EMAIL_SERVER");
            EnvironmentConfiguration.SetValueFromEnvVar<EmailConfiguration, string>(emailConfiguration, configuration => configuration.To, "NOTIFIER_EMAIL_TO");
            EnvironmentConfiguration.SetValueFromEnvVar<EmailConfiguration, string>(emailConfiguration, configuration => configuration.Username, "NOTIFIER_EMAIL_USERNAME");
            EnvironmentConfiguration.SetValueFromEnvVar<EmailConfiguration, string>(emailConfiguration, configuration => configuration.FromAddress, "NOTIFIER_EMAIL_FROM_ADDRESS");
            EnvironmentConfiguration.SetValueFromEnvVar<EmailConfiguration, string>(emailConfiguration, configuration => configuration.FromName, "NOTIFIER_EMAIL_FROM_NAME");
            EnvironmentConfiguration.SetValueFromEnvVar<EmailConfiguration, bool>(emailConfiguration, configuration => configuration.UseSsl, "NOTIFIER_EMAIL_USE_SSL");
        }
    }
}