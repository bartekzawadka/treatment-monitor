using System;

namespace Treatment.Monitor.BusinessLogic.Dto
{
    public class MedicineApplicationDto : IdentifiableDto
    {
        public string Name { get; set; }

        public string CronExpression { get; set; }

        public DateTime StartDate { get; set; }

        public int NumberOfDays { get; set; }
    }
}