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
        public ZoneService(ICacheManager cacheManager, IRepository<Zone> repository)
            : base(cacheManager, repository)
        {
        }
    }
}