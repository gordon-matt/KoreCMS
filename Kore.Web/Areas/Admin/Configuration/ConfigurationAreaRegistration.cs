using System.Web.Mvc;
using Kore.Infrastructure;
using Kore.Web.Configuration;
//using Kore.Web.Mvc.RoboUI;

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
            //RoboSettings.RegisterAreaLayoutPath(KoreWebConstants.Areas.Configuration, KoreWebConstants.DefaultAdminLayoutPath);
        }
    }
}