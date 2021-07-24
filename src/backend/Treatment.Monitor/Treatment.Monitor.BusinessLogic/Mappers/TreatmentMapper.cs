using System;
using System.Collections.Generic;
using System.Linq;
using Treatment.Monitor.BusinessLogic.Dto;
using Treatment.Monitor.BusinessLogic.Services;
using Treatment.Monitor.DataLayer.Models;
using TreatmentModel = Treatment.Monitor.DataLayer.Models.Treatment;

namespace Treatment.Monitor.BusinessLogic.Mappers
{
    public class TreatmentMapper : ITreatmentMapper
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public TreatmentMapper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }
        
        public static TreatmentListItemDto GetListItemDtoFromModel(TreatmentModel model) =>
            new()
            {
                Id = model.Id,
                Name = model.Name,
                Terminated = model.Terminated,
                StartDate = model.StartDate
            };

        public static TreatmentDto GetDtoFromModel(TreatmentModel model) =>
            new()
            {
                Id = model.Id,
                Name = model.Name,
                Terminated = model.Terminated,
                StartDate = model.StartDate,
                Medicines = model.MedicineApplications?.Select(GetMedicineApplicationDtoFromModel).ToList() ?? new List<MedicineApplicationDto>()
            };

        private static MedicineApplicationDto GetMedicineApplicationDtoFromModel(MedicineApplication model) =>
            new()
            {
                Id = model.Id,
                Name = model.Name,
                StartDate = model.StartDate,
                NumberOfDays = model.NumberOfDays
            };

        public TreatmentModel GetModelFromDto(TreatmentDto dto) =>
            new()
            {
                Id = dto.Id,
                Name = dto.Name,
                StartDate = _dateTimeProvider.ConvertDateToPolishTimeZone(dto.StartDate),
                Terminated = dto.Terminated,
                MedicineApplications = dto.Medicines?.Select(GetMedicineApplicationModelFromDto).ToList() ?? new List<MedicineApplication>()
            };

        private MedicineApplication GetMedicineApplicationModelFromDto(MedicineApplicationDto dto) =>
            new()
            {
                Id = string.IsNullOrWhiteSpace(dto.Id) ? Guid.NewGuid().ToString() : dto.Id,
                Name = dto.Name,
                StartDate = _dateTimeProvider.ConvertDateToPolishTimeZone(dto.StartDate),
                NumberOfDays = dto.NumberOfDays
            };
    }
}