using System.Collections.Generic;
using Kore.Localization;
using Kore.Web.Infrastructure;

namespace Kore.Plugins.Widgets.RevolutionSlider.Infrastructure
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
                    ModuleId = "viewmodels/plugins/widgets/revolutionslider",
                    Route = "plugins/widgets/revolutionslider",
                    JsPath = "/Plugins/Widgets.RevolutionSlider/Scripts/index",
                    Title = localizer(LocalizableStrings.RevolutionSlider)
                });

                return routes;
            }
        }

        #endregion IDurandalRouteProvider Members
    }
}