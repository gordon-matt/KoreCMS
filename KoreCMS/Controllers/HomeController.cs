using System;
using System.Web.Mvc;
using Kore.Web.Mvc.Optimization;

namespace KoreCMS.Controllers
{
    public class HomeController : BaseController
    {
        [Compress]
        [Route("")]
        public ActionResult Index()
        {
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