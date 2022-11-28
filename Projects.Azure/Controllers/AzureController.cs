using Microsoft.AspNetCore.Mvc;

namespace Projects.Azure.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AzureController : ControllerBase
    {

        private readonly ILogger<AzureController> _logger;

        public AzureController(ILogger<AzureController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}