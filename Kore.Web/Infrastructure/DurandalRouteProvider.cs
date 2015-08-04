using System.Collections.Generic;
using Kore.Infrastructure;
using Kore.Web.Mvc.Resources;

namespace Kore.Web.Infrastructure
{
    public class DurandalRouteProvider : IDurandalRouteProvider
    {
        #region IDurandalRouteProvider Members

        public IEnumerable<DurandalRoute> Routes
        {
            get
            {
                var routes = new List<DurandalRoute>();

                var workContext = EngineContext.Current.Resolve<IWebWorkContext>();
                var scriptRegister = new ScriptRegister(workContext);

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/indexing",
                    Route = "indexing",
                    JsPath = scriptRegister.GetBundleUrl("kore-web/indexing")
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/log",
                    Route = "log",
                    JsPath = scriptRegister.GetBundleUrl("kore-web/log")
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/membership",
                    Route = "membership",
                    JsPath = scriptRegister.GetBundleUrl("kore-web/membership")
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/plugins",
                    Route = "plugins",
                    JsPath = scriptRegister.GetBundleUrl("kore-web/plugins")
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/scheduledtasks",
                    Route = "scheduledtasks",
                    JsPath = scriptRegister.GetBundleUrl("kore-web/scheduled-tasks")
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/configuration/settings",
                    Route = "configuration/settings",
                    JsPath = scriptRegister.GetBundleUrl("kore-web/settings")
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/configuration/themes",
                    Route = "configuration/themes",
                    JsPath = scriptRegister.GetBundleUrl("kore-web/themes")
                });

                return routes;
            }
        }

        #endregion IDurandalRouteProvider Members
    }
}