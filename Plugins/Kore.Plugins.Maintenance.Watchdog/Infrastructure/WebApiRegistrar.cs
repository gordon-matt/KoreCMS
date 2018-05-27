using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Kore.Plugins.Maintenance.Watchdog.Data.Domain;
using Kore.Plugins.Maintenance.Watchdog.Models;
using Kore.Web.Infrastructure;

namespace Kore.Plugins.Maintenance.Watchdog.Infrastructure
{
    public class WebApiRegistrar : IWebApiRegistrar
    {
        #region IWebApiRegistrar Members

        public void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<WatchdogInstance>("WatchdogInstanceApi");
            //builder.EntitySet<ServiceInfoResult>("ServiceInfoResultApi");

            var stopServiceAction = builder.EntityType<WatchdogInstance>().Collection.Action("StopService");
            stopServiceAction.Parameter<int>("instanceId");
            stopServiceAction.Parameter<string>("name");
            stopServiceAction.Returns<ChangeStatusResult>();

            var startServiceAction = builder.EntityType<WatchdogInstance>().Collection.Action("StartService");
            startServiceAction.Parameter<int>("instanceId");
            startServiceAction.Parameter<string>("name");
            startServiceAction.Returns<ChangeStatusResult>();

            var restartServiceAction = builder.EntityType<WatchdogInstance>().Collection.Action("RestartService");
            restartServiceAction.Parameter<int>("instanceId");
            restartServiceAction.Parameter<string>("name");
            restartServiceAction.Returns<ChangeStatusResult>();

            var addServiceAction = builder.EntityType<WatchdogInstance>().Collection.Action("AddService");
            addServiceAction.Parameter<int>("instanceId");
            addServiceAction.Parameter<string>("name");
            addServiceAction.Returns<IHttpActionResult>();

            var removeServiceAction = builder.EntityType<WatchdogInstance>().Collection.Action("RemoveService");
            removeServiceAction.Parameter<int>("instanceId");
            removeServiceAction.Parameter<string>("name");
            removeServiceAction.Returns<IHttpActionResult>();

            var getServicesFunction = builder.EntityType<WatchdogInstance>().Collection.Function("GetServices");
            getServicesFunction.Parameter<int>("watchdogInstanceId");
            getServicesFunction.ReturnsCollection<ServiceInfoResult>();

            config.MapODataServiceRoute("OData_Kore_Plugin_Watchdog", "odata/kore/watchdog", builder.GetEdmModel());
        }

        #endregion IWebApiRegistrar Members
    }
}