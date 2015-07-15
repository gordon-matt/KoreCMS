using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Widgets.RevolutionSlider.Data.Domain;

namespace Kore.Plugins.Widgets.RevolutionSlider.Services
{
    public interface ISliderService : IGenericDataService<RevolutionSlider>
    {
    }

    public class SliderService : GenericDataService<RevolutionSlider>, ISliderService
    {
        public SliderService(ICacheManager cacheManager, IRepository<RevolutionSlider> repository)
            : base(cacheManager, repository)
        {
        }
    }
}