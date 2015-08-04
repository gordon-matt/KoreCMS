using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

namespace Kore.Plugins.Ecommerce.Simple.Controllers
{
    [RouteArea(Constants.RouteArea)]
    public class AdminHomeController : KoreController
    {
        [Compress]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(SimpleCommercePermissions.ViewMenu))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Store));

            ViewBag.Title = T(LocalizableStrings.Store);

            return PartialView();
        }
    }
}