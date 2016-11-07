using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Newtonsoft.Json.Linq;

namespace Kore.Web.ContentManagement.Areas.Admin.Sitemap.Controllers
{
    [Authorize]
    [RouteArea(CmsConstants.Areas.Sitemap)]
    [RoutePrefix("xml-sitemap")]
    public class XmlSitemapController : KoreController
    {
        [Compress]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
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

        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            return Json(new
            {
                ConfirmGenerateFile = T(KoreCmsLocalizableStrings.Sitemap.ConfirmGenerateFile).Text,
                GenerateFileSuccess = T(KoreCmsLocalizableStrings.Sitemap.GenerateFileSuccess).Text,
                GenerateFileError = T(KoreCmsLocalizableStrings.Sitemap.GenerateFileError).Text,
                ChangeFrequencies = new
                {
                    Always = T(KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Always).Text,
                    Hourly = T(KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Hourly).Text,
                    Daily = T(KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Daily).Text,
                    Weekly = T(KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Weekly).Text,
                    Monthly = T(KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Monthly).Text,
                    Yearly = T(KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Yearly).Text,
                    Never = T(KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Never).Text,
                },
                Columns = new
                {
                    ChangeFrequency = T(KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequency).Text,
                    Id = T(KoreCmsLocalizableStrings.Sitemap.Model.Id).Text,
                    Location = T(KoreCmsLocalizableStrings.Sitemap.Model.Location).Text,
                    Priority = T(KoreCmsLocalizableStrings.Sitemap.Model.Priority).Text,
                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}