using System;
using System.Linq.Expressions;
using System.Reflection;
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

        public static void SetValueFromEnvVar<T, TValue>(T obj, Expression<Func<T, object>> expression, string envVarName)
        {
            var envVar = Environment.GetEnvironmentVariable(envVarName);
            if (string.IsNullOrWhiteSpace(envVar))
            {
                return;
            }

            var envVarValue = (TValue)Convert.ChangeType(envVar, typeof(TValue));

            string memberExpression;
            if (expression.Body is MemberExpression body) {
                memberExpression =  body.Member.Name;
            }
            else
            {
                var op = ((UnaryExpression)expression.Body).Operand;
                memberExpression = ((MemberExpression)op).Member.Name;
            }

            PropertyInfo property = typeof(T).GetProperty(memberExpression);
            property?.SetValue(obj, envVarValue);
        }
    }
}