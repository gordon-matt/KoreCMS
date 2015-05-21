using System.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Newsletters
{
    public class NewslettersAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return Constants.Areas.Newsletters; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
        }
    }
}