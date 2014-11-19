using System;
using System.Linq;
using System.Web.Mvc;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Services;
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
        public ActionResult Menu(string name, string templateViewName)
        {
            var service = EngineContext.Current.Resolve<IMenuService>();
            var menu = service.FindByName(name);

            if (menu == null)
            {
                throw new ArgumentException("There is no menu named, '" + name + "'");
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
            var widgets = widgetProviders.SelectMany(x => x.GetWidgets(WorkContext.CurrentCultureCode)).ToList();
            return View("Kore.Web.ContentManagement.Views.Frontend.WidgetsByZone", widgets);
        }
    }
}