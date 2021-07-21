using System;
using Microsoft.Extensions.Configuration;

namespace Treatment.Monitor.Configuration
{
    public class EnvironmentConfiguration
    {
        public static string GetDbConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(Consts.DatabaseName);
            var connectionStringVar = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (!string.IsNullOrWhiteSpace(connectionStringVar))
            {
                connectionString = connectionStringVar;
            }

            return connectionString;
        }
    }
}