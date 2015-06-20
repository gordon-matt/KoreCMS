using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Widgets.RoyalVideoPlayer.Data.Domain;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer.Services
{
    public interface IPlaylistService : IGenericDataService<Playlist>
    {
    }

    public class PlaylistService : GenericDataService<Playlist>, IPlaylistService
    {
        public PlaylistService(ICacheManager cacheManager, IRepository<Playlist> repository)
            : base(cacheManager, repository)
        {
        }
    }
}