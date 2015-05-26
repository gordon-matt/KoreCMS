using Kore.Plugins.Widgets.Bootstrap3.FormBuilder.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.Bootstrap3.FormBuilder
{
    public class Bootstrap3FormBuilderPlugin : BasePlugin
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