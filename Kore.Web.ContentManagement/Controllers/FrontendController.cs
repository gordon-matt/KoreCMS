using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kore.Collections;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Services;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.ContentManagement.Areas.Admin.Widgets;
using Kore.Web.Mvc;
using Kore.Web.Navigation;
using MenuItem = Kore.Web.ContentManagement.Areas.Admin.Menus.Domain.MenuItem;

namespace Kore.Web.ContentManagement.Controllers
{
    [RouteArea("")]
    [RoutePrefix("kore-cms")]
    public class FrontendController : KoreController
    {
        [ChildActionOnly]
        [Route("auto-breadcrumbs")]
        public ActionResult AutoBreadcrumbs(string templateViewName)
        {
            string currentUrlSlug = Request.Url.ToString().RightOfLastIndexOf('/');

            var pageService = EngineContext.Current.Resolve<IPageService>();

            var currentPage = pageService.Repository.Table.FirstOrDefault(y => y.Slug == currentUrlSlug);
            var query = pageService.Repository.Table.Where(x => x.IsEnabled).ToHashSet();

            var breadcrumbs = new List<Breadcrumb>();

            if (currentPage != null)
            {
                var parentId = currentPage.ParentId;
                while (parentId != null)
                {
                    var page = query.FirstOrDefault(y => y.Id == parentId);
                    breadcrumbs.Add(new Breadcrumb
                    {
                        Text = page.Name,
                        Url = "/" + page.Slug
                    });
                    parentId = page.ParentId;
                }

                breadcrumbs.Reverse();

                breadcrumbs.Add(new Breadcrumb
                {
                    Text = currentPage.Name
                });
            }

            return View(templateViewName, breadcrumbs);
        }

        [ChildActionOnly]
        [Route("auto-menu")]
        public ActionResult AutoMenu(string templateViewName, bool includeHomePageLink = true)
        {
            var pageService = EngineContext.Current.Resolve<IPageService>();
            var menuItems = new List<MenuItem>();
            var menuId = Guid.NewGuid();

            if (includeHomePageLink)
            {
                menuItems.Add(new MenuItem
                {
                    Id = menuId,
                    Text = "Home", //TODO: Localize
                    Url = "/",
                    Enabled = true,
                    ParentId = null,
                    Position = -1 // Always first
                });
            }

            var pages = pageService.Repository.Table
                .Where(x => x.IsEnabled)
                .OrderBy(x => x.Order)
                .ThenBy(x => x.Name)
                .ToHashSet()
                .Select((x, index) => new MenuItem
            {
                Id = x.Id,
                Text = x.Name,
                Url = "/" + x.Slug,
                Enabled = true,
                ParentId = x.ParentId,
                Position = index
            });

            menuItems.AddRange(pages);

            ViewBag.MenuId = menuId;
            return View(templateViewName, menuItems);
        }

        [ChildActionOnly]
        [Route("auto-sub-menu")]
        public ActionResult AutoSubMenu(string templateViewName)
        {
            string currentUrlSlug = Request.Url.ToString().RightOfLastIndexOf('/');

            var pageService = EngineContext.Current.Resolve<IPageService>();
            var menuItems = new List<MenuItem>();
            var menuId = Guid.NewGuid();

            var currentPage = pageService.Repository.Table.FirstOrDefault(y => y.Slug == currentUrlSlug);
            var query = pageService.Repository.Table.Where(x => x.IsEnabled);

            if (currentPage != null)
            {
                query = query.Where(x => x.ParentId == currentPage.Id);
            }
            else
            {
                query = query.Where(x => x.ParentId == null);
            }

            var pages = query
                .OrderBy(x => x.Order)
                .ThenBy(x => x.Name)
                .ToHashSet()
                .Select((x, index) => new MenuItem
                {
                    Id = x.Id,
                    Text = x.Name,
                    Url = "/" + x.Slug,
                    Enabled = true,
                    ParentId = x.ParentId,
                    Position = index
                });

            menuItems.AddRange(pages);

            ViewBag.MenuId = menuId;
            return View(templateViewName, menuItems);
        }

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