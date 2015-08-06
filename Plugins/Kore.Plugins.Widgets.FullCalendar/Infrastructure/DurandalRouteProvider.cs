using System.Collections.Generic;
using Kore.Localization;
using Kore.Web.Infrastructure;

namespace Kore.Plugins.Widgets.FullCalendar.Infrastructure
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
                    ModuleId = "viewmodels/plugins/widgets/fullcalendar",
                    Route = "plugins/widgets/fullcalendar",
                    JsPath = "/Plugins/Widgets.FullCalendar/Scripts/index",
                    Title = localizer(LocalizableStrings.FullCalendar)
                });

                return routes;
            }
        }

        #endregion IDurandalRouteProvider Members
    }
}