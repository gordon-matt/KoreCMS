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

            bundles.Add(new ScriptBundle("~/bundles/js/kore-web/plugins")
                .Include("~/Scripts/Kore.Web.Areas.Admin.Plugins.Scripts.index.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kore-web/scheduled-tasks")
                .Include("~/Scripts/Kore.Web.Areas.Admin.ScheduledTasks.Scripts.index.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kore-web/settings")
                .Include("~/Scripts/Kore.Web.Areas.Admin.Configuration.Scripts.settings.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kore-web/themes")
                .Include("~/Scripts/Kore.Web.Areas.Admin.Configuration.Scripts.themes.js"));
        }

        #endregion IResourceBundleRegistrar Members
    }
}