using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using PluginInterface;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

var kernel = Kernel.CreateBuilder()
                    .AddAzureOpenAIChatCompletion(
                        deploymentName: configuration["AzureOpenAI:ChatDeploymentName"],
                        endpoint: configuration["AzureOpenAI:Endpoint"],
                        apiKey: configuration["AzureOpenAI:ApiKey"]
                    )
                    .Build();

builder.Services.AddSingleton(kernel);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var plugIns = configuration.GetSection("Plugins").GetChildren();

plugIns.ToList().ForEach(async plugin =>
{
    string assemblyPath = plugin["AssemblyPath"];
    string plugInName = plugin["PluginName"];

    //do some validation here

    var assembly = Assembly.LoadFrom(assemblyPath);
    foreach (var type in assembly.GetTypes())
    {
        if (typeof(ISemanticKernelPlugin).IsAssignableFrom(type) && !type.IsAbstract)
        {
            // Create an instance of the class
            var instance = Activator.CreateInstance(type) as ISemanticKernelPlugin;

            if (instance == null)
            {
                throw new Exception("Could not create instance of plugin");
            }

            // Call the RegisterPlugin method
            await instance.RegisterPluginAsync(kernel, plugInName );
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
