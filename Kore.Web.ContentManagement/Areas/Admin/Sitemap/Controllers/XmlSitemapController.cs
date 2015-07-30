using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

namespace Kore.Web.ContentManagement.Areas.Admin.Sitemap.Controllers
{
    [Authorize]
    [RouteArea(CmsConstants.Areas.Sitemap)]
    [RoutePrefix("xml-sitemap")]
    public class XmlSitemapController : KoreController
    {
        [Compress]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(CmsPermissions.SitemapRead))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Sitemap.Title));
            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Sitemap.XMLSitemap));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Sitemap.XMLSitemap);

            return PartialView("Kore.Web.ContentManagement.Areas.Admin.Sitemap.Views.XmlSitemap.Index");
        }
    }
}