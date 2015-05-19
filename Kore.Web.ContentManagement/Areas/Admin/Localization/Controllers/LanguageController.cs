using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Localization.Controllers
{
    [Authorize]
    [RouteArea(Constants.Areas.Localization)]
    [RoutePrefix("languages")]
    public class LanguageController : KoreController
    {
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Localization.Title));
            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Localization.Languages));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Localization.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.Localization.Languages);

            return View("Kore.Web.ContentManagement.Areas.Admin.Localization.Views.Language.Index");
        }

        //[Route("clear-localizable-strings")]
        //public ActionResult Clear()
        //{
        //    if (!CheckPermission(StandardPermissions.FullAccess))
        //    {
        //        return new HttpUnauthorizedResult();
        //    }

        //    // First: Clear All

        //    var repository = EngineContext.Current.Resolve<IRepository<LocalizableString>>();
        //    var recordIds = repository.Table.Select(x => x.Id).ToHashSet();
        //    repository.Delete(x => recordIds.Contains(x.Id));

        //    // Then: Refresh (Add Default Values)

        //    var localizedStringsProviders = EngineContext.Current.ResolveAll<IDefaultLocalizableStringsProvider>();

        //    var toInsert = new HashSet<LocalizableString>();
        //    foreach (var provider in localizedStringsProviders)
        //    {
        //        foreach (var translation in provider.GetTranslations())
        //        {
        //            foreach (var localizedString in translation.LocalizedStrings)
        //            {
        //                toInsert.Add(new LocalizableString
        //                {
        //                    Id = Guid.NewGuid(),
        //                    CultureCode = translation.CultureCode,
        //                    TextKey = localizedString.Key,
        //                    TextValue = localizedString.Value
        //                });
        //            }
        //        }
        //    }
        //    repository.Insert(toInsert);

        //    // Finally: Reset Cache

        //    var cacheManager = EngineContext.Current.Resolve<ICacheManager>();
        //    cacheManager.Clear();

        //    return RedirectToAction("Index");
        //}

        //[ActionName("TableAction")]
        //[Button("SetDefault")]
        //[HttpPost]
        //[Route("post/{id?}")]
        //public ActionResult SetDefault(string id)
        //{
        //    if (!CheckPermission(StandardPermissions.FullAccess))
        //    {
        //        return new HttpUnauthorizedResult();
        //    }

        //    siteSettings.DefaultLanguage = id;
        //    var settingsService = EngineContext.Current.Resolve<ISettingService>();
        //    settingsService.SaveSetting(siteSettings);
        //    return //TODO
        //}
    }
}