using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.Common.Areas.Admin.Regions.Domain;

namespace Kore.Web.Common.Areas.Admin.Regions.Services
{
    public interface IRegionSettingsService : IGenericDataService<RegionSettings>
    {
    }

    public class RegionSettingsService : GenericDataService<RegionSettings>, IRegionSettingsService
    {
        public RegionSettingsService(ICacheManager cacheManager, IRepository<RegionSettings> repository)
            : base(cacheManager, repository)
        {
        }
    }
}