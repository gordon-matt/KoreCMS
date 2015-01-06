using System.Web.Mvc;
using Kore.Web.Mvc;

namespace Kore.Web.Areas.Admin.Configuration.Controllers
{
    [Authorize]
    [RouteArea(KoreWebConstants.Areas.Configuration)]
    [RoutePrefix("Themes")]
    public class ThemeController : KoreController
    {
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(ConfigurationPermissions.ManageThemes))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.General.Themes));
            ViewBag.Title = T(KoreWebLocalizableStrings.General.Themes);

            return View("Kore.Web.Areas.Admin.Configuration.Views.Theme.Index");
        }
    }
}