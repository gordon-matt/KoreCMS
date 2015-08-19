using System.Web.Http;
using System.Web.OData.Builder;
using Kore.Plugins.Widgets.RevolutionSlider.Data.Domain;
using Kore.Web.Infrastructure;
using System.Web.OData.Extensions;
using Slider = Kore.Plugins.Widgets.RevolutionSlider.Data.Domain.RevolutionSlider;

namespace Kore.Plugins.Widgets.RevolutionSlider.Infrastructure
{
    public class WebApiRegistrar : IWebApiRegistrar
    {
        #region IWebApiRegistrar Members

        public void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Slider>("RevolutionSliderApi");
            builder.EntitySet<RevolutionSlide>("RevolutionSlideApi");
            builder.EntitySet<RevolutionLayer>("RevolutionLayerApi");

            config.MapODataServiceRoute("OData_Kore_RevolutionSlider", "odata/kore/revolution-slider", builder.GetEdmModel());
        }

        #endregion IWebApiRegistrar Members
    }
}