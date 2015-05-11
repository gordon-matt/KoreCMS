using System.Web.Http;
using System.Web.Http.OData.Builder;
using Kore.Plugins.Widgets.Google.Data.Domain;
using Kore.Plugins.Widgets.Google.Models;
using Kore.Web.Infrastructure;

namespace Kore.Plugins.Widgets.Google.Infrastructure
{
    public class WebApiRegistrar : IWebApiRegistrar
    {
        #region IWebApiRegistrar Members

        public void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<GoogleSitemapPageConfig>("GoogleXmlSitemapApi");

            var getConfigAction = builder.Entity<GoogleSitemapPageConfig>().Collection.Action("GetConfig");
            getConfigAction.ReturnsCollection<GoogleSitemapPageConfigModel>();

            var setConfigAction = builder.Entity<GoogleSitemapPageConfig>().Collection.Action("SetConfig");
            setConfigAction.Parameter<int>("id");
            setConfigAction.Parameter<byte>("changeFrequency");
            setConfigAction.Parameter<float>("priority");
            setConfigAction.Returns<IHttpActionResult>();

            var generateAction = builder.Entity<GoogleSitemapPageConfig>().Collection.Action("Generate");
            generateAction.Returns<IHttpActionResult>();

            config.Routes.MapODataRoute("OData_Kore_Plugin_Google", "odata/kore/plugins/google", builder.GetEdmModel());
        }

        #endregion IWebApiRegistrar Members
    }
}