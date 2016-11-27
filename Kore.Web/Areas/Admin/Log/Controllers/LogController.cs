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
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(LogPermissions.ReadLog))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.Log.Title));
            ViewBag.Title = T(KoreWebLocalizableStrings.Log.Title);

            return PartialView("Kore.Web.Areas.Admin.Log.Views.Log.Index");
        }

        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            return Json(new
            {
                ClearConfirm = T(KoreWebLocalizableStrings.Log.ClearConfirm).Text,
                ClearError = T(KoreWebLocalizableStrings.Log.ClearError).Text,
                ClearSuccess = T(KoreWebLocalizableStrings.Log.ClearSuccess).Text,
                Delete = T(KoreWebLocalizableStrings.General.Delete).Text,
                DeleteRecordConfirm = T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord).Text,
                DeleteRecordError = T(KoreWebLocalizableStrings.General.DeleteRecordError).Text,
                DeleteRecordSuccess = T(KoreWebLocalizableStrings.General.DeleteRecordSuccess).Text,
                GetRecordError = T(KoreWebLocalizableStrings.General.GetRecordError).Text,
                View = T(KoreWebLocalizableStrings.General.View).Text,
                Columns = new
                {
                    EventLevel = T(KoreWebLocalizableStrings.Log.Model.EventLevel).Text,
                    EventMessage = T(KoreWebLocalizableStrings.Log.Model.EventMessage).Text,
                    EventDateTime = T(KoreWebLocalizableStrings.Log.Model.EventDateTime).Text
                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}