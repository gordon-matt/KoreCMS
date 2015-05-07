using Kore.Plugins.Widgets.Google.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.Google
{
    public class GooglePlugin : BasePlugin
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