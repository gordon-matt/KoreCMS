using System.Web.Mvc;
using Kore.Web;
using Kore.Web.Models;
using KoreCMS.Data;

namespace KoreCMS.Controllers
{
    [RoutePrefix("installation")]
    public class InstallationController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            var model = new InstallationModel
            {
                AdminEmail = "admin@yourSite.com",
            };
            return View(model);
        }

        [HttpPost]
        [Route("install-post")]
        public ActionResult InstallPost(InstallationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            InstallationHelper.Install<ApplicationDbContext>(this.HttpContext.Request, model);

            return RedirectToAction("Index", "Home");
        }

        //private static void TryUpdateNLogConfig()
        //{
        //    string path = HostingEnvironment.MapPath("~/NLog.config");
        //    var configFile = ConfigurationManager.OpenExeConfiguration(path);
        //    var nlogSection = configFile.GetSection("nlog");
        //    var nLogConfig = new NLog.
        //}
    }
}