using System.Web.Mvc;

namespace Kore.Web.Areas.Admin.Localization
{
    public class LocalizationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return KoreWebConstants.Areas.Localization; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
        }
    }
}