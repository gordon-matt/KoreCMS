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

        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            string json = string.Format(
@"{{
    ExecutedTaskSuccess: '{0}',
    ExecutedTaskError: '{1}',

    Columns: {{
        Enabled: '{2}',
        LastEndUtc: '{3}',
        LastStartUtc: '{4}',
        LastSuccessUtc: '{5}',
        Name: '{6}',
        Seconds: '{7}',
        StopOnError: '{8}',
    }}
}}",
   T(KoreWebLocalizableStrings.ScheduledTasks.ExecutedTaskSuccess),
   T(KoreWebLocalizableStrings.ScheduledTasks.ExecutedTaskError),
   T(KoreWebLocalizableStrings.ScheduledTasks.Model.Enabled),
   T(KoreWebLocalizableStrings.ScheduledTasks.Model.LastEndUtc),
   T(KoreWebLocalizableStrings.ScheduledTasks.Model.LastStartUtc),
   T(KoreWebLocalizableStrings.ScheduledTasks.Model.LastSuccessUtc),
   T(KoreWebLocalizableStrings.ScheduledTasks.Model.Name),
   T(KoreWebLocalizableStrings.ScheduledTasks.Model.Seconds),
   T(KoreWebLocalizableStrings.ScheduledTasks.Model.StopOnError));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
        }
    }
}