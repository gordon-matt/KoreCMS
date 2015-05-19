using System.Web.Mvc;

//using Kore.Web.Mvc.RoboUI;

namespace Kore.Web.Areas.Admin.Plugins
{
    public class PluginsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return KoreWebConstants.Areas.Plugins; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //RoboSettings.RegisterAreaLayoutPath(KoreWebConstants.Areas.Plugins, KoreWebConstants.DefaultAdminLayoutPath);
        }
    }
}