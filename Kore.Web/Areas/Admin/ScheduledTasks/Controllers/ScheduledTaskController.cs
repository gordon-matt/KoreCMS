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
            if (!CheckPermission(ScheduledTasksPermissions.ReadScheduledTasks))
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
            string json = string.Format(
@"{{
    Edit: '{0}',
    ExecutedTaskSuccess: '{1}',
    ExecutedTaskError: '{2}',
    GetRecordError: '{3}',
    RunNow: '{4}',
    UpdateRecordError: '{5}',
    UpdateRecordSuccess: '{6}',

    Columns: {{
        Enabled: '{7}',
        LastEndUtc: '{8}',
        LastStartUtc: '{9}',
        LastSuccessUtc: '{10}',
        Name: '{11}',
        Seconds: '{12}',
        StopOnError: '{13}',
    }}
}}",
   T(KoreWebLocalizableStrings.General.Edit),
   T(KoreWebLocalizableStrings.ScheduledTasks.ExecutedTaskSuccess),
   T(KoreWebLocalizableStrings.ScheduledTasks.ExecutedTaskError),
   T(KoreWebLocalizableStrings.General.GetRecordError),
   T(KoreWebLocalizableStrings.ScheduledTasks.RunNow),
   T(KoreWebLocalizableStrings.General.UpdateRecordError),
   T(KoreWebLocalizableStrings.General.UpdateRecordSuccess),
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