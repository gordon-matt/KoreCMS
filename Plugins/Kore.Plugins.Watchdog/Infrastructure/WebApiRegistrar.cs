using System.Web.Http;
using System.Web.Http.OData.Builder;
using Kore.Plugins.Watchdog.Data.Domain;
using Kore.Plugins.Watchdog.Models;
using Kore.Web.Infrastructure;

namespace Kore.Plugins.Watchdog.Infrastructure
{
    public class WebApiRegistrar : IWebApiRegistrar
    {
        #region IWebApiRegistrar Members

        public void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<WatchdogInstance>("WatchdogInstanceApi");

            var stopServiceAction = builder.Entity<WatchdogInstance>().Collection.Action("StopService");
            stopServiceAction.Parameter<int>("instanceId");
            stopServiceAction.Parameter<string>("name");
            stopServiceAction.Returns<ChangeStatusResult>();

            var startServiceAction = builder.Entity<WatchdogInstance>().Collection.Action("StartService");
            startServiceAction.Parameter<int>("instanceId");
            startServiceAction.Parameter<string>("name");
            startServiceAction.Returns<ChangeStatusResult>();

            var restartServiceAction = builder.Entity<WatchdogInstance>().Collection.Action("RestartService");
            restartServiceAction.Parameter<int>("instanceId");
            restartServiceAction.Parameter<string>("name");
            restartServiceAction.Returns<ChangeStatusResult>();

            var addServiceAction = builder.Entity<WatchdogInstance>().Collection.Action("AddService");
            addServiceAction.Parameter<int>("instanceId");
            addServiceAction.Parameter<string>("name");
            addServiceAction.Returns<IHttpActionResult>();

            var removeServiceAction = builder.Entity<WatchdogInstance>().Collection.Action("RemoveService");
            removeServiceAction.Parameter<int>("instanceId");
            removeServiceAction.Parameter<string>("name");
            removeServiceAction.Returns<IHttpActionResult>();

            var getServicesAction = builder.Entity<WatchdogInstance>().Collection.Action("GetServices");
            getServicesAction.Parameter<int>("watchdogInstanceId");
            getServicesAction.ReturnsCollection<ServiceInfoResult>();

            config.Routes.MapODataRoute("OData_Kore_Plugin_Watchdog", "odata/kore/watchdog", builder.GetEdmModel());
        }

        #endregion IWebApiRegistrar Members
    }
}