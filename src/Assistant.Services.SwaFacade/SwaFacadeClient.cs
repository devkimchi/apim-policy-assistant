namespace ApimPolicyAssistant.Services.SwaFacade;

/// <summary>
/// This represents the facade client entity to backend API.
/// </summary>
public partial class SwaFacadeClient
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
