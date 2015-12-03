using System.Linq;
using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging.Services
{
    public interface IMessageTemplateService : IGenericDataService<Domain.MessageTemplate>
    {
        Domain.MessageTemplate Find(string name);
    }

    public class MessageTemplateService : GenericDataService<Domain.MessageTemplate>, IMessageTemplateService
    {
        public MessageTemplateService(ICacheManager cacheManager, IRepository<Domain.MessageTemplate> repository)
            : base(cacheManager, repository)
        {
        }

        public Domain.MessageTemplate Find(string name)
        {
            return FindOne(x => x.Name == name);
        }
    }
}