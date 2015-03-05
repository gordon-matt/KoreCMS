using System;
using System.Web.Mvc;
using Kore.Web.ContentManagement.Areas.Admin.Localization.Services;
using Kore.Web.Mvc;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Localization.Controllers
{
    [Authorize]
    [RouteArea(Constants.Areas.Localization)]
    [RoutePrefix("localizable-strings")]
    public class LocalizableStringController : KoreController
    {
        private readonly Lazy<ILanguageService> languageService;

        public LocalizableStringController(Lazy<ILanguageService> languageService)
        {
            this.languageService = languageService;
        }

        [Route("{languageId}")]
        public ActionResult Index(Guid languageId)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new HttpUnauthorizedResult();
            }

            var language = languageService.Value.Find(languageId);

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Localization.Languages), Url.Action("Index", "Language"));
            WorkContext.Breadcrumbs.Add(language.Name);
            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Localization.LocalizableStrings));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Localization.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.Localization.LocalizableStrings);

            ViewBag.CultureCode = language.CultureCode;

            return View("Kore.Web.ContentManagement.Areas.Admin.Localization.Views.LocalizableString.Index");
        }

        //TODO: Test
        [Route("translate/{text}")]
        public JsonResult Translate(string key)
        {
            return Json(new { Translation = T(key).Text });
        }
    }
}