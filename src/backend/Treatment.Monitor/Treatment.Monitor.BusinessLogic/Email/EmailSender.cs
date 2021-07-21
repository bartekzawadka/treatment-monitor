using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Treatment.Monitor.Configuration.Settings;

namespace Treatment.Monitor.BusinessLogic.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailSender(EmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public async Task SendAsync(EmailNotificationContext notificationContext)
        {
            var client = new SmtpClient(_emailConfiguration.Server)
            {
                Port = _emailConfiguration.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailConfiguration.Username, _emailConfiguration.Password),
                EnableSsl = _emailConfiguration.UseSsl
            };

            var time = $"{notificationContext.Medicine.StartDate:HH:mm}";
            var message = new MailMessage
            {
                From = new MailAddress(_emailConfiguration.FromAddress, _emailConfiguration.FromName),
                To = { new MailAddress(_emailConfiguration.To)},
                Subject = $"{time} - {notificationContext.Medicine.Name}" +
                          $" ({notificationContext.TreatmentName})",
                Body = $"Leczenie - {notificationContext.TreatmentName}{Environment.NewLine}" +
                       $"Podanie leku {notificationContext.Medicine.Name} o godzinie {time}"
            };

            await client.SendMailAsync(message);
        }
    }
}