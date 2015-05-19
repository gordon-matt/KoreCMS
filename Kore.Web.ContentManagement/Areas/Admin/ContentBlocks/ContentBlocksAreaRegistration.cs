using System.Web.Mvc;

//using Kore.Web.Mvc.RoboUI;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks
{
    public class ContentBlocksAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return Constants.Areas.ContentBlocks; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //RoboSettings.RegisterAreaLayoutPath(Constants.Areas.ContentBlocks, KoreWebConstants.DefaultAdminLayoutPath);
        }
    }
}