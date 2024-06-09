using System.ComponentModel.DataAnnotations;

namespace Configuration.Common.ViewModels
{
    public class ConfigViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public string ApplicationName { get; set; }
    }
}