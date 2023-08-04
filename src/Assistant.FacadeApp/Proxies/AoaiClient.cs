using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace ApimAIAssistant.FacadeApp.Proxies;

/// <summary>
/// This provides interface to the <see cref="AoaiClient"/> class.
/// </summary>
public interface IAoaiClient
{
    /// <summary>
    /// Gets or sets the base URL.
    /// </summary>
    string BaseUrl { get; set; }

    /// <summary>
    /// Gets the prompt completion.
    /// </summary>
    /// <param name="completionsPostRequest">Prompt value.</param>
    /// <returns>Returns the prompt completion.</returns>
    Task<string> GetCompletionsAsync(string completionsPostRequest);

    /// <summary>
    /// Gets the prompt completion.
    /// </summary>
    /// <param name="completionsPostRequest">Prompt value.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
    /// <returns>Returns the prompt completion.</returns>
    Task<string> GetCompletionsAsync(string completionsPostRequest, CancellationToken cancellationToken);
}

/// <summary>
/// This represents the proxy client entity to backend API for AOAI.
/// </summary>
public partial class AoaiClient : IAoaiClient
{
    public AoaiClient(IHttpClientFactory factory)
        : this(factory.ThrowIfNullOrDefault().CreateClient("aoai"))
    {
    }
}
