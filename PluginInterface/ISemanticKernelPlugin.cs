﻿using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

namespace PluginInterface
{
    public interface ISemanticKernelPlugin
    {

        Task RegisterPluginAsync(Kernel kernel, string pluginName, IConfiguration configuration);
    }
}
