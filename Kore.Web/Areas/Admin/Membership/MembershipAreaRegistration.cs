using System.Web.Mvc;

namespace Kore.Web.Areas.Admin.Membership
{
    public class MembershipAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return KoreWebConstants.Areas.Membership; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
        }
    }
}