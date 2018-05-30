using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Newtonsoft.Json.Linq;

namespace Kore.Web.Areas.Admin.ScheduledTasks.Controllers
{
    [Authorize]
    [RouteArea(KoreWebConstants.Areas.ScheduledTasks)]
    public class ScheduledTaskController : KoreController
    {
        [Compress]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(KoreWebPermissions.ScheduledTasksRead))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.ScheduledTasks.Title));

            ViewBag.Title = T(KoreWebLocalizableStrings.ScheduledTasks.Title);
            ViewBag.SubTitle = T(KoreWebLocalizableStrings.ScheduledTasks.ManageScheduledTasks);

            return PartialView("Kore.Web.Areas.Admin.ScheduledTasks.Views.ScheduledTask.Index");
        }

        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            return Json(new
            {
                Edit = T(KoreWebLocalizableStrings.General.Edit).Text,
                ExecutedTaskSuccess = T(KoreWebLocalizableStrings.ScheduledTasks.ExecutedTaskSuccess).Text,
                ExecutedTaskError = T(KoreWebLocalizableStrings.ScheduledTasks.ExecutedTaskError).Text,
                GetRecordError = T(KoreWebLocalizableStrings.General.GetRecordError).Text,
                RunNow = T(KoreWebLocalizableStrings.ScheduledTasks.RunNow).Text,
                UpdateRecordError = T(KoreWebLocalizableStrings.General.UpdateRecordError).Text,
                UpdateRecordSuccess = T(KoreWebLocalizableStrings.General.UpdateRecordSuccess).Text,
                Columns = new
                {
                    Enabled = T(KoreWebLocalizableStrings.ScheduledTasks.Model.Enabled).Text,
                    LastEndUtc = T(KoreWebLocalizableStrings.ScheduledTasks.Model.LastEndUtc).Text,
                    LastStartUtc = T(KoreWebLocalizableStrings.ScheduledTasks.Model.LastStartUtc).Text,
                    LastSuccessUtc = T(KoreWebLocalizableStrings.ScheduledTasks.Model.LastSuccessUtc).Text,
                    Name = T(KoreWebLocalizableStrings.ScheduledTasks.Model.Name).Text,
                    Seconds = T(KoreWebLocalizableStrings.ScheduledTasks.Model.Seconds).Text,
                    StopOnError = T(KoreWebLocalizableStrings.ScheduledTasks.Model.StopOnError).Text,
                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}