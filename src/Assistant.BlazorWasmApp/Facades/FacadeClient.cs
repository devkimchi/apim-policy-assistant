namespace ApimAIAssistant.BlazorWasmApp.Facades;

/// <summary>
/// This represents the facade client entity to backend API.
/// </summary>
public partial class FacadeClient
{
    /// <summary>
    /// Gets the <see cref="System.Net.Http.HttpClient"/> instance.
    /// </summary>
    public virtual HttpClient HttpClient
    {
        get
        {
            return this._httpClient;
        }
    }
}
