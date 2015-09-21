using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Kore.Infrastructure;
using Kore.Web;
using Kore.Web.Infrastructure;
using Kore.Web.Php.ViewEngines;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace KoreCMS
{
    public class MvcApplication : HttpApplicationBase
    {
        protected override void OnApplicationStart()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Uncomment this if you want to write some PHP views as well,
            //  as described here: https://phpviewengine.codeplex.com/
            //ViewEngines.Engines.Add(new PhpViewEngine());

            TryUpdateNLogConnectionString();
        }

        private static void TryUpdateNLogConnectionString()
        {
            try
            {
                var dataSettings = EngineContext.Current.Resolve<DataSettings>();
                var target = LogManager.Configuration.FindTargetByName("database");

                DatabaseTarget databaseTarget = null;
                var wrapperTarget = target as WrapperTargetBase;

                // Unwrap the target if necessary.
                if (wrapperTarget == null)
                {
                    databaseTarget = target as DatabaseTarget;
                }
                else
                {
                    databaseTarget = wrapperTarget.WrappedTarget as DatabaseTarget;
                }

                databaseTarget.ConnectionString = dataSettings.ConnectionString;
            }
            catch { }
        }
    }
}