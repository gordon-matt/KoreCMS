using System.Web.Mvc;
using Kore.Web.Mvc;

namespace Kore.Plugins.Ecommerce.Simple.Controllers
{
    [Authorize]
    [RouteArea(Constants.RouteArea)]
    [RoutePrefix("categories")]
    public class CategoryController : KoreController
    {
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(SimpleCommercePermissions.ReadCategories))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Store));
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Categories));

            ViewBag.Title = T(LocalizableStrings.Store);
            ViewBag.SubTitle = T(LocalizableStrings.Categories);

            return View();
        }
    }
}