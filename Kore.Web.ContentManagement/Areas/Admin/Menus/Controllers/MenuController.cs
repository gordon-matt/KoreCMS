using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

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
    }
}