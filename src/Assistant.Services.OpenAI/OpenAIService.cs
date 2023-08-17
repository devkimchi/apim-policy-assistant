using ApimAIAssistant.Services.OpenAI.Configurations;

using Azure;
using Azure.AI.OpenAI;

namespace ApimAIAssistant.Services.OpenAI;

public interface IOpenAIService
{
    Task<string> GetCompletionsAsync(string prompt);
}

public class OpenAIService : IOpenAIService
{
    private readonly OpenAIApiSettings _openAISettings;
    private readonly PromptSettings _promptSettings;

    public OpenAIService(OpenAIApiSettings openAISettings, PromptSettings promptSettings)
    {
        this._openAISettings = openAISettings ?? throw new ArgumentNullException(nameof(openAISettings));
        this._promptSettings = promptSettings ?? throw new ArgumentNullException(nameof(promptSettings));
    }

    public async Task<string> GetCompletionsAsync(string prompt)
    {
        var endpoint = new Uri(this._openAISettings.Endpoint);
        var credential = new AzureKeyCredential(this._openAISettings.AuthKey);
        var client = new OpenAIClient(endpoint, credential);

        var chatCompletionsOptions = new ChatCompletionsOptions()
        {
            Messages =
                {
                    new ChatMessage(ChatRole.System, this._promptSettings.System),

                    new ChatMessage(ChatRole.User, this._promptSettings.Users[0]),
                    new ChatMessage(ChatRole.System, this._promptSettings.Assistants[0]),

                    new ChatMessage(ChatRole.User, this._promptSettings.Users[1]),
                    new ChatMessage(ChatRole.System, this._promptSettings.Assistants[1]),

                    new ChatMessage(ChatRole.User, this._promptSettings.Users[2]),
                    new ChatMessage(ChatRole.System, this._promptSettings.Assistants[2]),

                    new ChatMessage(ChatRole.User, prompt)
                },
            MaxTokens = 3000,
            Temperature = 0.7f,
        };

        var deploymentId = this._openAISettings.DeploymentId;

        var response = default(string);
        try
        {
            var result = await client.GetChatCompletionsAsync(deploymentId, chatCompletionsOptions);
            response = result.Value.Choices[0].Message.Content;
        }
        catch
        {
            throw;
        }

        return response;
    }
}
