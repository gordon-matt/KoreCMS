using System.Web.Mvc;
using Kore.Infrastructure;
using Kore.Web.Configuration;
//using Kore.Web.Mvc.RoboUI;

namespace Kore.Web.ContentManagement.Areas.Admin.Menus
{
    public class MenusAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return Constants.Areas.Menus; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //RoboSettings.RegisterAreaLayoutPath(Constants.Areas.Menus, KoreWebConstants.DefaultAdminLayoutPath);
        }
    }
}