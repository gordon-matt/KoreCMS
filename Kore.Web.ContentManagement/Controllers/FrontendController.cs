using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Blog;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Services;
using Kore.Web.ContentManagement.Areas.Admin.Pages;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.ContentManagement.Infrastructure;
using Kore.Web.Mvc;
using Kore.Web.Navigation;
using MenuItem = Kore.Web.ContentManagement.Areas.Admin.Menus.Domain.MenuItem;

namespace Kore.Web.ContentManagement.Controllers
{
    [RouteArea("")]
    [RoutePrefix("kore-cms")]
    public class FrontendController : KoreController
    {
        public readonly Lazy<BlogSettings> blogSettings;

        public FrontendController(Lazy<BlogSettings> blogSettings)
        {
            this.blogSettings = blogSettings;
        }

        [ChildActionOnly]
        [Route("auto-breadcrumbs")]
        public ActionResult AutoBreadcrumbs(string templateViewName)
        {
            var breadcrumbs = new List<Breadcrumb>();

            string currentUrlSlug = Request.Url.LocalPath.TrimStart('/');

            if (currentUrlSlug == "blog")
            {
                breadcrumbs.Add(new Breadcrumb
                {
                    Text = blogSettings.Value.PageTitle
                });
                return View(templateViewName, breadcrumbs);
            }

            var pageService = EngineContext.Current.Resolve<IPageService>();
            var pageVersionService = EngineContext.Current.Resolve<IPageVersionService>();

            var currentPageVersion = pageVersionService.Repository.Table
                .Include(x => x.Page)
                .FirstOrDefault(y => y.Slug == currentUrlSlug);

            var allPages = pageService.Find(x => x.IsEnabled);

            if (currentPageVersion != null)
            {
                var parentId = currentPageVersion.Page.ParentId;
                while (parentId != null)
                {
                    var parentPage = allPages.FirstOrDefault(y => y.Id == parentId);

                    if (parentPage == null)
                    {
                        break;
                    }

                    if (PageSecurityHelper.CheckUserHasAccessToPage(parentPage, User))
                    {
                        var currentVersion = pageVersionService.GetCurrentVersion(parentPage.Id, WorkContext.CurrentCultureCode);
                        breadcrumbs.Add(new Breadcrumb
                        {
                            Text = currentVersion.Title,
                            Url = parentPage.IsEnabled ? "/" + currentVersion.Slug : null
                        });
                    }

                    parentId = parentPage.ParentId;
                }

                breadcrumbs.Reverse();

                breadcrumbs.Add(new Breadcrumb
                {
                    Text = currentPageVersion.Title
                });
            }
            else
            {
                // This is not a CMS page, so use breadcrumbs specified in controller actions...
                breadcrumbs.AddRange(WorkContext.Breadcrumbs);
            }

            return View(templateViewName, breadcrumbs);
        }

        [ChildActionOnly]
        [Route("auto-menu")]
        public ActionResult AutoMenu(string templateViewName, bool includeHomePageLink = true)
        {
            var pageService = EngineContext.Current.Resolve<IPageService>();
            var pageVersionService = EngineContext.Current.Resolve<IPageVersionService>();

            var menuItems = new List<MenuItem>();
            var menuId = Guid.NewGuid();

            if (includeHomePageLink)
            {
                menuItems.Add(new MenuItem
                {
                    Id = menuId,
                    Text = T(KoreWebLocalizableStrings.General.Home),
                    Url = "/",
                    Enabled = true,
                    ParentId = null,
                    Position = -1 // Always first
                });
            }

            var pageVersions = pageVersionService.GetCurrentVersions(
                WorkContext.CurrentCultureCode,
                enabledOnly: true,
                shownOnMenusOnly: true);

            var authorizedPages = pageVersions.Where(x => PageSecurityHelper.CheckUserHasAccessToPage(x.Page, User));

            var items = authorizedPages
                .Select(x => new MenuItem
            {
                Id = x.Page.Id,
                Text = x.Title,
                Url = "/" + x.Slug,
                Enabled = true,
                ParentId = x.Page.ParentId,
                Position = x.Page.Order
            });

            menuItems.AddRange(items);

            if (PageSecurityHelper.CheckUserHasAccessToBlog(User) && blogSettings.Value.ShowOnMenus)
            {
                menuItems.Add(new MenuItem
                {
                    Id = menuId,
                    Text = blogSettings.Value.PageTitle,
                    Url = "/blog",
                    Enabled = true,
                    ParentId = null,
                    Position = blogSettings.Value.MenuPosition
                });
            }

            var menuProviders = EngineContext.Current.ResolveAll<IAutoMenuProvider>();
            foreach (var menuProvider in menuProviders)
            {
                menuItems.AddRange(menuProvider.GetMainMenuItems());
            }

            menuItems = menuItems
                .OrderBy(x => x.Position)
                .ThenBy(x => x.Text)
                .ToList();

            ViewBag.MenuId = menuId;
            return View(templateViewName, menuItems);
        }

        [ChildActionOnly]
        [Route("auto-sub-menu")]
        public ActionResult AutoSubMenu(string templateViewName)
        {
            // we need a better way to get slug, because it could be something like /store/categories/category-1/product-1
            // and this current way would only return product-1
            string currentUrlSlug = Request.Url.LocalPath.TrimStart('/');
            var menuItems = new List<MenuItem>();
            var menuId = Guid.NewGuid();

            var menuProviders = EngineContext.Current.ResolveAll<IAutoMenuProvider>();

            // If home page
            if (string.IsNullOrEmpty(currentUrlSlug))
            {
                if (blogSettings.Value.ShowOnMenus &&
                    PageSecurityHelper.CheckUserHasAccessToBlog(User))
                {
                    menuItems.Add(new MenuItem
                    {
                        Id = menuId,
                        Text = blogSettings.Value.PageTitle,
                        Url = "/blog",
                        Enabled = true,
                        ParentId = null,
                        Position = blogSettings.Value.MenuPosition
                    });
                }

                foreach (var menuProvider in menuProviders)
                {
                    menuItems.AddRange(menuProvider.GetMainMenuItems().Where(x => x.ParentId == null));
                }
            }
            foreach (var menuProvider in menuProviders)
            {
                if (currentUrlSlug.StartsWith(menuProvider.RootUrlSlug))
                {
                    menuItems.AddRange(menuProvider.GetSubMenuItems(currentUrlSlug));
                }
            }

            var pageVersionService = EngineContext.Current.Resolve<IPageVersionService>();

            var pageVersions = Enumerable.Empty<PageVersion>();

            bool hasCmsPages = true;

            // If on home page
            if (string.IsNullOrEmpty(currentUrlSlug))
            {
                pageVersions = pageVersionService.GetCurrentVersions(
                    WorkContext.CurrentCultureCode,
                    enabledOnly: true,
                    shownOnMenusOnly: true,
                    topLevelOnly: true);
            }
            else
            {
                // We don't care about culture here because the only thing we're interested in getting is
                //  the Page ID, which will of course be the same for all versions of a page.
                var anyVersion = pageVersionService.Repository.Table.FirstOrDefault(y => y.Slug == currentUrlSlug);

                // If the current page is a CMS page
                if (anyVersion != null)
                {
                    pageVersions = pageVersionService.GetCurrentVersions(
                        WorkContext.CurrentCultureCode,
                        enabledOnly: true,
                        shownOnMenusOnly: true,
                        parentId: anyVersion.Page.Id);
                }
                else
                {
                    hasCmsPages = false;
                }
            }

            if (hasCmsPages)
            {
                var authorizedPages = pageVersions.Where(x => PageSecurityHelper.CheckUserHasAccessToPage(x.Page, User));

                var items = authorizedPages
                    .Select(x => new MenuItem
                    {
                        Id = x.Page.Id,
                        Text = x.Title,
                        Url = "/" + x.Slug,
                        Enabled = true,
                        ParentId = x.Page.ParentId,
                        Position = x.Page.Order
                    });

                menuItems.AddRange(items);
            }

            menuItems = menuItems
                .OrderBy(x => x.Position)
                .ThenBy(x => x.Text)
                .ToList();

            ViewBag.MenuId = menuId;
            return View(templateViewName, menuItems);
        }

        [ChildActionOnly]
        [Route("menu")]
        public ActionResult Menu(string name, string templateViewName, bool filterByUrl = false)
        {
            string currentUrlSlug = filterByUrl ? Request.Url.LocalPath.TrimStart('/') : null;

            var pageVersionService = EngineContext.Current.Resolve<IPageVersionService>();

            // Check if it's a CMS page or not.
            if (currentUrlSlug != null && pageVersionService.Find(x => x.Slug == currentUrlSlug) == null)
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
        [Route("content-blocks-by-zone")]
        public ActionResult ContentBlocksByZone(string zoneName, bool renderAsWidgets = false, WidgetColumns widgetColumns = WidgetColumns.Default)
        {
            var contentBlockProviders = EngineContext.Current.ResolveAll<IContentBlockProvider>();

            var contentBlocks = contentBlockProviders
                .SelectMany(x => x.GetContentBlocks(zoneName, WorkContext.CurrentCultureCode))
                .ToList();

            ViewBag.RenderAsWidgets = renderAsWidgets;
            ViewBag.WidgetColumns = widgetColumns;

            var viewEngineResult = ViewEngines.Engines.FindView(ControllerContext, "ContentBlocksByZone", null);

            // If someone has provided a custom template (see LocationFormatProvider)
            if (viewEngineResult.View != null)
            {
                return View(contentBlocks);
            }

            return View("Kore.Web.ContentManagement.Views.Frontend.ContentBlocksByZone", contentBlocks);
        }

        [ChildActionOnly]
        [Route("entity-type-content-blocks-by-zone")]
        public ActionResult EntityTypeContentBlocksByZone(string zoneName, string entityType, object entityId, bool renderAsWidgets = false, WidgetColumns widgetColumns = WidgetColumns.Default)
        {
            var providers = EngineContext.Current.ResolveAll<IEntityTypeContentBlockProvider>();

            var contentBlocks = providers
                .SelectMany(x => x.GetContentBlocks(zoneName, entityType, entityId.ToString()))
                .ToList();

            ViewBag.RenderAsWidgets = renderAsWidgets;
            ViewBag.WidgetColumns = widgetColumns;

            var viewEngineResult = ViewEngines.Engines.FindView(ControllerContext, "ContentBlocksByZone", null);

            // If someone has provided a custom template (see LocationFormatProvider)
            if (viewEngineResult.View != null)
            {
                return View("ContentBlocksByZone", contentBlocks);
            }

            return View("Kore.Web.ContentManagement.Views.Frontend.ContentBlocksByZone", contentBlocks);
        }
    }
}