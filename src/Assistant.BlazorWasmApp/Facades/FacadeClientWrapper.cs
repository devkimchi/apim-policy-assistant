namespace ApimAIAssistant.BlazorWasmApp.Facades;

/// <summary>
/// This represents the proxy client entity to backend API for AOAI.
/// </summary>
public class FacadeClientWrapper : FacadeClient
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FacadeClientWrapper"/> class.
    /// </summary>
    /// <param name="httpClient"><see cref="HttpClient"/> instance.</param>
    public FacadeClientWrapper(HttpClient httpClient)
        : base(httpClient)
    {
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
