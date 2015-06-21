using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Widgets.RoyalVideoPlayer.Data.Domain;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer.Services
{
    public interface IPlaylistVideoService : IGenericDataService<PlaylistVideo>
    {
    }

    public class PlaylistVideoService : GenericDataService<PlaylistVideo>, IPlaylistVideoService
    {
        public PlaylistVideoService(ICacheManager cacheManager, IRepository<PlaylistVideo> repository)
            : base(cacheManager, repository)
        {
        }
    }
}