using ApimAIAssistant.BlazorWasmApp;
using ApimAIAssistant.Facades.SwaFacade;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient() { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp =>
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
