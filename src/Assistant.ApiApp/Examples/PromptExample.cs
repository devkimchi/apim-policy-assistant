using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace ApimAIAssistant.ApiApp.Examples;

/// <summary>
/// This represents the example entity for prompt.
/// </summary>
public class PromptExample : OpenApiExample<string>
{
    /// <inheritdoc />
    public override IOpenApiExample<string> Build(NamingStrategy namingStrategy = null)
    {
        var prompt = "Show me the APIM policy document in general.";

        this.Examples.Add(OpenApiExampleResolver.Resolve("prompt", prompt, namingStrategy));

        return this;
    }
}
