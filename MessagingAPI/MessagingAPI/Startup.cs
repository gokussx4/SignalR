using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Json;
using Microsoft.ServiceBus;
using Microsoft.Owin.Cors;
using Newtonsoft.Json;
using MessagingAPI.Formatters;
using Newtonsoft.Json.Serialization;
using Microsoft.Owin.Security.OAuth;
using MessagingAPI.Providers;

[assembly: OwinStartup(typeof(MessagingAPI.Startup))]

namespace MessagingAPI
{
    public partial class Startup
    {
        private const string signalRConnection = "Endpoint=sb://angindy.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SOatgbZU+0IaRpxA7TVilYr1IzqYVgCxK9ml7K8p8pg=";
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            ConfigureAuth(app);

            var settings = new JsonSerializerSettings { ContractResolver = new SignalRContractResolver() };
            var serializer = JsonSerializer.Create(settings);
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);

            app.Map("/signalr", map =>
            {
                // Setup the CORS middleware to run before SignalR.
                // By default this will allow all origins. You can 
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
                map.UseCors(CorsOptions.AllowAll);

                map.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
                {
                    Provider = new QueryStringOAuthBearerProvider()
                });

                var hubConfiguration = new HubConfiguration
                {
                    // You can enable JSONP by uncommenting line below.
                    // JSONP requests are insecure but some older browsers (and some
                    // versions of IE) require JSONP to work cross domain
                    // EnableJSONP = true
                    Resolver = GlobalHost.DependencyResolver
                };
                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch already runs under the "/signalr"
                // path.
                map.RunSignalR(hubConfiguration);
            });

            GlobalHost.HubPipeline.RequireAuthentication();

            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(signalRConnection);

            if (!namespaceManager.QueueExists("messages"))
            {
                namespaceManager.CreateQueue("messages");
            }
        }
    }
}
