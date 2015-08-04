using System.Collections.Generic;
using Kore.Infrastructure;
using Kore.Web.Infrastructure;
using Kore.Web.Mvc.Resources;

namespace Kore.Web.Common.Infrastructure
{
    public class RequireJSConfigProvider : IRequireJSConfigProvider
    {
        #region IRequireJSConfigProvider Members

        public IDictionary<string, string> Paths
        {
            get
            {
                var workContext = EngineContext.Current.Resolve<IWebWorkContext>();
                var scriptRegister = new ScriptRegister(workContext);

                var paths = new Dictionary<string, string>();

                paths.Add("jquery-maphilight", scriptRegister.GetBundleUrl("third-party/jquery-maphilight"));

                return paths;
            }
        }

        public IDictionary<string, string[]> Shim
        {
            get
            {
                var shim = new Dictionary<string, string[]>();

                shim.Add("jquery-maphilight", new[] { "jquery" });

                return shim;
            }
        }

        #endregion IRequireJSConfigProvider Members
    }
}