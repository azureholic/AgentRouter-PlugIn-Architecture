using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning;
using Orchestrator.Performance;

namespace Orchestrator.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {
        private readonly IConfiguration _config;
        private Kernel _kernel;
        private CallDuration callDuration = new CallDuration();

        public ChatbotController(IConfiguration config, Kernel kernel)
        {
            _config = config;
            _kernel = kernel;
        }


        [HttpPost]
        public async Task<ActionResult> Completion()
        {
            //for performance measurement
            callDuration.Start();

            string goal = "how is the weather tomorrow?";
            string finalResult = "Could not get a result";
                        
            //Stepwise Planner
            var stepwise = new FunctionCallingStepwisePlanner();
                
            var stepwiseResult = await stepwise.ExecuteAsync(_kernel, goal);

            foreach (var line in stepwiseResult.ChatHistory)
            {
                Console.WriteLine(line);
            };

            finalResult = stepwiseResult.FinalAnswer;

            //Handlebars Planner
            //var handlebar = new HandlebarsPlanner();
            //var handlebarPlan = await handlebar.CreatePlanAsync(_kernel, goal);
            //Console.WriteLine("--- prompt ---");
            //Console.WriteLine(handlebarPlan.Prompt);
            //Console.WriteLine("--- end prompt ---");

            //Console.WriteLine("--- plan ---");
            //Console.WriteLine(handlebarPlan);
            //Console.WriteLine("--- end plan ---");
            //var handlebarResult = await handlebarPlan.InvokeAsync(_kernel);

            //finalResult = handlebarResult;
         
         
            callDuration.Stop();
            return Ok(finalResult);
        }
    }
}
