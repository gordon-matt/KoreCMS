using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.Messaging.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging.Services
{
    public interface IQueuedEmailService : IGenericDataService<QueuedEmail>
    {
    }

    public class QueuedEmailService : GenericDataService<QueuedEmail>, IQueuedEmailService
    {
        public QueuedEmailService(ICacheManager cacheManager, IRepository<QueuedEmail> repository)
            : base(cacheManager, repository)
        {
        }
    }
}