using System.Web.Mvc;
using KoreCMS.Data;

namespace KoreCMS.Controllers
{
    public class HomeController : BaseController
    {
        [Route("")]
        public ActionResult Index()
        {
            ////FOR TEST ONLY
            //using (var db = new ApplicationDbContext())
            //{
            //    db.Database.ExecuteSqlCommand("TRUNCATE TABLE Kore_LocalizableStrings");
            //    db.Seed();
            //}
            ////END TEST

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