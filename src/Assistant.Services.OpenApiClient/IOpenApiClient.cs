namespace ApimPolicyAssistant.Services.Abstractions;

/// <summary>
/// This provides interfaces to the OpenAPI proxy client classes.
/// </summary>
public interface IOpenApiClient
{
    /// <summary>
    /// Gets or sets the base URL.
    /// </summary>
    string? BaseUrl { get; set; }

    /// <summary>
    /// Sets the API key.
    /// </summary>
    /// <param name="apiKey">API key.</param>
    /// <returns>Returns the <see cref="IOpenApiClient"/> instance.</returns>
    IOpenApiClient SetApiKey(string apiKey);

    /// <summary>
    /// Gets the prompt completion.
    /// </summary>
    /// <param name="prompt">Prompt value.</param>
    /// <returns>Returns the prompt completion.</returns>
    Task<string> GetCompletionsAsync(string prompt);
}
