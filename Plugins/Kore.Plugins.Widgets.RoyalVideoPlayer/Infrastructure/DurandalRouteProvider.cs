using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                var routes = new List<DurandalRoute>();

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/plugins/royalvideoplayer",
                    Route = "plugins/royalvideoplayer",
                    JsPath = "/Plugins/Widgets.RoyalVideoPlayer/Scripts/index"
                });

                return routes;
            }
        }

        #endregion IDurandalRouteProvider Members
    }
}
