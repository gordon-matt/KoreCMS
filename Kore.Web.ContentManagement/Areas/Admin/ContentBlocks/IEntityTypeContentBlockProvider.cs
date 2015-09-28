using System.Collections.Generic;
using System.Linq;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks
{
    public interface IEntityTypeContentBlockProvider
    {
        IEnumerable<IContentBlock> GetContentBlocks(string zoneName, string entityType, string entityId);
    }

    public class DefaultEntityTypeContentBlockProvider : IEntityTypeContentBlockProvider
    {
        private readonly IEntityTypeContentBlockService entityTypeContentBlockService;

        public DefaultEntityTypeContentBlockProvider(IEntityTypeContentBlockService entityTypeContentBlockService)
        {
            this.entityTypeContentBlockService = entityTypeContentBlockService;
        }

        public virtual IEnumerable<IContentBlock> GetContentBlocks(string zoneName, string entityType, string entityId)
        {
            var workContext = EngineContext.Current.Resolve<IWebWorkContext>();

            var contentBlocks = entityTypeContentBlockService.GetContentBlocks(entityType, entityId, zoneName, workContext.CurrentCultureCode);
            return contentBlocks.ToList();
        }
    }
}