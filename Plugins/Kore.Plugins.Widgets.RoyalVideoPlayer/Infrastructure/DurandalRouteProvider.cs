using System.Collections.Generic;
using Kore.Localization;
using Kore.Web.Infrastructure;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer.Infrastructure
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
                    ModuleId = "viewmodels/plugins/royalvideoplayer",
                    Route = "plugins/royalvideoplayer",
                    JsPath = "/Plugins/Widgets.RoyalVideoPlayer/Scripts/index",
                    Title = localizer(LocalizableStrings.RoyalVideoPlayer)
                });

                return routes;
            }
        }

        #endregion IDurandalRouteProvider Members
    }
}