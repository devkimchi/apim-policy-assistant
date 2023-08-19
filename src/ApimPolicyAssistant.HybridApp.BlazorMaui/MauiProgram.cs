using System.Reflection;
using ApimPolicyAssistant.Services.Abstractions;
using ApimPolicyAssistant.Services.AssistantProxy;
using ApimPolicyAssistant.Services.AssistantProxy.Configurations;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ApimPolicyAssistant.HybridApp.BlazorMaui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("ApimPolicyAssistant.HybridApp.BlazorMaui.appsettings.json");
#if DEBUG
        using var streamDev = assembly.GetManifestResourceStream("ApimPolicyAssistant.HybridApp.BlazorMaui.appsettings.Development.json");
#endif
        var config = new ConfigurationBuilder()
                         .AddJsonStream(stream)
#if DEBUG
                         .AddJsonStream(streamDev)
#endif
                         .Build();
        var apimSettings = config.GetSection(ApimSettings.Name)
                                 .Get<ApimSettings>();
        builder.Services.AddSingleton(apimSettings);

        builder.Services.AddScoped<HttpClient>();
        builder.Services.AddScoped<IOpenApiClient, AssistantProxyClientWrapper>(sp =>
        {
            var http = sp.GetService<HttpClient>();
            var wrapper = new AssistantProxyClientWrapper(http)
            {
                BaseUrl = apimSettings.BaseUrl,
                ReadResponseAsString = true
            };
            wrapper.SetApiKey(apimSettings.SubscriptionKey);

            return wrapper;
        });

        return builder.Build();
    }
}
