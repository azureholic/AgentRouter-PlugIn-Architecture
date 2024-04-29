# AgentRouter-Plugin-Architecture

This repo contains an example of how development teams individually develop an agent service (API) and provide an artifact to include their agent in a generic orchestrator.
This solution is based on LLM Function Calling and the Semantic Kernel.

![High Level Architecture](images/high%20level%20architecture.jpg)

## Components

### PluginInterface
This is a class library that describes the contract for Semantic Kernel Plugins that will work for the Orchestrator. All Plugins should implement this interface.

You can consider packaging this library in a NuGet package to distribute across your development teams.

### WeatherAgent
This is a standard .NET WebAPI that provides a weather forecast for a specific date. The data is fictional.

### WeatherForecastPlugin
This is a stand alone project that Implements the ISemanticKernelPlugin, so it can be loaded and can register itself in the plugin collection of the Orchestrator.
The plugin is a class where the methods that need to be exposed as Functions to the Semantic Kernel contain the attribute [KernelFunction].
The orchestrator holds no reference to this assembly and it should be provided as a DLL to the Orchestrator team.


### Orchestrator
This API is an example of how to build an orchestrator that fronts all of your agents.
Based on the config it will load the plugins assemblies at start.
The chatbot Controller will create a plan and executes that plan based on the available plugins and user question.

Note: BogusFunctions are added to test how big the prompt will get before hitting limits. These functions will never be executed, but are considered by the planner.