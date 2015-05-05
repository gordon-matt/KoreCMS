using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kore.Collections;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Blog;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Services;
using Kore.Web.ContentManagement.Areas.Admin.Pages;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
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

            string currentUrlSlug = Request.Url.ToString().RightOfLastIndexOf('/');

            if (currentUrlSlug == "blog")
            {
                breadcrumbs.Add(new Breadcrumb
                {
                    Text = blogSettings.Value.PageTitle
                });
                return View(templateViewName, breadcrumbs);
            }

            var pageService = EngineContext.Current.Resolve<IPageService>();

            var currentPage = pageService.Repository.Table.FirstOrDefault(y => y.Slug == currentUrlSlug);
            var query = pageService.Repository.Table.Where(x => x.IsEnabled).ToHashSet();

            if (currentPage != null)
            {
                var parentId = currentPage.ParentId;
                while (parentId != null)
                {
                    var page = query.FirstOrDefault(y => y.Id == parentId);

                    if (PageSecurityHelper.CheckUserHasAccessToPage(page, User))
                    {
                        breadcrumbs.Add(new Breadcrumb
                        {
                            Text = page.Name,
                            Url = "/" + page.Slug
                        });
                    }

                    parentId = page.ParentId;
                }

                breadcrumbs.Reverse();

                breadcrumbs.Add(new Breadcrumb
                {
                    Text = currentPage.Name
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
                .ToHashSet();

            var authorizedPages = pages.Where(x => PageSecurityHelper.CheckUserHasAccessToPage(x, User));

            var items = authorizedPages
                .Select(x => new MenuItem
            {
                Id = x.Id,
                Text = x.Name,
                Url = "/" + x.Slug,
                Enabled = true,
                ParentId = x.ParentId,
                Position = x.Order
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

            var pages = query.ToHashSet();

            var authorizedPages = pages.Where(x => PageSecurityHelper.CheckUserHasAccessToPage(x, User));

            var items = authorizedPages
                .Select(x => new MenuItem
                {
                    Id = x.Id,
                    Text = x.Name,
                    Url = "/" + x.Slug,
                    Enabled = true,
                    ParentId = x.ParentId,
                    Position = x.Order
                });

            menuItems.AddRange(items);

            if (PageSecurityHelper.CheckUserHasAccessToBlog(User) &&
                currentUrlSlug == string.Empty &&
                blogSettings.Value.ShowOnMenus)
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
        [Route("content-blocks-by-zone")]
        public ActionResult ContentBlocksByZone(string zoneName)
        {
            var contentBlockProviders = EngineContext.Current.ResolveAll<IContentBlockProvider>();
            var contentBlocks = contentBlockProviders.SelectMany(x => x.GetContentBlocks(zoneName, WorkContext.CurrentCultureCode)).ToList();
            return View("Kore.Web.ContentManagement.Views.Frontend.ContentBlocksByZone", contentBlocks);
        }
    }
}