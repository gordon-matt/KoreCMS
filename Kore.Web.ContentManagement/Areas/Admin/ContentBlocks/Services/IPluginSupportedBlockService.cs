using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services
{
    public interface IPluginSupportedBlockService : IGenericDataService<PluginSupportedBlock>
    {
    }

    public class PluginSupportedBlockService : GenericDataService<PluginSupportedBlock>, IPluginSupportedBlockService
    {
        public PluginSupportedBlockService(
            ICacheManager cacheManager,
            IRepository<PluginSupportedBlock> repository)
            : base(cacheManager, repository)
        {
        }
    }
}