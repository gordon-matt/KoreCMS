using System;
using System.Linq;
using System.Web.Mvc;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Services;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.ContentManagement.Areas.Admin.Widgets;
using Kore.Web.Mvc;

namespace Kore.Web.ContentManagement.Controllers
{
    [RouteArea("")]
    [RoutePrefix("kore-cms")]
    public class FrontendController : KoreController
    {
        [ChildActionOnly]
        [Route("menu")]
        public ActionResult Menu(string name, string templateViewName, bool filterByUrl = false)
        {
            string currentUrlSlug = filterByUrl ? Request.Url.ToString().RightOfLastIndexOf('/') : null;

            var pageService = EngineContext.Current.Resolve<IPageService>();

            // Check if it's a CMS page or not.
            if (currentUrlSlug != null && pageService.GetPageBySlug(currentUrlSlug) == null)
            {
                // It's not a CMS page, so don't try to filter by slug...
                // Set slug to null, to query for a menu without any URL filter
                currentUrlSlug = null;
            }

            var service = EngineContext.Current.Resolve<IMenuService>();
            var menu = service.FindByName(name, currentUrlSlug);

            if (menu == null)
            {
                return new EmptyResult();
                //throw new ArgumentException("There is no menu named, '" + name + "'");
            }

            var menuItems = EngineContext.Current.Resolve<IMenuItemService>().GetMenuItems(menu.Id, true);

            ViewBag.MenuId = menu.Id;
            return View(templateViewName, menuItems);
        }

        [ChildActionOnly]
        [Route("widgets-by-zone")]
        public ActionResult WidgetsByZone(string zoneName)
        {
            var widgetProviders = EngineContext.Current.ResolveAll<IWidgetProvider>();
            var widgets = widgetProviders.SelectMany(x => x.GetWidgets(zoneName, WorkContext.CurrentCultureCode)).ToList();
            return View("Kore.Web.ContentManagement.Views.Frontend.WidgetsByZone", widgets);
        }
    }
}