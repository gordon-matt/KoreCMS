using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using Kore.Plugins.Widgets.RoyalVideoPlayer.Data.Domain;
using Kore.Plugins.Widgets.RoyalVideoPlayer.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer.Controllers.Api
{
    public class PlaylistApiController : GenericTenantODataController<Playlist, int>
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
        public virtual async Task<IHttpActionResult> GetPlaylistsForVideo(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return Unauthorized();
            }

            int videoId = (int)parameters["videoId"];

            var video = videoService.Value.FindOne(videoId);

            if (video == null)
            {
                return NotFound();
            }

            List<int> playlistIds = null;
            using (var connection = playlistVideoService.Value.OpenConnection())
            {
                playlistIds = await connection
                    .Query(x => x.VideoId == videoId)
                    .Select(x => x.PlaylistId)
                    .ToListAsync();
            }

            return Ok(playlistIds);
        }
    }
}