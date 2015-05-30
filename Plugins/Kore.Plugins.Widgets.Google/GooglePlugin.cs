using Kore.Plugins.Widgets.Google.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.Google
{
    public class GooglePlugin : BasePlugin
    {
        public override void Install()
        {
            base.Install();
            InstallLanguagePack<LanguagePackInvariant>();
        }

        public override void Uninstall()
        {
            base.Uninstall();
            UninstallLanguagePack<LanguagePackInvariant>();
        }
    }
}