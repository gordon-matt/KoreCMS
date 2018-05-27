using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging.Controllers
{
    [Authorize]
    [RouteArea(CmsConstants.Areas.Messaging)]
    [RoutePrefix("queued-email")]
    public class QueuedEmailController : KoreController
    {
        [Compress]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(CmsPermissions.QueuedEmailsRead))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Messaging.Title));
            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Messaging.QueuedEmails));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Messaging.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.Messaging.QueuedEmails);

            return PartialView("Kore.Web.ContentManagement.Areas.Admin.Messaging.Views.QueuedEmail.Index");
        }

        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            return Json(new
            {
                Delete = T(KoreWebLocalizableStrings.General.Delete).Text,
                DeleteRecordConfirm = T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord).Text,
                DeleteRecordError = T(KoreWebLocalizableStrings.General.DeleteRecordError).Text,
                DeleteRecordSuccess = T(KoreWebLocalizableStrings.General.DeleteRecordSuccess).Text,
                Columns = new
                {
                    CreatedOnUtc = T(KoreCmsLocalizableStrings.Messaging.QueuedEmailModel.CreatedOnUtc).Text,
                    SentOnUtc = T(KoreCmsLocalizableStrings.Messaging.QueuedEmailModel.SentOnUtc).Text,
                    SentTries = T(KoreCmsLocalizableStrings.Messaging.QueuedEmailModel.SentTries).Text,
                    Subject = T(KoreCmsLocalizableStrings.Messaging.QueuedEmailModel.Subject).Text,
                    ToAddress = T(KoreCmsLocalizableStrings.Messaging.QueuedEmailModel.ToAddress).Text
                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}