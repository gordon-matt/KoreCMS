using System.Web.Mvc;
using Kore.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Menus.Controllers
{
    [Authorize]
    [RouteArea(Constants.Areas.Menus)]
    public class MenuController : KoreController
    {
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(MenusPermissions.ManageMenus))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Menus.Title));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Menus.Title);

            return View("Kore.Web.ContentManagement.Areas.Admin.Menus.Views.Menu.Index");
        }
    }
}