using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Services
{
    public interface IBlogService : IGenericDataService<BlogEntry>
    {
    }

    public class BlogService : GenericDataService<BlogEntry>, IBlogService
    {
        public BlogService(ICacheManager cacheManager, IRepository<BlogEntry> repository)
            : base(cacheManager, repository)
        {
        }
    }
}