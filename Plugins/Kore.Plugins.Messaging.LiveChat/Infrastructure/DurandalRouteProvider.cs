using System.Collections.Generic;
using Kore.Localization;
using Kore.Web.Infrastructure;

namespace Kore.Plugins.Messaging.LiveChat.Infrastructure
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
                    ModuleId = "viewmodels/plugins/messaging/livechat",
                    Route = "plugins/messaging/livechat",
                    JsPath = "/Plugins/Messaging.LiveChat/Scripts/index",
                    Title = localizer(LocalizableStrings.LiveChat)
                });

                return routes;
            }
        }

        #endregion IDurandalRouteProvider Members
    }
}