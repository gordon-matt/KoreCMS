using System.Web.Mvc;
using Kore.Web.Infrastructure;
using Kore.Web.Mvc.Optimization;
using Kore.Caching;

namespace KoreCMS.Controllers
{
    public class HomeController : Controller
    {
        [Compress]
        [Route("")]
        public ActionResult Index()
        {
            if (!DataSettingsHelper.IsDatabaseInstalled)
            {
                return RedirectToAction("Index", "Installation");
            }
            return View();
        }

        //[Route("about-us")]
        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //[Route("contact-us")]
        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}