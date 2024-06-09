using Configuration.Library;
using Microsoft.AspNetCore.Mvc;
using System;

namespace SERVICE_C.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly ConfigurationReader _configurationReader;

        public ConfigurationController(ConfigurationReader configurationReader)
        {
            _configurationReader = configurationReader ?? throw new ArgumentNullException(nameof(configurationReader));
        }

        [HttpGet]
        public IActionResult Get()
        {
            var stringResult = _configurationReader.GetValue<string>("SiteName");

            var result = $"stringResult: {stringResult}";

            return Ok(result);
        }
    }
}
