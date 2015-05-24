using Kore.Plugins.Widgets.Bootstrap3.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.Bootstrap3
{
    public class Bootstrap3Plugin : BasePlugin
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