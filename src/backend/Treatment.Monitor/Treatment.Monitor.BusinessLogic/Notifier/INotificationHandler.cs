using System.Threading.Tasks;
using Hangfire.Server;

namespace Treatment.Monitor.BusinessLogic.Notifier
{
    public interface INotificationHandler
    {
        Task HandleAsync(NotificationExecutionContext executionContext, PerformContext context);
    }
}