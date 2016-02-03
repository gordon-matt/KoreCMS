using Kore.Web.Plugins;

namespace Kore.Plugins.Messaging.LiveChat
{
    public class LiveChatPlugin : BasePlugin
    {
        public override void Install()
        {
            base.Install();
            //InstallLanguagePack<LanguagePackInvariant>();
        }

        public override void Uninstall()
        {
            //UninstallLanguagePack<LanguagePackInvariant>();
            base.Uninstall();
        }
    }
}