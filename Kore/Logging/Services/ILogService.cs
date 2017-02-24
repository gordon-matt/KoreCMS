using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Logging.Domain;

namespace Kore.Logging.Services
{
    public interface ILogService : IGenericDataService<LogEntry>
    {
    }

    public class LogService : GenericDataService<LogEntry>, ILogService
    {
        public LogService(ICacheManager cacheManager, IRepository<LogEntry> repository)
            : base(cacheManager, repository)
        {
        }
    }
}