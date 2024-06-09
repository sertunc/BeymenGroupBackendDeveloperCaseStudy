namespace Configuration.Common.Models
{
    public class ConfigurationDatabase
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ConfigurationCollectionName { get; set; } = null!;
    }
}
