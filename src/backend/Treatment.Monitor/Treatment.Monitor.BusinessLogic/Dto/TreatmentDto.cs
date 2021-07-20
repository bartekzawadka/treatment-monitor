using System;
using System.Collections.Generic;

namespace Treatment.Monitor.BusinessLogic.Dto
{
    public class TreatmentDto : IdentifiableDto
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public bool Terminated { get; set; }

        public ICollection<MedicineApplicationDto> Medicines { get; set; }
    }
}