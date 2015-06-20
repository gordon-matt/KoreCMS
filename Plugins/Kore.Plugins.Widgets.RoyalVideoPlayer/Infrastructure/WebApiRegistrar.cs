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

            config.Routes.MapODataRoute("OData_Kore_Plugin_RoyalVideoPlayer", "odata/kore/plugins/royal-video-player", builder.GetEdmModel());
        }

        #endregion IWebApiRegistrar Members
    }
}