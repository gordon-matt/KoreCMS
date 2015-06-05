using System.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Sitemap
{
    public class SitemapAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return CmsConstants.Areas.Sitemap; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
        }
    }
}