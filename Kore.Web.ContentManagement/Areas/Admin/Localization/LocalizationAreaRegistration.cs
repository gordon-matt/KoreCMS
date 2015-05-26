using System.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Localization
{
    public class LocalizationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return CmsConstants.Areas.Localization; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
        }
    }
}