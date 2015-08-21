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
            string json = string.Format(
@"{{
    ConfirmGenerateFile: '{0}',
    GenerateFileSuccess: '{1}',
    GenerateFileError: '{2}',

    ChangeFrequencies: {{
        Always: '{3}',
        Hourly: '{4}',
        Daily: '{5}',
        Weekly: '{6}',
        Monthly: '{7}',
        Yearly: '{8}',
        Never: '{9}',
    }},

    Columns: {{
        ChangeFrequency: '{10}',
        Id: '{11}',
        Location: '{12}',
        Priority: '{13}',
    }}
}}",
   T(KoreCmsLocalizableStrings.Sitemap.ConfirmGenerateFile),
   T(KoreCmsLocalizableStrings.Sitemap.GenerateFileSuccess),
   T(KoreCmsLocalizableStrings.Sitemap.GenerateFileError),
   T(KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Always),
   T(KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Hourly),
   T(KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Daily),
   T(KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Weekly),
   T(KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Monthly),
   T(KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Yearly),
   T(KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequencies.Never),
   T(KoreCmsLocalizableStrings.Sitemap.Model.ChangeFrequency),
   T(KoreCmsLocalizableStrings.Sitemap.Model.Id),
   T(KoreCmsLocalizableStrings.Sitemap.Model.Location),
   T(KoreCmsLocalizableStrings.Sitemap.Model.Priority));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
        }
    }
}