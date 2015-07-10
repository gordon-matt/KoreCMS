using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Services
{
    public interface IBlogCategoryService : IGenericDataService<BlogCategory>
    {
    }

    public class BlogCategoryService : GenericDataService<BlogCategory>, IBlogCategoryService
    {
        public BlogCategoryService(ICacheManager cacheManager, IRepository<BlogCategory> repository)
            : base(cacheManager, repository)
        {
        }
    }
}