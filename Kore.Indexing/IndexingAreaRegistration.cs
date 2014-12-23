using System.Web.Mvc;
using Kore.Web.Mvc.RoboUI;

namespace Kore.Indexing
{
    public class IndexingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return Constants.Area; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RoboSettings.RegisterAreaLayoutPath(Constants.Area, "~/Areas/Admin/Views/Shared/_Layout.cshtml");
        }
    }
}