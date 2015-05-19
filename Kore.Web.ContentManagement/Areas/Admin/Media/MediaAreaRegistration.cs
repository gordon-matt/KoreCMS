using System.Web.Mvc;

//using Kore.Web.Mvc.RoboUI;

namespace Kore.Web.ContentManagement.Areas.Admin.Media
{
    public class MediaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return Constants.Areas.Media; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //RoboSettings.RegisterAreaLayoutPath(Constants.Areas.Media, KoreWebConstants.DefaultAdminLayoutPath);
        }
    }
}