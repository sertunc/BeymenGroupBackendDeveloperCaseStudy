using Configuration.Business.Abstractions;
using Configuration.Common.Models;
using Configuration.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Configuration.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly ILogger<ConfigurationController> _logger;
        private readonly IConfigurationBusiness _configurationBusiness;
        private readonly AppSettings _appSettings;

        public ConfigurationController(
            ILogger<ConfigurationController> logger,
            IOptionsSnapshot<AppSettings> appSettings,
            IConfigurationBusiness configurationBusiness)
        {
            //IOptionsSnapshot kullanarak uygulama çalışırken appsettings.json'a eklenen yeni application nameleri alabiliriz.
            _appSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(appSettings));
            _configurationBusiness = configurationBusiness ?? throw new ArgumentNullException(nameof(configurationBusiness));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogDebug("Getting configuration list");

            var result = await _configurationBusiness.GetAllAsync();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            _logger.LogDebug("Getting configuration item by id={0}", id);

            var result = await _configurationBusiness.GetAsync(id);

            return StatusCode(200, result);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(ConfigViewModel model)
        {
            _logger.LogDebug("Configuration item add or update with id={0} name={1}", model.Id, model.Name);

            var result = await _configurationBusiness.AddOrUpdateAsync(model);

            return StatusCode(200, result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(ConfigViewModel model)
        {
            _logger.LogDebug("Configuration item delete with id={0} name={1}", model.Id, model.Name);

            var result = await _configurationBusiness.DeleteAsync(model);

            return StatusCode(200, result);
        }

        [HttpGet("GetValueTypes")]
        public IActionResult GetValueTypeList()
        {
            _logger.LogDebug("Getting GetValueTypeList");

            var result = _configurationBusiness.GetValueTypeList();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetApplicationNames")]
        public IActionResult GetApplicationNames()
        {
            var applicationNames = _appSettings.ApplicationNames;

            if (applicationNames == null || applicationNames.Count == 0)
            {
                return NotFound();
            }

            return Ok(applicationNames);
        }
    }
}