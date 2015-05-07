using System.Web.Mvc;
using Kore.Web.Mvc;

namespace Kore.Plugins.Ecommerce.Simple.Controllers
{
    [RouteArea("")]
    [RoutePrefix("store")]
    public class StoreController : KoreController
    {
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(SimpleCommercePermissions.ManageStore))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Store));

            ViewBag.Title = T(LocalizableStrings.Store);

            return View();
        }
    }
}