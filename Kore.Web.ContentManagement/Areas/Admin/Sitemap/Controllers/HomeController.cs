using System.Web.Hosting;
using System.Web.Mvc;
using Kore.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Sitemap.Controllers
{
    [RouteArea("")]
    public class HomeController : KoreController
    {
        [Route("sitemap.xml")]
        public ActionResult SitemapXml()
        {
            int tenantId = WorkContext.CurrentTenant.Id;
            string filePath = HostingEnvironment.MapPath(string.Format("~/sitemap-{0}.xml", tenantId));

            if (!System.IO.File.Exists(filePath))
            {
                return HttpNotFound();
            }

            string fileContent = System.IO.File.ReadAllText(filePath);
            return Content(filePath, "text/xml");
        }
    }
}