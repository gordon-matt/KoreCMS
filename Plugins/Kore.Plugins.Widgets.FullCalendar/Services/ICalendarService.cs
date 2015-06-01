using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Widgets.FullCalendar.Data.Domain;

namespace Kore.Plugins.Widgets.FullCalendar.Services
{
    public interface ICalendarService : IGenericDataService<Calendar>
    {
    }

    public class CalendarService : GenericDataService<Calendar>, ICalendarService
    {
        public CalendarService(ICacheManager cacheManager, IRepository<Calendar> repository)
            : base(cacheManager, repository)
        {
        }
    }
}