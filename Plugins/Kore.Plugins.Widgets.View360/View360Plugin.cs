using Kore.Plugins.Widgets.View360.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.View360
{
    public class View360Plugin : BasePlugin
    {
        public override void Install()
        {
            base.Install();
            InstallLocalizableStrings<DefaultLocalizableStringsProvider>();
        }

        public override void Uninstall()
        {
            base.Uninstall();
            UninstallLocalizableStrings<DefaultLocalizableStringsProvider>();
        }
    }
}