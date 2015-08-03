using System.Collections.Generic;
using Kore.Web.Infrastructure;

namespace KoreCMS.Infrastructure
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
                    ModuleId = "viewmodels/admin/dashboard",
                    //Title = "Kore Admin",
                    Route = "",
                    JsPath = "viewmodels/admin/dashboard"
                    //Nav = true
                });

                return routes;
            }
        }

        #endregion IDurandalRouteProvider Members
    }
}