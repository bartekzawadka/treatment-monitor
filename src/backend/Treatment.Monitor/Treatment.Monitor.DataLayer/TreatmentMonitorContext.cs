using MongoDB.Driver;
using Treatment.Monitor.DataLayer.Sys;

namespace Treatment.Monitor.DataLayer
{
    public class TreatmentMonitorContext
    {
        public IMongoDatabase Database { get; }

        public TreatmentMonitorContext(string connectionString)
        {
            IMongoClient client = new MongoClient(connectionString);
            Database = client.GetDatabase(Consts.DatabaseName);
        }
    }
}