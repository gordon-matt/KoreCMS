using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.OData;
using System.Web.Http.Results;
using Kore.Plugins.Widgets.RoyalVideoPlayer.Data.Domain;
using Kore.Plugins.Widgets.RoyalVideoPlayer.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer.Controllers.Api
{
    public class PlaylistApiController : GenericODataController<Playlist, int>
    {
        private readonly Lazy<IVideoService> videoService;
        private readonly Lazy<IPlaylistVideoService> playlistVideoService;

        public PlaylistApiController(
            IPlaylistService service,
            Lazy<IVideoService> videoService,
            Lazy<IPlaylistVideoService> playlistVideoService)
            : base(service)
        {
            this.videoService = videoService;
            this.playlistVideoService = playlistVideoService;
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

        [HttpPost]
        public virtual IHttpActionResult GetPlaylistsForVideo(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            int videoId = (int)parameters["videoId"];

            var video = videoService.Value.FindOne(videoId);

            if (video == null)
            {
                return NotFound();
            }

            var playlistIds = playlistVideoService.Value.Query(x => x.VideoId == videoId)
                .Select(x => x.PlaylistId)
                .ToList();

            return Ok(playlistIds);
        }
    }
}