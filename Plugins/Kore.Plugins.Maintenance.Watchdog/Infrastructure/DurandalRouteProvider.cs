using System.Collections.Generic;
using Kore.Localization;
using Kore.Web.Infrastructure;

namespace Kore.Plugins.Maintenance.Watchdog.Infrastructure
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

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/maintenance/watchdog",
                    Route = "maintenance/watchdog",
                    JsPath = "/Plugins/Maintenance.Watchdog/Scripts/index",
                    Title = localizer(LocalizableStrings.ManageServices)
                });

                return routes;
            }
        }

        #endregion IDurandalRouteProvider Members
    }
}