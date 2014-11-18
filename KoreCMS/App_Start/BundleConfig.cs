using System.Web.Optimization;
using Kore.Infrastructure;
using Kore.Web.Infrastructure;

namespace KoreCMS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Scripts

            bundles.Add(new ScriptBundle("~/bundles/js/elfinder").Include("~/Scripts/elfinder.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/jquery").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/js/jquery-migrate").Include("~/Scripts/jquery-migrate-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/js/jquery-ui").Include("~/Scripts/jquery-ui-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/js/jqueryval").Include("~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/js/kendo-ui").Include(
                "~/Scripts/kendo/2014.1.318/kendo.web.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/knockout").Include("~/Scripts/knockout-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/js/knockout-mapping").Include("~/Scripts/knockout.mapping-latest.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/js/modernizr").Include("~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/js/notify").Include("~/Scripts/notify.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/slugify").Include("~/Scripts/jquery.slugify.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/tinymce").Include(
                "~/Scripts/tinymce/tinymce.min.js",
                "~/Scripts/tinymce/jquery.tinymce.min.js",
                "~/Scripts/wysiwyg.min.js"));

            #endregion Scripts

            #region Styles

            IItemTransform cssRewriteUrlTransform = new CssRewriteUrlTransform();

            bundles.Add(new StyleBundle("~/bundles/content/css")
                .Include("~/Content/bootstrap.css", cssRewriteUrlTransform)
                .Include("~/Content/site.css", cssRewriteUrlTransform));

            bundles.Add(new StyleBundle("~/bundles/content/elfinder")
                .Include("~/Content/elfinder/elfinder.min.css", cssRewriteUrlTransform)
                .Include("~/Content/elfinder/elfinder.MacOS.css", cssRewriteUrlTransform));

            bundles.Add(new StyleBundle("~/bundles/content/jquery-ui")
                .Include("~/Content/themes/base/accordion.css", cssRewriteUrlTransform)
                .Include("~/Content/themes/base/autocomplete.css", cssRewriteUrlTransform)
                .Include("~/Content/themes/base/button.css", cssRewriteUrlTransform)
                .Include("~/Content/themes/base/datepicker.css", cssRewriteUrlTransform)
                .Include("~/Content/themes/base/dialog.css", cssRewriteUrlTransform)
                .Include("~/Content/themes/base/draggable.css", cssRewriteUrlTransform)
                .Include("~/Content/themes/base/menu.css", cssRewriteUrlTransform)
                .Include("~/Content/themes/base/progressbar.css", cssRewriteUrlTransform)
                .Include("~/Content/themes/base/resizable.css", cssRewriteUrlTransform)
                .Include("~/Content/themes/base/selectable.css", cssRewriteUrlTransform)
                .Include("~/Content/themes/base/selectmenu.css", cssRewriteUrlTransform)
                .Include("~/Content/themes/base/sortable.css", cssRewriteUrlTransform)
                .Include("~/Content/themes/base/slider.css", cssRewriteUrlTransform)
                .Include("~/Content/themes/base/spinner.css", cssRewriteUrlTransform)
                .Include("~/Content/themes/base/tabs.css", cssRewriteUrlTransform)
                .Include("~/Content/themes/base/tooltip.css", cssRewriteUrlTransform)
                .Include("~/Content/jquery-ui-bootstrap.css", cssRewriteUrlTransform));

            bundles.Add(new StyleBundle("~/bundles/content/kendo-ui")
                .Include("~/Content/kendo/2014.1.318/kendo.rtl.min.css", cssRewriteUrlTransform)
                .Include("~/Content/kendo/2014.1.318/kendo.default.min.css", cssRewriteUrlTransform)
                .Include("~/Content/kendo/2014.1.318/kendo.common.min.css", cssRewriteUrlTransform)
                .Include("~/Content/kendo/2014.1.318/kendo.bootstrap.min.css", cssRewriteUrlTransform));

            bundles.Add(new StyleBundle("~/bundles/content/kore-icons")
                .Include("~/Areas/Admin/Content/css/icons.css", cssRewriteUrlTransform));

            #endregion Styles

            var registrars = EngineContext.Current.ResolveAll<IResourceBundleRegistrar>();

            foreach (var registrar in registrars)
            {
                registrar.Register(bundles);
            }

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}