using System.Web.Http;
using System.Web.Http.OData.Builder;
using Kore.Web.Common.Areas.Admin.Regions.Controllers.Api;
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
            builder.EntitySet<RegionSettings>("RegionSettingsApi");

            RegisterRegionSettingsODataActions(builder);

            config.Routes.MapODataRoute("OData_Kore_Common", "odata/kore/common", builder.GetEdmModel());
        }

        private static void RegisterRegionSettingsODataActions(ODataModelBuilder builder)
        {
            var getSettingsAction = builder.Entity<RegionSettings>().Collection.Action("GetSettings");
            getSettingsAction.Parameter<string>("settingsId");
            getSettingsAction.Parameter<int>("regionId");
            getSettingsAction.Returns<EdmRegionSettings>();

            var saveSettingsAction = builder.Entity<RegionSettings>().Collection.Action("SaveSettings");
            saveSettingsAction.Parameter<string>("settingsId");
            saveSettingsAction.Parameter<int>("regionId");
            saveSettingsAction.Parameter<string>("fields");
            saveSettingsAction.Returns<IHttpActionResult>();
        }

        #endregion IWebApiRegistrar Members
    }
}