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
            if (!CheckPermission(ConfigurationPermissions.ReadThemes))
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
            string json = string.Format(
@"{{
    Set: '{0}',
    SetDesktopThemeError: '{1}',
    SetDesktopThemeSuccess: '{2}',
    SetMobileThemeError: '{3}',
    SetMobileThemeSuccess: '{4}',

    Columns: {{
        IsDefaultDesktopTheme: '{5}',
        IsDefaultMobileTheme: '{6}',
        MobileTheme: '{7}',
        PreviewImageUrl: '{8}',
        SupportRtl: '{9}'
    }}
}}",
   T(KoreWebLocalizableStrings.General.Set),
   T(KoreWebLocalizableStrings.Themes.SetDesktopThemeError),
   T(KoreWebLocalizableStrings.Themes.SetDesktopThemeSuccess),
   T(KoreWebLocalizableStrings.Themes.SetMobileThemeError),
   T(KoreWebLocalizableStrings.Themes.SetMobileThemeSuccess),
   T(KoreWebLocalizableStrings.Themes.Model.IsDefaultDesktopTheme),
   T(KoreWebLocalizableStrings.Themes.Model.IsDefaultMobileTheme),
   T(KoreWebLocalizableStrings.Themes.Model.MobileTheme),
   T(KoreWebLocalizableStrings.Themes.Model.PreviewImageUrl),
   T(KoreWebLocalizableStrings.Themes.Model.SupportRtl));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
        }
    }
}