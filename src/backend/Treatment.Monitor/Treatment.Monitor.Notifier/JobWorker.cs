using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Treatment.Monitor.Notifier
{
    public class JobWorker : BackgroundService
    {
        private readonly ILogger<JobWorker> _logger;
        private readonly IHostEnvironment _hostEnvironment;
        private BackgroundJobServer _server;
        private const int Interval = 500;

        public JobWorker(ILogger<JobWorker> logger, IHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting hangfire background server. Environment: {_hostEnvironment.EnvironmentName}");
            _server = new BackgroundJobServer();
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(Interval, stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _server.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}