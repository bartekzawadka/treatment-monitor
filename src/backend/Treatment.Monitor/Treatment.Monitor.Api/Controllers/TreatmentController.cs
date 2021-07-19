using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Treatment.Monitor.BusinessLogic.Dto;
using Treatment.Monitor.BusinessLogic.ServiceAction;
using Treatment.Monitor.BusinessLogic.Services;

namespace Treatment.Monitor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TreatmentController : ControllerBase
    {
        private readonly ITreatmentService _treatmentService;

        public TreatmentController(ITreatmentService treatmentService)
        {
            _treatmentService = treatmentService;
        }

        [HttpGet]
        public Task<List<TreatmentListItemDto>> GetListAsync() => _treatmentService.GetListAsync();

        [HttpGet("{id}")]
        public Task<IServiceActionResult<TreatmentDto>> GetByIdAsync(string id) => _treatmentService.GetByIdAsync(id);

        [HttpPost]
        public Task<IServiceActionResult<TreatmentDto>> InsertAsync([FromBody] TreatmentDto dto) =>
            _treatmentService.CreateAsync(dto);

        [HttpPut("{id}")]
        public Task<IServiceActionResult<TreatmentDto>> UpdateAsync(string id, [FromBody] TreatmentDto dto) =>
            _treatmentService.UpdateAsync(id, dto);

        [HttpDelete("{id}")]
        public Task<IServiceActionResult> DeleteAsync(string id) => _treatmentService.DeleteAsync(id);
    }
}