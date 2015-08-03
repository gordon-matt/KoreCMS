using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Kore.Web.Security.Membership.Permissions;
using Newtonsoft.Json.Linq;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging.Controllers
{
    [Authorize]
    [RouteArea(CmsConstants.Areas.Messaging)]
    [RoutePrefix("queued-email")]
    public class QueuedEmailController : KoreController
    {
        [Compress]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Messaging.Title));
            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Messaging.QueuedEmails));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Messaging.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.Messaging.QueuedEmails);

            return PartialView("Kore.Web.ContentManagement.Areas.Admin.Messaging.Views.QueuedEmail.Index");
        }

        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            string json = string.Format(
@"{{
    Delete: '{0}',
    DeleteRecordConfirm: '{1}',
    DeleteRecordError: '{2}',
    DeleteRecordSuccess: '{3}',
    Columns: {{
        CreatedOnUtc: '{4}',
        SentOnUtc: '{5}',
        SentTries: '{6}',
        Subject: '{7}',
        ToAddress: '{8}'
    }}
}}",
   T(KoreWebLocalizableStrings.General.Delete),
   T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord),
   T(KoreWebLocalizableStrings.General.DeleteRecordError),
   T(KoreWebLocalizableStrings.General.DeleteRecordSuccess),
   T(KoreCmsLocalizableStrings.Messaging.QueuedEmailModel.CreatedOnUtc),
   T(KoreCmsLocalizableStrings.Messaging.QueuedEmailModel.SentOnUtc),
   T(KoreCmsLocalizableStrings.Messaging.QueuedEmailModel.SentTries),
   T(KoreCmsLocalizableStrings.Messaging.QueuedEmailModel.Subject),
   T(KoreCmsLocalizableStrings.Messaging.QueuedEmailModel.ToAddress));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
        }
    }
}