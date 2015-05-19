using System.Web.Mvc;

//using Kore.Web.Mvc.RoboUI;

namespace Kore.Web.ContentManagement.Areas.Admin.Membership
{
    public class MembershipAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return Constants.Areas.Membership; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //RoboSettings.RegisterAreaLayoutPath(Constants.Areas.Membership, KoreWebConstants.DefaultAdminLayoutPath);
        }
    }
}