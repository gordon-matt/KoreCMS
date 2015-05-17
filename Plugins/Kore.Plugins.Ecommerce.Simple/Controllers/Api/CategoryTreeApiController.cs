using System.Collections.Generic;
using System.Linq;
using System.Web.Http.OData;
using Kore.Collections;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Ecommerce.Simple.Controllers.Api
{
    public class CategoryTreeApiController : ODataController
    {
        private readonly IRepository<Category> categoryRepository;

        public CategoryTreeApiController(IRepository<Category> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [EnableQuery]
        public IEnumerable<CategoryTreeItem> Get()
        {
            if (!CheckPermission(SimpleCommercePermissions.ReadCategories))
            {
                return Enumerable.Empty<CategoryTreeItem>();
            }

            var pages = categoryRepository.Table.ToHashSet();

            var hierarchy = pages
                .Where(x => x.ParentId == null)
                .OrderBy(x => x.Order)
                .ThenBy(x => x.Name)
                .Select(x => new CategoryTreeItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    //IsEnabled = x.IsEnabled,
                    SubCategories = GetSubPages(pages, x.Id).ToList()
                });

            return hierarchy.ToHashSet();
        }

        private static IEnumerable<CategoryTreeItem> GetSubPages(IEnumerable<Category> pages, int parentId)
        {
            return pages
                .Where(x => x.ParentId == parentId)
                .OrderBy(x => x.Order)
                .ThenBy(x => x.Name)
                .Select(x => new CategoryTreeItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    //IsEnabled = x.IsEnabled,
                    SubCategories = GetSubPages(pages, x.Id).ToList()
                });
        }

        protected static bool CheckPermission(Permission permission)
        {
            var authorizationService = EngineContext.Current.Resolve<IAuthorizationService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            return authorizationService.TryCheckAccess(permission, workContext.CurrentUser);
        }
    }

    public class CategoryTreeItem
    {
        public CategoryTreeItem()
        {
            SubCategories = new List<CategoryTreeItem>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        //public bool IsEnabled { get; set; }

        public List<CategoryTreeItem> SubCategories { get; set; }
    }
}