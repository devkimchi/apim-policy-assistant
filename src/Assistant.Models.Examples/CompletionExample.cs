using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace ApimAIAssistant.Models.Examples;

/// <summary>
/// This represents the example entity for completion.
/// </summary>
public class CompletionExample : OpenApiExample<string>
{
    /// <inheritdoc />
    public override IOpenApiExample<string> Build(NamingStrategy namingStrategy = null)
    {
        var completion = "```xml\n<policies>\n  <inbound />\n  <backend>\n    <forward-request />\n  </backend>\n  <outbound />\n  <on-error />\n</policies>\n```";

        this.Examples.Add(OpenApiExampleResolver.Resolve("completion", completion, namingStrategy));

        return this;
    }
}
