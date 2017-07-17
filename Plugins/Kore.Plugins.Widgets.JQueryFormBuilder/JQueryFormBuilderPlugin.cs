using Kore.Plugins.Widgets.JQueryFormBuilder.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.JQueryFormBuilder
{
    public class JQueryFormBuilderPlugin : BasePlugin
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