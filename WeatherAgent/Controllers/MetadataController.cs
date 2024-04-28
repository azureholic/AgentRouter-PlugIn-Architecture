using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherAgent.Manifest;

namespace WeatherAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetadataController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var manifest = AgentManifest.GetManifest(Request.Host.Host, Request.Host.Port);

            return Ok(manifest);

        }
    }
}
