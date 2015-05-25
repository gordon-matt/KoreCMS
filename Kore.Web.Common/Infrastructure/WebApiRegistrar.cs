using System.Web.Http;
using System.Web.Http.OData.Builder;
using Kore.Web.Common.Areas.Admin.Regions.Domain;
using Kore.Web.Infrastructure;

namespace Kore.Web.Common.Infrastructure
{
    public class WebApiRegistrar : IWebApiRegistrar
    {
        #region IWebApiRegistrar Members

        public void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Region>("RegionApi");

            config.Routes.MapODataRoute("OData_Kore_Common", "odata/kore/common", builder.GetEdmModel());
        }

        #endregion IWebApiRegistrar Members
    }
}