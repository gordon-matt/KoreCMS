using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services
{
    public interface IZoneService : IGenericDataService<Zone>
    {
    }

    public class ZoneService : GenericDataService<Zone>, IZoneService
    {
        private readonly ICacheManager cacheManager;

        public ZoneService(IRepository<Zone> repository, ICacheManager cacheManager)
            : base(repository)
        {
            this.cacheManager = cacheManager;
        }

        public override Zone Find(params object[] keyValues)
        {
            string id = string.Join("|", keyValues);
            return cacheManager.Get(string.Join(Constants.CacheKeys.ContentZoneById, id), () =>
            {
                return base.Repository.Find(keyValues);
            });
        }
    }
}