using System.Collections.Generic;
using Kore.Infrastructure;
using Kore.Localization;
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
                var localizer = LocalizationUtilities.Resolve();
                var routes = new List<DurandalRoute>();

                var workContext = EngineContext.Current.Resolve<IWebWorkContext>();
                var scriptRegister = new ScriptRegister(workContext);

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/indexing",
                    Route = "indexing",
                    JsPath = scriptRegister.GetBundleUrl("kore-web/indexing"),
                    Title = localizer(KoreWebLocalizableStrings.Indexing.Title)
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/log",
                    Route = "log",
                    JsPath = scriptRegister.GetBundleUrl("kore-web/log"),
                    Title = localizer(KoreWebLocalizableStrings.Log.Title)
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/membership",
                    Route = "membership",
                    JsPath = scriptRegister.GetBundleUrl("kore-web/membership"),
                    Title = localizer(KoreWebLocalizableStrings.Membership.Title)
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/plugins",
                    Route = "plugins",
                    JsPath = scriptRegister.GetBundleUrl("kore-web/plugins"),
                    Title = localizer(KoreWebLocalizableStrings.Plugins.Title)
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/scheduledtasks",
                    Route = "scheduledtasks",
                    JsPath = scriptRegister.GetBundleUrl("kore-web/scheduled-tasks"),
                    Title = localizer(KoreWebLocalizableStrings.ScheduledTasks.Title)
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/configuration/settings",
                    Route = "configuration/settings",
                    JsPath = scriptRegister.GetBundleUrl("kore-web/settings"),
                    Title = localizer(KoreWebLocalizableStrings.General.Settings)
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/configuration/themes",
                    Route = "configuration/themes",
                    JsPath = scriptRegister.GetBundleUrl("kore-web/themes"),
                    Title = localizer(KoreWebLocalizableStrings.General.Themes)
                });

                return routes;
            }
        }

        #endregion IDurandalRouteProvider Members
    }
}