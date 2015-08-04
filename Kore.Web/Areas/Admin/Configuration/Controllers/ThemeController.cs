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
        public JsonResult GetTranslations()
        {
            string json = string.Format(
@"{{
    Set: '{0}',
    SetDesktopThemeError: '{0}',
    SetDesktopThemeSuccess: '{0}',
    SetMobileThemeError: '{0}',
    SetMobileThemeSuccess: '{0}',

    Columns: {{
        IsDefaultDesktopTheme: '{0}',
        IsDefaultMobileTheme: '{0}',
        MobileTheme: '{0}',
        PreviewImageUrl: '{0}',
        SupportRtl: '{0}'
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