using System.Web.Mvc;
using Kore.Web;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

namespace Kore.Plugins.Widgets.FullCalendar.Controllers
{
    [Authorize]
    [RouteArea(Constants.RouteArea)]
    public class CalendarController : KoreController
    {
        [Compress]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(FullCalendarPermissions.ReadCalendar))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.Plugins.Title));
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.FullCalendar));

            ViewBag.Title = T(LocalizableStrings.FullCalendar);

            return PartialView();
        }
    }
}