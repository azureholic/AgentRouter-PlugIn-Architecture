
using LLama;
using LLama.Common;
using LLamaSharp.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Newtonsoft.Json.Linq;
using ChatHistory = Microsoft.SemanticKernel.ChatCompletion.ChatHistory;

namespace Orchestrator.Llama;

public class LlamaFunctionCall
{
    private Kernel _kernel;
    private LLamaSharpChatCompletion _llamaSharpChatCompletion;
    public LlamaFunctionCall(Kernel kernel, LLamaSharpChatCompletion chatCompletionService)
    {
        _kernel = kernel;
        _llamaSharpChatCompletion = chatCompletionService;
}
    public async Task<string> GetResult(string problem)
    {
       var plugins = _kernel.Plugins.GetFunctionsMetadata();

        // Create chat history
        ChatHistory history = [];

        
        var functionsPrompt = CreateFunctionsMetaObject(plugins);

        // Prompt for llama hf function calling v3
        var prompt = $"""
                      [INST] You have access to the following functions. Use them if required:
                      {functionsPrompt}
                      """;

        history.AddSystemMessage(prompt);

        // Get chat completion service
        //var chatCompletionService = _kernel.GetRequiredService<LLamaSharpChatCompletion>();
        history.AddUserMessage(problem);
       
        MyChatRequestSettings llamaSettings = new()
        {
            //not yet released
            AutoInvoke = true

        };

        

        var result = await _llamaSharpChatCompletion.GetChatMessageContentAsync(
            history,
            executionSettings: llamaSettings
            );

        var finalResult = result.ToString();

        return finalResult;
    }

    private static JToken? CreateFunctionsMetaObject(IList<KernelFunctionMetadata> plugins)
    {
        if (plugins.Count < 1) return null;
        if (plugins.Count == 1) return CreateFunctionMetaObject(plugins[0]);

        JArray promptFunctions = [];
        foreach (var plugin in plugins)
        {
            var pluginFunctionWrapper = CreateFunctionMetaObject(plugin);
            promptFunctions.Add(pluginFunctionWrapper);
        }

        return promptFunctions;
    }

    private static JObject CreateFunctionMetaObject(KernelFunctionMetadata plugin)
    {
        var pluginFunctionWrapper = new JObject()
        {
            { "type", "function" },
        };

        var pluginFunction = new JObject()
        {
            { "name", plugin.Name },
            { "description", plugin.Description },
        };

        var pluginFunctionParameters = new JObject()
        {
            { "type", "object" },
        };
        var pluginProperties = new JObject();
        foreach (var parameter in plugin.Parameters)
        {
            var property = new JObject()
            {
                { "type", parameter.ParameterType?.ToString() },
                { "description", parameter.Description },
            };

            pluginProperties.Add(parameter.Name, property);
        }

        pluginFunctionParameters.Add("properties", pluginProperties);
        pluginFunction.Add("parameters", pluginFunctionParameters);
        pluginFunctionWrapper.Add("function", pluginFunction);

        return pluginFunctionWrapper;
    }
}
