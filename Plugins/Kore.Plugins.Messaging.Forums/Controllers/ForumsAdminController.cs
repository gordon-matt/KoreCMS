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
            return Json(new
            {
                Create = T(KoreWebLocalizableStrings.General.Create).Text,
                Delete = T(KoreWebLocalizableStrings.General.Delete).Text,
                DeleteRecordConfirm = T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord).Text,
                DeleteRecordError = T(KoreWebLocalizableStrings.General.DeleteRecordError).Text,
                DeleteRecordSuccess = T(KoreWebLocalizableStrings.General.DeleteRecordSuccess).Text,
                Edit = T(KoreWebLocalizableStrings.General.Edit).Text,
                Forums = T(LocalizableStrings.Forums).Text,
                GetRecordError = T(KoreWebLocalizableStrings.General.GetRecordError).Text,
                InsertRecordError = T(KoreWebLocalizableStrings.General.InsertRecordError).Text,
                InsertRecordSuccess = T(KoreWebLocalizableStrings.General.InsertRecordSuccess).Text,
                UpdateRecordError = T(KoreWebLocalizableStrings.General.UpdateRecordError).Text,
                UpdateRecordSuccess = T(KoreWebLocalizableStrings.General.UpdateRecordSuccess).Text,
                Columns = new
                {
                    Name = T(KoreWebLocalizableStrings.General.Name).Text,
                    DisplayOrder = T(KoreWebLocalizableStrings.General.Order).Text,
                    CreatedOnUtc = T(KoreWebLocalizableStrings.General.DateCreatedUtc).Text
                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}