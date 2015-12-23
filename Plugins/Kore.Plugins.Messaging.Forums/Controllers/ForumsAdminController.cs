using System.Web.Mvc;
using Kore.Web;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Newtonsoft.Json.Linq;

namespace Kore.Plugins.Messaging.Forums.Controllers
{
    [Authorize]
    [RouteArea(Constants.RouteArea)]
    public class ForumsAdminController : KoreController
    {
        [Compress]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(ForumPermissions.ReadForums))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Forums), Url.Action("Index"));

            ViewBag.Title = T(LocalizableStrings.Forums);

            return PartialView();
        }

        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            string json = string.Format(
@"{{
    Create: '{0}',
    Delete: '{1}',
    DeleteRecordConfirm: '{2}',
    DeleteRecordError: '{3}',
    DeleteRecordSuccess: '{4}',
    Edit: '{5}',
    Forums: '{6}',
    GetRecordError: '{7}',
    InsertRecordError: '{8}',
    InsertRecordSuccess: '{9}',
    UpdateRecordError: '{10}',
    UpdateRecordSuccess: '{11}',
    Columns: {{
        Name: '{12}',
        DisplayOrder: '{13}',
        CreatedOnUtc: '{14}'
    }}
}}",
   T(KoreWebLocalizableStrings.General.Create),
   T(KoreWebLocalizableStrings.General.Delete),
   T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord),
   T(KoreWebLocalizableStrings.General.DeleteRecordError),
   T(KoreWebLocalizableStrings.General.DeleteRecordSuccess),
   T(KoreWebLocalizableStrings.General.Edit),
   T(LocalizableStrings.Forums),
   T(KoreWebLocalizableStrings.General.GetRecordError),
   T(KoreWebLocalizableStrings.General.InsertRecordError),
   T(KoreWebLocalizableStrings.General.InsertRecordSuccess),
   T(KoreWebLocalizableStrings.General.UpdateRecordError),
   T(KoreWebLocalizableStrings.General.UpdateRecordSuccess),
   T(KoreWebLocalizableStrings.General.Name),
   T(KoreWebLocalizableStrings.General.Order),
   T(KoreWebLocalizableStrings.General.DateCreatedUtc));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
        }
    }
}