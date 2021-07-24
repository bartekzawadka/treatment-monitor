using Treatment.Monitor.BusinessLogic.Dto;
using TreatmentModel = Treatment.Monitor.DataLayer.Models.Treatment;

namespace Treatment.Monitor.BusinessLogic.Mappers
{
    public interface ITreatmentMapper
    {
        TreatmentModel GetModelFromDto(TreatmentDto dto);
    }
}