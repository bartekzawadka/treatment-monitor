using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Treatment.Monitor.BusinessLogic.Cron;
using Treatment.Monitor.BusinessLogic.Dto;
using Treatment.Monitor.BusinessLogic.Mappers;
using Treatment.Monitor.BusinessLogic.Notifier;
using Treatment.Monitor.BusinessLogic.ServiceAction;
using Treatment.Monitor.DataLayer.Models;
using Treatment.Monitor.DataLayer.Repositories;
using Treatment.Monitor.DataLayer.Sys;
using TreatmentModel = Treatment.Monitor.DataLayer.Models.Treatment;

namespace Treatment.Monitor.BusinessLogic.Services
{
    public class TreatmentService : ITreatmentService
    {
        private readonly IGenericRepository<TreatmentModel> _treatmentRepository;

        public TreatmentService(IGenericRepository<TreatmentModel> treatmentRepository)
        {
            _treatmentRepository = treatmentRepository;
        }

        public Task<List<TreatmentListItemDto>> GetListAsync() =>
            _treatmentRepository.GetAllAsAsync(
                x => TreatmentMapper.GetListItemDtoFromModel(x),
                Filter<TreatmentModel>.GetDefaultWithSortingDescending(nameof(TreatmentModel.StartDate)));

        public async Task<IServiceActionResult<TreatmentDto>> GetByIdAsync(string id)
        {
            if (!await _treatmentRepository.ExistsAsync(Filter<TreatmentModel>.GetFilterById(id)))
            {
                return ServiceActionResult<TreatmentDto>.GetNotFound();
            }

            var result = await _treatmentRepository.GetByIdAsAsync(id, x => TreatmentMapper.GetDtoFromModel(x));
            return ServiceActionResult<TreatmentDto>.GetSuccess(result);
        }

        public async Task<IServiceActionResult<TreatmentDto>> CreateAsync(TreatmentDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Id)
                && await _treatmentRepository.ExistsAsync(Filter<TreatmentModel>.GetFilterById(dto.Id)))
            {
                return ServiceActionResult<TreatmentDto>.GetDataError("Treatment already exists with provided ID");
            }

            var medicines = dto.Medicines ?? new List<MedicineApplicationDto>();
            var medicinesModel = medicines
                .Select(TreatmentMapper.GetMedicineApplicationModelFromDto)
                .ToList();
            var model = TreatmentModel.Create(dto.Name, medicinesModel);
            
            await _treatmentRepository.InsertAsync(model);
            UpdateNotificationJobs(model);

            return ServiceActionResult<TreatmentDto>.GetCreated(TreatmentMapper.GetDtoFromModel(model));
        }

        public async Task<IServiceActionResult<TreatmentDto>> UpdateAsync(string id, TreatmentDto dto)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return ServiceActionResult<TreatmentDto>.GetDataError("Missing treatment ID");
            }

            if (!await _treatmentRepository.ExistsAsync(Filter<TreatmentModel>.GetFilterById(id)))
            {
                return ServiceActionResult<TreatmentDto>.GetNotFound($"No treatment found by ID {id}");
            }

            var model = await _treatmentRepository.GetByIdAsync(id);
            if (model == null)
            {
                return ServiceActionResult<TreatmentDto>.GetNotFound($"No treatment found by ID {id}");
            }
            
            RemoveNotificationJobsForTreatment(model, dto);

            model = TreatmentMapper.GetModelFromDto(dto);
            UpdateNotificationJobs(model);
            await _treatmentRepository.UpdateAsync(id, model);

            return ServiceActionResult<TreatmentDto>.GetCreated(TreatmentMapper.GetDtoFromModel(model));
        }

        private static void RemoveNotificationJobsForTreatment(TreatmentModel treatment, TreatmentDto dto)
        {
            var modelMedicines = treatment.MedicineApplications ?? new List<MedicineApplication>();
            var dtoMedicines = dto.Medicines ?? new List<MedicineApplicationDto>();
            var dtoMedicinesIds = dtoMedicines
                .Where(x => !string.IsNullOrWhiteSpace(x.Id))
                .Select(x => x.Id)
                .ToList();

            var toBeRemoved = modelMedicines
                .Where(x => !dtoMedicinesIds.Contains(x.Id))
                .ToList();

            RemoveNotificationJobs(toBeRemoved.Select(x => x.Id));
        }
        
        private static void RemoveNotificationJobs(IEnumerable<string> medicineIds)
        {
            foreach (var medicineId in medicineIds)
            {
                RecurringJob.RemoveIfExists(medicineId);
            }
        }
        
        private static void UpdateNotificationJobs(TreatmentModel treatment)
        {
            var isWindows = Environment.OSVersion.Platform is PlatformID.Win32S 
                or PlatformID.Win32Windows
                or PlatformID.Win32NT
                or PlatformID.WinCE;
            var timezone = isWindows 
                ? TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")
                : TimeZoneInfo.FindSystemTimeZoneById("Europe/Warsaw");

            foreach (var treatmentMedicineApplication in treatment.MedicineApplications ?? new List<MedicineApplication>())
            {
                var cronExpression = new CronExpressionBuilder()
                    .WithHour(treatmentMedicineApplication.StartDate.Hour)
                    .WithMinute(treatmentMedicineApplication.StartDate.Minute)
                    .BuildExpression();
                
                RecurringJob.AddOrUpdate<INotificationHandler>(
                    treatmentMedicineApplication.Id,
                    o => o.HandleAsync(
                        new NotificationExecutionContext
                        {
                            MedicineId = treatmentMedicineApplication.Id,
                            TreatmentId = treatment.Id
                        },
                        null),
                    cronExpression,
                    timezone);
            }
        }

        public async Task<IServiceActionResult> DeleteAsync(string id)
        {
            var model = await _treatmentRepository.GetByIdAsync(id);
            if (model == null)
            {
                return ServiceActionResult.GetNotFound($"No treatment found by ID {id}");
            }

            var medicines = model.MedicineApplications ?? new List<MedicineApplication>();
            var ids = medicines.Select(x => x.Id);

            RemoveNotificationJobs(ids);

            await _treatmentRepository.DeleteAsync(id);

            return ServiceActionResult.GetSuccess();
        }
    }
}