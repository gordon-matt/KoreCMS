using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services
{
    public interface IEntityTypeContentBlockService : IGenericDataService<EntityTypeContentBlock>
    {
    }

    public class EntityTypeContentBlockService : GenericDataService<EntityTypeContentBlock>, IEntityTypeContentBlockService
    {
        public EntityTypeContentBlockService(
            ICacheManager cacheManager,
            IRepository<EntityTypeContentBlock> repository)
            : base(cacheManager, repository)
        {
        }
    }
}