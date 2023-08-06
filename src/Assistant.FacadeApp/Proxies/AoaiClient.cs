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
public class AoaiClient : AoaiFacadeClient, IAoaiClient
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AoaiClient"/> class.
    /// </summary>
    /// <param name="factory"><see cref="IHttpClientFactory"/> instance.</param>
    public AoaiClient(IHttpClientFactory factory)
        : base(factory.ThrowIfNullOrDefault().CreateClient("aoai"))
    {
    }

    /// <inheritdoc />
    public async Task<string> GetCompletionsAsync(string prompt, string baseUrl, string apiKey)
    {
        this.BaseUrl = baseUrl.ThrowIfNullOrWhiteSpace();
        this.HttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey.ThrowIfNullOrWhiteSpace());

        return await this.GetCompletionsAsync(prompt).ConfigureAwait(false);
    }

    /// <inheritdoc />
    protected override async Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(HttpResponseMessage response, IReadOnlyDictionary<string, IEnumerable<string>> headers, CancellationToken cancellationToken)
    {
        if (typeof(T) != typeof(string))
        {
            return await base.ReadObjectResponseAsync<T>(response, headers, cancellationToken).ConfigureAwait(false);
        }

        var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var responseObject = (T)Convert.ChangeType(responseText, typeof(T));
        return new ObjectResponseResult<T>(responseObject, responseText);
    }
}
