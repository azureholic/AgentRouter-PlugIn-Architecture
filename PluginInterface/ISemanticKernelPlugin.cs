using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using System.Net.Http;

namespace PluginInterface
{
    public interface ISemanticKernelPlugin
    {

        Task RegisterPluginAsync(Kernel kernel, string pluginName, IConfiguration configuration, IHttpClientFactory httpClientFactory);
    }
}
