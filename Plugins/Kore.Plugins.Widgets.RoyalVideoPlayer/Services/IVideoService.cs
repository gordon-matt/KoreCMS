using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Widgets.RoyalVideoPlayer.Data.Domain;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer.Services
{
    public interface IVideoService : IGenericDataService<Video>
    {
    }

    public class VideoService : GenericDataService<Video>, IVideoService
    {
        public VideoService(ICacheManager cacheManager, IRepository<Video> repository)
            : base(cacheManager, repository)
        {
        }
    }
}