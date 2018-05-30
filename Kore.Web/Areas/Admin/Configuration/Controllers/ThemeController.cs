using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Newtonsoft.Json.Linq;

namespace Kore.Web.Areas.Admin.Configuration.Controllers
{
    [Authorize]
    [RouteArea(KoreWebConstants.Areas.Configuration)]
    [RoutePrefix("Themes")]
    public class ThemeController : KoreController
    {
        [Compress]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(KoreWebPermissions.ThemesRead))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.General.Configuration));
            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.General.Themes));

            ViewBag.Title = T(KoreWebLocalizableStrings.General.Configuration);
            ViewBag.SubTitle = T(KoreWebLocalizableStrings.General.Themes);

            return PartialView("Kore.Web.Areas.Admin.Configuration.Views.Theme.Index");
        }

        [Route("get-translations")]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public JsonResult GetTranslations()
        {
            return Json(new
            {
                Set = T(KoreWebLocalizableStrings.General.Set).Text,
                SetDesktopThemeError = T(KoreWebLocalizableStrings.Themes.SetDesktopThemeError).Text,
                SetDesktopThemeSuccess = T(KoreWebLocalizableStrings.Themes.SetDesktopThemeSuccess).Text,
                SetMobileThemeError = T(KoreWebLocalizableStrings.Themes.SetMobileThemeError).Text,
                SetMobileThemeSuccess = T(KoreWebLocalizableStrings.Themes.SetMobileThemeSuccess).Text,
                Columns = new
                {
                    IsDefaultDesktopTheme = T(KoreWebLocalizableStrings.Themes.Model.IsDefaultDesktopTheme).Text,
                    IsDefaultMobileTheme = T(KoreWebLocalizableStrings.Themes.Model.IsDefaultMobileTheme).Text,
                    MobileTheme = T(KoreWebLocalizableStrings.Themes.Model.MobileTheme).Text,
                    PreviewImageUrl = T(KoreWebLocalizableStrings.Themes.Model.PreviewImageUrl).Text,
                    SupportRtl = T(KoreWebLocalizableStrings.Themes.Model.SupportRtl).Text
                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}