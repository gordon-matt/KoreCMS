using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using Kore.Collections;
using Kore.Data;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers.Api
{
    [Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class PageTreeController : ODataController
    {
        private readonly IRepository<Page> pageRepository;

        public PageTreeController(IRepository<Page> pageRepository)
        {
            this.pageRepository = pageRepository;
        }

        [EnableQuery]
        public virtual IEnumerable<PageTreeItem> Get()
        {
            var pages = pageRepository.Table.ToHashSet();

            var hierarchy = pages
                .Where(x => x.ParentId == null)
                .OrderBy(x => x.Name)
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
                .Where(x => x.ParentId == parentId)
                .OrderBy(x => x.Name)
                .Select(x => new PageTreeItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsEnabled = x.IsEnabled,
                    SubPages = GetSubPages(pages, x.Id).ToList()
                });
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

    //public class PageTreeComplexItem
    //{
    //    public PageTreeComplexItem()
    //    {
    //        SubPages = new List<PageTreeComplexItem>();
    //    }

    //    public Guid Id { get; set; }

    //    public string Name { get; set; }

    //    public bool IsEnabled { get; set; }

    //    public List<PageTreeComplexItem> SubPages { get; set; }
    //}
}