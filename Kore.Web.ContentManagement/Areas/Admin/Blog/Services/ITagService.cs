using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Services
{
    public interface ITagService : IGenericDataService<Tag>
    {
    }

    public class TagService : GenericDataService<Tag>, ITagService
    {
        public TagService(ICacheManager cacheManager, IRepository<Tag> repository)
            : base(cacheManager, repository)
        {
        }
    }
}