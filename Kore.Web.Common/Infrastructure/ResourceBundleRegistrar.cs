using System.Web.Optimization;
using Kore.Web.Infrastructure;

namespace Kore.Web.Common.Infrastructure
{
    public class ResourceBundleRegistrar : IResourceBundleRegistrar
    {
        #region IResourceBundleRegistrar Members

        public void Register(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js/kore-common/regions")
                .Include("~/Scripts/Kore.Web.Common.Areas.Admin.Regions.Scripts.index.js"));
        }

        #endregion IResourceBundleRegistrar Members
    }
}