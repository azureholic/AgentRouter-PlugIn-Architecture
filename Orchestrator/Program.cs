using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using PluginInterface;
using System.Reflection;
using LLama.Common;
using LLama;
using LLamaSharp.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Plugins.Core;
using Orchestrator.BogusFunctions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
var skbuilder = Kernel.CreateBuilder();

var mode = configuration["OrchestratorSettings:Mode"];

if (mode == "Llama")
{

    string modelPath = configuration["Llama:ModelPath"];
    int gpuLayerCount = int.Parse(configuration["Llama:GpuLayerCount"]);

    var parameters = new ModelParams(modelPath)
    {
        ContextSize = 1024, // The longest length of chat as memory.
        GpuLayerCount = gpuLayerCount // How many layers to offload to GPU. Please adjust it according to your GPU memory.
    };
    using var model = LLamaWeights.LoadFromFile(parameters);
    //using var context = model.CreateContext(parameters);
    var executor = new StatelessExecutor(model, parameters);

    builder.Services.AddSingleton(new LLamaSharpChatCompletion(executor));

    //skbuilder.Services.AddKeyedSingleton("local-llama", new LLamaSharpChatCompletion(executor));


}
else
{
    skbuilder.AddAzureOpenAIChatCompletion(
                        deploymentName: configuration["AzureOpenAI:ChatDeploymentName"],
                        endpoint: configuration["AzureOpenAI:Endpoint"],
                        apiKey: configuration["AzureOpenAI:ApiKey"]
                    );
}

//add build in plugins
skbuilder.Plugins.AddFromType<TimePlugin>();

//add a 20 bogus plugins
for (int i = 0; i < 20; i++)
{
    skbuilder.Plugins.AddFromType<BogusFunctions>("BogusFunctions" + i.ToString());
}

var kernel = skbuilder.Build();

builder.Services.AddSingleton(kernel);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Load plugins from config
var plugIns = configuration.GetSection("Plugins").GetChildren();

plugIns.ToList().ForEach(async plugin =>
{
    string assemblyPath = plugin["AssemblyPath"];
    string plugInName = plugin["PluginName"];

    //do some validation here
    //TODO:

    var assembly = Assembly.LoadFrom(assemblyPath);
    foreach (var type in assembly.GetTypes())
    {
        if (typeof(ISemanticKernelPlugin).IsAssignableFrom(type) && !type.IsAbstract)
        {

                    
            var instance = Activator.CreateInstance(type) as ISemanticKernelPlugin;

            if (instance == null)
            {
                throw new Exception("Could not create instance of plugin");
            }

            // Call the RegisterPlugin method
            await instance.RegisterPluginAsync(kernel, plugInName, configuration );
        }
    }
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
