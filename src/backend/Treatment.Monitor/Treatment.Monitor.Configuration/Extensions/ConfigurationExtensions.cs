using Microsoft.Extensions.Configuration;

namespace Treatment.Monitor.Configuration.Extensions
{
    public static class ConfigurationExtensions
    {
        public static TInterface GetObjectFromConfigurationSection<TSection, TInterface>(
            this IConfiguration configuration,
            string sectionName = null)
            where TSection : TInterface
            where TInterface : class
        {
            string sectionNameUpdated = !string.IsNullOrWhiteSpace(sectionName) ? sectionName : typeof(TSection).Name;
            IConfigurationSection section = configuration.GetSection(sectionNameUpdated);
            return section.Get<TSection>();
        }
    }
}