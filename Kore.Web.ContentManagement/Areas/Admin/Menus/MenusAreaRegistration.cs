using System.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Menus
{
    public class MenusAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return CmsConstants.Areas.Menus; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
        }
    }
}