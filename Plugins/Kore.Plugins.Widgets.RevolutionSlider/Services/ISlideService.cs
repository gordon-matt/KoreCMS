using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Widgets.RevolutionSlider.Data.Domain;

namespace Kore.Plugins.Widgets.RevolutionSlider.Services
{
    public interface ISlideService : IGenericDataService<Slide>
    {
    }

    public class SlideService : GenericDataService<Slide>, ISlideService
    {
        public SlideService(ICacheManager cacheManager, IRepository<Slide> repository)
            : base(cacheManager, repository)
        {
        }
    }
}