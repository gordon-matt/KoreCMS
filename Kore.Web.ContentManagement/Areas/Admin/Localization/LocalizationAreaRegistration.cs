using System.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Localization
{
    public class LocalizationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return Constants.Areas.Localization; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
        }
    }
}