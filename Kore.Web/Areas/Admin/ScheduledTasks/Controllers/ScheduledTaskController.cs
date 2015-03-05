using System.Web.Mvc;
using Kore.Web.Mvc;

namespace Kore.Web.Areas.Admin.ScheduledTasks.Controllers
{
    [Authorize]
    [RouteArea(KoreWebConstants.Areas.ScheduledTasks)]
    public class ScheduledTaskController : KoreController
    {
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(ScheduledTasksPermissions.ManageScheduledTasks))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.ScheduledTasks.Title));

            ViewBag.Title = T(KoreWebLocalizableStrings.ScheduledTasks.Title);
            ViewBag.SubTitle = T(KoreWebLocalizableStrings.ScheduledTasks.ManageScheduledTasks);

            return View("Kore.Web.Areas.Admin.ScheduledTasks.Views.ScheduledTask.Index");
        }
    }
}