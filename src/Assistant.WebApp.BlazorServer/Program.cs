using ApimPolicyAssistant.Services.Abstractions;
using ApimPolicyAssistant.Services.AssistantProxy;
using ApimPolicyAssistant.Services.AssistantProxy.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var apimSettings = builder.Configuration
                          .GetSection(ApimSettings.Name)
                          .Get<ApimSettings>();
builder.Services.AddSingleton(apimSettings);

builder.Services.AddHttpClient<IOpenApiClient, AssistantProxyClientWrapper>(httpClient =>
{
    var wrapper = new AssistantProxyClientWrapper(httpClient)
    {
        BaseUrl = apimSettings.BaseUrl,
        ReadResponseAsString = true
    };
    wrapper.SetApiKey(apimSettings.SubscriptionKey);

    return wrapper;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
