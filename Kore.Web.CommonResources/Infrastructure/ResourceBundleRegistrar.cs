using System.Web.Optimization;
using Kore.Web.Infrastructure;

namespace Kore.Web.CommonResources.Infrastructure
{
    public class ResourceBundleRegistrar : IResourceBundleRegistrar
    {
        #region IResourceBundleRegistrar Members

        public void Register(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js/kore/common")
                .Include("~/Scripts/Kore.Web.CommonResources.Scripts.kore-common.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kore/elfinder")
                .Include("~/Scripts/Kore.Web.CommonResources.Scripts.kore-elfinder.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kore/jqueryval")
                .Include("~/Scripts/Kore.Web.CommonResources.Scripts.kore-jqval.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kore/section-switching")
                .Include("~/Scripts/Kore.Web.CommonResources.Scripts.kore-section-switching.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kore/tinymce")
                .Include("~/Scripts/Kore.Web.CommonResources.Scripts.kore-tinymce.js"));


            bundles.Add(new ScriptBundle("~/bundles/js/third-party/momentjs")
                .Include("~/Scripts/Kore.Web.CommonResources.Scripts.moment-with-locales.min.js"));
        }

        #endregion IResourceBundleRegistrar Members
    }
}