using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.OData;
using Kore.Collections;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers.Api
{
    public class PageTreeApiController : ODataController
    {
        private readonly IPageService service;

        public PageTreeApiController(IPageService service)
        {
            this.service = service;
        }

        [EnableQuery]
        public IEnumerable<PageTreeItem> Get()
        {
            if (!CheckPermission(CmsPermissions.PagesRead))
            {
                return Enumerable.Empty<PageTreeItem>();
            }

            var pages = service.Find();

            var hierarchy = pages
                .Where(x => x.ParentId == null)
                .OrderBy(x => x.Order)
                .ThenBy(x => x.Name)
                .Select(x => new PageTreeItem
                {
                    Id = x.Id,
                    Title = x.Name,
                    IsEnabled = x.IsEnabled,
                    SubPages = GetSubPages(pages, x.Id).ToList()
                });

            return hierarchy.ToHashSet();
        }

        private static IEnumerable<PageTreeItem> GetSubPages(IEnumerable<Page> pages, Guid parentId)
        {
            return pages
                .Where(x => x.ParentId == parentId)
                .OrderBy(x => x.Order)
                .ThenBy(x => x.Name)
                .Select(x => new PageTreeItem
                {
                    Id = x.Id,
                    Title = x.Name,
                    IsEnabled = x.IsEnabled,
                    SubPages = GetSubPages(pages, x.Id).ToList()
                });
        }

        protected static bool CheckPermission(Permission permission)
        {
            var authorizationService = EngineContext.Current.Resolve<IAuthorizationService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            return authorizationService.TryCheckAccess(permission, workContext.CurrentUser);
        }
    }

    public class PageTreeItem
    {
        public PageTreeItem()
        {
            SubPages = new List<PageTreeItem>();
        }

        public Guid Id { get; set; }

        public string Title { get; set; }

        public bool IsEnabled { get; set; }

        public List<PageTreeItem> SubPages { get; set; }
    }
}