using Kore.Plugins.Watchdog.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Watchdog
{
    public class WatchdogPlugin : BasePlugin
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