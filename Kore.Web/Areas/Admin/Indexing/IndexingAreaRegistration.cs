using System.Web.Mvc;
using Kore.Web.Mvc.RoboUI;

namespace Kore.Web.Areas.Admin.Indexing
{
    public class IndexingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return KoreWebConstants.Areas.Indexing; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RoboSettings.RegisterAreaLayoutPath(KoreWebConstants.Areas.Indexing, "~/Areas/Admin/Views/Shared/_Layout.cshtml");
        }
    }
}