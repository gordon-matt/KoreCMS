using System.Web.Http;
using System.Web.Http.OData.Builder;
using Kore.Plugins.Widgets.RevolutionSlider.Data.Domain;
using Kore.Web.Infrastructure;

namespace Kore.Plugins.Widgets.RevolutionSlider.Infrastructure
{
    public class WebApiRegistrar : IWebApiRegistrar
    {
        #region IWebApiRegistrar Members

        public void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Slider>("RevolutionSliderApi");
            builder.EntitySet<Slide>("RevolutionSlideApi");

            config.Routes.MapODataRoute("OData_Kore_RevolutionSlider", "odata/kore/revolution-slider", builder.GetEdmModel());
        }

        #endregion IWebApiRegistrar Members
    }
}