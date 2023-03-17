using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DevSnap.Core.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private CoreSettings coreSettings { get; set; }

        public ConfigurationController(IOptions<CoreSettings> settings)
        {
            coreSettings = settings.Value;
        }

        [HttpGet("appname")]
        public IActionResult AppName()
        {
            return Ok(coreSettings.Services);
        }
    }
}
