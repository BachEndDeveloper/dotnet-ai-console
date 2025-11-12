using dotnetAiConsole;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

Console.WriteLine("Hello, World!");

string modelId = "gpt-4.1";
string endpoint = "https://demo.openai.azure.com";
string apiKey = "api_key_kere";

#region Ignore SSL Errors. Only for development purposes!

HttpClientHandler httpClientHandler = new()
{
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
};

HttpClient httpClient = new(httpClientHandler);

#endregion

var builder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey, httpClient: httpClient);

// Add enterprise components
builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Warning));

// Build the kernel
Kernel kernel = builder.Build();

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

kernel.Plugins.AddFromType<LightsPlugin>("Lights");

// Enable planning
OpenAIPromptExecutionSettings openAiPromptExecutionSettings = new()
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
};

ChatHistory chatHistory =
    new(
        "You are a helpful AI assistant for managing smart home devices. Only use the provided functions to control the devices. You are not allowed to make up any functions or actions on your own.");
Console.WriteLine("What would you like to do? (Type 'exit' to quit)");
while (true)
{
    Console.Write("User: ");
    string userInput = Console.ReadLine() ?? string.Empty;

    if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }


    chatHistory.AddUserMessage(userInput);
    var response = await chatCompletionService.GetChatMessageContentAsync(
        chatHistory,
        openAiPromptExecutionSettings,
        kernel);

    Console.WriteLine(string.Empty);
    Console.WriteLine($"Assistant: {response.Content}");
    Console.WriteLine(string.Empty);
}