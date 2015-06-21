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
            //builder.EntitySet<PlaylistVideo>("PlaylistVideoApi");
            builder.EntitySet<Video>("VideoApi");

            RegisterPlaylistODataActions(builder);

            config.Routes.MapODataRoute("OData_FWD_RoyalVideoPlayer", "odata/fwd/royal-video-player", builder.GetEdmModel());
        }

        private static void RegisterPlaylistODataActions(ODataModelBuilder builder)
        {
            var updatePlaylistVideosAction = builder.Entity<Playlist>().Collection.Action("UpdatePlaylistVideos");
            updatePlaylistVideosAction.Parameter<int>("playlistId");
            updatePlaylistVideosAction.Parameter<string>("videoIds");
            updatePlaylistVideosAction.Returns<IHttpActionResult>();

            var getPlaylistVideosAction = builder.Entity<Playlist>().Collection.Action("GetPlaylistVideos");
            getPlaylistVideosAction.Parameter<int>("playlistId");
            getPlaylistVideosAction.Returns<IHttpActionResult>();
        }

        #endregion IWebApiRegistrar Members
    }
}