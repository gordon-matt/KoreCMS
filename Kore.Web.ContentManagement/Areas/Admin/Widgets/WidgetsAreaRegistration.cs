using System.Web.Mvc;
using Kore.Web.Mvc.RoboUI;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets
{
    public class WidgetsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return Constants.Areas.Widgets; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RoboSettings.RegisterAreaLayoutPath(Constants.Areas.Widgets, "~/Areas/Admin/Views/Shared/_Layout.cshtml");
        }
    }
}