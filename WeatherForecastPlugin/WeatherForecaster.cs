using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using PluginInterface;
using System.ComponentModel;
using System.Net.Http;

namespace WeatherForecastPlugin;


public class WeatherForecaster : ISemanticKernelPlugin
{
   
    private static IConfiguration _config;
    private static IHttpClientFactory _httpClientFactory;

    [KernelFunction]
    [Description("Get the weather forecast for a specific date.")]
    public static async Task<string> GetWeatherForecast(
        [Description("The date for which to get the weather forecast, format as YYYY-MM-DD.")]
        string date)
    {
        Console.WriteLine($"Parameter value provided: {date}");
        Console.WriteLine($"{_config["OrchestratorSettings:Mode"]}");

        var client = _httpClientFactory.CreateClient();
        var forecastData = await client.GetStringAsync($"http://localhost:5236/weatherforecast?date={date}");

        Console.WriteLine(forecastData);

        return forecastData;
    }

    public async Task RegisterPluginAsync(Kernel kernel, string pluginName, IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        kernel.ImportPluginFromType<WeatherForecaster>(pluginName);
        _config = configuration;
        _httpClientFactory = httpClientFactory;
    }

}
