using System.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Membership
{
    public class MembershipAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return CmsConstants.Areas.Membership; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
        }
    }
}