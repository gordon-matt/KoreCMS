using System.Web.Mvc;
using Kore.Infrastructure;
using Kore.Web.Configuration;
//using Kore.Web.Mvc.RoboUI;

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
            //RoboSettings.RegisterAreaLayoutPath(Constants.Areas.Localization, KoreWebConstants.DefaultAdminLayoutPath);
        }
    }
}