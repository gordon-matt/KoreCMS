using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Services
{
    public interface ICategoryService : IGenericDataService<Category>
    {
    }

    public class CategoryService : GenericDataService<Category>, ICategoryService
    {
        public CategoryService(ICacheManager cacheManager, IRepository<Category> repository)
            : base(cacheManager, repository)
        {
        }
    }
}