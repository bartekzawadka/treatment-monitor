using System.Collections.Generic;
using System.Threading.Tasks;
using Treatment.Monitor.BusinessLogic.Dto;
using Treatment.Monitor.BusinessLogic.ServiceAction;

namespace Treatment.Monitor.BusinessLogic.Services
{
    public interface ITreatmentService
    {
        Task<List<TreatmentListItemDto>> GetListAsync();

        Task<IServiceActionResult<TreatmentDto>> GetByIdAsync(string id);

        Task<IServiceActionResult<TreatmentDto>> CreateAsync(TreatmentDto dto);

        Task<IServiceActionResult<TreatmentDto>> UpdateAsync(string id, TreatmentDto dto);

        Task<IServiceActionResult> DeleteAsync(string id);
    }
}