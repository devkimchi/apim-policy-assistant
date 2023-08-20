using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

using ApimPolicyAssistant.Services.Abstractions;
using ApimPolicyAssistant.Services.AssistantProxy;

using Autofac;
using Autofac.Integration.Web;

namespace ApimPolicyAssistant.WebApp.WebForm
{
    public class Global : HttpApplication
    {
        // Provider that holds the application container.
        private static IContainerProvider _containerProvider;

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

            var builder = new ContainerBuilder();
            builder.RegisterType<AssistantProxyClientWrapper>()
                   .As<IOpenApiClient>()
                   .InstancePerRequest();

            var container = builder.Build();
            _containerProvider = new ContainerProvider(container);
        }
    }
}
