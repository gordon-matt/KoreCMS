using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

namespace Kore.Web.Areas.Admin.ScheduledTasks.Controllers
{
    [Authorize]
    [RouteArea(KoreWebConstants.Areas.ScheduledTasks)]
    public class ScheduledTaskController : KoreController
    {
        [Compress]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(ScheduledTasksPermissions.ReadScheduledTasks))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.ScheduledTasks.Title));

            ViewBag.Title = T(KoreWebLocalizableStrings.ScheduledTasks.Title);
            ViewBag.SubTitle = T(KoreWebLocalizableStrings.ScheduledTasks.ManageScheduledTasks);

            return PartialView("Kore.Web.Areas.Admin.ScheduledTasks.Views.ScheduledTask.Index");
        }
    }
}