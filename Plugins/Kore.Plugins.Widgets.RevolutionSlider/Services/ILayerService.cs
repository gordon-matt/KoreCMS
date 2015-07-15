using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Widgets.RevolutionSlider.Data.Domain;

namespace Kore.Plugins.Widgets.RevolutionSlider.Services
{
    public interface ILayerService : IGenericDataService<RevolutionLayer>
    {
    }

    public class LayerService : GenericDataService<RevolutionLayer>, ILayerService
    {
        public LayerService(ICacheManager cacheManager, IRepository<RevolutionLayer> repository)
            : base(cacheManager, repository)
        {
        }
    }
}