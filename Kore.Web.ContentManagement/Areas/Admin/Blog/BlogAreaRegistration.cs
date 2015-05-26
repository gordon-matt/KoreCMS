using System.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog
{
    public class BlogAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return CmsConstants.Areas.Blog; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
        }
    }
}