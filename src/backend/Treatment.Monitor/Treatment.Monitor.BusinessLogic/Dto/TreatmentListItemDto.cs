using System;

namespace Treatment.Monitor.BusinessLogic.Dto
{
    public class TreatmentListItemDto : IdentifiableDto
    {
        public string Name { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public bool Terminated { get; set; }
    }
}