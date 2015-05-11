using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Widgets.Google.Controllers
{
    [Authorize]
    [RouteArea(Constants.RouteArea)]
    [RoutePrefix("xml-sitemap")]
    public class GoogleXmlSitemapController : KoreController
    {
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(GooglePermissions.SitemapRead))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Google));
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.XMLSitemap));
            ViewBag.Title = T(LocalizableStrings.Google);

            return View();
        }
    }
}