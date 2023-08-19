using ApimPolicyAssistant.ApiApp.SwaFacade.Configurations;
using ApimPolicyAssistant.Services.Abstractions;
using ApimPolicyAssistant.Services.AssistantProxy;
using ApimPolicyAssistant.Services.AssistantProxy.Configurations;

using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
                //.ConfigureOpenApi()
                .ConfigureHostConfiguration(config => config.AddEnvironmentVariables())
                .ConfigureServices(services =>
                {
                    var apimSettings = services.BuildServiceProvider()
                            .GetService<IConfiguration>()
                            .Get<ApimSettings>(ApimSettings.Name);
                    services.AddSingleton(apimSettings);

                    var graphSettings = services.BuildServiceProvider()
                            .GetService<IConfiguration>()
                            .Get<MSGraphSettings>(MSGraphSettings.Name);
                    services.AddSingleton(graphSettings);

                    services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
                    {
                        var options = new OpenApiConfigurationOptions()
                        {
                            OpenApiVersion = OpenApiVersionType.V3,
                            Info = new OpenApiInfo()
                            {
                                Version = DefaultOpenApiConfigurationOptions.GetOpenApiDocVersion(),
                                Title = DefaultOpenApiConfigurationOptions.GetOpenApiDocTitle(),
                                Description = DefaultOpenApiConfigurationOptions.GetOpenApiDocDescription(),
                                License = new OpenApiLicense()
                                {
                                    Name = "MIT",
                                    Url = new Uri("http://opensource.org/licenses/MIT"),
                                }
                            },
                        };

                        return options;
                    });

                    services.AddHttpClient<IOpenApiClient, AssistantProxyClientWrapper>(httpClient =>
                    {
                        var wrapper = new AssistantProxyClientWrapper(httpClient)
                        {
                            BaseUrl = apimSettings.BaseUrl,
                            ReadResponseAsString = true
                        };
                        wrapper.SetApiKey(apimSettings.SubscriptionKey);

                        return wrapper;
                    });
                })
                .Build();

host.Run();
