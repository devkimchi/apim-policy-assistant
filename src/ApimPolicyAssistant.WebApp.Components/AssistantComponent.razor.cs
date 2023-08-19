using ApimPolicyAssistant.Services.Abstractions;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ApimPolicyAssistant.WebApp.Components;

/// <summary>
/// This represents the APIM policy assistant component entity.
/// </summary>
public partial class AssistantComponent : ComponentBase
{
    /// <summary>
    /// Gets or sets the <see cref="IOpenApiClient"/> instance.
    /// </summary>
    [Inject]
    protected IOpenApiClient? Api { get; set; }

    /// <summary>
    /// Gets or sets the prompt.
    /// </summary>
    protected string? Prompt { get; set; } = "Show me the APIM policy in general.";

    /// <summary>
    /// Gets or sets the completion.
    /// </summary>
    protected string? Completion { get; set; }

    /// <summary>
    /// Handles the event when the "Complete!" button is clicked.
    /// </summary>
    /// <param name="ev"><see cref="MouseEventArgs"/> instance.</param>
    protected async Task CompleteAsync(MouseEventArgs ev)
        => this.Completion = await Api.GetCompletionsAsync(this.Prompt);

    /// <summary>
    /// Handles the event when the "Clear!" button is clicked.
    /// </summary>
    /// <param name="ev"><see cref="MouseEventArgs"/> instance.</param>
    protected async Task ClearAsync(MouseEventArgs ev)
    {
        this.Prompt = default;
        this.Completion = default;

        await Task.CompletedTask;
    }
}
