using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Newtonsoft.Json.Linq;

namespace Kore.Web.ContentManagement.Areas.Admin.Menus.Controllers
{
    [Authorize]
    [RouteArea(CmsConstants.Areas.Menus)]
    public class MenuController : KoreController
    {
        [Compress]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(CmsPermissions.MenusRead))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Menus.Title));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Menus.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.Menus.ManageMenus);

            return PartialView("Kore.Web.ContentManagement.Areas.Admin.Menus.Views.Menu.Index");
        }

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
    GetRecordError: '{6}',
    InsertRecordError: '{7}',
    InsertRecordSuccess: '{8}',
    NewItem: '{9}',
    Toggle: '{10}',
    UpdateRecordError: '{11}',
    UpdateRecordSuccess: '{12}',
    Columns: {{
        Menu: {{
            Name: '{13}',
            UrlFilter: '{14}'
        }},
        MenuItem: {{
            Text: '{15}',
            Url: '{16}',
            Position: '{17}',
            Enabled: '{18}'
        }}
    }}
}}",
   T(KoreWebLocalizableStrings.General.Create),
   T(KoreWebLocalizableStrings.General.Delete),
   T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord),
   T(KoreWebLocalizableStrings.General.DeleteRecordError),
   T(KoreWebLocalizableStrings.General.DeleteRecordSuccess),
   T(KoreWebLocalizableStrings.General.Edit),
   T(KoreWebLocalizableStrings.General.GetRecordError),
   T(KoreWebLocalizableStrings.General.InsertRecordError),
   T(KoreWebLocalizableStrings.General.InsertRecordSuccess),
   T(KoreCmsLocalizableStrings.Menus.NewItem),
   T(KoreWebLocalizableStrings.General.Toggle),
   T(KoreWebLocalizableStrings.General.UpdateRecordError),
   T(KoreWebLocalizableStrings.General.UpdateRecordSuccess),
   T(KoreCmsLocalizableStrings.Menus.MenuModel.Name),
   T(KoreCmsLocalizableStrings.Menus.MenuModel.UrlFilter),
   T(KoreCmsLocalizableStrings.Menus.MenuItemModel.Text),
   T(KoreCmsLocalizableStrings.Menus.MenuItemModel.Url),
   T(KoreCmsLocalizableStrings.Menus.MenuItemModel.Position),
   T(KoreCmsLocalizableStrings.Menus.MenuItemModel.Enabled));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
        }
    }
}