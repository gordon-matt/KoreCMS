using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.Results;
using Kore.Collections;
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
        public virtual IHttpActionResult UpdatePlaylistVideos(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            int playlistId = (int)parameters["playlistId"];
            string videoIds = (string)parameters["videoIds"];

            if (string.IsNullOrEmpty(videoIds))
            {
                return BadRequest("videosIds cannot be empty. Please send a pipe '|' delimited list of video IDs.");
            }

            var ids = new List<int>();
            try
            {
                ids = videoIds.Split('|').ToListOf<int>();
            }
            catch
            {
                return BadRequest("videosIds cannot be read. Please send a pipe '|' delimited list of video IDs.");
            }

            var playlist = Service.FindOne(playlistId);

            if (playlist == null)
            {
                return NotFound();
            }

            // Delete any from DB that are not in the list of IDs provided

            var toDelete = playlistVideoService.Value.Find(x =>
                x.PlaylistId == playlistId &&
                !ids.Contains(x.VideoId));

            playlistVideoService.Value.Delete(toDelete);

            // Insert new entries from the list of IDs provided where those IDs are not already mapped.

            var existingVideosIds = videoService.Value.Repository.Table
                .Where(x => ids.Contains(x.Id))
                .Select(x => x.Id);

            var existingMappedIds = playlistVideoService.Value.Repository.Table
                .Where(x => x.PlaylistId == playlistId)
                .Select(x => x.VideoId)
                .ToList();

            var toInsert = ids
                .Where(x =>
                    existingVideosIds.Contains(x) &&
                    !existingMappedIds.Contains(x))
                .Select(x => new PlaylistVideo
                {
                    PlaylistId = playlistId,
                    VideoId = x
                });

            playlistVideoService.Value.Insert(toInsert);

            return Ok();
        }

        [HttpPost]
        public virtual IHttpActionResult GetPlaylistVideos(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            int playlistId = (int)parameters["playlistId"];

            var playlist = Service.FindOne(playlistId);

            if (playlist == null)
            {
                return NotFound();
            }

            var videoIds = playlistVideoService.Value.Repository.Table
                .Where(x => x.PlaylistId == playlistId)
                .Select(x => x.VideoId)
                .ToList();

            return Ok(videoIds);
        }
    }
}