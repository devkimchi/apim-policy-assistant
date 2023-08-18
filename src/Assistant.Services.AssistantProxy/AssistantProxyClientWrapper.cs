namespace ApimPolicyAssistant.Services.AssistantProxy;

/// <summary>
/// This provides interface to the <see cref="AssistantProxyClientWrapper"/> class.
/// </summary>
public interface IAssistantProxyClientWrapper
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
public class AssistantProxyClientWrapper : AssistantProxyClient, IAssistantProxyClientWrapper
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AssistantProxyClientWrapper"/> class.
    /// </summary>
    /// <param name="httpClient"><see cref="HttpClient"/> instance.</param>
    public AssistantProxyClientWrapper(HttpClient httpClient)
        : base(httpClient)
    {
    }

    /// <inheritdoc />
    public async Task<string> GetCompletionsAsync(string prompt, string baseUrl, string apiKey)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            throw new ArgumentNullException(nameof(prompt));
        }
        this.BaseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
        this.HttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey ?? throw new ArgumentNullException(nameof(apiKey)));

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
