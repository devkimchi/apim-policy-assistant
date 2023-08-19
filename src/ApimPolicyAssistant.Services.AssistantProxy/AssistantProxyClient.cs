namespace ApimPolicyAssistant.Services.AssistantProxy;

/// <summary>
/// This represents the proxy client entity to backend API for AOAI.
/// </summary>
public partial class AssistantProxyClient
{
    /// <summary>
    /// Gets the <see cref="System.Net.Http.HttpClient"/> instance.
    /// </summary>
    protected virtual HttpClient HttpClient
    {
        get
        {
            return this._httpClient;
        }
    }
}
