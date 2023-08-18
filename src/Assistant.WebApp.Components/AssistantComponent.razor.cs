using ApimPolicyAssistant.Services.SwaFacade;

using Microsoft.AspNetCore.Components;

namespace ApimPolicyAssistant.WebApp.Components;

public partial class AssistantComponent : ComponentBase
{
    [Inject]
    protected SwaFacadeClientWrapper? Facade { get; set; }

    protected string? Prompt { get; set; } = "Show me the APIM policy in general.";
    protected string? Completion { get; set; }

    protected async Task CompleteAsync()
        => this.Completion = await Facade.GetCompletionsAsync(this.Prompt);

    protected async Task ClearAsync()
    {
        this.Prompt = default;
        this.Completion = default;

        await Task.CompletedTask;
    }
}
