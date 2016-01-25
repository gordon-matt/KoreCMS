using System.Web.Http;
using System.Web.OData.Builder;
using Kore.Web.Common.Areas.Admin.Regions.Controllers.Api;
using Kore.Web.Common.Areas.Admin.Regions.Domain;
using Kore.Web.Infrastructure;
using System.Web.OData.Extensions;

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

            RegisterRegionODataActions(builder);
            RegisterRegionSettingsODataActions(builder);

            config.MapODataServiceRoute("OData_Kore_Common", "odata/kore/common", builder.GetEdmModel());
        }

        private static void RegisterRegionODataActions(ODataModelBuilder builder)
        {
            var getLocalizedActionFunction = builder.EntityType<Region>().Collection.Function("GetLocalized");
            getLocalizedActionFunction.Parameter<int>("id");
            getLocalizedActionFunction.Parameter<string>("cultureCode");
            getLocalizedActionFunction.ReturnsFromEntitySet<Region>("RegionApi");

            var saveLocalizedAction = builder.EntityType<Region>().Collection.Action("SaveLocalized");
            saveLocalizedAction.Parameter<string>("cultureCode");
            saveLocalizedAction.Parameter<Region>("entity");
            saveLocalizedAction.Returns<IHttpActionResult>();
        }

        private static void RegisterRegionSettingsODataActions(ODataModelBuilder builder)
        {
            var getSettingsAction = builder.EntityType<RegionSettings>().Collection.Action("GetSettings");
            getSettingsAction.Parameter<string>("settingsId");
            getSettingsAction.Parameter<int>("regionId");
            getSettingsAction.Returns<EdmRegionSettings>();

            var saveSettingsAction = builder.EntityType<RegionSettings>().Collection.Action("SaveSettings");
            saveSettingsAction.Parameter<string>("settingsId");
            saveSettingsAction.Parameter<int>("regionId");
            saveSettingsAction.Parameter<string>("fields");
            saveSettingsAction.Returns<IHttpActionResult>();
        }

        #endregion IWebApiRegistrar Members
    }
}