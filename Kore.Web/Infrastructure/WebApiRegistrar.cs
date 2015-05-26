using System.Web.Http;
using System.Web.Http.OData.Builder;
using Kore.Configuration.Domain;
using Kore.Logging.Domain;
using Kore.Tasks.Domain;
using Kore.Web.Areas.Admin.Configuration.Models;
using Kore.Web.Areas.Admin.Plugins.Models;

namespace Kore.Web.Infrastructure
{
    public class WebApiRegistrar : IWebApiRegistrar
    {
        #region IWebApiRegistrar Members

        public void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<LogEntry>("LogApi");
            builder.EntitySet<EdmPluginDescriptor>("PluginApi");
            builder.EntitySet<ScheduledTask>("ScheduledTaskApi");
            builder.EntitySet<Setting>("SettingsApi");
            builder.EntitySet<EdmThemeConfiguration>("ThemeApi");

            RegisterPluginODataActions(builder);
            RegisterScheduledTaskODataActions(builder);
            RegisterThemeODataActions(builder);

            config.Routes.MapODataRoute("OData_Kore_Web", "odata/kore/web", builder.GetEdmModel());
        }

        #endregion IWebApiRegistrar Members

        private static void RegisterPluginODataActions(ODataModelBuilder builder)
        {
            var installAction = builder.Entity<EdmPluginDescriptor>().Collection.Action("Install");
            installAction.Parameter<string>("systemName");
            installAction.Returns<IHttpActionResult>();

            var uninstallAction = builder.Entity<EdmPluginDescriptor>().Collection.Action("Uninstall");
            uninstallAction.Parameter<string>("systemName");
            uninstallAction.Returns<IHttpActionResult>();
        }

        private static void RegisterScheduledTaskODataActions(ODataModelBuilder builder)
        {
            var runNowAction = builder.Entity<ScheduledTask>().Collection.Action("RunNow");
            runNowAction.Parameter<int>("taskId");
            runNowAction.Returns<IHttpActionResult>();
        }

        private static void RegisterThemeODataActions(ODataModelBuilder builder)
        {
            var setDesktopThemeAction = builder.Entity<EdmThemeConfiguration>().Collection.Action("SetDesktopTheme");
            setDesktopThemeAction.Parameter<string>("themeName");
            setDesktopThemeAction.Returns<IHttpActionResult>();

            var setMobileThemeAction = builder.Entity<EdmThemeConfiguration>().Collection.Action("SetMobileTheme");
            setMobileThemeAction.Parameter<string>("themeName");
            setMobileThemeAction.Returns<IHttpActionResult>();
        }
    }
}