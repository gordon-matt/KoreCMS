using System.Web.Mvc;

namespace Kore.Web.Common.Areas.Admin.Regions
{
    public class RegionsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return Constants.Areas.Regions; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
        }
    }
}