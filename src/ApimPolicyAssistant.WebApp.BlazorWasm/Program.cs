using ApimPolicyAssistant.Services.Abstractions;
using ApimPolicyAssistant.Services.SwaFacade;
using ApimPolicyAssistant.WebApp.BlazorWasm;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient() { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IOpenApiClient, SwaFacadeClientWrapper>(sp =>
{
    var http = sp.GetService<HttpClient>();
    var facade = new SwaFacadeClientWrapper(http) { ReadResponseAsString = true };
    if (!builder.HostEnvironment.IsDevelopment())
    {
        var baseUrl = $"{builder.HostEnvironment.BaseAddress.TrimEnd('/')}/api";
        facade.BaseUrl = baseUrl;
    }

    return facade;
});

await builder.Build().RunAsync();
