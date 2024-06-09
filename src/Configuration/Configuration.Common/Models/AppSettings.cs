using System.Collections.Generic;

namespace Configuration.Common.Models
{
    public class AppSettings
    {
        public ConfigurationDatabase ConfigurationDatabase { get; set; }
        public List<string> ApplicationNames { get; set; }
    }
}
