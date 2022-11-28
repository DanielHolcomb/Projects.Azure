using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projects.Azure.Interfaces;

namespace Projects.Azure.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AzureController : ControllerBase
    {

        private readonly ILogger<AzureController> _logger;
        private readonly IAzureService _azureService;

        public AzureController(ILogger<AzureController> logger, IAzureService azureService)
        {
            _logger = logger;
            _azureService = azureService;
        }

        [HttpGet]
        [Route("Resource/Groups")]
        [Authorize]
        public  IActionResult GetResourceGroups()
        {
            try
            {
                var response = _azureService.GetAllResourceGroups();
                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Resources/{resourceGroupName}")]
        [Authorize]
        public async Task<IActionResult> GetResources(string resourceGroupName)
        {
            try
            {
                var response = await _azureService.GetAllResources(resourceGroupName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }
    }
}