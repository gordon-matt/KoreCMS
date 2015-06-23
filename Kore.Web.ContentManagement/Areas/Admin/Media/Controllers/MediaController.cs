using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

namespace Kore.Web.ContentManagement.Areas.Admin.Media.Controllers
{
    [Authorize]
    [RouteArea(CmsConstants.Areas.Media)]
    [RoutePrefix("media-library")]
    public class MediaController : KoreController
    {
        [Compress]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(CmsPermissions.MediaRead))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Media.Title));
            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Media.ManageMedia));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Media.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.Media.ManageMedia);

            return View("Kore.Web.ContentManagement.Areas.Admin.Media.Views.Media.Index");
        }
    }
}