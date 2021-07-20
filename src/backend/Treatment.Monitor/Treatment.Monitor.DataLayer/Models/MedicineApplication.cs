using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Treatment.Monitor.DataLayer.Models
{
    public class MedicineApplication
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CronExpression { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime StartDate { get; set; }

        public int NumberOfDays { get; set; }
    }
}