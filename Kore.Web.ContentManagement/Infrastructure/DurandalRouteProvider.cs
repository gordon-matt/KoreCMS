using System.Collections.Generic;
using Kore.Infrastructure;
using Kore.Web.Infrastructure;
using Kore.Web.Mvc.Resources;

namespace Kore.Web.ContentManagement.Infrastructure
{
    public class DurandalRouteProvider : IDurandalRouteProvider
    {
        #region IDurandalRouteProvider Members

        public IEnumerable<DurandalRoute> Routes
        {
            get
            {
                var routes = new List<DurandalRoute>();

                var workContext = EngineContext.Current.Resolve<IWebWorkContext>();
                var scriptRegister = new ScriptRegister(workContext);

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/blog",
                    Route = "blog",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/blog")
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/blocks/content-blocks",
                    Route = "blocks/content-blocks(/:pageId)",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/content-blocks")
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/blocks/entity-type-content-blocks",
                    Route = "blocks/entity-type-content-blocks/:entityType/:entityId",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/entity-type-content-blocks")
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/localization/languages",
                    Route = "localization/languages",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/languages")
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/localization/localizable-strings",
                    Route = "localization/localizable-strings/:cultureCode",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/localizable-strings")
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/media",
                    Route = "media",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/media")
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/menus",
                    Route = "menus",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/menus")
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/messaging/templates",
                    Route = "messaging/templates",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/message-templates")
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/messaging/queued-email",
                    Route = "messaging/queued-email",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/queued-emails")
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/pages",
                    Route = "pages",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/pages")
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/newsletters/subscribers",
                    Route = "newsletters/subscribers",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/newsletters/subscribers")
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/sitemap/xml-sitemap",
                    Route = "sitemap/xml-sitemap",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/xml-sitemap")
                });

                return routes;
            }
        }

        #endregion IDurandalRouteProvider Members
    }
}