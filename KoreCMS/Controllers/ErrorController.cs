using System.Web.Mvc;

namespace KoreCMS.Controllers
{
    [RoutePrefix("error")]
    public class ErrorController : BaseController
    {
        [Route("404-not-found")]
        public ActionResult Error404()
        {
            return View();
        }
    }
}