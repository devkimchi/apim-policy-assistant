using System.Net.Http;
using System.Reflection;
using System.Windows;
using ApimPolicyAssistant.Services.Abstractions;
using ApimPolicyAssistant.Services.AssistantProxy;
using ApimPolicyAssistant.Services.AssistantProxy.Configurations;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApimPolicyAssistant.HybridApp.BlazorWpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddWpfBlazorWebView();

        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("ApimPolicyAssistant.HybridApp.BlazorWpf.appsettings.json");
#if DEBUG
        using var streamDev = assembly.GetManifestResourceStream("ApimPolicyAssistant.HybridApp.BlazorWpf.appsettings.Development.json");
#endif
        var config = new ConfigurationBuilder()
                         .AddJsonStream(stream)
#if DEBUG
                         .AddJsonStream(streamDev)
#endif
                         .Build();
        var apimSettings = config.GetSection(ApimSettings.Name)
                                 .Get<ApimSettings>();
        serviceCollection.AddSingleton(apimSettings);

        serviceCollection.AddScoped<HttpClient>();
        serviceCollection.AddScoped<IOpenApiClient, AssistantProxyClientWrapper>(sp =>
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

        Resources.Add("services", serviceCollection.BuildServiceProvider());
    }

    private void Handle_UrlLoading(object sender, UrlLoadingEventArgs urlLoadingEventArgs)
    {
        if (urlLoadingEventArgs.Url.Host != "0.0.0.0")
        {
            urlLoadingEventArgs.UrlLoadingStrategy = UrlLoadingStrategy.OpenInWebView;
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        this.Width = SystemParameters.PrimaryScreenWidth / 2;
        this.Height = SystemParameters.PrimaryScreenHeight * 2 / 3;
    }
}
