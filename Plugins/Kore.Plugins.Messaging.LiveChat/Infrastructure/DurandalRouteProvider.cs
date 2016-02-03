//using System.Collections.Generic;
//using Kore.Localization;
//using Kore.Web.Infrastructure;

//namespace Kore.Plugins.Messaging.LiveChat.Infrastructure
//{
//    public class DurandalRouteProvider : IDurandalRouteProvider
//    {
//        #region IDurandalRouteProvider Members

//        public IEnumerable<DurandalRoute> Routes
//        {
//            get
//            {
//                var localizer = LocalizationUtilities.Resolve();
//                var routes = new List<DurandalRoute>();

//                routes.Add(new DurandalRoute
//                {
//                    ModuleId = "viewmodels/plugins/messaging/forums",
//                    Route = "plugins/messaging/forums",
//                    JsPath = "/Plugins/Messaging.LiveChat/Scripts/index",
//                    Title = localizer(LocalizableStrings.Forums)
//                });

//                return routes;
//            }
//        }

//        #endregion IDurandalRouteProvider Members
//    }
//}