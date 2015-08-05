using System.Collections.Generic;
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
                var routes = new List<DurandalRoute>();

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/plugins/widgets/fullcalendar",
                    Route = "plugins/widgets/fullcalendar",
                    JsPath = "/Plugins/Widgets.FullCalendar/Scripts/index"
                });

                return routes;
            }
        }

        #endregion IDurandalRouteProvider Members
    }
}