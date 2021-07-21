using System;
using System.Collections.Generic;
using System.Linq;
using Treatment.Monitor.BusinessLogic.Dto;
using Treatment.Monitor.DataLayer.Models;
using TreatmentModel = Treatment.Monitor.DataLayer.Models.Treatment;

namespace Treatment.Monitor.BusinessLogic.Mappers
{
    public static class TreatmentMapper
    {
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

        public static MedicineApplicationDto GetMedicineApplicationDtoFromModel(MedicineApplication model) =>
            new()
            {
                Id = model.Id,
                Name = model.Name,
                StartDate = model.StartDate,
                NumberOfDays = model.NumberOfDays
            };

        public static TreatmentModel GetModelFromDto(TreatmentDto dto) =>
            new()
            {
                Id = dto.Id,
                Name = dto.Name,
                Terminated = dto.Terminated,
                MedicineApplications = dto.Medicines?.Select(GetMedicineApplicationModelFromDto).ToList() ?? new List<MedicineApplication>()
            };

        public static MedicineApplication GetMedicineApplicationModelFromDto(MedicineApplicationDto dto) =>
            new()
            {
                Id = dto.Id ?? Guid.NewGuid().ToString(),
                Name = dto.Name,
                StartDate = dto.StartDate,
                NumberOfDays = dto.NumberOfDays
            };
    }
}