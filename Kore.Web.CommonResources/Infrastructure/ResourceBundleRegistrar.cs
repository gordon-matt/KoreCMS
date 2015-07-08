using System.Web.Optimization;
using Kore.Web.Infrastructure;

namespace Kore.Web.CommonResources.Infrastructure
{
    public class ResourceBundleRegistrar : IResourceBundleRegistrar
    {
        #region IResourceBundleRegistrar Members

        public void Register(BundleCollection bundles)
        {
            // Note to self: don't have '-' in folder name (file name is fine)...
            // If there's a '-' in the folder name, then the file won't be found

            #region Scripts

            bundles.Add(new ScriptBundle("~/bundles/js/kore/common")
                .Include("~/Scripts/Kore.Web.CommonResources.Scripts.kore-common.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kore/jqueryval")
                .Include("~/Scripts/Kore.Web.CommonResources.Scripts.kore-jqval.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kore/knockout-chosen")
                .Include("~/Scripts/Kore.Web.CommonResources.Scripts.kore-knockout-chosen.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kore/section-switching")
                .Include("~/Scripts/Kore.Web.CommonResources.Scripts.kore-section-switching.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kore/tinymce")
                .Include("~/Scripts/Kore.Web.CommonResources.Scripts.kore-tinymce.js"));



            bundles.Add(new ScriptBundle("~/bundles/js/third-party/bootstrap-fileinput")
                .Include("~/Scripts/Kore.Web.CommonResources.Scripts.bootstrapFileInput.fileinput.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/third-party/bootstrap-slider")
                .Include("~/Scripts/Kore.Web.CommonResources.Scripts.bootstrap-slider.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/third-party/bootstrap-slider-knockout")
                .Include("~/Scripts/Kore.Web.CommonResources.Scripts.bootstrap-slider-knockout-binding.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/third-party/momentjs")
                .Include("~/Scripts/Kore.Web.CommonResources.Scripts.moment-with-locales.min.js"));

            #endregion

            #region Styles

            IItemTransform cssRewriteUrlTransform = new CssRewriteUrlTransform();

            bundles.Add(new StyleBundle("~/bundles/content/third-party/bootstrap-fileinput")
                .Include("~/Content/Kore.Web.CommonResources.Content.bootstrapFileInput.css.fileinput.min.css", cssRewriteUrlTransform));

            bundles.Add(new StyleBundle("~/bundles/content/third-party/bootstrap-slider")
                .Include("~/Content/Kore.Web.CommonResources.Content.bootstrap-slider.min.css", cssRewriteUrlTransform));

            bundles.Add(new StyleBundle("~/bundles/content/third-party/metro")
                .Include("~/Content/Kore.Web.CommonResources.Content.metro.blue.css", cssRewriteUrlTransform));

            #endregion
        }

        #endregion IResourceBundleRegistrar Members
    }
}