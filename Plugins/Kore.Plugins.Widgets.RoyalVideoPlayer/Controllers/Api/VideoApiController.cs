using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using Kore.Collections;
using Kore.EntityFramework.Data;
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

        //[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        public virtual async Task<IEnumerable<Video>> GetVideosByPlaylistId([FromODataUri] int playlistId, ODataQueryOptions<Video> options)
        {
            if (!CheckPermission(ReadPermission))
            {
                return Enumerable.Empty<Video>().AsQueryable();
            }

            List<int> videoIds = null;
            using (var connection = playlistVideoService.Value.OpenConnection())
            {
                videoIds = await connection.Query(x => x.PlaylistId == playlistId)
                    .Select(x => x.VideoId)
                    .ToListAsync();
            }

            var settings = new ODataValidationSettings()
            {
                AllowedQueryOptions = AllowedQueryOptions.All
            };
            options.Validate(settings);

            using (var connection = Service.OpenConnection())
            {
                var query = connection.Query(x => videoIds.Contains(x.Id));
                var results = options.ApplyTo(query);
                return await (results as IQueryable<Video>).ToHashSetAsync();
            }
        }

        [HttpPost]
        public virtual async Task<IHttpActionResult> AssignVideoToPlaylists(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return Unauthorized();
            }

            int videoId = (int)parameters["videoId"];
            var playlists = (IEnumerable<int>)parameters["playlists"];

            if (playlists.IsNullOrEmpty())
            {
                return BadRequest("playlists cannot be empty.");
            }

            var video = await Service.FindOneAsync(videoId);

            if (video == null)
            {
                return NotFound();
            }

            // Delete any from DB that are not in the list of IDs provided

            var toDelete = await playlistVideoService.Value.FindAsync(x =>
                x.VideoId == videoId &&
                !playlists.Contains(x.PlaylistId));

            await playlistVideoService.Value.DeleteAsync(toDelete);

            // Insert new entries from the list of IDs provided where those IDs are not already mapped.

            List<int> existingPlaylistIds = null;
            List<int> existingMappedIds = null;

            using (var connection = playlistService.Value.OpenConnection())
            {
                existingPlaylistIds = await connection.Query(x => playlists.Contains(x.Id))
                    .Select(x => x.Id)
                    .ToListAsync();
            }

            using (var connection = playlistVideoService.Value.OpenConnection())
            {
                existingMappedIds = await connection.Query(x => x.VideoId == videoId)
                    .Select(x => x.PlaylistId)
                    .ToListAsync();
            }

            var toInsert = playlists
                .Where(x =>
                    existingPlaylistIds.Contains(x) &&
                    !existingMappedIds.Contains(x))
                .Select(x => new PlaylistVideo
                {
                    PlaylistId = x,
                    VideoId = videoId
                });

            await playlistVideoService.Value.InsertAsync(toInsert);

            return Ok();
        }
    }
}