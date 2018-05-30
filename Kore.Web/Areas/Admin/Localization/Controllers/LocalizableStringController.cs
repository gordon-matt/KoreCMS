using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kore.Localization.Services;
using Kore.Web.Configuration;
using Kore.Web.Areas.Admin.Localization.Models;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

namespace Kore.Web.Areas.Admin.Localization.Controllers
{
    [Authorize]
    [RouteArea(KoreWebConstants.Areas.Localization)]
    [RoutePrefix("localizable-strings")]
    public class LocalizableStringController : KoreController
    {
        private readonly Lazy<ILanguageService> languageService;
        private readonly Lazy<ILocalizableStringService> localizableStringService;
        private readonly KoreSiteSettings siteSettings;

        public LocalizableStringController(
            Lazy<ILanguageService> languageService,
            Lazy<ILocalizableStringService> localizableStringService,
            KoreSiteSettings siteSettings)
        {
            this.languageService = languageService;
            this.localizableStringService = localizableStringService;
            this.siteSettings = siteSettings;
        }

        [Compress]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(KoreWebPermissions.LocalizableStringsRead))
            {
                return new HttpUnauthorizedResult();
            }

            //var language = languageService.Value.FindOne(languageId);

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.Localization.Languages), Url.Action("Index", "Language"));
            //WorkContext.Breadcrumbs.Add(language.Name);
            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.Localization.LocalizableStrings));

            ViewBag.Title = T(KoreWebLocalizableStrings.Localization.Title);
            ViewBag.SubTitle = T(KoreWebLocalizableStrings.Localization.LocalizableStrings);

            //ViewBag.CultureCode = language.CultureCode;

            return PartialView("Kore.Web.Areas.Admin.Localization.Views.LocalizableString.Index");
        }

        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            return Json(new
            {
                Columns = new
                {
                    InvariantValue = T(KoreWebLocalizableStrings.Localization.LocalizableStringModel.InvariantValue).Text,
                    Key = T(KoreWebLocalizableStrings.Localization.LocalizableStringModel.Key).Text,
                    LocalizedValue = T(KoreWebLocalizableStrings.Localization.LocalizableStringModel.LocalizedValue).Text,
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [Compress]
        [Route("export/{cultureCode}")]
        public ActionResult ExportLanguagePack(string cultureCode)
        {
            int tenantId = WorkContext.CurrentTenant.Id;

            var localizedStrings = localizableStringService.Value.Find(x =>
                x.TenantId == tenantId &&
                x.CultureCode == cultureCode &&
                x.TextValue != null);

            var languagePack = new LanguagePackFile
            {
                CultureCode = cultureCode,
                LocalizedStrings = localizedStrings.ToDictionary(k => k.TextKey, v => v.TextValue)
            };

            string json = languagePack.ToJson();
            string fileName = string.Format("{0}_LanguagePack_{1}_{2:yyyy-MM-dd}.json", siteSettings.SiteName, cultureCode, DateTime.Now);
            return File(new UTF8Encoding().GetBytes(json), "application/json", fileName);
        }

        //TODO: Test
        [Compress]
        [Route("translate/{key}")]
        public JsonResult Translate(string key)
        {
            return Json(new { Translation = T(key).Text });
        }
    }
}