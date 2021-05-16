using System.Web.Http;
using Kore.Plugins.Widgets.FullCalendar.Data.Domain;
using Kore.Web.Infrastructure;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;

namespace Kore.Plugins.Widgets.FullCalendar.Infrastructure
{
    public class WebApiRegistrar : IWebApiRegistrar
    {
        #region IWebApiRegistrar Members

        public void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Calendar>("CalendarApi");
            builder.EntitySet<CalendarEvent>("CalendarEventApi");

            config.MapODataServiceRoute("OData_Kore_Plugin_FullCalendar", "odata/kore/plugins/full-calendar", builder.GetEdmModel());
        }

        #endregion IWebApiRegistrar Members
    }
}