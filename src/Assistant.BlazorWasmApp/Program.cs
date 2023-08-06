using ApimAIAssistant.BlazorWasmApp;
using ApimAIAssistant.BlazorWasmApp.Facades;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
    var http = new HttpClient();
    if (!builder.HostEnvironment.IsDevelopment())
    {
        http.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    }

    return http;
});
builder.Services.AddScoped(sp =>
{
    var http = sp.GetService<HttpClient>();
    var facade = new FacadeClientWrapper(http);
    if (!builder.HostEnvironment.IsDevelopment())
    {
        var baseUrl = $"{builder.HostEnvironment.BaseAddress.TrimEnd('/')}/api";
        facade.BaseUrl = baseUrl;
    }

    return facade;
});

await builder.Build().RunAsync();
