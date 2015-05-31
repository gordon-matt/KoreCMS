using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

namespace Kore.Web.Areas.Admin.Configuration.Controllers
{
    [Authorize]
    [RouteArea(KoreWebConstants.Areas.Configuration)]
    [RoutePrefix("Themes")]
    public class ThemeController : KoreController
    {
        [Compress]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(ConfigurationPermissions.ReadThemes))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.General.Configuration));
            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.General.Themes));

            ViewBag.Title = T(KoreWebLocalizableStrings.General.Configuration);
            ViewBag.SubTitle = T(KoreWebLocalizableStrings.General.Themes);

            return View("Kore.Web.Areas.Admin.Configuration.Views.Theme.Index");
        }
    }
}