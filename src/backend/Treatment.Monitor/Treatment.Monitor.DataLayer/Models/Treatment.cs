using System;
using System.Collections.Generic;

namespace Treatment.Monitor.DataLayer.Models
{
    public class Treatment : Document
    {
        public string Name { get; set; }
        
        public DateTime StartDate { get; set; }

        public bool Terminated { get; set; }

        public ICollection<MedicineApplication> MedicineApplications { get; set; }
    }
}