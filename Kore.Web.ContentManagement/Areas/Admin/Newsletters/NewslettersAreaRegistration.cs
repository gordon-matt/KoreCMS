using System.Web.Mvc;
using Kore.Infrastructure;
using Kore.Web.Configuration;
//using Kore.Web.Mvc.RoboUI;

namespace Kore.Web.ContentManagement.Areas.Admin.Newsletters
{
    public class NewslettersAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return Constants.Areas.Newsletters; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //RoboSettings.RegisterAreaLayoutPath(Constants.Areas.Newsletters, KoreWebConstants.DefaultAdminLayoutPath);
        }
    }
}