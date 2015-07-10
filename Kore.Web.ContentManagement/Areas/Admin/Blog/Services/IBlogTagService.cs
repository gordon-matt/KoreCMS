using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Services
{
    public interface IBlogTagService : IGenericDataService<BlogTag>
    {
    }

    public class BlogTagService : GenericDataService<BlogTag>, IBlogTagService
    {
        public BlogTagService(ICacheManager cacheManager, IRepository<BlogTag> repository)
            : base(cacheManager, repository)
        {
        }
    }
}