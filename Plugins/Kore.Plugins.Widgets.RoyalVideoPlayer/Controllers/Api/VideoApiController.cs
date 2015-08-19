using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.OData;
using System.Web.Http.Results;
using Kore.Collections;
using Kore.Plugins.Widgets.RoyalVideoPlayer.Data.Domain;
using Kore.Plugins.Widgets.RoyalVideoPlayer.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer.Controllers.Api
{
    public class VideoApiController : GenericODataController<Video, int>
    {
        private readonly Lazy<IPlaylistService> playlistService;
        private readonly Lazy<IPlaylistVideoService> playlistVideoService;

        public VideoApiController(
            IVideoService service,
            Lazy<IPlaylistService> playlistService,
            Lazy<IPlaylistVideoService> playlistVideoService)
            : base(service)
        {
            this.playlistService = playlistService;
            this.playlistVideoService = playlistVideoService;
        }

        protected override int GetId(Video entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Video entity)
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

        [EnableQuery]
        [HttpPost]
        public virtual IEnumerable<Video> GetVideosByPlaylistId(ODataActionParameters parameters)
        {
            if (!CheckPermission(ReadPermission))
            {
                return Enumerable.Empty<Video>();
            }

            int playlistId = (int)parameters["playlistId"];

            var videoIds = playlistVideoService.Value.Repository.Table
                .Where(x => x.PlaylistId == playlistId)
                .Select(x => x.VideoId)
                .ToList();

            var query = Service.Repository.Table.Where(x => videoIds.Contains(x.Id));

            // Since OData v3 doesn't seem to allow filter queries on action methods yet,
            //  we'll just have to make do with sending all the data to client and filtering & sorting there for this special case
            return query.ToHashSet();
        }

        [HttpPost]
        public virtual IHttpActionResult AssignVideoToPlaylists(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            int videoId = (int)parameters["videoId"];
            var playlists = (IEnumerable<int>)parameters["playlists"];

            if (playlists.IsNullOrEmpty())
            {
                return BadRequest("playlists cannot be empty.");
            }

            var video = Service.FindOne(videoId);

            if (video == null)
            {
                return NotFound();
            }

            // Delete any from DB that are not in the list of IDs provided

            var toDelete = playlistVideoService.Value.Find(x =>
                x.VideoId == videoId &&
                !playlists.Contains(x.PlaylistId));

            playlistVideoService.Value.Delete(toDelete);

            // Insert new entries from the list of IDs provided where those IDs are not already mapped.

            var existingPlaylistIds = playlistService.Value.Repository.Table
                .Where(x => playlists.Contains(x.Id))
                .Select(x => x.Id);

            var existingMappedIds = playlistVideoService.Value.Repository.Table
                .Where(x => x.VideoId == videoId)
                .Select(x => x.PlaylistId)
                .ToList();

            var toInsert = playlists
                .Where(x =>
                    existingPlaylistIds.Contains(x) &&
                    !existingMappedIds.Contains(x))
                .Select(x => new PlaylistVideo
                {
                    PlaylistId = x,
                    VideoId = videoId
                });

            playlistVideoService.Value.Insert(toInsert);

            return Ok();
        }
    }
}