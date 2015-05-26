using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.Log.Controllers
{
    [Authorize]
    [RouteArea(KoreWebConstants.Areas.Log)]
    public class LogController : KoreController
    {
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.Log.Title));
            ViewBag.Title = T(KoreWebLocalizableStrings.Log.Title);

            return View("Kore.Web.Areas.Admin.Log.Views.Log.Index");
        }
    }
}