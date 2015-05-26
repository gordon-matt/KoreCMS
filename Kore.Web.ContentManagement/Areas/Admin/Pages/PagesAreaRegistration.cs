using System.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages
{
    public class PagesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return CmsConstants.Areas.Pages; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
        }
    }
}