using Kore.Plugins.Widgets.RevolutionSlider.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.RevolutionSlider
{
    public class RevolutionSliderPlugin : BasePlugin
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