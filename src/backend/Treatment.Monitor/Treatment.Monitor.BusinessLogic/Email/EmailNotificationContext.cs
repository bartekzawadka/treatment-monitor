using Treatment.Monitor.DataLayer.Models;

namespace Treatment.Monitor.BusinessLogic.Email
{
    public class EmailNotificationContext
    {
        public string TreatmentName { get; set; }

        public MedicineApplication Medicine { get; set; }

        public EmailNotificationContext(string treatmentName, MedicineApplication medicine)
        {
            TreatmentName = treatmentName;
            Medicine = medicine;
        }
    }
}