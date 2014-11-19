using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.Widgets.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets.Services
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
            return cacheManager.Get("Zones_GetById_" + string.Join("|", keyValues), () =>
            {
                return base.Repository.Find(keyValues);
            });
        }
    }
}