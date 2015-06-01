using Kore.Plugins.Widgets.FullCalendar.Data.Domain;
using Kore.Plugins.Widgets.FullCalendar.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Widgets.FullCalendar.Controllers.Api
{
    public class CalendarEventApiController : GenericODataController<CalendarEvent, int>
    {
        public CalendarEventApiController(ICalendarEventService service)
            : base(service)
        {
        }

        protected override int GetId(CalendarEvent entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(CalendarEvent entity)
        {
        }

        protected override Permission ReadPermission
        {
            get { return FullCalendarPermissions.ReadCalendar; }
        }

        protected override Permission WritePermission
        {
            get { return FullCalendarPermissions.WriteCalendar; }
        }
    }
}