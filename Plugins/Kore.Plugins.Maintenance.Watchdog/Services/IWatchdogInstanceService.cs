using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Maintenance.Watchdog.Data.Domain;

namespace Kore.Plugins.Maintenance.Watchdog.Services
{
    public interface IWatchdogInstanceService : IGenericDataService<WatchdogInstance>
    {
    }

    public class WatchdogInstanceService : GenericDataService<WatchdogInstance>, IWatchdogInstanceService
    {
        public WatchdogInstanceService(ICacheManager cacheManager, IRepository<WatchdogInstance> repository)
            : base(cacheManager, repository)
        {
        }
    }
}