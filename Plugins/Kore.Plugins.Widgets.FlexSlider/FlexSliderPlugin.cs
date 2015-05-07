using Kore.Plugins.Widgets.FlexSlider.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.FlexSlider
{
    public class FlexSliderPlugin : BasePlugin
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