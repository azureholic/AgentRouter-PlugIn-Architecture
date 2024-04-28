using Microsoft.SemanticKernel;
using PluginInterface;
using System.ComponentModel;

namespace WeatherForecastPlugin;


public class WeatherForecaster : ISemanticKernelPlugin
{
    

    [KernelFunction]
    [Description("Get the weather forecast for a specific date.")]
    public static string GetWeatherForecast(
        [Description("The date for which to get the weather forecast, format as YYYY-MM-DD.")]
        string date)
    {
        return $"nice weather {date}";
    }

    public async Task RegisterPluginAsync(Kernel kernel, string pluginName)
    {


        kernel.ImportPluginFromFunctions(pluginName,
            [
                kernel.CreateFunctionFromMethod((Kernel kernel) =>
                    kernel.ImportPluginFromType<WeatherForecaster>(
                        Guid.NewGuid().ToString("N")),
                        "WeatherForecast")
            ]
         );

        var fresult = await kernel.InvokeAsync(pluginName, "WeatherForecast");
        Console.WriteLine(fresult.Function.Name);


    }

}
