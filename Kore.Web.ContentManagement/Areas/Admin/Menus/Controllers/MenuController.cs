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
        [OutputCache(Duration = 86400, VaryByParam = "none")]
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
                InsertRecordError = T(KoreWebLocalizableStrings.General.InsertRecordError).Text,
                InsertRecordSuccess = T(KoreWebLocalizableStrings.General.InsertRecordSuccess).Text,
                NewItem = T(KoreCmsLocalizableStrings.Menus.NewItem).Text,
                Toggle = T(KoreWebLocalizableStrings.General.Toggle).Text,
                UpdateRecordError = T(KoreWebLocalizableStrings.General.UpdateRecordError).Text,
                UpdateRecordSuccess = T(KoreWebLocalizableStrings.General.UpdateRecordSuccess).Text,
                Columns = new
                {
                    Menu = new
                    {
                        Name = T(KoreCmsLocalizableStrings.Menus.MenuModel.Name).Text,
                        UrlFilter = T(KoreCmsLocalizableStrings.Menus.MenuModel.UrlFilter).Text
                    },
                    MenuItem = new
                    {
                        Text = T(KoreCmsLocalizableStrings.Menus.MenuItemModel.Text).Text,
                        Url = T(KoreCmsLocalizableStrings.Menus.MenuItemModel.Url).Text,
                        Position = T(KoreCmsLocalizableStrings.Menus.MenuItemModel.Position).Text,
                        Enabled = T(KoreCmsLocalizableStrings.Menus.MenuItemModel.Enabled).Text
                    }
                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}