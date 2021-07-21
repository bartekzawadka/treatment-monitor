using System.Threading.Tasks;

namespace Treatment.Monitor.BusinessLogic.Email
{
    public interface IEmailSender
    {
        Task SendAsync(EmailNotificationContext notificationContext);
    }
}