using System.Web.Http;
using System.Web.Http.OData.Builder;
using Kore.Plugins.Widgets.RoyalVideoPlayer.Data.Domain;
using Kore.Web.Infrastructure;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer.Infrastructure
{
    public class WebApiRegistrar : IWebApiRegistrar
    {
        #region IWebApiRegistrar Members

        public void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Playlist>("PlaylistApi");
            builder.EntitySet<Video>("VideoApi");

            RegisterPlaylistODataActions(builder);
            RegisterVideoODataActions(builder);

            config.Routes.MapODataRoute("OData_FWD_RoyalVideoPlayer", "odata/fwd/royal-video-player", builder.GetEdmModel());
        }

        private static void RegisterPlaylistODataActions(ODataModelBuilder builder)
        {
            var getPlaylistsForVideoAction = builder.Entity<Playlist>().Collection.Action("GetPlaylistsForVideo");
            getPlaylistsForVideoAction.Parameter<int>("videoId");
            getPlaylistsForVideoAction.Returns<IHttpActionResult>();
        }

        private static void RegisterVideoODataActions(ODataModelBuilder builder)
        {
            var assignVideoToPlaylistsAction = builder.Entity<Video>().Collection.Action("AssignVideoToPlaylists");
            assignVideoToPlaylistsAction.Parameter<int>("videoId");
            assignVideoToPlaylistsAction.CollectionParameter<int>("playlists");
            assignVideoToPlaylistsAction.Returns<IHttpActionResult>();

            var getVideosByPlaylistIdAction = builder.Entity<Video>().Collection.Action("GetVideosByPlaylistId");
            getVideosByPlaylistIdAction.Parameter<int>("playlistId");
            getVideosByPlaylistIdAction.ReturnsCollectionFromEntitySet<Video>("VideoApi");
        }

        #endregion IWebApiRegistrar Members
    }
}