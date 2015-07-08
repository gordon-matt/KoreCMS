using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Services
{
    public interface IPostService : IGenericDataService<Post>
    {
    }

    public class PostService : GenericDataService<Post>, IPostService
    {
        public PostService(ICacheManager cacheManager, IRepository<Post> repository)
            : base(cacheManager, repository)
        {
        }
    }
}