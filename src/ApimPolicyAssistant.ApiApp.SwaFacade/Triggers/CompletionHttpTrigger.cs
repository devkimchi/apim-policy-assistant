using System.Net;

using ApimPolicyAssistant.Models.Examples;
using ApimPolicyAssistant.Services.Abstractions;
using ApimPolicyAssistant.Services.AssistantProxy.Configurations;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace ApimPolicyAssistant.ApiApp.SwaFacade.Triggers;

/// <summary>
/// This represents the HTTP trigger facade entity for ChatGPT completion.
/// </summary>
public class CompletionHttpTrigger
{
    private readonly ApimSettings _apimSettings;
    private readonly IOpenApiClient _assistant;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompletionHttpTrigger"/> class.
    /// </summary>
    /// <param name="apimSettings"><see cref="ApimSettings"/> instance.</param>
    /// <param name="assistant"><see cref="IAssistantProxyClientWrapper"/> instance.</param>
    /// <param name="loggerFactory"><see cref="ILoggerFactory"/> instance.</param>
    public CompletionHttpTrigger(ApimSettings apimSettings, IOpenApiClient assistant, ILoggerFactory loggerFactory)
    {
        this._apimSettings = apimSettings.ThrowIfNullOrDefault();
        this._assistant = assistant.ThrowIfNullOrDefault();
        this._logger = loggerFactory.ThrowIfNullOrDefault().CreateLogger<CompletionHttpTrigger>();
    }

    /// <summary>
    /// Invokes the HTTP trigger that completes the prompt.
    /// </summary>
    /// <param name="req"><see cref="HttpRequestData"/> instance.</param>
    /// <returns><see cref="HttpResponseData"/> instance.</returns>
    [Function(nameof(CompletionHttpTrigger.GetCompletionsAsync))]
    [OpenApiOperation(operationId: "getCompletions", tags: new[] { "completions" }, Summary = "Gets the completion from the OpenAI API", Description = "This gets the completion from the OpenAI API.", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(string), Required = true, Example = typeof(PromptExample), Description = "The prompt to generate the completion.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Example = typeof(CompletionExample), Summary = "The completion generated from the OpenAI API.", Description = "This returns the completion generated from the OpenAI API.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "text/plain", bodyType: typeof(string), Example = typeof(BadRequestExample), Summary = "Invalid request.", Description = "This indicates the request is invalid.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "text/plain", bodyType: typeof(string), Example = typeof(InternalServerErrorExample), Summary = "Internal server error.", Description = "This indicates the server is not working as expected.")]
    public async Task<HttpResponseData> GetCompletionsAsync([HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "completions")] HttpRequestData req)
    {
        this._logger.LogInformation("C# HTTP trigger function processed a request.");

        var response = default(HttpResponseData);
        var prompt = req.ReadAsString();
        try
        {
            var completion = await this._assistant
                                       .GetCompletionsAsync(prompt);

            response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString(completion);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, ex.Message);

            response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString(ex.Message);
        }

        return response;
    }
}
