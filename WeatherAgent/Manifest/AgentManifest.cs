namespace WeatherAgent.Manifest;

public static class AgentManifest
{


    public static object GetManifest(string host, int? port)
    {
        var swaggerUrl = (port == null) ? 
            $"https://{host}/swagger/v1/swagger.json" : 
            $"https://{host}:{port}/swagger/v1/swagger.json";
        
        var manifest = new {
            applicationName = "WeatherForecast Plugin",
            description = "This plugin returns a weather forecast",
            publisher = new {
                name = "Alice",
                contactEmail = "alice@contoso.com"
            },
            apiDependencies = new {
                weatherApi = new {
                    apiDescriptionUrl = swaggerUrl,
                    requests = new[] { new
                        {
                            method = "Get",
                            uriTemplate = "/weather"
                        }
                    }
                }
            }           
        };

        return manifest;

    }
}
