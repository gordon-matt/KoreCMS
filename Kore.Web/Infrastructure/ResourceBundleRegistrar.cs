using System.Web.Optimization;

namespace Kore.Web.Infrastructure
{
    public class ResourceBundleRegistrar : IResourceBundleRegistrar
    {
        #region IResourceBundleRegistrar Members

        public void Register(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js/kore-web/log")
                .Include("~/Scripts/Kore.Web.Areas.Admin.Log.Scripts.index.js"));
        }

        #endregion IResourceBundleRegistrar Members
    }
}