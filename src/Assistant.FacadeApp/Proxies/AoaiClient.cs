using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace ApimAIAssistant.FacadeApp.Proxies;

/// <summary>
/// This provides interface to the <see cref="AoaiClient"/> class.
/// </summary>
public interface IAoaiClient
{
    /// <summary>
    /// Gets the prompt completion.
    /// </summary>
    /// <param name="prompt">Prompt value.</param>
    /// <param name="baseUrl">Base URL value.</param>
    /// <param name="apiKey">API key value.</param>
    /// <returns>Returns the prompt completion.</returns>
    Task<string> GetCompletionsAsync(string prompt, string baseUrl, string apiKey);
}

/// <summary>
/// This represents the proxy client entity to backend API for AOAI.
/// </summary>
public partial class AoaiClient : IAoaiClient
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AoaiClient"/> class.
    /// </summary>
    /// <param name="factory"><see cref="IHttpClientFactory"/> instance.</param>
    public AoaiClient(IHttpClientFactory factory)
        : this(factory.ThrowIfNullOrDefault().CreateClient("aoai"))
    {
    }

    /// <inheritdoc />
    public async Task<string> GetCompletionsAsync(string prompt, string baseUrl, string apiKey)
    {
        this.BaseUrl = baseUrl.ThrowIfNullOrWhiteSpace();
        this._httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey.ThrowIfNullOrWhiteSpace());

        return await this.GetCompletionsAsync(prompt).ConfigureAwait(false);
    }
}
