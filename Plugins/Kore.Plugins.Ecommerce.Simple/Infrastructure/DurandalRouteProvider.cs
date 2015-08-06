using System.Collections.Generic;
using Kore.Localization;
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
                var localizer = LocalizationUtilities.Resolve();
                var routes = new List<DurandalRoute>();

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/plugins/ecommerce/simple",
                    Route = "plugins/ecommerce/simple",
                    JsPath = "/Plugins/Ecommerce.Simple/Scripts/admin-home",
                    Title = localizer(LocalizableStrings.Store)
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/plugins/ecommerce/simple/categories",
                    Route = "plugins/ecommerce/simple/categories",
                    JsPath = "/Plugins/Ecommerce.Simple/Scripts/admin-categories",
                    Title = localizer(LocalizableStrings.Categories)
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/plugins/ecommerce/simple/orders",
                    Route = "plugins/ecommerce/simple/orders",
                    JsPath = "/Plugins/Ecommerce.Simple/Scripts/admin-orders",
                    Title = localizer(LocalizableStrings.Orders)
                });

                return routes;
            }
        }

        #endregion IDurandalRouteProvider Members
    }
}