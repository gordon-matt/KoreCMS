using System.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks
{
    public class ContentBlocksAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return CmsConstants.Areas.ContentBlocks; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
        }
    }
}