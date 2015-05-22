using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.OData;
using Kore.Collections;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class PageTreeApiController : ODataController
    {
        private readonly IRepository<Page> pageRepository;

        public PageTreeApiController(IRepository<Page> pageRepository)
        {
            this.pageRepository = pageRepository;
        }

        [EnableQuery]
        public IEnumerable<PageTreeItem> Get()
        {
            if (!CheckPermission(CmsPermissions.PagesRead))
            {
                return Enumerable.Empty<PageTreeItem>();
            }

            var pages = pageRepository.Table.ToHashSet();

            var hierarchy = pages
                .Where(x => x.ParentId == null && x.RefId == null)
                .OrderBy(x => x.Order)
                .ThenBy(x => x.Name)
                .Select(x => new PageTreeItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsEnabled = x.IsEnabled,
                    SubPages = GetSubPages(pages, x.Id).ToList()
                });

            return hierarchy.ToHashSet();
        }

        private static IEnumerable<PageTreeItem> GetSubPages(IEnumerable<Page> pages, Guid parentId)
        {
            return pages
                .Where(x => x.ParentId == parentId && x.RefId == null)
                .OrderBy(x => x.Order)
                .ThenBy(x => x.Name)
                .Select(x => new PageTreeItem
                {
                    Id = x.Id,
                    Name = x.Name,
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

        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        public List<PageTreeItem> SubPages { get; set; }
    }
}