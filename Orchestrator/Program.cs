using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Core;
using Orchestrator.BogusFunctions;
using PluginInterface;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
var skbuilder = Kernel.CreateBuilder();

var mode = configuration["OrchestratorSettings:Mode"];

skbuilder.AddAzureOpenAIChatCompletion(
                    deploymentName: configuration["AzureOpenAI:ChatDeploymentName"],
                    endpoint: configuration["AzureOpenAI:Endpoint"],
                    apiKey: configuration["AzureOpenAI:ApiKey"]
                );


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

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
