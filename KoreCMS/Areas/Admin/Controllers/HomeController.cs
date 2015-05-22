using System.Web.Mvc;
using Kore.Web;
using Kore.Web.Security.Membership.Permissions;
using KoreCMS.Controllers;

namespace KoreCMS.Areas.Admin.Controllers
{
    [Authorize]
    [RouteArea(KoreWebConstants.Areas.Admin)]
    public class HomeController : BaseController
    {
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(StandardPermissions.DashboardAccess))
            {
                return new HttpUnauthorizedResult();
            }

            ViewBag.Title = T(LocalizableStrings.Dashboard.Title);

            return View();
        }
    }
}