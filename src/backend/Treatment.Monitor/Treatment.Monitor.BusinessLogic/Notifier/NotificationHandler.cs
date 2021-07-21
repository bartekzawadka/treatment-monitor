using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using Treatment.Monitor.DataLayer.Models;
using Treatment.Monitor.DataLayer.Repositories;
using TreatmentModel = Treatment.Monitor.DataLayer.Models.Treatment;

namespace Treatment.Monitor.BusinessLogic.Notifier
{
    public class NotificationHandler : INotificationHandler
    {
        private readonly IGenericRepository<TreatmentModel> _treatmentRepository;
        private readonly ILogger<NotificationHandler> _logger;

        public NotificationHandler(
            IGenericRepository<TreatmentModel> treatmentRepository,
            ILogger<NotificationHandler> logger)
        {
            _treatmentRepository = treatmentRepository;
            _logger = logger;
        }

        public async Task HandleAsync(NotificationExecutionContext executionContext, PerformContext context)
        {
            try
            {
                _logger.LogInformation(GetEnrichedLogMessage(executionContext, "Starting execution"));
                var treatment = await _treatmentRepository.GetByIdAsync(executionContext.TreatmentId);
                if (treatment == null)
                {
                    _logger.LogWarning("Treatment with provided ID could not be found. Exiting");
                    RecurringJob.RemoveIfExists(context.BackgroundJob.Id);
                    return;
                }

                if (treatment.Terminated)
                {
                    _logger.LogWarning("Treatment with provided ID is marked as terminated. Exiting");
                    RecurringJob.RemoveIfExists(context.BackgroundJob.Id);
                    return;
                }

                ICollection<MedicineApplication> medicines =
                    treatment.MedicineApplications ?? new List<MedicineApplication>();
                var medicine = medicines.SingleOrDefault(x => x.Id == executionContext.MedicineId);
                if (medicine == null)
                {
                    _logger.LogWarning("Medicine with provided ID could not be found. Exiting");
                    RecurringJob.RemoveIfExists(context.BackgroundJob.Id);
                    return;
                }

                var now = DateTime.Now;
                var notificationsLimitDate = medicine.StartDate.AddDays(medicine.NumberOfDays);
                if (now > notificationsLimitDate.AddHours(12))
                {
                    _logger.LogWarning("Medicine application should be terminated by now. Exiting");
                    RecurringJob.RemoveIfExists(context.BackgroundJob.Id);
                    return;
                }

                // TODO: send
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, GetEnrichedLogMessage(executionContext, "Notification processing failed"));
                throw;
            }
        }

        private static string GetEnrichedLogMessage(NotificationExecutionContext context, string message) =>
            $"[{nameof(NotificationHandler)}][Treatment ID: {context.TreatmentId}][Medicine ID: {context.MedicineId}]" +
            $" {message}";
    }
}