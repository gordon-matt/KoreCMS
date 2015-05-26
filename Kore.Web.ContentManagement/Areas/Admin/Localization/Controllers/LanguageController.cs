using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Localization.Controllers
{
    [Authorize]
    [RouteArea(CmsConstants.Areas.Localization)]
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
    }
}