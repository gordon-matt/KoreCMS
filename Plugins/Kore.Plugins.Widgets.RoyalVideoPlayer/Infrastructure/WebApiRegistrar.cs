using System.Web.Http;
using System.Web.OData.Builder;
using Kore.Plugins.Widgets.RoyalVideoPlayer.Data.Domain;
using Kore.Web.Infrastructure;
using System.Web.OData.Extensions;

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

            config.MapODataServiceRoute("OData_FWD_RoyalVideoPlayer", "odata/fwd/royal-video-player", builder.GetEdmModel());
        }

        private static void RegisterPlaylistODataActions(ODataModelBuilder builder)
        {
            var getPlaylistsForVideoAction = builder.EntityType<Playlist>().Collection.Action("GetPlaylistsForVideo");
            getPlaylistsForVideoAction.Parameter<int>("videoId");
            getPlaylistsForVideoAction.Returns<IHttpActionResult>();
        }

        private static void RegisterVideoODataActions(ODataModelBuilder builder)
        {
            var assignVideoToPlaylistsAction = builder.EntityType<Video>().Collection.Action("AssignVideoToPlaylists");
            assignVideoToPlaylistsAction.Parameter<int>("videoId");
            assignVideoToPlaylistsAction.CollectionParameter<int>("playlists");
            assignVideoToPlaylistsAction.Returns<IHttpActionResult>();

            var getVideosByPlaylistIdFunction = builder.EntityType<Video>().Collection.Function("GetVideosByPlaylistId");
            getVideosByPlaylistIdFunction.Parameter<int>("playlistId");
            getVideosByPlaylistIdFunction.ReturnsCollectionFromEntitySet<Video>("VideoApi");
        }

        #endregion IWebApiRegistrar Members
    }
}