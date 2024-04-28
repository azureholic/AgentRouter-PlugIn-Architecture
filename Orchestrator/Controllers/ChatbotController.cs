using Json.More;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning;

namespace Orchestrator.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {
        private readonly IConfiguration _config;
        private Kernel _kernel;

        public ChatbotController(IConfiguration config, Kernel kernel) {
            _config = config;
            _kernel = kernel;
        }


        [HttpPost]
        public async Task<ActionResult> Completion()
        {
            string goal = "what is the weather tomorrow?";
            var stepwise = new FunctionCallingStepwisePlanner();
            var result = await stepwise.ExecuteAsync(_kernel, goal);
            return Ok(result.FinalAnswer);
        }
    }
}
