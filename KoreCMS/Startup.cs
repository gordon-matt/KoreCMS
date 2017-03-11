using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Cors;
using Kore.Infrastructure;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartupAttribute(typeof(KoreCMS.Startup))]

namespace KoreCMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            if (!DataSettingsHelper.IsDatabaseInstalled)
            {
                return;
            }

            var corsPolicy = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true,
                AllowAnyOrigin = true,
                SupportsCredentials = true
            };

            // For OData
            corsPolicy.ExposedHeaders.Add("DataServiceVersion");
            corsPolicy.ExposedHeaders.Add("MaxDataServiceVersion");

            var corsOptions = new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => Task.FromResult(corsPolicy)
                }
            };

            app.UseCors(corsOptions);

            ConfigureAuth(app);

            var owinStartupConfigurations = EngineContext.Current.ResolveAll<IOwinStartupConfiguration>();

            var existingConfigurations = new List<string>();
            foreach (var owinStartupConfiguration in owinStartupConfigurations)
            {
                owinStartupConfiguration.Configuration(app, existingConfigurations);
            }
        }
    }
}