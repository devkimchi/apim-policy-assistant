namespace ApimPolicyAssistant.Services.AssistantProxy.Configurations;

/// <summary>
/// This represents the settings entity for APIM.
/// </summary>
public class ApimSettings
{
    /// <summary>
    /// Gets the name of the configuration.
    /// </summary>
    public const string Name = "Apim";

    /// <summary>
    /// Gets or sets the base URL.
    /// </summary>
    public virtual string? BaseUrl { get; set; }

    /// <summary>
    /// Gets or sets the subscription key.
    /// </summary>
    public virtual string? SubscriptionKey { get; set; }
}
