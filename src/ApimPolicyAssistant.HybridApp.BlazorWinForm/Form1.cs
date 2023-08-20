using System.Reflection;

using ApimPolicyAssistant.Services.Abstractions;
using ApimPolicyAssistant.Services.AssistantProxy;
using ApimPolicyAssistant.Services.AssistantProxy.Configurations;

using Microsoft.AspNetCore.Components.WebView;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApimPolicyAssistant.HybridApp.BlazorWinForm;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        var services = new ServiceCollection();
        services.AddWindowsFormsBlazorWebView();

        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("ApimPolicyAssistant.HybridApp.BlazorWinForm.appsettings.json");
#if DEBUG
        using var streamDev = assembly.GetManifestResourceStream("ApimPolicyAssistant.HybridApp.BlazorWinForm.appsettings.Development.json");
#endif
        var config = new ConfigurationBuilder()
                         .AddJsonStream(stream)
#if DEBUG
                         .AddJsonStream(streamDev)
#endif
                         .Build();
        var apimSettings = config.GetSection(ApimSettings.Name)
                                 .Get<ApimSettings>();
        services.AddSingleton(apimSettings);

        services.AddScoped<HttpClient>();
        services.AddScoped<IOpenApiClient, AssistantProxyClientWrapper>(sp =>
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

        blazorWebView1.HostPage = "wwwroot\\index.html";
        blazorWebView1.Services = services.BuildServiceProvider();
        blazorWebView1.RootComponents.Add<App>("#app");

        blazorWebView1.UrlLoading += this.Handle_UrlLoading;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        this.Width = Screen.FromControl(this).WorkingArea.Width / 2;
        this.Height = Screen.FromControl(this).WorkingArea.Height * 2 / 3;
    }

    private void Handle_UrlLoading(object sender, UrlLoadingEventArgs urlLoadingEventArgs)
    {
        if (urlLoadingEventArgs.Url.Host != "0.0.0.0")
        {
            urlLoadingEventArgs.UrlLoadingStrategy = UrlLoadingStrategy.OpenInWebView;
        }
    }
}
