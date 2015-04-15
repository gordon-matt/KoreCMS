using System.Web.Http;
using System.Web.Http.OData.Builder;
using Kore.Web.Infrastructure;

namespace Kore.Plugins.Widgets.FlexSlider.Infrastructure
{
    public class WebApiRegistrar : IWebApiRegistrar
    {
        #region IWebApiRegistrar Members

        public void Register(HttpConfiguration config)
        {
            //ODataModelBuilder builder = new ODataConventionModelBuilder();
            //builder.EntitySet<FlexSliderSitemapPageConfig>("FlexSliderXmlSitemap");

            //var getConfigAction = builder.Entity<FlexSliderSitemapPageConfig>().Collection.Action("GetConfig");
            //getConfigAction.ReturnsCollection<FlexSliderSitemapPageConfigModel>();

            //var setConfigAction = builder.Entity<FlexSliderSitemapPageConfig>().Collection.Action("SetConfig");
            //setConfigAction.Parameter<int>("id");
            //setConfigAction.Parameter<byte>("changeFrequency");
            //setConfigAction.Parameter<float>("priority");
            //setConfigAction.Returns<IHttpActionResult>();

            //var generateAction = builder.Entity<FlexSliderSitemapPageConfig>().Collection.Action("Generate");
            //generateAction.Returns<IHttpActionResult>();

            //config.Routes.MapODataRoute("OData_Kore_Plugin_FlexSlider", "odata/kore/plugins/google", builder.GetEdmModel());
        }

        #endregion IWebApiRegistrar Members
    }
}