using System.Collections.Generic;
using Kore.Infrastructure;
using Kore.Localization;
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
                var localizer = LocalizationUtilities.Resolve();
                var routes = new List<DurandalRoute>();

                var workContext = EngineContext.Current.Resolve<IWebWorkContext>();
                var scriptRegister = new ScriptRegister(workContext);

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/blog",
                    Route = "blog",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/blog"),
                    Title = localizer(KoreCmsLocalizableStrings.Blog.Title)
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/blocks/content-blocks",
                    Route = "blocks/content-blocks(/:pageId)",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/content-blocks"),
                    Title = localizer(KoreCmsLocalizableStrings.ContentBlocks.Title)
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/blocks/entity-type-content-blocks",
                    Route = "blocks/entity-type-content-blocks/:entityType/:entityId",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/entity-type-content-blocks"),
                    Title = localizer(KoreCmsLocalizableStrings.ContentBlocks.Title)
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/media",
                    Route = "media",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/media"),
                    Title = localizer(KoreCmsLocalizableStrings.Media.Title)
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/menus",
                    Route = "menus",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/menus"),
                    Title = localizer(KoreCmsLocalizableStrings.Menus.Title)
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/messaging/templates",
                    Route = "messaging/templates",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/message-templates"),
                    Title = localizer(KoreCmsLocalizableStrings.Messaging.MessageTemplates)
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/messaging/queued-email",
                    Route = "messaging/queued-email",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/queued-emails"),
                    Title = localizer(KoreCmsLocalizableStrings.Messaging.QueuedEmails)
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/pages",
                    Route = "pages",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/pages"),
                    Title = localizer(KoreCmsLocalizableStrings.Pages.Title)
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/newsletters/subscribers",
                    Route = "newsletters/subscribers",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/newsletters/subscribers"),
                    Title = localizer(KoreCmsLocalizableStrings.Newsletters.Subscribers)
                });

                routes.Add(new DurandalRoute
                {
                    ModuleId = "viewmodels/admin/sitemap/xml-sitemap",
                    Route = "sitemap/xml-sitemap",
                    JsPath = scriptRegister.GetBundleUrl("kore-cms/xml-sitemap"),
                    Title = localizer(KoreCmsLocalizableStrings.Sitemap.XMLSitemap)
                });

                return routes;
            }
        }

        #endregion IDurandalRouteProvider Members
    }
}