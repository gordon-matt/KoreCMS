using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Kore.Web.Security.Membership.Permissions;
using Newtonsoft.Json.Linq;

namespace Kore.Web.Areas.Admin.Log.Controllers
{
    [Authorize]
    [RouteArea(KoreWebConstants.Areas.Log)]
    public class LogController : KoreController
    {
        [Compress]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.Log.Title));
            ViewBag.Title = T(KoreWebLocalizableStrings.Log.Title);

            return PartialView("Kore.Web.Areas.Admin.Log.Views.Log.Index");
        }

        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            string json = string.Format(
@"{{
    ClearConfirm: '{0}',
    ClearError: '{1}',
    ClearSuccess: '{2}',
    Delete: '{3}',
    DeleteRecordConfirm: '{4}',
    DeleteRecordError: '{5}',
    DeleteRecordSuccess: '{6}',
    GetRecordError: '{7}',
    View: '{8}',

    Columns: {{
        EventLevel: '{9}',
        EventMessage: '{10}',
        EventDateTime: '{11}'
    }}
}}",
   T(KoreWebLocalizableStrings.Log.ClearConfirm),
   T(KoreWebLocalizableStrings.Log.ClearError),
   T(KoreWebLocalizableStrings.Log.ClearSuccess),
   T(KoreWebLocalizableStrings.General.Delete),
   T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord),
   T(KoreWebLocalizableStrings.General.DeleteRecordError),
   T(KoreWebLocalizableStrings.General.DeleteRecordSuccess),
   T(KoreWebLocalizableStrings.General.GetRecordError),
   T(KoreWebLocalizableStrings.General.View),
   T(KoreWebLocalizableStrings.Log.Model.EventLevel),
   T(KoreWebLocalizableStrings.Log.Model.EventMessage),
   T(KoreWebLocalizableStrings.Log.Model.EventDateTime));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
        }
    }
}