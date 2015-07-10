using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Services
{
    public interface IBlogPostService : IGenericDataService<BlogPost>
    {
    }

    public class BlogPostService : GenericDataService<BlogPost>, IBlogPostService
    {
        public BlogPostService(ICacheManager cacheManager, IRepository<BlogPost> repository)
            : base(cacheManager, repository)
        {
        }
    }
}