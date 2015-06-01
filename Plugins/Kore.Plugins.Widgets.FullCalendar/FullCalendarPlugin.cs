using Kore.Plugins.Widgets.FullCalendar.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.FullCalendar
{
    public class FullCalendarPlugin : BasePlugin
    {
        public override void Install()
        {
            base.Install();
            InstallLanguagePack<LanguagePackInvariant>();
        }

        public override void Uninstall()
        {
            UninstallLanguagePack<LanguagePackInvariant>();
            base.Uninstall();
        }
    }
}