using Kore.Plugins.Widgets.OwlCarousel.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.OwlCarousel
{
    public class OwlCarouselPlugin : BasePlugin
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