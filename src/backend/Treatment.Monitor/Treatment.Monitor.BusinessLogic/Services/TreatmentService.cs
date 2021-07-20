using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Treatment.Monitor.BusinessLogic.Dto;
using Treatment.Monitor.BusinessLogic.Mappers;
using Treatment.Monitor.BusinessLogic.ServiceAction;
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

            var model = TreatmentMapper.GetModelFromDto(dto);
            await _treatmentRepository.UpdateAsync(id, model);
            return ServiceActionResult<TreatmentDto>.GetCreated(TreatmentMapper.GetDtoFromModel(model));
        }

        public async Task<IServiceActionResult> DeleteAsync(string id)
        {
            if (!await _treatmentRepository.ExistsAsync(Filter<TreatmentModel>.GetFilterById(id)))
            {
                return ServiceActionResult.GetNotFound($"No treatment found by ID {id}");
            }

            await _treatmentRepository.DeleteAsync(id);
            return ServiceActionResult.GetSuccess();
        }
    }
}