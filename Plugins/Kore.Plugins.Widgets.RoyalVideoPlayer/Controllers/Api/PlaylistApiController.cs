using Kore.Plugins.Widgets.RoyalVideoPlayer.Data.Domain;
using Kore.Plugins.Widgets.RoyalVideoPlayer.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer.Controllers.Api
{
    public class PlaylistApiController : GenericODataController<Playlist, int>
    {
        public PlaylistApiController(IPlaylistService service)
            : base(service)
        {
        }

        protected override int GetId(Playlist entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Playlist entity)
        {
        }

        protected override Permission ReadPermission
        {
            get { return Permissions.Read; }
        }

        protected override Permission WritePermission
        {
            get { return Permissions.Write; }
        }
    }
}