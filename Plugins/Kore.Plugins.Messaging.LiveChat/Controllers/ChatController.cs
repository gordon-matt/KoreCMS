using System.Web.Mvc;
using Kore.Web.Mvc;

namespace Kore.Plugins.Messaging.LiveChat.Controllers
{
    [RouteArea(Constants.RouteArea)]
    public class ChatController : KoreController
    {
        [Route("")]
        public ActionResult Index()
        {
            return PartialView();
        }

        [Route("agent")]
        public ActionResult Agent()
        {
            return View();
        }

        [Route("setup")]
        public ActionResult Setup()
        {
            return View("Install");
        }
    }
}