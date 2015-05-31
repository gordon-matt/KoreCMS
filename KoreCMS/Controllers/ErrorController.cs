using System.Web.Mvc;
using Kore.Web.Mvc.Optimization;

namespace KoreCMS.Controllers
{
    [RoutePrefix("error")]
    public class ErrorController : BaseController
    {
        [Compress]
        [Route("404-not-found")]
        public ActionResult Error404()
        {
            return View();
        }
    }
}