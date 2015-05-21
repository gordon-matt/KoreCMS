using System.Web.Mvc;

namespace Kore.Web.Areas.Admin.Configuration
{
    public class ConfigurationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return KoreWebConstants.Areas.Configuration; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
        }
    }
}