using System.Web.Mvc;
using Kore.Infrastructure;
using Kore.Web.Configuration;
using Kore.Web.Mvc.RoboUI;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages
{
    public class PagesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return Constants.Areas.Pages; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RoboSettings.RegisterAreaLayoutPath(Constants.Areas.Pages, KoreWebConstants.DefaultAdminLayoutPath);
        }
    }
}