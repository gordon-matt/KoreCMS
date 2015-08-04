using System.Collections.Generic;
using Kore.Web.Infrastructure;

namespace Kore.Plugins.Ecommerce.Simple.Infrastructure
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
                    ModuleId = "viewmodels/plugins/ecommerce/simple",
                    Route = "plugins/ecommerce/simple",
                    JsPath = "/Plugins/Ecommerce.Simple/Scripts/admin-home"
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/plugins/ecommerce/simple/categories",
                    Route = "plugins/ecommerce/simple/categories",
                    JsPath = "/Plugins/Ecommerce.Simple/Scripts/admin-categories"
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/plugins/ecommerce/simple/orders",
                    Route = "plugins/ecommerce/simple/orders",
                    JsPath = "/Plugins/Ecommerce.Simple/Scripts/admin-orders"
                });

                return routes;
            }
        }

        #endregion IDurandalRouteProvider Members
    }
}