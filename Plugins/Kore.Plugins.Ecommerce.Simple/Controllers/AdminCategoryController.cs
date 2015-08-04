using System.Web.Mvc;
using Kore.Web;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Newtonsoft.Json.Linq;

namespace Kore.Plugins.Ecommerce.Simple.Controllers
{
    [Authorize]
    [RouteArea(Constants.RouteArea)]
    [RoutePrefix("categories")]
    public class AdminCategoryController : KoreController
    {
        [Compress]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(SimpleCommercePermissions.ReadCategories))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Store), Url.Action("Index", "AdminHome"));
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Categories));

            ViewBag.Title = T(LocalizableStrings.Store);
            ViewBag.SubTitle = T(LocalizableStrings.Categories);

            return PartialView();
        }

        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            string json = string.Format(
@"{{
    CircularRelationshipError: '{0}',
    Create: '{1}',
    Delete: '{2}',
    DeleteRecordConfirm: '{3}',
    DeleteRecordError: '{4}',
    DeleteRecordSuccess: '{5}',
    Edit: '{6}',
    GetRecordError: '{7}',
    InsertRecordError: '{8}',
    InsertRecordSuccess: '{9}',
    UpdateRecordError: '{10}',
    UpdateRecordSuccess: '{11}',
    Columns: {{
        Product: {{
            Name: '{12}',
            Price: '{13}'
        }}
    }}
}}",
   T(LocalizableStrings.CircularRelationshipError),
   T(KoreWebLocalizableStrings.General.Create),
   T(KoreWebLocalizableStrings.General.Delete),
   T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord),
   T(KoreWebLocalizableStrings.General.DeleteRecordError),
   T(KoreWebLocalizableStrings.General.DeleteRecordSuccess),
   T(KoreWebLocalizableStrings.General.Edit),
   T(KoreWebLocalizableStrings.General.GetRecordError),
   T(KoreWebLocalizableStrings.General.InsertRecordError),
   T(KoreWebLocalizableStrings.General.InsertRecordSuccess),
   T(KoreWebLocalizableStrings.General.UpdateRecordError),
   T(KoreWebLocalizableStrings.General.UpdateRecordSuccess),
   T(LocalizableStrings.ProductModel.Name),
   T(LocalizableStrings.ProductModel.Price));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
        }
    }
}