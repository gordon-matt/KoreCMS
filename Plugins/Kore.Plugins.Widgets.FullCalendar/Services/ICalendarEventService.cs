using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Widgets.FullCalendar.Data.Domain;

namespace Kore.Plugins.Widgets.FullCalendar.Services
{
    public interface ICalendarEventService : IGenericDataService<CalendarEvent>
    {
    }

    public class CalendarEventService : GenericDataService<CalendarEvent>, ICalendarEventService
    {
        public CalendarEventService(ICacheManager cacheManager, IRepository<CalendarEvent> repository)
            : base(cacheManager, repository)
        {
        }
    }
}