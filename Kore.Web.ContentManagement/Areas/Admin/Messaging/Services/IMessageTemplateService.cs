using System.Linq;
using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;

namespace Kore.Web.ContentManagement.Messaging.Services
{
    public interface IMessageTemplateService : IGenericDataService<Domain.MessageTemplate>
    {
        Domain.MessageTemplate GetTemplate(string name);
    }

    public class MessageTemplateService : GenericDataService<Domain.MessageTemplate>, IMessageTemplateService
    {
        private readonly ICacheManager cacheManager;

        public MessageTemplateService(IRepository<Domain.MessageTemplate> repository, ICacheManager cacheManager)
            : base(repository)
        {
            this.cacheManager = cacheManager;
        }

        public Domain.MessageTemplate GetTemplate(string name)
        {
            return cacheManager.Get("GetTemplate_ByName_" + name, () =>
            {
                return Repository.Table.FirstOrDefault(x => x.Name == name);
            });
        }
    }
}