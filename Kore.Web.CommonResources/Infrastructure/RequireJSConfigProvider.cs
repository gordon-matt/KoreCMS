using System.Collections.Generic;
using Kore.Infrastructure;
using Kore.Web.Infrastructure;
using Kore.Web.Mvc.Resources;

namespace Kore.Web.CommonResources.Infrastructure
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

                paths.Add("kore-common", scriptRegister.GetBundleUrl("kore/common"));
                paths.Add("kore-chosen-knockout", scriptRegister.GetBundleUrl("kore/knockout-chosen"));
                paths.Add("kore-jqueryval", scriptRegister.GetBundleUrl("kore/jqueryval"));
                paths.Add("kore-section-switching", scriptRegister.GetBundleUrl("kore/section-switching"));
                paths.Add("kore-tinymce", scriptRegister.GetBundleUrl("kore/tinymce"));
                paths.Add("bootstrap-fileinput", scriptRegister.GetBundleUrl("third-party/bootstrap-fileinput"));
                paths.Add("bootstrap-slider", scriptRegister.GetBundleUrl("third-party/bootstrap-slider"));
                paths.Add("bootstrap-slider-knockout", scriptRegister.GetBundleUrl("third-party/bootstrap-slider-knockout"));
                paths.Add("momentjs", scriptRegister.GetBundleUrl("third-party/momentjs"));

                return paths;
            }
        }

        public IDictionary<string, string[]> Shim
        {
            get
            {
                var shim = new Dictionary<string, string[]>();

                shim.Add("kore-chosen-knockout", new[] { "chosen", "knockout" });
                shim.Add("kore-jqueryval", new[] { "jqueryval" });
                shim.Add("kore-tinymce", new[] { "tinymce" });
                shim.Add("bootstrap-fileinput", new[] { "jquery", "bootstrap" });
                shim.Add("bootstrap-slider", new[] { "jquery", "bootstrap" });
                shim.Add("bootstrap-slider-knockout", new[] { "jquery", "bootstrap", "knockout" });

                return shim;
            }
        }

        #endregion IRequireJSConfigProvider Members
    }
}