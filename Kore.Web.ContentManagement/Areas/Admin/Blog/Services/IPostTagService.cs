using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Services
{
    public interface IPostTagService : IGenericDataService<PostTag>
    {
    }

    public class PostTagService : GenericDataService<PostTag>, IPostTagService
    {
        public PostTagService(ICacheManager cacheManager, IRepository<PostTag> repository)
            : base(cacheManager, repository)
        {
        }
    }
}