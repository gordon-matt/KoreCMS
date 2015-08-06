using System.Collections.Generic;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Web.Infrastructure;
using Kore.Web.Mvc.Resources;

namespace Kore.Web.Common.Infrastructure
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
                    ModuleId = "viewmodels/admin/regions",
                    Route = "regions",
                    JsPath = scriptRegister.GetBundleUrl("kore-common/regions"),
                    Title = localizer(LocalizableStrings.Regions.Title)
                });

                return routes;
            }
        }

        #endregion IDurandalRouteProvider Members
    }
}