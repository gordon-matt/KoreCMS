using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Widgets.RevolutionSlider.Data.Domain;

namespace Kore.Plugins.Widgets.RevolutionSlider.Services
{
    public interface ISlideService : IGenericDataService<RevolutionSlide>
    {
    }

    public class SlideService : GenericDataService<RevolutionSlide>, ISlideService
    {
        public SlideService(ICacheManager cacheManager, IRepository<RevolutionSlide> repository)
            : base(cacheManager, repository)
        {
        }
    }
}