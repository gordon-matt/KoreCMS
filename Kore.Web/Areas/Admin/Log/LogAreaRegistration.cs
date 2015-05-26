using System.Web.Mvc;

namespace Kore.Web.Areas.Admin.Log
{
    public class LogAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return KoreWebConstants.Areas.Log; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
        }
    }
}