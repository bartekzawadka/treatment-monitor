using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Hosting;

namespace Treatment.Monitor.Notifier
{
    public class JobWorker : BackgroundService
    {
        private BackgroundJobServer _server;
        private const int Interval = 500;

        public override Task StartAsync(CancellationToken cancellationToken)
        {
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