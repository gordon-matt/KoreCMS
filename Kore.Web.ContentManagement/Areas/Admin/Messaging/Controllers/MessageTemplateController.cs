using System.Threading.Tasks;
using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Kore.Web.Security.Membership.Permissions;
using Newtonsoft.Json.Linq;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging.Controllers
{
    [Authorize]
    [RouteArea(CmsConstants.Areas.Messaging)]
    [RoutePrefix("templates")]
    public class MessageTemplateController : KoreController
    {
        [Compress]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(CmsPermissions.MessageTemplatesRead))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Messaging.Title));
            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Messaging.MessageTemplates));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Messaging.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.Messaging.MessageTemplates);

            return PartialView("Kore.Web.ContentManagement.Areas.Admin.Messaging.Views.MessageTemplate.Index");
        }

        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            return Json(new
            {
                Create = T(KoreWebLocalizableStrings.General.Create).Text,
                Delete = T(KoreWebLocalizableStrings.General.Delete).Text,
                DeleteRecordConfirm = T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord).Text,
                DeleteRecordError = T(KoreWebLocalizableStrings.General.DeleteRecordError).Text,
                DeleteRecordSuccess = T(KoreWebLocalizableStrings.General.DeleteRecordSuccess).Text,
                Edit = T(KoreWebLocalizableStrings.General.Edit).Text,
                GetRecordError = T(KoreWebLocalizableStrings.General.GetRecordError).Text,
                GetTokensError = T(KoreCmsLocalizableStrings.Messaging.GetTokensError).Text,
                InsertRecordError = T(KoreWebLocalizableStrings.General.InsertRecordError).Text,
                InsertRecordSuccess = T(KoreWebLocalizableStrings.General.InsertRecordSuccess).Text,
                Toggle = T(KoreWebLocalizableStrings.General.Toggle).Text,
                UpdateRecordError = T(KoreWebLocalizableStrings.General.UpdateRecordError).Text,
                UpdateRecordSuccess = T(KoreWebLocalizableStrings.General.UpdateRecordSuccess).Text,
                Columns = new
                {
                    Name = T(KoreCmsLocalizableStrings.Messaging.MessageTemplateModel.Name).Text,
                    Subject = T(KoreCmsLocalizableStrings.Messaging.MessageTemplateModel.Subject).Text,
                    Enabled = T(KoreCmsLocalizableStrings.Messaging.MessageTemplateModel.Enabled).Text
                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}