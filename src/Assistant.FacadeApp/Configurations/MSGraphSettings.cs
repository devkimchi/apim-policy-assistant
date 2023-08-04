namespace ApimAIAssistant.FacadeApp.Configurations;

/// <summary>
/// This represents the settings entity for Microsoft Graph.
/// </summary>
public class MSGraphSettings
{
    /// <summary>
    /// Gets the name of the configuration.
    /// </summary>
    public const string Name = "MSGraph";

    /// <summary>
    /// Gets or sets the tenant ID.
    /// </summary>
    public virtual string? TenantId { get; set; }

    /// <summary>
    /// Gets or sets the client ID.
    /// </summary>
    public virtual string? ClientId { get; set; }

    /// <summary>
    /// Gets or sets the client secret.
    /// </summary>
    public virtual string? ClientSecret { get; set; }
}
