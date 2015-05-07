using Kore.Plugins.Ecommerce.Simple.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Ecommerce.Simple
{
    public class SimpleCommercePlugin : BasePlugin
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