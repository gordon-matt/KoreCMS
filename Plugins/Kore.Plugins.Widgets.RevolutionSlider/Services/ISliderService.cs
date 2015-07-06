using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Widgets.RevolutionSlider.Data.Domain;

namespace Kore.Plugins.Widgets.RevolutionSlider.Services
{
    public interface ISliderService : IGenericDataService<Slider>
    {
    }

    public class SliderService : GenericDataService<Slider>, ISliderService
    {
        public SliderService(ICacheManager cacheManager, IRepository<Slider> repository)
            : base(cacheManager, repository)
        {
        }
    }
}