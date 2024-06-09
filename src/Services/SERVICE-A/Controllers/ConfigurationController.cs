using Configuration.Library;
using Microsoft.AspNetCore.Mvc;

namespace SERVICE_A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly ConfigurationReader _configurationReader;

        public ConfigurationController(ConfigurationReader configurationReader)
        {
            _configurationReader = configurationReader ?? throw new System.ArgumentNullException(nameof(configurationReader));
        }

        [HttpGet]
        public IActionResult Get()
        {
            var stringResult = _configurationReader.GetValue<string>("SiteName");
            var boolResult = _configurationReader.GetValue<bool>("IsBasketEnabled");
            var intResult = _configurationReader.GetValue<int>("ItemCount");

            var result = $"stringResult: {stringResult}, boolResult: {boolResult}, intResult: {intResult}";

            return Ok(result);
        }
    }
}
