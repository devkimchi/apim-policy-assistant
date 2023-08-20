using System;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

using ApimPolicyAssistant.Services.Abstractions;
using ApimPolicyAssistant.Services.AssistantProxy;
using ApimPolicyAssistant.Services.AssistantProxy.Configurations;

using Autofac;
using Autofac.Integration.Web;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace ApimPolicyAssistant.WebApp.WebForm
{
    public class Global : HttpApplication, IContainerProviderAccessor
    {
        // Provider that holds the application container.
        private static IContainerProvider _containerProvider;

        public static IConfiguration Configuration;
        private static IConfigurationRefresher _configurationRefresher;

        // Instance property that will be used by Autofac HttpModules
        // to resolve and inject dependencies.
        public IContainerProvider ContainerProvider
        {
            get { return _containerProvider; }
        }

        private void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var assembly = Assembly.GetExecutingAssembly();
            var config = default(IConfigurationRoot);
            using (var stream = assembly.GetManifestResourceStream("ApimPolicyAssistant.WebApp.WebForm.appsettings.json"))
#if DEBUG
            using (var streamDev = assembly.GetManifestResourceStream("ApimPolicyAssistant.WebApp.WebForm.appsettings.Development.json"))
#endif
            {
                config = new ConfigurationBuilder()
                                 .AddJsonStream(stream)
#if DEBUG
                                 .AddJsonStream(streamDev)
#endif
                                 .Build();
            }
            var apimSettings = config.GetSection(ApimSettings.Name)
                                     .Get<ApimSettings>();

            var builder = new ContainerBuilder();
            builder.RegisterInstance<ApimSettings>(apimSettings).SingleInstance();
            builder.RegisterType<HttpClient>().InstancePerRequest();
            builder.Register<AssistantProxyClientWrapper>(p =>
            {
                var http = p.Resolve<HttpClient>();
                var wrapper = new AssistantProxyClientWrapper(http)
                {
                    BaseUrl = apimSettings.BaseUrl,
                    ReadResponseAsString = true
                };
                wrapper.SetApiKey(apimSettings.SubscriptionKey);

                return wrapper;
            })
                   .As<IOpenApiClient>()
                   .InstancePerRequest();

            var container = builder.Build();
            _containerProvider = new ContainerProvider(container);
        }
    }
}
