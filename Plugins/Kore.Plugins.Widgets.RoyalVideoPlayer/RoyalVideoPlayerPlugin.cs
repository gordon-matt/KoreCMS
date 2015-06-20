using Kore.Plugins.Widgets.RoyalVideoPlayer.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer
{
    public class RoyalVideoPlayerPlugin : BasePlugin
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