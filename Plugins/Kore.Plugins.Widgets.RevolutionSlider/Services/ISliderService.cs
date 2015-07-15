using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Widgets.RevolutionSlider.Data.Domain;
using Slider = Kore.Plugins.Widgets.RevolutionSlider.Data.Domain.RevolutionSlider;

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