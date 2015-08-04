using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

namespace Kore.Plugins.Maintenance.Watchdog.Controllers
{
    [Authorize]
    [RouteArea(Constants.RouteArea)]
    [RoutePrefix("watchdog")]
    public class WatchdogController : KoreController
    {
        private readonly WatchdogSettings settings;

        public WatchdogController(WatchdogSettings settings)
        {
            this.settings = settings;
        }

        [Compress]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(WatchdogPermissions.Read))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Watchdog));
            ViewBag.Title = T(LocalizableStrings.ManageServices);

            ViewBag.AllowAddRemove = settings.AllowAddRemove;
            ViewBag.OnlyShowWatched = settings.OnlyShowWatched;

            return PartialView();
        }
    }
}