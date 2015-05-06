using System.Web.Mvc;
using Kore.Web.Mvc;

namespace Kore.Plugins.Ecommerce.Simple.Controllers
{
    [Authorize]
    [RouteArea(Constants.RouteArea)]
    [RoutePrefix("categories")]
    public class CategoryController : KoreController
    {

    }
}